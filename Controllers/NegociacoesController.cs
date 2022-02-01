using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using alldux_plataforma.Data;
using alldux_plataforma.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace alldux_plataforma.Controllers
{
    [Authorize(Policy = "Negociacoes")]
    public class NegociacoesController : Controller
    {
        private ContentDbContext context;
        private UserManager<ApplicationUser> userManager;

        public NegociacoesController(ContentDbContext _context, UserManager<ApplicationUser> _userManager)
        {
            context = _context;
            userManager = _userManager;
        }

        //versao do vinicius

        private void Erro(string msg, Exception ex){
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
            Console.WriteLine("ERRO: "+msg);
            Console.WriteLine("----------------------------------------------------------------------------------------------------");
            Console.WriteLine(ex.Message);
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
            TempData["alerta"] = msg;
            TempData["tipo"] = "danger";
            TempData["exception"] = ex.Message;
        }

        private void Alerta(string msg, string type){
            TempData["alerta"] = msg;
            TempData["tipo"] = type;

            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
            Console.WriteLine(type.ToUpper()+": "+msg);
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
        }

        private List<Medicamento> ListaMedicamentos()
        {
            try{
                return context.Medicamentos
                        .AsNoTracking()
                        .Include(e => e.Variacoes)
                        .OrderBy(e => e.PrincipioAtivo)
                        .ToList();

            }catch (Exception ex){
                Erro("Erro buscando informações de medicamentos.", ex);
                return null;
            }
        }

        private Medicamento FindMedicamentoPorVariacao(string Id)
        {
            try{
                var medicamento = context.Medicamentos
                                        .Include(e => e.Variacoes)
                                        .Where(e => e.Variacoes.Any(e => e.Id == new Guid(Id)))
                                        .FirstOrDefault();
                
                return medicamento;

            }catch (Exception ex){
                Erro("Erro buscando informações de medicamentos.", ex);
                return null;
            }
        }

        private List<Medicamento> FindMedicamentosNegociacao(Negociacao neg){
            List<Medicamento> listaMedicamentosNegociacao = new List<Medicamento>();
            
            foreach (var item in neg.NegociacaoItem)
            {
                var medicamento = FindMedicamentoPorVariacao(item.MedicamentoId);
                if(!listaMedicamentosNegociacao.Contains(medicamento) && medicamento != null ){
                    listaMedicamentosNegociacao.Add(medicamento);
                }
            }
            return listaMedicamentosNegociacao;
        }

        private List<Negociacao> ListaNegociacoes(){
            return context.Negociacao
                                .AsNoTracking()
                                .OrderBy(e => e.CreatedDate)
                                .ToList();
        }

        private Negociacao FindNegociacao(string Id){
            try{
                var negociacao =  context.Negociacao
                                            .AsNoTracking()
                                            .Where(n => n.Id == new Guid(Id))
                                            .Include(i => i.NegociacaoItem)
                                            .FirstOrDefault();
                List<NegociacaoItem> itensDelete = new List<NegociacaoItem>();
                
                foreach(var item in negociacao.NegociacaoItem){
                    var medicamento = FindMedicamento(item.MedicamentoId);
                    if(medicamento.Id !=Guid.Empty){
                        item.AddMedicamentoInfo(medicamento);
                    }else{
                        //remove 
                        itensDelete.Add(item);
                    }
                }

                foreach(var del in itensDelete){
                    negociacao.NegociacaoItem.Remove(del);
                }

                return negociacao;
            }catch(Exception ex){
                Erro("Erro procurando negociação.", ex);
                return null;
            }
        }
        private Negociacao FindNegociacaoTrial(string Id){
            try{
                List<NegociacaoItem> itensDelete = new List<NegociacaoItem>();
                var negociacao =  context.Negociacao
                                            .AsNoTracking()
                                            .Where(n => n.Id == new Guid(Id))
                                            .Include(i => i.NegociacaoItem)
                                            .FirstOrDefault();                            
                
                foreach(var item in negociacao.NegociacaoItem){
                    var medicamento = FindMedicamento(item.MedicamentoId);
                    if(medicamento.Id != Guid.Empty){
                        item.AddMedicamentoInfo(medicamento);
                    }else{
                        itensDelete.Add(item);
                    }
                }

                foreach(var del in itensDelete){
                    negociacao.NegociacaoItem.Remove(del);
                }

                negociacao.NegociacaoItem = negociacao.NegociacaoItem.OrderByDescending(e => e.Margem).Take(5).ToList();

                return negociacao;
            }catch(Exception ex){
                Erro("Erro procurando negociação.", ex);
                return null;
            }
        }

        private MedicamentoVariacao FindMedicamento(string id){
            try{
                var Variacao = context.MedicamentoVariacao
                                            .AsNoTracking()
                                            .Where(e => e.Id == new Guid(id))
                                            .Include(i => i.Medicamento)
                                            .FirstOrDefault();
                if(Variacao != null){
                    return Variacao;
                }else{
                    return new MedicamentoVariacao();
                }
            }catch(Exception ex){
                Erro("Erro procurando medicamento "+id, ex);
                return new MedicamentoVariacao();
            }
        }

        private bool AddNegociacao(Negociacao negociacao){
            try{
                context.Negociacao.Add(negociacao);
                context.SaveChanges();
                return true;
                
            }catch (Exception ex){
                Erro("Ocorreu um erro ao criar a negociação, por favor contate o administrador.", ex);
                return false;
            }
        }

        public IActionResult Index(string Id)
        {   
            Negociacao Model = new Negociacao();
            var appUserRoles = userManager.GetRolesAsync(userManager.GetUserAsync(HttpContext.User).Result).Result.FirstOrDefault();
            
            if(appUserRoles == "TestUser") {
                try{
                    ViewBag.ListaNegociacoes = ListaNegociacoes();
                    if(!String.IsNullOrEmpty(Id)){
                        var neg = FindNegociacaoTrial(Id);
                        if(neg != null) Model = neg;
                    }
                }catch(Exception ex){
                    Erro("Negociação não encontrada. (TRIAL)", ex);
                }
                
                return View(Model);
                //return RedirectToAction("AcessoRestrito", "Home");
            }

            try{
                ViewBag.ListaNegociacoes = ListaNegociacoes();
                if(!String.IsNullOrEmpty(Id)){
                    var neg = FindNegociacao(Id);
                    if(neg != null) Model = neg;
                }
            }catch(Exception ex){
                Erro("Negociação não encontrada.", ex);
            }

            return View(Model);
        }
        public IActionResult Adicionar()
        {
            ViewBag.ListaMedicamentos = ListaMedicamentos();
            return View();
        }

        [HttpPost]
        public IActionResult Adicionar(Negociacao Negociacao)
        {
            ApplicationUser usuario = userManager.GetUserAsync(HttpContext.User).Result;

            Negociacao novaNegociacao = new Negociacao{
                Titulo = Negociacao.Titulo,
                Parceiro = Negociacao.Parceiro,
                DestaqueLongo = Negociacao.DestaqueLongo,
                DestaqueCurto = Negociacao.DestaqueCurto,
                Texto = Negociacao.Texto,
                Obs = Negociacao.Obs,
                DataInicio = Negociacao.DataInicio,
                DataFim = Negociacao.DataFim,
                CreatedId = new Guid(usuario.Id),
                CreatedDate = DateTime.Now,
            };
            
            foreach(var item in Negociacao.NegociacaoItem){
                if(item.MedicamentoId != null){
                    item.NegociacaoId = novaNegociacao.Id;
                    novaNegociacao.NegociacaoItem.Add(item);
                }
            }

            var adiciona = AddNegociacao(novaNegociacao);

            if(adiciona){
                Alerta("Negociacao criada com sucesso.", "success");
                return RedirectToAction("Index", "Negociacoes", new { });
            }

            return RedirectToAction("Index", "Negociacoes", new {});
        }
        public IActionResult Remover(string Id)
        {
            try{
                var remove = FindNegociacao(Id);
                context.Negociacao.Remove(remove);
                context.SaveChanges();
                Alerta("Negociacao removida com sucesso.", "success");
            }catch(Exception ex){
                Erro("Negociacao não encontrada.", ex);
            }
            
            return RedirectToAction("Index", "Negociacoes", new {});
        }


    }
}
using System;
using System.Linq;
using Microsoft.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using alldux_plataforma.Data;
using alldux_plataforma.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Text.Json;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace alldux_plataforma.Controllers
{
    [Authorize(Policy = "DiretrizesPrecificadas")]
    public class DiretrizesPrecificadasController : Controller
    {
        private ContentDbContext context;
        private UserManager<ApplicationUser> userManager;

        public DiretrizesPrecificadasController(ContentDbContext _context, UserManager<ApplicationUser> _userManager)
        {
            context = _context;
            userManager = _userManager;
        }

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

        private void AdicionaDiretriz(DiretrizPrecificada diretriz)
        {
            try{
                context.DiretrizPrecificadas.Add(diretriz);
                context.SaveChanges();
            }catch (Exception ex){
                Erro("Diretriz Precificada Controller > AdicionaDiretriz", ex);
            }
        }
        private void UpdateDiretrizPrec(Guid Id, DiretrizPrecificada diretrizNova)
        {   
            try{
                var diretrizAtualizar = FindDiretrizesPrec(Id);
                context.DiretrizPrecificadas.Remove(diretrizAtualizar);
                context.SaveChanges();

                diretrizNova.Id = Id;
                context.DiretrizPrecificadas.Add(diretrizNova);
                context.SaveChanges();
            }catch(Exception ex){
                Erro("Erro salvando tabela, por favor contate o administrador.", ex);
                RedirectToAction("Index", "Diretrizes", new {});
            }
        }
        private void UpdateDiretrizPrecTabela(Guid Id, DiretrizPrecificadaTabela tabelaNova)
        {   
            var tabelaAtualizar = FindDiretrizesPrecTabela(Id);
            context.DiretrizPrecificadaTabela.Remove(tabelaAtualizar);
            context.SaveChanges();

            tabelaNova.Id = Id;
            context.DiretrizPrecificadaTabela.Add(tabelaNova);
            context.SaveChanges();
        }

        private DiretrizesClinicas FindDiretrizesEdit(Guid Id)
        {
            try{
                var Diretriz = context.DiretrizesClinicas
                                    .Include(e => e.DiretrizPrecificada)
                                    .Single(d => d.Id == Id);
                return Diretriz;
            }catch (Exception ex){
                Erro("Diretrizes Precificadas Controller > FindDiretrizesEdit()", ex);
                return null;
            }
        }

        private DiretrizPrecificada FindDiretrizesPrec(Guid Id)
        {
            try{
                var Diretriz = context.DiretrizPrecificadas
                                .AsNoTracking()
                                .Where(e => e.Id == Id)
                                .Include(e => e.DiretrizPrecificadaTabela.OrderBy(e => e.Index))
                                .ThenInclude(e => e.DiretrizPrecificadaRegistro.OrderBy(e => e.Index))
                                .Single();
                foreach(var tabela in Diretriz.DiretrizPrecificadaTabela){
                    AssociaMedicamento(tabela);
                }
                return Diretriz;
            }catch  (Exception ex){
                Erro("FindDiretrizesPrec", ex);
                return null;
            }
        }
        private DiretrizPrecificadaTabela FindDiretrizesPrecTabela(Guid? Id)
        {
            try{
                var tabela = new DiretrizPrecificadaTabela();
                tabela = context.DiretrizPrecificadaTabela
                            .AsNoTracking()
                            .Where(d => d.Id == Id)
                            .Include(e => e.DiretrizPrecificadaRegistro.OrderBy(e => e.Index))
                            .Single();

                AssociaMedicamento(tabela);
                return tabela;
            }catch  (Exception ex){
                Erro("Erro listando detalhes da diretriz precificada, por favor contate o administrador.", ex);
                return null;
            }
        }

        private DiretrizPrecificada FindDiretrizesPrecInfo(Guid Id){
            try{
                var Diretriz = context.DiretrizPrecificadas
                                .AsNoTracking()
                                .Where(e => e.Id == Id)
                                .Single();
                return Diretriz;
            }catch  (Exception ex){
                Erro("FindDiretrizesPrecInfo", ex);
                return null;
            }
        }

        public DiretrizPrecificadaTabela AssociaMedicamento(DiretrizPrecificadaTabela tabela)
        {
            foreach(var registro in tabela.DiretrizPrecificadaRegistro){
                if(!String.IsNullOrEmpty(registro.TISS) && registro.Ordem != "#quebra#"){
                    var medicamento = context.MedicamentoVariacao
                                                .AsNoTracking()
                                                .Where(e => e.TISS == registro.TISS)
                                                .Include(r => r.Medicamento)
                                                .FirstOrDefault();

                    registro.MedicamentoNome = medicamento.Nome;
                    registro.MedicamentoPrincipioAtivo = medicamento.Medicamento.PrincipioAtivo;
                    registro.ValorCpMgAlldux = medicamento.PrecoAllduxMg();
                }else{
                    registro.ValorCpMgAlldux = 0;
                    if(String.IsNullOrEmpty(registro.TISS) && registro.Ordem != "#quebra#") Console.WriteLine("Medicamento sem TISS cadastrado: "+registro.Id);
                }
            }
            return tabela;
        }

        private void RemoveDriretrizPrec(string id)
        {
            var remove = FindDiretrizesPrec(new Guid(id));
            if(remove != null){
                context.DiretrizPrecificadas.Remove(remove);
                context.SaveChanges();
            }
        }

        private List<string> ListaLinha()
        {
            try{
                var listaP = context.DiretrizPrecificadaTabela.AsNoTracking().Select(e => e.Linha).ToList();
                List<string> cat = new List<string>();
                foreach(var item in listaP){
                    if(!cat.Contains(item)){
                        cat.Add(item);
                    }
                }
                return cat;
            }catch (Exception ex){
                Erro("Ocorreu um erro ao listar informações, por favor contate o administrador.", ex);
                return null;
            }
        }

        private List<string> ListaFinalidade()
        {
            try{
                var listaP = context.DiretrizPrecificadaTabela.AsNoTracking().Select(e => e.Finalidade).ToList();
                List<string> cat = new List<string>();
                foreach(var item in listaP){
                    if(!cat.Contains(item)){
                        cat.Add(item);
                    }
                }
                return cat;
            }catch (Exception ex){
                Erro("Ocorreu um erro ao listar informações, por favor contate o administrador.", ex);
                return null;
            }
        }

        private List<Medicamento> ListaMedicamentos(){
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

        [Authorize(Roles="Admin")]
        public IActionResult Adicionar(string Id)
        {
            DiretrizesClinicas DiretrizClinica = FindDiretrizesEdit(new Guid(Id));
            DiretrizPrecificada novaDiretriz = new DiretrizPrecificada(){
                Conteudo = "",
                DiretrizesClinicasId = new Guid(Id)
            };
            
            ViewBag.Titulo = DiretrizClinica.Titulo;
            return View(novaDiretriz);
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public IActionResult Adicionar(DiretrizPrecificada novaDiretriz)
        {
            AdicionaDiretriz(novaDiretriz);
            return RedirectToAction("Exibir", "DiretrizesPrecificadas", new { novaDiretriz.Id });
        }

        public async Task<IActionResult> Exibir(string Id)
        {
            var appUser = await userManager.GetUserAsync(HttpContext.User);
            var appUserRoles = await userManager.GetRolesAsync(appUser);
            var appUserRole = appUserRoles.FirstOrDefault();

            List<string> protocolosLivres = new List<string>();
            protocolosLivres.Add("9ddd643d-a308-450b-aabe-99dada40635a");
            protocolosLivres.Add("52022fcc-4889-4dd3-8c85-f6ac941c6800");
            protocolosLivres.Add("696ffc1f-f0d4-4622-941b-5b2c9b8cdef8");
            protocolosLivres.Add("2a858656-37f3-41a2-9cc3-9707c5e4924f");
            protocolosLivres.Add("5ef278e8-a2bd-48cd-bf28-0120e64eb4d7"); //Mama
            protocolosLivres.Add("85316b30-6fd2-41e2-9569-467e56db1678"); //Colon
            protocolosLivres.Add("556a08a3-e742-491d-8e38-3577a44ee959"); //Pulmao

            List<string> protocolosTrial = new List<string>();
            protocolosTrial.Add("67a45b35-70fd-4e01-8fa3-cc4cfd76b809");// mama
            protocolosTrial.Add("653cfda9-e76c-4f98-aac3-84f4ff34d444");// mama
            protocolosTrial.Add("3c67c6fc-4ed7-4609-87df-31cf600a28ec");// mama
            protocolosTrial.Add("953811f9-4028-42c6-bd87-a67ba8881dd8");//pulmao
            protocolosTrial.Add("720d0c3b-662c-4ff8-9690-dbf30b438f62");//pulmao
            protocolosTrial.Add("449084a7-27fd-45ff-a92c-20e610c22a77");//colon
            protocolosTrial.Add("2e792772-28eb-4fc1-aa38-a541a922a3ae");//colon
            protocolosTrial.Add("a5ba215f-da06-4d86-b8a1-e587aa27f36b");//colon

            if(!protocolosLivres.Contains(Id)){
                if(appUserRole == "TestUser") return RedirectToAction("AcessoRestrito", "Home");
            }

            try{
                DiretrizesClinicas DiretrizClinica = FindDiretrizesEdit(new Guid(Id));
                DiretrizPrecificada DiretrizPrec = FindDiretrizesPrec(DiretrizClinica.Id);
                
                ViewBag.Titulo = DiretrizClinica.Titulo;
                
                if(appUserRole == "TestUser") {
                    List<DiretrizPrecificadaTabela> DiretrizPrecTrial = new List<DiretrizPrecificadaTabela>();
                    DiretrizPrecTrial.AddRange(DiretrizPrec.DiretrizPrecificadaTabela);

                    foreach(var item in DiretrizPrec.DiretrizPrecificadaTabela){
                        if(!protocolosTrial.Contains(item.Id.ToString())){
                            DiretrizPrecTrial.Remove(item);
                        }
                    }
                    DiretrizPrec.DiretrizPrecificadaTabela = DiretrizPrecTrial;
                }

                return View(DiretrizPrec);

            }catch (Exception ex){
                Erro("Diretriz Precificada Controller > Exibir (GET)", ex);
                return RedirectToAction("Index", "Diretrizes", new {});
            }
        }

        [Authorize(Roles="Admin")]
        public IActionResult AdicionarTabela(string Id)
        {   
            DiretrizPrecificada DiretrizPrec = FindDiretrizesPrec(new Guid(Id));
            DiretrizesClinicas DiretrizClinica = FindDiretrizesEdit(DiretrizPrec.DiretrizesClinicasId);
            
            ViewBag.ListaMedicamentos = ListaMedicamentos();
            ViewBag.Conteudo = DiretrizPrec.Conteudo;
            ViewBag.Indice = DiretrizPrec.DiretrizPrecificadaTabela.Count();
            ViewBag.Titulo = DiretrizClinica.Titulo;
            ViewBag.DiretrizPrecificadaId = DiretrizPrec.Id;
            return View(new DiretrizPrecificadaTabela());
        }

        [HttpPost]
        [Produces("application/json")]
        [Authorize(Roles="Admin")]
        public JsonResult AdicionarTabelaPost([FromBody] DiretrizPrecificadaTabela novaTabela)
        {
            try{
                context.DiretrizPrecificadaTabela.Add(novaTabela);
                context.SaveChanges();               
                return Json("{ sucess : true }");
            }catch (Exception ex){
                Erro("Diretriz Precificada Controller > AdicionarTabelaPost", ex);
                return Json("{ error : true }");
            }
        }

        [Authorize(Roles="Admin")]  
        public IActionResult Editar(string Id)
        {
            try{
                DiretrizPrecificada DiretrizPrec = FindDiretrizesPrec(new Guid(Id));
                DiretrizesClinicas DiretrizClin = FindDiretrizesEdit(DiretrizPrec.DiretrizesClinicasId);

                if(DiretrizPrec != null){
                    ViewBag.Titulo = DiretrizClin.Titulo;
                    return View(DiretrizPrec);
                }

                return RedirectToAction("Index", "Diretrizes", new {});

            }catch (Exception ex){
                Erro("Diretriz Precificada Controller > Exibir GET", ex);
                return RedirectToAction("Index", "Diretrizes", new {});
            }
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public IActionResult Editar(DiretrizPrecificada Diretriz)
        {
            var diretrizAtual = FindDiretrizesPrec(Diretriz.Id);

            DiretrizPrecificada novaDiretriz = new DiretrizPrecificada(){
                Id = Guid.Empty,
                Conteudo = Diretriz.Conteudo,
                DiretrizesClinicasId = Diretriz.DiretrizesClinicasId,
                DiretrizPrecificadaTabela = diretrizAtual.DiretrizPrecificadaTabela
            };

            UpdateDiretrizPrec(Diretriz.Id, novaDiretriz);
            return RedirectToAction("Exibir", "DiretrizesPrecificadas", new { Id = Diretriz.Id.ToString() });
        }

        public IActionResult Remover(string Id)
        {
            try{
                RemoveDriretrizPrec(Id);
                Alerta("Diretriz removida com sucesso.", "success");
                return RedirectToAction("Index", "Diretrizes", new { });
            }catch(Exception ex){
                Erro("Erro removendo diretriz.", ex);
                return RedirectToAction("Index", "Diretrizes", new { });
            }
        }

        public IActionResult ExcluirTabela(string Id)
        {
            try{
                var tabela = FindDiretrizesPrecTabela(new Guid(Id));
                var returnId = tabela.DiretrizPrecificadaId;
                context.DiretrizPrecificadaTabela.Remove(tabela);
                context.SaveChanges();
                return RedirectToAction("Editar", "DiretrizesPrecificadas", new { Id = returnId });
            }catch(Exception ex){
                Erro("Ocorreu um erro ao excluir a tabela, por favor contate o administrador.", ex);
                return RedirectToAction("Index", "Diretrizes", new { });
            }
        }

        [Authorize(Roles="Admin")]  
        public IActionResult EditarTabela(string Id)
        {
            return RedirectToAction("EditarTabelaJson", "DiretrizesPrecificadas", new { Id = Id });
        }

        [Authorize(Roles="Admin")]  
        public IActionResult EditarTabelaJson(string Id)
        {
            try{
                DiretrizPrecificadaTabela tabela = FindDiretrizesPrecTabela(new Guid(Id));
                DiretrizPrecificada diretriz = FindDiretrizesPrecInfo(tabela.DiretrizPrecificadaId);
                DiretrizesClinicas diretriClin = FindDiretrizesEdit(diretriz.DiretrizesClinicasId);

                if(tabela != null){
                    ViewBag.Titulo = diretriClin.Titulo;
                    ViewBag.Linha = ListaLinha();
                    ViewBag.Finalidade = ListaFinalidade();
                    ViewBag.ListaMedicamentos = ListaMedicamentos();
                    return View(tabela);
                }

                return RedirectToAction("Index", "Diretrizes", new {});

            }catch (Exception ex){
                Erro("Ocorreu um erro ao localizar a tabela, por favor entre em contato com o administrador.", ex);
                return RedirectToAction("Index", "Diretrizes", new {});
            }
        }

        [HttpPost]
        [Produces("application/json")]
        [Authorize(Roles="Admin")]  
        public JsonResult EditarTabelaJsonPost([FromBody] DiretrizPrecificadaTabela tabela)
        {
            try{
                DiretrizPrecificadaTabela tabelaNova = new DiretrizPrecificadaTabela(){
                    Id = new Guid(),
                    Index = tabela.Index,
                    Titulo = tabela.Titulo,
                    ChaveTabela = tabela.ChaveTabela,
                    ChaveTabelaReduzida = tabela.ChaveTabelaReduzida,
                    Observacoes = tabela.Observacoes,
                    Linha = tabela.Linha,
                    Finalidade = tabela.Finalidade,
                    DiretrizPrecificadaId = tabela.DiretrizPrecificadaId,
                    DiretrizPrecificadaRegistro = tabela.DiretrizPrecificadaRegistro
                };

                UpdateDiretrizPrecTabela(tabela.Id, tabelaNova);
                
                return Json("{ sucess : true }");

            }catch (Exception ex){
                Erro("Diretriz Precificada Controller > EditarTabelaJsonPost", ex);
                return Json("{ error : true }");
            }
        }

    }
}
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using alldux_plataforma.Data;
using alldux_plataforma.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace alldux_plataforma.Controllers
{
    [Authorize(Roles="Admin")]    
    public class FaqController : Controller
    {
        private ContentDbContext context;
        private UserManager<ApplicationUser> userManager;

        public FaqController(ContentDbContext _context, UserManager<ApplicationUser> _userManager)
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

        private void AdicionaFaq(Faq faq)
        {
            try{
                context.Faq.Add(faq);
                context.SaveChanges();
            }catch (Exception ex){
                Erro("Erro ao adicionar Faq " + faq.Pergunta, ex);
            }
        }
        private List<Faq> ListaFaq()
        {
            try{
                return context.Faq
                            .AsNoTracking()
                            .OrderByDescending(e => e.CreateDate)
                            .OrderByDescending(e => e.LastUpdate)
                            .ToList();
            }
            catch (Exception ex){
                Erro("Erro buscando informações de Faq.", ex);
                return null;
            }
        }
        private Faq FindFaq(Guid Id)
        {
            try{
                return context.Faq
                            .AsNoTracking()                           
                            .Where(e => e.Id == Id)
                            .Single();

            }catch (Exception ex){
                Erro("Faq não encontrado.", ex);
                return null;
            }
        }
        private void UpdateFaq(Guid Id, Faq faqNovo)
        {   
            try{
                var faqAtualizar = FindFaq(Id);
                context.Faq.Remove(faqAtualizar);
                context.SaveChanges();

                faqNovo.Id = Id;
                context.Faq.Add(faqNovo);
                context.SaveChanges();
            }catch(Exception ex){
                Erro("Erro ao editar Faq, por favor contate o administrador.", ex);
            }
        }

        public IActionResult Index()
        {
            return View(ListaFaq());
        }

        public IActionResult Adicionar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Adicionar(Faq faq)
        {
            ApplicationUser usuario = userManager.GetUserAsync(HttpContext.User).Result;

            faq.Id = new Guid();
            faq.CreateDate = DateTime.Now;
            faq.LastUpdate = DateTime.Now;
            faq.CreateId = usuario.Id.ToString();

            try
            {
                AdicionaFaq(faq);
                Alerta("Faq "+ faq.Pergunta+" adicionado com sucesso.", "success");
                return RedirectToAction("Index", "Faq", new {});
            }catch(Exception ex){
                Erro("Erro ao adicionar Faq, por favor contate o administrador.", ex);
                return View(faq);
            }
        }
        
        public IActionResult Editar(string Id)
        {
            Faq faqEditar = new Faq();

            try{
                faqEditar = FindFaq(new Guid(Id));
            }catch(Exception ex){
                Erro("Ocorreu um erro ao Editar o Faq, por favor contate o administrador.", ex);
                return RedirectToAction("Index", "Faq", new { });
            }
            
            return View(faqEditar);
        }

        [HttpPost]
        public IActionResult Editar(Faq faqEditado)
        {
            
            Faq faqEdit = new Faq(){
                Id = Guid.Empty,
                Pergunta = faqEditado.Pergunta,
                Resposta = faqEditado.Resposta,
                CreateDate = faqEditado.CreateDate,
                CreateId = faqEditado.CreateId,
                LastUpdate = DateTime.Now
            };            

            try{
                UpdateFaq(faqEditado.Id, faqEdit);
            }catch(Exception ex){
                Erro("Ocorreu um erro editando o Faq, por favor contate o administrador", ex);
                return View();
            }

            return RedirectToAction("Index", "Faq", new {  });
        }

        public IActionResult Excluir(string Id){
            try{
                var faqExcluir = context.Faq.Where(e => e.Id == new Guid(Id)).Single();
                context.Faq.Remove(faqExcluir);
                context.SaveChanges();
            }catch(Exception ex){
                Erro("Ocorreu um erro ao excluir o Faq, por favor contate o administrador: "+Id, ex);
                return RedirectToAction("Index", "Faq", new { });
            }
            
            Alerta("Faq excluido com sucesso.", "success");
            return RedirectToAction("Index", "Faq", new { });
        }
    }
}
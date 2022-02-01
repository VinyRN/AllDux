using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using alldux_plataforma.Data;
using alldux_plataforma.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace alldux_plataforma.Controllers
{
    [Authorize(Policy = "DiretrizesClinicas")]
    public class DiretrizesClinicasController : Controller
    {
        private ContentDbContext context;
        private UserManager<ApplicationUser> userManager;

        public DiretrizesClinicasController(ContentDbContext _context, UserManager<ApplicationUser> _userManager)
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

        private void UpdateDiretriz(Guid Id, DiretrizesClinicas diretrizNova)
        {   
            try{
                var diretrizAtualizar = FindDiretrizesEdit(Id);
                var diretrizPrecRelacionada = diretrizPrecRel(Id);

                 if(diretrizPrecRelacionada != null){
                    diretrizPrecRelacionada.DiretrizesClinicasId = Guid.Empty;
                }

                context.DiretrizesClinicas.Remove(diretrizAtualizar);
                context.SaveChanges();

                diretrizNova.Id = Id;
                context.DiretrizesClinicas.Add(diretrizNova);
                context.SaveChanges();

                if(diretrizPrecRelacionada != null){
                    diretrizPrecRelacionada.DiretrizesClinicasId = Id;
                    context.DiretrizPrecificadas.Add(diretrizPrecRelacionada);
                }

                context.SaveChanges();
            }catch(Exception ex){
                Erro("Ocorreu um erro ao atualizar a diretriz, por favor contate o administrador", ex);
            }
        }

        private DiretrizPrecificada diretrizPrecRel(Guid Id){
            try{
                var diretrizPrecRel = context.DiretrizPrecificadas
                                            .AsNoTracking()
                                            .Where(d => d.DiretrizesClinicasId == Id)
                                            .Include(d => d.DiretrizPrecificadaTabela)
                                            .ThenInclude(d => d.DiretrizPrecificadaRegistro)
                                            .Single();
                return diretrizPrecRel;
            }catch{
                return null;
            }
        }

        private DiretrizesClinicas FindDiretrizesEdit(Guid Id)
        {
            try{
                var Diretriz = context.DiretrizesClinicas
                    .Where(e => e.Id == Id)
                    .Include(e => e.DiretrizModulo.OrderBy(p => p.Index))
                    .ThenInclude(s => s.DiretrizSecao.OrderBy(p => p.Index))
                    // .Include(d => d.DiretrizPrecificada)
                    // .ThenInclude(d => d.DiretrizPrecificadaTabela)
                    // .ThenInclude(d => d.DiretrizPrecificadaRegistro)
                    .Single();

                return Diretriz;

            }catch (Exception ex){
                Erro("Erro. Diretriz não encontrada.", ex);
                return null;
            }
        }

        private void RemoveDriretriz(string Id)
        {
            try{
                var remove = FindDiretrizesEdit(new Guid(Id));
                context.DiretrizesClinicas.Remove(remove);
                context.SaveChanges();
                Alerta("Diretriz Removida com sucesso.", "success");
            }catch(Exception ex){
                Erro("Erro. Diretriz não encontrada.", ex);
            }
        }

        public async Task<IActionResult> Index(string Id)
        {
            List<string> protocolosLivres = new List<string>();
            protocolosLivres.Add("9ddd643d-a308-450b-aabe-99dada40635a");
            protocolosLivres.Add("52022fcc-4889-4dd3-8c85-f6ac941c6800");
            protocolosLivres.Add("696ffc1f-f0d4-4622-941b-5b2c9b8cdef8");
            protocolosLivres.Add("2a858656-37f3-41a2-9cc3-9707c5e4924f");
            protocolosLivres.Add("5ef278e8-a2bd-48cd-bf28-0120e64eb4d7"); //Mama
            protocolosLivres.Add("85316b30-6fd2-41e2-9569-467e56db1678"); //Colon
            protocolosLivres.Add("556a08a3-e742-491d-8e38-3577a44ee959"); //Pulmao

            if(!protocolosLivres.Contains(Id)){
                var appUser = await userManager.GetUserAsync(HttpContext.User);
                var appUserRoles = await userManager.GetRolesAsync(appUser);
                var appUserRole = appUserRoles.FirstOrDefault();

                if(appUserRole == "TestUser") return RedirectToAction("AcessoRestrito", "Home");
            }
            
            try{
                DiretrizesClinicas Diretriz = FindDiretrizesEdit(new Guid(Id));
                return View(Diretriz);
            }catch(Exception ex){
                Erro("Erro. Diretriz não encontrada.", ex);
                return RedirectToAction("Index", "Diretrizes", new { });
            }
        }

        [Authorize(Roles="Admin")]
        public IActionResult Editar(string Id)
        {
            try{
                DiretrizesClinicas Diretriz = FindDiretrizesEdit(new Guid(Id));
                return View(Diretriz);
            }catch(Exception ex){
                Erro("Ocorreu um erro ao editar a diretriz, por favor entre em contato com o administrador.", ex);
                return RedirectToAction("Index", "Diretrizes", new { });
            }            
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public IActionResult Editar(DiretrizesClinicas Diretriz)
        {
            try{
                DiretrizesClinicas novaDiretriz = new DiretrizesClinicas (){
                    Id = Guid.Empty,
                    Titulo = Diretriz.Titulo,
                    Lead = Diretriz.Lead,
                    Categoria = Diretriz.Categoria,
                    Tags = Diretriz.Tags,
                    CreateId = userManager.GetUserAsync(HttpContext.User).Result.Id.ToString(),
                    CreateDate = Diretriz.CreateDate,
                    LastUpdate = DateTime.Now,
                    DiretrizModulo = Diretriz.DiretrizModulo
                };

                UpdateDiretriz(Diretriz.Id, novaDiretriz);

                return RedirectToAction("Index", "DiretrizesClinicas", new { Id = Diretriz.Id.ToString() });
            }catch(Exception ex){
                Erro("Ocorreu um erro ao editar a diretriz, por favor entre em contato com o administrador.", ex);
                return RedirectToAction("Index", "Diretrizes", new { });
            }
        }

        [Authorize(Roles="Admin")]
        public IActionResult Remover(string Id)
        {   
            RemoveDriretriz(Id);
            return RedirectToAction("Index", "Diretrizes", new { });
        }
    }
}
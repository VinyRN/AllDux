using alldux_plataforma.Data;
using alldux_plataforma.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;

namespace alldux_webapp.Controllers
{
    [Authorize]
    public class DiretrizesController : Controller
    {
        private ContentDbContext context;
        private UserManager<ApplicationUser> userManager;

        public DiretrizesController(ContentDbContext _context, UserManager<ApplicationUser> _userManager){
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

        private void Alerta(string msg, string type, Exception ex){
            TempData["alerta"] = msg;
            TempData["tipo"] = type;

            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
            Console.WriteLine(type.ToUpper()+": "+msg);
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
        }

        private List<DiretrizesClinicas> ListaDiretrizes()
        {   
            try{
                List<DiretrizesClinicas> listaDiretrizes =  context.DiretrizesClinicas
                                                                        .AsNoTracking()
                                                                        .OrderByDescending(e => e.CreateDate)
                                                                        .OrderByDescending(e => e.LastUpdate)
                                                                        .ToList();
                return listaDiretrizes;
            }catch (Exception ex){
                Erro("Ocorreu um erro ao listar as diretrizes, por favor contate o administrador.", ex);
                return null;
            }
        }

        private IEnumerable<string> ListaCategorias()
        {
            try{
                IEnumerable<string> listaCategorias = context.DiretrizesClinicas
                                                .AsNoTracking()
                                                .OrderBy(e => e.Categoria)
                                                .Select(e => e.Categoria)
                                                .ToList()
                                                .Distinct();

                return listaCategorias;
            }catch (Exception ex){
                Erro("Ocorreu um erro ao listar as categorias, por favor contate o administrador.", ex);
                return null;
            }
        }

        private List<string> PossuiPrecificada(){
            try{
                var lista = context.DiretrizPrecificadas
                                .AsNoTracking()
                                .Select(e => e.Id.ToString())
                                .ToList();
                return lista;
            }catch (Exception ex){
                Erro("Ocorreu um erro ao listar as categorias, por favor contate o administrador. (2)", ex);
                return null;
            }
        }
        private bool AddDiretriz(DiretrizesClinicas diretriz){
            try{
                context.DiretrizesClinicas.Add(diretriz);
                context.SaveChanges();
                return true;
            }catch (Exception ex){
                Erro("Ocorreu um erro ao adicionar a diretriz, por favor contate o administrador.", ex);
                return false;
            }
        }

        public IActionResult Index()
        {
            List<string> protocolosLivres = new List<string>();
            protocolosLivres.Add("9ddd643d-a308-450b-aabe-99dada40635a");
            protocolosLivres.Add("52022fcc-4889-4dd3-8c85-f6ac941c6800");
            protocolosLivres.Add("696ffc1f-f0d4-4622-941b-5b2c9b8cdef8");
            protocolosLivres.Add("2a858656-37f3-41a2-9cc3-9707c5e4924f");
            protocolosLivres.Add("5ef278e8-a2bd-48cd-bf28-0120e64eb4d7"); //Mama
            protocolosLivres.Add("85316b30-6fd2-41e2-9569-467e56db1678"); //Colon
            protocolosLivres.Add("556a08a3-e742-491d-8e38-3577a44ee959"); //Pulmao

            ViewBag.protocolosLivres = protocolosLivres;
            ViewBag.categorias = ListaCategorias();
            ViewBag.precificadas = PossuiPrecificada();
            List<DiretrizesClinicas> listaDiretrizes = ListaDiretrizes();
            var appUserRoles = userManager.GetRolesAsync(userManager.GetUserAsync(HttpContext.User).Result).Result.FirstOrDefault();
            
            if(appUserRoles == "TestUser") {
                List<DiretrizesClinicas> novalista = new List<DiretrizesClinicas>();
                
                foreach(var item in listaDiretrizes){
                    if(protocolosLivres.Contains((item.Id.ToString()))){
                        novalista.Add(item);
                        //listaDiretrizes.Remove(item);
                    }
                }
                foreach(var item in novalista){
                    if(listaDiretrizes.Contains((item))){
                        listaDiretrizes.Remove(item);
                    }
                }
                
                novalista.AddRange(listaDiretrizes);
                return View(novalista);
            }

            return View(listaDiretrizes);
        }

        [Authorize(Roles="Admin")]
        public IActionResult Adicionar(){
            ViewBag.categorias = ListaCategorias();
            return View();
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public IActionResult Adicionar(DiretrizesClinicas Diretriz){
            
            ApplicationUser usuario = userManager.GetUserAsync(HttpContext.User).Result;
            Diretriz.CreateId = usuario.Id.ToString();
            Diretriz.CreateDate = DateTime.Now;
            Diretriz.LastUpdate = DateTime.Now;

            var adiciona = AddDiretriz(Diretriz);

            if(adiciona){
                return RedirectToAction("Index", "DiretrizesClinicas", new { Id = Diretriz.Id.ToString() });
            }
            
            return RedirectToAction("Index", "Diretrizes", new {});
        }

    }
}
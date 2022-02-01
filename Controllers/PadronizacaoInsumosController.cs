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
using System.Threading.Tasks;

namespace alldux_webapp.Controllers
{
    [Authorize(Policy = "PadronizacaoInsumos")]
    public class PadronizacaoInsumosController : Controller
    {
        private ContentDbContext context;
        private UserManager<ApplicationUser> userManager;

        public PadronizacaoInsumosController(ContentDbContext _context, UserManager<ApplicationUser> _userManager)
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

        private List<Medicamento> ListaMedicamentosPadronizados()
        {
            try{
                return context.Medicamentos
                            .AsNoTracking()
                            .OrderBy(e => e.PrincipioAtivo)
                            .Include(e => e.Variacoes.Where(e => e.Padronizado == true))
                            .ToList();

            }catch (Exception ex){
                Erro("Erro buscando informações de medicamentos.", ex);
                return null;
            }

        }
        private List<Medicamento> ListaMedicamentosPadronizadosTrial()
        {
            try{
                return context.Medicamentos
                            .AsNoTracking()
                            .OrderBy(e => e.PrincipioAtivo)
                            .Include(e => e.Variacoes.Where(e => e.Padronizado == true))
                            .Take(5)
                            .ToList();

            }catch (Exception ex){
                Erro("Erro buscando informações de medicamentos.", ex);
                return null;
            }

        }

        public IActionResult Index()
        {
            var appUserRoles = userManager.GetRolesAsync(userManager.GetUserAsync(HttpContext.User).Result).Result.FirstOrDefault();
            if(appUserRoles == "TestUser") {
                return View(ListaMedicamentosPadronizadosTrial());
            }

            return View(ListaMedicamentosPadronizados());
        }
    }
}
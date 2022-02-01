using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using alldux_plataforma.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace alldux_plataforma.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<ApplicationUser> userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userMgr)
        {
            _logger = logger;
            userManager = userMgr;
        }

        public async Task<IActionResult> Aceite_PoliticaPrivacidade(){
            ApplicationUser appUser = await userManager.GetUserAsync(HttpContext.User);
            
            await userManager.AddClaimAsync(appUser, new Claim("Aceite_PoliticaPrivacidade", "True"));
            await userManager.AddClaimAsync(appUser, new Claim("Aceite_PoliticaPrivacidade_Data", DateTime.Now.ToString()));

            return RedirectToAction("Logout", "Account", new { });
        }
     
        public IActionResult Index()
        {
            var appUserRoles = userManager.GetRolesAsync(userManager.GetUserAsync(HttpContext.User).Result).Result.FirstOrDefault();
            if(appUserRoles == "TestUser") {
                return RedirectToAction("IndexTrial");
            }
            return View();
        }

        public async Task<IActionResult> IndexTrial(){
            ApplicationUser appUser = await userManager.GetUserAsync(HttpContext.User);
            ViewBag.tempoRestante = Math.Round(appUser.CreatedDate.AddDays(14).Subtract(DateTime.Today).TotalDays);
            ViewBag.tempoRestanteTexto = (ViewBag.tempoRestante > 1) ? ViewBag.tempoRestante.ToString()+" dias" : ViewBag.tempoRestante.ToString()+" dia";  

            return View();
        }

        public async Task<IActionResult> PoliticaPrivacidade()
        {
            ApplicationUser appUser = await userManager.GetUserAsync(HttpContext.User);
            var claims = await userManager.GetClaimsAsync(appUser);
            foreach (var item in claims)
            {
                if(item.Type == "Aceite_PoliticaPrivacidade_Data"){
                    TempData["DataAceite"] = item.Value.ToString();
                    return View();
                }
            }
            return View();
        }
        public async Task<IActionResult> TermoResponsabilidade()
        {
            ApplicationUser appUser = await userManager.GetUserAsync(HttpContext.User);
            var claims = await userManager.GetClaimsAsync(appUser);
            foreach (var item in claims)
            {
                if(item.Type == "Aceite_PoliticaPrivacidade_Data"){
                    TempData["DataAceite"] = item.Value.ToString();
                    return View();
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AcessoRestrito()
        {
            return View();
        }
    }
}

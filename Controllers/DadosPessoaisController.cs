using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using alldux_plataforma.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using alldux_plataforma.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
//using System.Linq;
//using System.Collections.Generic;

namespace alldux_plataforma.Controllers
{
    [Authorize]
    public class DadosPessoaisController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private IPasswordHasher<ApplicationUser> passwordHasher;
        private IPasswordValidator<ApplicationUser> passwordValidator;
        private ContentDbContext context;

        public DadosPessoaisController(
            UserManager<ApplicationUser> usrMgr,
            IPasswordHasher<ApplicationUser> passwordHash,
            IPasswordValidator<ApplicationUser> passwordVal,
            ContentDbContext _context
            )
        {
            userManager = usrMgr;
            passwordHasher = passwordHash;
            passwordValidator = passwordVal;
            context = _context;

        }

        private static string ConversaoParcial(string texto) {
            int valor;
            string textoParcial = texto.Split(')', '-')[0];
            return int.TryParse(textoParcial, out valor) ? textoParcial.PadLeft(2, '0') : textoParcial.PadLeft(2, 'A');
        }

        private List<Faq> ListaFaq()
        {
            try
            {
                var listaFaq = context.Faq.AsNoTracking().OrderBy(e => e.Pergunta).ToList();
                var listaClassificada = listaFaq.OrderBy(x => ConversaoParcial(x.Pergunta)).ToList();
                return listaClassificada;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro buscando informações de Faq. "+ex);
                return new List<Faq>();
            }
        }
                
        public IActionResult Index()
        {
            ApplicationUser usuario = userManager.GetUserAsync(HttpContext.User).Result;
            
            return View(usuario); 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public async Task<IActionResult> Update(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);            

            if (user != null)                          
                return View(user);            
            else            
                return RedirectToAction("Index");            
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, string UserName, string PhoneNumber, string password)
        {            
            ApplicationUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(UserName))
                    user.UserName = UserName;
                else
                    ModelState.AddModelError("", "Nome de usuário não pode estar em branco");

                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, user, password);
                    if (validPass.Succeeded)
                        user.PasswordHash = passwordHasher.HashPassword(user, password);
                    else
                        Errors(validPass);
                }
                else                    
                    validPass = await passwordValidator.ValidateAsync(userManager, user, user.PasswordHash);

                if (!string.IsNullOrEmpty(PhoneNumber))
                    user.PhoneNumber = PhoneNumber;
                else
                    ModelState.AddModelError("", "Telefone não pode estar em branco");

                if (validPass.Succeeded)
                {
                    IdentityResult result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);                    
                }
            }
            else
                ModelState.AddModelError("", "Usuário não encontrado");

            return View(user);
        }
           
        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        public IActionResult Ajuda()
        {
            var ListaFaqBanco = ListaFaq();
            ViewBag.ListaFaqCount = ListaFaqBanco.Count;
            return View(ListaFaqBanco);
        }
        public IActionResult Contato()
        {
            return View();
        }
    }
}

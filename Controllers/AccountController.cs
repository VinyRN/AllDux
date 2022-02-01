using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using alldux_plataforma.Models;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Security.Claims;
using alldux_plataforma.Services;
using System.Collections.Generic;
using alldux_plataforma.Data;
using Microsoft.EntityFrameworkCore;

namespace alldux_plataforma.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender _emailSender;
        private ContentDbContext context;

        public AccountController(
            UserManager<ApplicationUser> userMgr, 
            SignInManager<ApplicationUser> signinMgr, 
            IEmailSender emailSender,
            ContentDbContext _context)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            _emailSender = emailSender;
            context = _context;
        }

        private void Erro(string msg, Exception ex)
        {
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
            Console.WriteLine("ERRO: " + msg);
            Console.WriteLine("----------------------------------------------------------------------------------------------------");
            Console.WriteLine(ex.Message);
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
            TempData["alerta"] = msg;
            TempData["tipo"] = "danger";
            TempData["exception"] = ex.Message;
        }
        private async Task<IList<string>> GetUserRoles(ApplicationUser user) => await userManager.GetRolesAsync(user);

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            Login login = new Login();
            login.ReturnUrl = returnUrl;
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appUser = await userManager.FindByEmailAsync(login.Email);
                if (appUser != null && appUser.Active)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUser, login.Password, false, false);

                    if (result.Succeeded){
                        //verifica se TestUser está na validade
                        var userRoles = await GetUserRoles(appUser);
                        var appUserRoles = userRoles.FirstOrDefault();
                        
                        if(appUserRoles == "TestUser"){
                            var totalDias = (int)DateTime.Today.Subtract(appUser.CreatedDate).TotalDays;
                            if(totalDias > 15){
                                await signInManager.SignOutAsync();
                                return RedirectToAction("AcessoExpirado", "Cadastro");
                            }
                        }

                        //grava data e hora do ultimo login
                        var claims = await userManager.GetClaimsAsync(appUser);
                        int loginCount = 1;

                        foreach (var item in claims)
                        {

                            if(item.Type == "Meta_LastLoginCount"){
                                loginCount = Int32.Parse(item.Value) + 1;
                                await userManager.RemoveClaimAsync(appUser, item);
                            }

                            string[] palavras = item.Type.Split(new char[]{'_'});
                            if(palavras.Contains("LastLogin")){
                                IdentityResult removeClaim = await userManager.RemoveClaimAsync(appUser, item);
                            }
                        }
                        await userManager.AddClaimAsync(appUser, new Claim("Meta_LastLogin", DateTime.Now.ToString()));
                        await userManager.AddClaimAsync(appUser, new Claim("Meta_LastLoginCount", loginCount.ToString()));
                        
                        var claimsuP = await userManager.GetClaimsAsync(appUser);
                        
                        return Redirect(login.ReturnUrl ?? "/");
                    }
                }else{
                    ModelState.AddModelError(nameof(login.Email), "Usuário inativo ou não encontrado.");
                    return View(login);
                }
                //grava log de erro de login
                ModelState.AddModelError(nameof(login.Email), "Falha no login: Usuário ou senha inválidos");
            }

            return View(login);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

         [AllowAnonymous]
        public IActionResult Contato()
        {
            return View(); 
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPassword model, EmailRequest request)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var resetURL = Url.Action("ResetPassword", "Account", new { token = token, email = model.Email }, Request.Scheme);
                    
                    request.ToEmail = model.Email;
                    request.Subject = "Confirme a sua conta";
                    request.Body = "Confirme a sua conta clicando <a href=\"" + resetURL + "\">AQUI</a>";
                    await _emailSender.SendEmailAsync(request);

                    return View("ForgotPasswordConfirmation");
                }
                else
                    ModelState.AddModelError("", "Usuário não encontrado");
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            return View(new ResetPassword { Token = token, Email = email });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user,
                        model.Token, model.Password);

                    if (!result.Succeeded)
                    {
                        foreach (var erro in result.Errors)
                        {
                            ModelState.AddModelError("", erro.Description);
                        }
                        return View();
                    }
                    
                    return View("ResetPasswordConfirmation");
                }
                ModelState.AddModelError("", "Invalid Request");
            }

            return View();
        }

        public IActionResult SendMessage()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SendMessage(EmailRequest request)
        {
            await _emailSender.SendEmailAsync(request);
            return View();
        }
    }   
}
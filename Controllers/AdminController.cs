using Microsoft.AspNetCore.Mvc;
using alldux_plataforma.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using System.Security.Claims;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using alldux_plataforma.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.IO;
using alldux_plataforma.Services;
using Microsoft.AspNetCore.Hosting;
using System.Text;

namespace alldux_plataforma.Controllers
{
    [Authorize(Roles="Admin,BrandAdmin")]
    public class AdminController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private IPasswordHasher<ApplicationUser> passwordHasher;
        private IPasswordValidator<ApplicationUser> passwordValidator;
        private IUserValidator<ApplicationUser> userValidator;
        private RoleManager<IdentityRole> roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(
            UserManager<ApplicationUser> usrMgr, 
            IPasswordHasher<ApplicationUser> passwordHash, 
            IPasswordValidator<ApplicationUser> passwordVal, 
            IUserValidator<ApplicationUser> userValid,
            IEmailSender emailSender,
            IWebHostEnvironment webHostEnvironment,
            RoleManager<IdentityRole> roleMnger
            )
        {
            userManager = usrMgr;
            passwordHasher = passwordHash;
            passwordValidator = passwordVal;
            userValidator = userValid;
            roleManager = roleMnger;
            _emailSender = emailSender;
            _webHostEnvironment= webHostEnvironment;
        }
        private async Task<ApplicationUser> GetCurrentUserAsync() => await userManager.GetUserAsync(HttpContext.User);
        private async Task<string> GetCurrentUserId()
        {
            ApplicationUser usr = await GetCurrentUserAsync();
            string retorno = usr?.Id;
            return retorno;
        }
        private async Task<string> GetCurrentUserCompany(){
            ApplicationUser usr  = await GetCurrentUserAsync();
            string retorno = usr?.Company;
            return retorno;
        }
        private async Task<IList<IdentityRole>> GetAllRoles(){
            List<IdentityRole> roles = await roleManager.Roles.ToListAsync();
            return roles;
        }
        private async Task<IEnumerable<string>> GetUserRoles(ApplicationUser user) => await userManager.GetRolesAsync(user);
        private async Task<IList<ApplicationUser>> GetUsersWithRoles(){
            var listaUsuarios = await userManager.Users.ToListAsync();

            foreach(var item in listaUsuarios){
                var userType = await userManager.GetRolesAsync(item);
                item.UserType = userType.FirstOrDefault();
            }
            return listaUsuarios;
        }

        public async Task<IActionResult> Index()
        {   
            ViewBag.Claims = User?.Claims;
            ViewBag.Roles = await GetAllRoles();
            ViewBag.Company = await GetCurrentUserCompany();
            var UsersWithRoles = await GetUsersWithRoles();
            return View(UsersWithRoles);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await GetAllRoles();  
            ViewBag.Company = await GetCurrentUserCompany();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            //Verifica se o CREATOR é MANAGER e preenche o campo EMPRESA com o mesmo valor
            ApplicationUser userCreator = await GetCurrentUserAsync();
            bool isManager = await userManager.IsInRoleAsync(userCreator, "BrandAdmin");
            string creatorCompany = await GetCurrentUserCompany();
            
            if(isManager) { user.Company = creatorCompany.ToString(); }
            
            if (ModelState.IsValid)
            {
                ApplicationUser appUser = new ApplicationUser
                {
                    UserName = user.Name,
                    Email = user.Email,
                    Company = user.Company,
                    Cargo = user.Cargo,
                    PhoneNumber = user.PhoneNumber,
                    CreatedDate = DateTime.Now,
                    CreatedId = GetCurrentUserId().Result,
                };
 
                IdentityResult result = await userManager.CreateAsync(appUser, user.Password);
                
                if (result.Succeeded){
                    //adicionar role
                    var applicationRole = await roleManager.FindByNameAsync(user.UserType);
                    if (applicationRole != null) {
                        IdentityResult roleResult = await userManager.AddToRoleAsync(appUser, applicationRole.Name);
                    }

                    //gravar Claims Iniciais
                    List<KeyValuePair<string, bool>> listaClaims = new List<KeyValuePair<string, bool>>();
                    listaClaims.Add(new KeyValuePair<string, bool>("Content_DiretrizesClinicas", !string.IsNullOrEmpty(user.Content_DiretrizesClinicas)));
                    listaClaims.Add(new KeyValuePair<string, bool>("Content_DiretrizesPrecificadas", !string.IsNullOrEmpty(user.Content_DiretrizesPrecificadas)));
                    listaClaims.Add(new KeyValuePair<string, bool>("Content_PadronizacaoInsumos", !string.IsNullOrEmpty(user.Content_PadronizacaoInsumos)));
                    listaClaims.Add(new KeyValuePair<string, bool>("Content_Negociacoes", !string.IsNullOrEmpty(user.Content_Negociacoes)));
                    listaClaims.Add(new KeyValuePair<string, bool>("Content_FerramentasAnalises", !string.IsNullOrEmpty(user.Content_FerramentasAnalises)));
                    
                    foreach(KeyValuePair<string, bool> item in listaClaims){
                        await userManager.AddClaimAsync(appUser, new Claim(item.Key, item.Value.ToString()));
                    }

                    //se der erro exclui o usuario criado e volta pra view
                    return RedirectToAction("Index");
                }else{
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            ViewBag.Roles = await GetAllRoles();
            return View(user);
        }

        public async Task<IActionResult> Update(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                ViewBag.Roles = await GetAllRoles();
                var UserRoles = await GetUserRoles(user);
                user.UserType = UserRoles.FirstOrDefault();
                return View(user);
            }else{
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(
            string id, 
            string email, 
            string PhoneNumber, 
            string password, 
            string Company, 
            string UserName,
            string UserType,
            string Content_DiretrizesClinicas,
            string Content_DiretrizesPrecificadas,
            string Content_PadronizacaoInsumos,
            string Content_Negociacoes,
            string Content_FerramentasAnalises
            )
        {
            //Verifica se o CREATOR é MANAGER e preenche o campo EMPRESA com o mesmo valor
            ApplicationUser userCreator = await GetCurrentUserAsync();
            bool isManager = await userManager.IsInRoleAsync(userCreator, "BrandAdmin");
            string creatorCompany = await GetCurrentUserCompany();
            if (isManager) { Company = creatorCompany.ToString(); }

            ApplicationUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                if(!string.IsNullOrEmpty(UserName))
                    user.UserName = UserName;
                else
                    ModelState.AddModelError("", "Nome de usuário não pode estar em branco");

                IdentityResult validEmail = null;
                if (!string.IsNullOrEmpty(email))
                {
                    validEmail = await userValidator.ValidateAsync(userManager, user);
                    if (validEmail.Succeeded)
                        user.Email = email;
                    else
                        Errors(validEmail);
                }
                else
                    ModelState.AddModelError("", "E-mail não pode estar em branco");
        
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
                //    ModelState.AddModelError("", "Senha não pode estar em branco");
                    validPass = await passwordValidator.ValidateAsync(userManager, user, user.PasswordHash);

                if(!string.IsNullOrEmpty(Company))
                    user.Company = Company;
                else
                    ModelState.AddModelError("", "Empresa não pode estar em branco");

                if(!string.IsNullOrEmpty(PhoneNumber))
                    user.PhoneNumber = PhoneNumber;
                else
                    ModelState.AddModelError("", "Telefone não pode estar em branco");

                if(!string.IsNullOrEmpty(UserType))
                    user.UserType = UserType;
                else
                    ModelState.AddModelError("", "Ocorreu um problema com o Tipo do Usuário.");

        
                if (validEmail != null && validPass != null && validEmail.Succeeded && validPass.Succeeded && !string.IsNullOrEmpty(Company))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var applicationRole = roleManager.FindByNameAsync(user.UserType)?.Result;
                        if (applicationRole != null) {
                            var AllRoles = await GetAllRoles();
                            foreach (var item in AllRoles)
                            {
                                await userManager.RemoveFromRoleAsync(user, item.ToString());
                            }
                            IdentityResult roleResult = await userManager.AddToRoleAsync(user, user.UserType);
                        }

                        //deleta as claims de conteudo
                        var claims = userManager.GetClaimsAsync(user).Result;

                        foreach (var item in claims)
                        {
                            string[] palavras = item.Type.Split(new char[]{'_'});
                            if(palavras.Contains("Content")){
                                await userManager.RemoveClaimAsync(user, item);
                            }
                        }

                        //grava as novas claims
                        List<KeyValuePair<string, bool>> listaClaims = new List<KeyValuePair<string, bool>>();
                        listaClaims.Add(new KeyValuePair<string, bool>("Content_DiretrizesClinicas", !string.IsNullOrEmpty(Content_DiretrizesClinicas)));
                        listaClaims.Add(new KeyValuePair<string, bool>("Content_DiretrizesPrecificadas", !string.IsNullOrEmpty(Content_DiretrizesPrecificadas)));
                        listaClaims.Add(new KeyValuePair<string, bool>("Content_PadronizacaoInsumos", !string.IsNullOrEmpty(Content_PadronizacaoInsumos)));
                        listaClaims.Add(new KeyValuePair<string, bool>("Content_Negociacoes", !string.IsNullOrEmpty(Content_Negociacoes)));
                        listaClaims.Add(new KeyValuePair<string, bool>("Content_FerramentasAnalises", !string.IsNullOrEmpty(Content_FerramentasAnalises)));
                        
                        foreach(KeyValuePair<string, bool> item in listaClaims){
                            await userManager.AddClaimAsync(user, new Claim(item.Key, item.Value.ToString()));
                        }

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Errors(result);
                    }
                }
            }
            else
                ModelState.AddModelError("", "Usuário não encontrado");

            ViewBag.Roles = GetAllRoles();
            var userType = await userManager.GetRolesAsync(user);
            user.UserType = userType.FirstOrDefault();
            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "Usuário não encontrado");
            return View("Index", userManager.Users);
        }
 
        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        //Trial
        public async Task<IActionResult> Trial()
        {
            ViewBag.Claims = User?.Claims;
            ViewBag.Roles = await GetAllRoles();
            ViewBag.Company = await GetCurrentUserCompany();
            var UsersWithRoles = await GetUsersWithRoles();
            return View(UsersWithRoles);
        }

        [HttpPost]
        public async Task<IActionResult> Trial(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            user.CreatedDate = DateTime.Now;
            IdentityResult result = await userManager.UpdateAsync(user);
            if (result.Succeeded){
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "Emails/email_boasVindas.html");
                string text = System.IO.File.ReadAllText(path, Encoding.UTF8);
                text = text.Replace("##NOME##", user.UserName.ToString())
                           .Replace("##prazo##", user.CreatedDate.AddDays(15).ToString())
                           .Replace("##login##", user.Email);
                
                EmailRequest EmailBoasVindas = new EmailRequest();
                EmailBoasVindas.ToEmail = user.Email;
                EmailBoasVindas.Subject = $"Bem vindo {user.UserName} a Alldux Plataforma";
                EmailBoasVindas.Body = text;
                await _emailSender.SendEmailAsync(EmailBoasVindas);

                TempData["alerta"] = "E-mail enviado com sucesso.";
                TempData["tipo"] = "success";
            }else{
                TempData["alerta"] = "Falha ao enviar E-mail.";
                TempData["tipo"] = "danger";
            }

            ViewBag.Claims = User?.Claims;
            ViewBag.Roles = await GetAllRoles();
            ViewBag.Company = await GetCurrentUserCompany();
            var UsersWithRoles = await GetUsersWithRoles();
            
            return View(UsersWithRoles);
        }

        [HttpPost]
        [Produces("application/json")]
        [Authorize(Roles="Admin")]
        public async Task<JsonResult> UserActive([FromBody] ApplicationUser user){
            if(user != null){
                try{
                    var userChange = await userManager.FindByIdAsync(user.Id);
                    userChange.Active = user.Active;
                    await userManager.UpdateAsync(userChange);
                    return Json("{ sucess : true }");
                }catch{
                    return Json("{ error : true }");
                }
            }else{
                return Json("{ error : true }");
            }
        }
    }
}
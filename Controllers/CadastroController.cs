using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using alldux_plataforma.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using System;
using System.Web;
using alldux_plataforma.Services;
using System.Collections.Generic;
using alldux_plataforma.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text;

namespace alldux_plataforma.Controllers
{
    public class CadastroController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private RoleManager<IdentityRole> roleManager;
        private ContentDbContext context;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public CadastroController(
            UserManager<ApplicationUser> userMgr, 
            SignInManager<ApplicationUser> signinMgr, 
            ContentDbContext _context,
            IEmailSender emailSender,
            IWebHostEnvironment webHostEnvironment,
            RoleManager<IdentityRole> roleMnger)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            context = _context;
            _emailSender = emailSender;
            _webHostEnvironment= webHostEnvironment;
            roleManager = roleMnger;
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

        private Task<ApplicationUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);
        private async Task<string> GetCurrentUserId()
        {
            ApplicationUser usr = await GetCurrentUserAsync();
            string retorno = usr?.Id;
            return retorno;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(User user)
        {
            //Validando os dados do formulario
            if (string.IsNullOrEmpty(user.Name)) ModelState.AddModelError("Name", "O nome é obrigatório");

            if (string.IsNullOrEmpty(user.Email))
                ModelState.AddModelError("Email", "O email é obrigatório");
            else{   
                if (!Regex.Match(user.Email, @"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$").Success) 
                    ModelState.AddModelError("Email", "O email é inválido.");
            }

            if (string.IsNullOrEmpty(user.PhoneNumber)) ModelState.AddModelError("PhoneNumber", "O telefone é obrigatório");
            if (string.IsNullOrEmpty(user.Company)) ModelState.AddModelError("Company", "A empresa é obrigatória");
            if (string.IsNullOrEmpty(user.Cargo)) ModelState.AddModelError("Cargo", "O cargo é obrigatório");
            // if (string.IsNullOrEmpty(user.NomeGestor)) ModelState.AddModelError("NomeGestor", "O nome do gestor é obrigatório");
            // if (string.IsNullOrEmpty(user.TelGestor)) ModelState.AddModelError("TelGestor", "O celular do gestor é obrigatório");
            if (string.IsNullOrEmpty(user.Password)) ModelState.AddModelError("Password", "A senha é obrigatória");
            if (user.Password != user.PasswordCheck) ModelState.AddModelError("Password", "As senhas não coincidem");
            if (!user.aceitesValues) ModelState.AddModelError("aceitesValues", "É preciso aceitar a Política de Privacidade e concordar com o Termo de Responsabilidade");

            if(!ModelState.IsValid){
                user.Password = "";
                return View(user);
            }

            //cadastra o usuario
            ApplicationUser appUser = new ApplicationUser
            {
                UserName = user.Name,
                Email = user.Email,
                Company = user.Company,
                Cargo = user.Cargo,
                NomeGestor = user.NomeGestor,
                TelGestor = user.TelGestor,
                PhoneNumber = user.PhoneNumber,
                CreatedDate = DateTime.Now,
                CreatedId = "Cadastro feito pela plataforma",
                UserType = "TestUser",
                Active = false
            };

            IdentityResult result = await userManager.CreateAsync(appUser, user.Password);

            if (result.Succeeded){

                //adicionar role
                var applicationRole = await roleManager.FindByNameAsync(appUser.UserType);
                if (applicationRole != null) {
                    IdentityResult roleResult = await userManager.AddToRoleAsync(appUser, appUser.UserType);
                }

                //gravar Claims Iniciais
                List<KeyValuePair<string, bool>> listaClaims = new List<KeyValuePair<string, bool>>();
                listaClaims.Add(new KeyValuePair<string, bool>("Content_DiretrizesClinicas", true));
                listaClaims.Add(new KeyValuePair<string, bool>("Content_DiretrizesPrecificadas", true));
                listaClaims.Add(new KeyValuePair<string, bool>("Content_PadronizacaoInsumos", true));
                listaClaims.Add(new KeyValuePair<string, bool>("Content_Negociacoes", true));
                listaClaims.Add(new KeyValuePair<string, bool>("Content_FerramentasAnalises", true));
                listaClaims.Add(new KeyValuePair<string, bool>("Aceite_PoliticaPrivacidade", true));
                listaClaims.Add(new KeyValuePair<string, bool>("Aceite_TermoResponsabilidade", true));
                
                foreach(KeyValuePair<string, bool> item in listaClaims){
                    await userManager.AddClaimAsync(appUser, new Claim(item.Key, item.Value.ToString()));
                }

                await userManager.AddClaimAsync(appUser, new Claim("Aceite_PoliticaPrivacidade_Data", DateTime.Now.ToString()));
                await userManager.AddClaimAsync(appUser, new Claim("Aceite_TermoResponsabilidade_Data", DateTime.Now.ToString()));

                TempData["UserName"] = user.Name.Split(' ').First();
                
                //enviar email flaviane
                EmailRequest request = new EmailRequest();
                request.ToEmail = "plataforma@alldux.com.br";
                request.Subject = $"Novo usuário Trial cadastrado na Plataforma: {user.Name}";
                request.Body = $"Novo usuário cadastrado na plataforma: <br /><br /> <b>{user.Name} - {user.Email}</b>. <br /><br /> Para liberar, <a href='https://alldux-plataforma-trial.azurewebsites.net/Admin/Trial'>clique aqui</a>";
                await _emailSender.SendEmailAsync(request);

                return RedirectToAction("BemVindo");

            }else{
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            return View();
        }
        
        public async Task<IActionResult> AcessoExpirado()
        {
            var user = await GetCurrentUserAsync();
            user.Active = false;
            IdentityResult result = await userManager.UpdateAsync(user);
            if (result.Succeeded){
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "Emails/AcessoExpirado.html");
                string text = System.IO.File.ReadAllText(path, Encoding.UTF8);
                text = text.Replace("##NOME##", user.UserName.ToString());
                
                EmailRequest EmailExpirado = new EmailRequest();
                EmailExpirado.ToEmail = user.Email;
                EmailExpirado.Subject = $"{user.UserName} seu tempo como Trial na Alldux Plataforma chegou ao fim.";
                EmailExpirado.Body = text;
                await _emailSender.SendEmailAsync(EmailExpirado);

            }
            
            return View();
        }

        public IActionResult PoliticaPrivacidade()
        {
            return View();
        }

        public IActionResult TermoResponsabilidade()
        {
            return View();
        }
        public IActionResult BemVindo()
        {
            return View();
        }



    }
}
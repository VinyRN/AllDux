using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using alldux_plataforma.Data;
using alldux_plataforma.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;

using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;


namespace alldux_plataforma.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FornecedoresAnexosController : Controller
    {
        private ContentDbContext context;

        private UserManager<ApplicationUser> userManager;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public FornecedoresAnexosController(ContentDbContext _context, UserManager<ApplicationUser> _userManager, IWebHostEnvironment hostingEnvironment)
        {
            context = _context;
            userManager = _userManager;
            _hostingEnvironment = hostingEnvironment;
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
        private void Alerta(string msg, string type)
        {
            TempData["alerta"] = msg;
            TempData["tipo"] = type;

            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
            Console.WriteLine(type.ToUpper() + ": " + msg);
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
        }
        
        private void AdicionarAnexo(FornecedoresAnexos ObjFornecedoresAnexo, IFormFile ObjArquivo)
        {
            try
            {
                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //Faz UPLOAD do Arquivo
                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                string PathUpload = Path.Combine(_hostingEnvironment.ContentRootPath, "PDF\\" + ObjFornecedoresAnexo.CNPJ);

                if (!Directory.Exists(PathUpload))
                {
                    Directory.CreateDirectory(PathUpload);
                }

                Guid IdAnexo = Guid.NewGuid();
                string Extensao = Path.GetExtension(ObjArquivo.FileName);
                string NomeFileNew = IdAnexo.ToString() + Extensao;


                if (ObjArquivo.Length > 0)
                {
                    string FilePathServer = Path.Combine(PathUpload, NomeFileNew);
                    using (Stream fileStream = new FileStream(FilePathServer, FileMode.Create, FileAccess.ReadWrite))
                    {
                        ObjArquivo.CopyTo(fileStream);
                    }
                }
                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                ObjFornecedoresAnexo.IdAnexoFornecedor = IdAnexo;
                ObjFornecedoresAnexo.anexo = NomeFileNew;
                ObjFornecedoresAnexo.CreatedDate = DateTime.Now;
                ObjFornecedoresAnexo.CreatedUser = new Guid(userManager.GetUserAsync(HttpContext.User).Result.Id);

                context.FornecedoresAnexos.Add(ObjFornecedoresAnexo);
                context.SaveChanges();

            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro ao adicionar anexo para o Fornecedor, por favor contate o administrador: ", ex);                
            }
        }
        private FornecedoresAnexos FindFornecedoresAnexos(Guid Id)
        {
            try
            {
                FornecedoresAnexos ObjAnexo = context.FornecedoresAnexos
                                              .AsNoTracking()
                                              .Where(e => e.IdAnexoFornecedor == Id)
                                              .Single();

                return ObjAnexo;
            }
            catch (Exception ex) 
            {

                Erro("Ocorreu pesquisar o fornecedor " + Id.ToString() + " , por favor contate o administrador: ", ex);
                return null;
            }
        }
        private void Excluir(FornecedoresAnexos ObjFornecedoresAnexo)
        {
            try
            {
                context.FornecedoresAnexos.Remove(ObjFornecedoresAnexo);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro ao excluir anexo do Fornecedor, por favor contate o administrador: ", ex);
            }

        }
        private FornecedoresAnexos InitAnexar(Guid Id)
        {
            try
            {
                //Monda lista com os estados da federacao
                List<TiposDocFornecedores> ObjListTiposDoc = new List<TiposDocFornecedores>();

                ObjListTiposDoc = context.TiposDocFornecedores
                                    .AsNoTracking()
                                    .OrderBy(e => e.DescrDocumento)
                                    .ToList();

                ViewBag.ListaTiposDocs = ObjListTiposDoc;

                Fornecedores ObjFornecedor = new Fornecedores();
                ObjFornecedor = context.Fornecedores
                                .AsNoTracking()
                                .Include(e => e.Anexos.OrderBy(a => a.CreatedDate))
                                .Where(e => e.IdFornecedor == Id)
                                .SingleOrDefault();

                FornecedoresAnexos ObjFornecedorAnexo = new FornecedoresAnexos();
                ObjFornecedorAnexo.CNPJ = ObjFornecedor.CNPJ;
                ObjFornecedorAnexo.NomeFantasia = ObjFornecedor.NomeFantasia;
                ObjFornecedorAnexo.IdFornecedor = ObjFornecedor.IdFornecedor;

                ViewBag.ListaAnexo = ObjFornecedor.Anexos;

                return ObjFornecedorAnexo;
            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro ao anexar documento para Fornecedor, por favor contate o administrador: ", ex);
                return null;
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Anexo(Guid Id)
        {
            try
            {
                FornecedoresAnexos ObjFornecedorAnexo = InitAnexar(Id);

                if (ObjFornecedorAnexo != null)
                {
                    return View("Index", ObjFornecedorAnexo);
                }
                else
                {
                    return RedirectToAction("Index", "Fornecedores", new { });
                }
                
            }
            catch (Exception ex)
            {

                Erro("Ocorreu um erro ao anexar documento para Fornecedor, por favor contate o administrador: ", ex);
                return RedirectToAction("Index", "Fornecedores", new { });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Adicionar(FornecedoresAnexos ObjFornecedoresAnexo, IFormFile arquivo)
        {

            try
            {

                AdicionarAnexo(ObjFornecedoresAnexo, arquivo);

                FornecedoresAnexos ObjForAnexo = InitAnexar(ObjFornecedoresAnexo.IdFornecedor);

                if (ObjForAnexo != null)
                {
                    return View("Index", ObjForAnexo);
                }
                else
                {
                    return RedirectToAction("Index", "Fornecedores", new { });
                }

                
            }
            catch (Exception ex)
            {

                Erro("Ocorreu um erro ao adicionar Fornecedor, por favor contate o administrador: ", ex);
                return RedirectToAction("Index", "Fornecedores", new { });
            }

        }

        public IActionResult Adicionar(Guid Id)
        {

            try
            {
                FornecedoresAnexos ObjForAnexo = InitAnexar(Id);

                if (ObjForAnexo != null)
                {
                    return View("Adicionar", ObjForAnexo);
                }
                else
                {
                    return RedirectToAction("Index", "Fornecedores", new { });
                }


            }
            catch (Exception ex)
            {

                Erro("Ocorreu um erro ao adicionar Fornecedor, por favor contate o administrador: ", ex);
                return RedirectToAction("Index", "Fornecedores", new { });
            }

        }

        public FileResult ViewAnexo(Guid Id)
        {

            try
            {
                FornecedoresAnexos ObjAnexo = context.FornecedoresAnexos
                                              .AsNoTracking()
                                              .Include(e => e.Fornecedor)
                                              .Where(e => e.IdAnexoFornecedor == Id)
                                              .SingleOrDefault();

                if (ObjAnexo != null)
                {
                    string PathView = Path.Combine(_hostingEnvironment.ContentRootPath, "PDF\\" + ObjAnexo.Fornecedor.CNPJ);

                    string ReportURL = PathView + "\\" + ObjAnexo.anexo;
                    byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
                    return File(FileBytes, "application/pdf");
                }
                else
                {
                    Alerta("Dados do Anexo " + Id.ToString() +   " não localizado.", "danger");
                    return null;
                }

            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro ao visualizar documento PDF, por favor contate o administrador: ", ex);
                return null;
                
            }
        }

        public IActionResult Excluir(Guid Id)
        {
            try
            {

                FornecedoresAnexos ObjFornecedorAnexo = FindFornecedoresAnexos(Id);

                Excluir(ObjFornecedorAnexo);

                ObjFornecedorAnexo = InitAnexar(ObjFornecedorAnexo.IdFornecedor);

                if (ObjFornecedorAnexo != null)
                {
                    return View("Index", ObjFornecedorAnexo);
                }
                else
                {
                    return RedirectToAction("Index", "Fornecedores", new { });
                }

            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro ao deletar anexco " + Id.ToString()  + " , por favor contate o administrador: ", ex);
                return RedirectToAction("Index", "Fornecedores", new { });
            }
        }
    }
}

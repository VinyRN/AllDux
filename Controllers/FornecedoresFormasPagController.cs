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
    public class FornecedoresFormasPagController : Controller
    {
        private ContentDbContext context;

        private UserManager<ApplicationUser> userManager;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public FornecedoresFormasPagController(ContentDbContext _context, UserManager<ApplicationUser> _userManager, IWebHostEnvironment hostingEnvironment)
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

        private Fornecedores FindFornecedor(Guid Id)
        {
            try
            {
                Fornecedores ObjFornecedor = new Fornecedores();

                ObjFornecedor = context.Fornecedores
                                .AsNoTracking()
                                .Include(e => e.FormasPagamento)
                                .Where(e => e.IdFornecedor == Id)
                                .SingleOrDefault();

                if (ObjFornecedor != null)
                {
                    IList<FornecedoresFormasPag> ObjListFormaPagTemp = new List<FornecedoresFormasPag>();
                    foreach (FornecedoresFormasPag ObjItem in ObjFornecedor.FormasPagamento)
                    {
                        FormasPagamento ObjFormaPag = context.FormasPagamento
                                                        .AsNoTracking()
                                                        .Where(e => e.IdFormaPagamento == ObjItem.IdFormaPagamento)
                                                        .SingleOrDefault();
                        if (ObjFormaPag != null)
                        {
                            ObjItem.Descricao = ObjFormaPag.DescrPagamento;
                            ObjListFormaPagTemp.Add(ObjItem);
                        }
                    }

                    ObjFornecedor.FormasPagamento = ObjListFormaPagTemp;
                    return ObjFornecedor;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                Erro("Erro buscando informações de fornecedor.", ex);
                return null;
            }


        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FormasPagamento(Guid Id)
        {
            try
            {
                Fornecedores ObjFornecedor = new Fornecedores();
                ObjFornecedor = FindFornecedor(Id);

                if ((ObjFornecedor.FormasPagamento != null) && (ObjFornecedor.FormasPagamento.Count > 0))
                {
                    return View("Index", ObjFornecedor);
                }
                else
                {
                    List<FormasPagamento> ObjListFormaPag = new List<FormasPagamento>();
                    ObjListFormaPag = context.FormasPagamento
                                     .AsNoTracking()
                                     .OrderBy(e => e.DescrPagamento)
                                     .ToList();

                    ViewBag.ListaFormasPag = ObjListFormaPag;

                    return View("Adicionar");
                }

            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro ao consultar as formas de pagamento do fornecedor, por favor contate o administrador: ", ex);
                return RedirectToAction("Index", "Fornecedores", new { });
            }
            
        }

    }
}

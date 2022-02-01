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

namespace alldux_plataforma.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FornecedoresController : Controller
    {
        private ContentDbContext context;
        private UserManager<ApplicationUser> userManager;
        private Guid IdLoginUser;

        public FornecedoresController(ContentDbContext _context, UserManager<ApplicationUser> _userManager)
        {
            context = _context;
            userManager = _userManager;
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

        private List<Fornecedores> FindFornecedores(string CNPJ, string NomeFantasia)
        {
            try
            {
                
                List<Fornecedores> ObjList = new List<Fornecedores>();

                if ( (!string.IsNullOrEmpty(CNPJ)) && (!string.IsNullOrEmpty(NomeFantasia)))
                {
                    ObjList = context.Fornecedores
                            .AsNoTracking()
                            .Include( e => e.Produtos)
                            .Where(e => e.NomeFantasia.Contains(NomeFantasia) && e.CNPJ == CNPJ)
                            .OrderBy(e => e.NomeFantasia)
                            .ToList();
                }
                else if ((!string.IsNullOrEmpty(CNPJ)) && (string.IsNullOrEmpty(NomeFantasia)))
                {
                    ObjList = context.Fornecedores
                            .AsNoTracking()
                            .Include(e => e.Produtos)
                            .Where(e => e.CNPJ == CNPJ)
                            .OrderBy(e => e.NomeFantasia)
                            .ToList();
                }
                else if ((string.IsNullOrEmpty(CNPJ)) && (!string.IsNullOrEmpty(NomeFantasia)))
                {
                    ObjList = context.Fornecedores
                            .AsNoTracking()
                            .Include(e => e.Produtos)
                            .Where(e => e.NomeFantasia.Contains(NomeFantasia))
                            .OrderBy(e => e.NomeFantasia)
                            .ToList();
                }


                if (ObjList != null && ObjList. Count > 0)
                {

                    return ObjList;
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
        private Fornecedores FindFornecedor(Guid Id)
        {
            try
            {

                Fornecedores ObjEnt = new Fornecedores();

                ObjEnt = context.Fornecedores
                                .AsNoTracking()
                                .Where(e => e.IdFornecedor == Id)
                                .SingleOrDefault();

                return ObjEnt;
            }
            catch (Exception ex)
            {
                Erro("Erro buscando informações do fornecedor " + Id.ToString() , ex);
                return null;
            }
        }
        private void EditarFornecedor(Fornecedores ObjFornecedor)
        {
            try
            {
                if (ObjFornecedor != null)
                {

                    ObjFornecedor.LastUpdate = DateTime.Now;
                    ObjFornecedor.LastUpdateUser = new Guid(userManager.GetUserAsync(HttpContext.User).Result.Id);

                    context.Fornecedores.Update(ObjFornecedor);
                    context.SaveChanges();
                }
                else
                {
                    Alerta("Erro ao editar fornecedor " + ObjFornecedor.IdFornecedor, "danger");
                }

                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            }
            catch (Exception ex)
            {
                Erro("Erro ao editar fornecedor" + ObjFornecedor.IdFornecedor, ex);

            }

        }
        private void AdicionarFornecedor(Fornecedores ObjFornecedor)
        {
            try
            {
                if (ObjFornecedor != null)
                {

                    ObjFornecedor.IdFornecedor = Guid.NewGuid();
                    ObjFornecedor.CreatedDate = DateTime.Now;
                    ObjFornecedor.CreatedUser = new Guid(userManager.GetUserAsync(HttpContext.User).Result.Id);

                    context.Fornecedores.Add(ObjFornecedor);
                    context.SaveChanges();
                }
                else
                {
                    Alerta("Erro ao adicionar fornecedor " + ObjFornecedor.IdFornecedor, "danger");
                }

                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            }
            catch (Exception ex)
            {
                Erro("Erro ao adicionar fornecedor" + ObjFornecedor.IdFornecedor, ex);

            }

        }
        private bool IsFornecedor()
        {
            try
            {
                //Pega o Codigo do user logado
                Guid UserId = new Guid(userManager.GetUserAsync(HttpContext.User).Result.Id);

                //Verifica se é um fornecedor
                Fornecedores ObjFornecedor = new Fornecedores();

                ObjFornecedor = context.Fornecedores
                                .AsNoTracking()
                                .Where(e => e.IdFornecedor == UserId)
                                .SingleOrDefault();

                if (ObjFornecedor != null)
                {
                    IdLoginUser = ObjFornecedor.IdFornecedor;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;                
            }

        }
        private List<FornecedoresProduto> FindListaProdutoFornecedor(Guid Id)
        {
            try
            {
                List<FornecedoresProduto> ObjListProduto = new List<FornecedoresProduto>();

                ObjListProduto = context.FornecedoresProduto
                                    .AsNoTracking()
                                    .Where( e => e.IdFornecedor == Id)
                                    .ToList();

                if ((ObjListProduto != null) && (ObjListProduto.Count > 0))
                {
                    List<FornecedoresProduto> ObjListProdutoView = new List<FornecedoresProduto>();

                    foreach (FornecedoresProduto ObjProdForItem in ObjListProduto)
                    {
                        MedicamentoVariacao ObjMedVar = new MedicamentoVariacao();

                        ObjMedVar = context.MedicamentoVariacao
                                    .AsNoTracking()
                                    .Include( e => e.Medicamento)
                                    .Where(e => e.Id == ObjProdForItem.MedicamentoVariacaoId)
                                    .SingleOrDefault();

                        if (ObjMedVar != null)
                        {
                            string lstrApresentacao = "";

                            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                            //Rotina para pegar a Apresentação do Documento
                            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                            //Pega Apresentacao BrasindicePF
                            BrasindicePF ObjBrasPF = new BrasindicePF();
                            ObjBrasPF = context.BrasindicePF
                                        .AsNoTracking()
                                        .Where(e => e.TISS == ObjMedVar.TISS)
                                        .SingleOrDefault();

                            if (ObjBrasPF != null)
                            {
                                lstrApresentacao = ObjBrasPF.ApresentacaoNome;
                            }
                            else
                            {
                                //Pega Apresentacao BrasindicePMC
                                BrasindicePMC ObjBrasPMC = new BrasindicePMC();
                                ObjBrasPMC = context.BrasindicePMC
                                            .AsNoTracking()
                                            .Where(e => e.TISS == ObjMedVar.TISS)
                                            .SingleOrDefault();

                                if (ObjBrasPMC != null)
                                {
                                    lstrApresentacao = ObjBrasPMC.ApresentacaoNome;
                                }
                                else
                                {
                                    lstrApresentacao = ObjMedVar.UnApresentacao + " " + ObjMedVar.UnApresentacaoTipo + "/" + ObjMedVar.UnMedida + " " + ObjMedVar.UnMedidaTipo;
                                }
                            }
                            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                            ObjProdForItem.PrincipioAtivo = ObjMedVar.Medicamento.PrincipioAtivo;
                            ObjProdForItem.Nome = ObjMedVar.Nome;
                            ObjProdForItem.Apresentacao = lstrApresentacao;

                            ObjListProdutoView.Add(ObjProdForItem);
                        }
                        else
                        {
                            Alerta("Erro ao buscar dados do produto ", "danger");
                        }

                        
                    }

                    return ObjListProdutoView;
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

            try
            {
                //Guid UserId = new Guid(userManager.GetUserAsync(HttpContext.User).Result.Id);
                //Fornecedores ObjFornecedor = FindFornecedor(UserId);

                if (IsFornecedor())
                {
                    //Monda lista com os estados da federacao
                    List<EstadosFederacao> ObjListEstados = new List<EstadosFederacao>();

                    ObjListEstados = context.EstadosFederacao
                                        .AsNoTracking()
                                        .OrderBy(e => e.IdUf)
                                        .ToList();

                    ViewBag.ListaEstados = ObjListEstados;

                    //Monda lista com os paises do mundo
                    List<Paises> ObjListPais = new List<Paises>();

                    ObjListPais = context.Paises
                                  .AsNoTracking()
                                  .OrderBy(e => e.NomePais)
                                  .ToList();

                    ViewBag.ListaPaises = ObjListPais;

                    return Editar(IdLoginUser);
                }
                else
                {
                    return View();
                }

            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Fornecedores(string CNPJ, string NomeFantasia)
        {
            try
            {
                List<Fornecedores> ObjListFornecedor = new List<Fornecedores>();
                ObjListFornecedor = FindFornecedores(CNPJ, NomeFantasia);

                if ((ObjListFornecedor != null) && (ObjListFornecedor.Count > 0))
                {
                    return View(ObjListFornecedor);
                }
                else
                {
                    //Alerta("Fornecedor não encontrado para o filtro informado.", "danger");
                    return RedirectToAction("Adicionar", "Fornecedores", new { });
                }                
            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro ao consultar o Fornecedor, por favor contate o administrador: ", ex);
                return RedirectToAction("Index", "Fornecedores", new { });
            }

        }       
        public IActionResult Fornecedores(Guid Id = new Guid())
        {
            try
            {

                string CNPJ = "";
                string NomeFantasia = "";

                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //Trata variavel temporaria com os valores para remontar a tela.
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                TempData.TryGetValue("Fornecedor", out object ObjProdTemp);
                
                Fornecedores ObjFornecedor = new Fornecedores();

                if (ObjProdTemp != null)
                {
                    ObjFornecedor = JsonConvert.DeserializeObject<Fornecedores>((string)ObjProdTemp);
                }
                else
                {
                    ObjFornecedor = FindFornecedor(Id);
                }                

                if (ObjFornecedor != null)
                {
                    CNPJ = ObjFornecedor.CNPJ;
                    NomeFantasia = ObjFornecedor.NomeFantasia;

                }
                else
                {
                    return View();
                }
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                List<Fornecedores> ObjListFornecedor = new List<Fornecedores>();
                ObjListFornecedor = FindFornecedores(CNPJ, NomeFantasia);

                if ((ObjListFornecedor != null) && (ObjListFornecedor.Count > 0))
                {
                    return View(ObjListFornecedor);
                }
                else
                {
                    Alerta("Fornecedor não encontrado para o filtro informado.", "danger");
                    return RedirectToAction("Index", "Fornecedores", new { });
                }
            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro ao consultar o Fornecedor, por favor contate o administrador: ", ex);
                return RedirectToAction("Index", "Fornecedores", new { });
            }

        }
        public IActionResult Editar(Guid Id)
        {
            try
            {
                //Monda lista com os estados da federacao
                List<EstadosFederacao> ObjListEstados = new List<EstadosFederacao>();

                ObjListEstados = context.EstadosFederacao
                                    .AsNoTracking()
                                    .OrderBy(e => e.IdUf)
                                    .ToList();

                ViewBag.ListaEstados = ObjListEstados;

                //Monda lista com os paises do mundo
                List<Paises> ObjListPais = new List<Paises>();

                ObjListPais = context.Paises
                              .AsNoTracking()
                              .OrderBy(e => e.NomePais)
                              .ToList();

                ViewBag.ListaPaises = ObjListPais;

                Fornecedores ObjFornecedor = FindFornecedor(Id);

                if (ObjFornecedor != null)
                {
                    return View("Editar", ObjFornecedor);
                }
                else
                {
                    return View();
                }
                
            }
            catch (Exception ex)
            {

                Erro("Ocorreu um erro ao editar Fornecedor, por favor contate o administrador: ", ex);
                return RedirectToAction("Index", "Fornecedores", new { });
            }
            
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Fornecedores ObjFornecedores)
        {

            try
            {

                EditarFornecedor(ObjFornecedores);

                if(IsFornecedor())
                {
                    //Monda lista com os estados da federacao
                    List<EstadosFederacao> ObjListEstados = new List<EstadosFederacao>();

                    ObjListEstados = context.EstadosFederacao
                                        .AsNoTracking()
                                        .OrderBy(e => e.IdUf)
                                        .ToList();

                    ViewBag.ListaEstados = ObjListEstados;

                    //Monda lista com os paises do mundo
                    List<Paises> ObjListPais = new List<Paises>();

                    ObjListPais = context.Paises
                                  .AsNoTracking()
                                  .OrderBy(e => e.NomePais)
                                  .ToList();

                    ViewBag.ListaPaises = ObjListPais;

                    return Editar(IdLoginUser);
                }
                else
                {
                    Fornecedores ObjFornecedor = new Fornecedores();
                    ObjFornecedor.CNPJ = ObjFornecedores.CNPJ;
                    ObjFornecedor.NomeFantasia = ObjFornecedores.NomeFantasia;


                    TempData["Fornecedor"] = JsonConvert.SerializeObject(ObjFornecedor);
                    return RedirectToAction("Fornecedores", "Fornecedores");
                }

            }
            catch (Exception ex)
            {

                Erro("Ocorreu um erro ao editar Fornecedor, por favor contate o administrador: ", ex);
                return RedirectToAction("Index", "Fornecedores", new { });
            }
        
        }

        public IActionResult Adicionar()
        {
            try
            {
                //Monda lista com os estados da federacao
                List<EstadosFederacao> ObjListEstados = new List<EstadosFederacao>();

                ObjListEstados = context.EstadosFederacao
                                    .AsNoTracking()
                                    .OrderBy(e => e.IdUf)
                                    .ToList();

                ViewBag.ListaEstados = ObjListEstados;

                //Monda lista com os paises do mundo
                List<Paises> ObjListPais = new List<Paises>();

                ObjListPais = context.Paises
                              .AsNoTracking()
                              .OrderBy(e => e.NomePais)
                              .ToList();

                ViewBag.ListaPaises = ObjListPais;

                return View("Adicionar",null);
            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro ao Adicionar Fornecedor, por favor contate o administrador: ", ex);
                return RedirectToAction("Index", "Fornecedores", new { });

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Adicionar(Fornecedores ObjFornecedores)
        {

            try
            {
                AdicionarFornecedor(ObjFornecedores);

                Fornecedores ObjFornecedor = new Fornecedores();
                ObjFornecedor.CNPJ = ObjFornecedores.CNPJ;
                ObjFornecedor.NomeFantasia = ObjFornecedores.NomeFantasia;


                TempData["Fornecedor"] = JsonConvert.SerializeObject(ObjFornecedor);
                return RedirectToAction("Fornecedores", "Fornecedores");

            }
            catch (Exception ex)
            {

                Erro("Ocorreu um erro ao adicionar Fornecedor, por favor contate o administrador: ", ex);
                return RedirectToAction("Index", "Fornecedores", new { });
            }

        }

        public IActionResult Produtos(Guid Id)
        {
            try
            {
                List<FornecedoresProduto> ObjListProd = new List<FornecedoresProduto>();
                ObjListProd = FindListaProdutoFornecedor(Id);

                return View(ObjListProd);

            }
            catch (Exception ex)
            {

                Erro("Ocorreu um erro ao editar Fornecedor, por favor contate o administrador: ", ex);
                return RedirectToAction("Index", "Fornecedores", new { });
            }

        }

    }
}

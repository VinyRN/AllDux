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
    public class ProdutoController : Controller
    {

        private ContentDbContext context;
        private UserManager<ApplicationUser> userManager;
        
        public ProdutoController(ContentDbContext _context, UserManager<ApplicationUser> _userManager)
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
        
        private ProdutoHubViewModels ListaProduto(string NomeFind, string Tipo)
        {
            try
            {
                //Busca os Medicamentos conforme pesquisa
                ProdutoHubViewModels ObjModel = new ProdutoHubViewModels();
                ObjModel.objListMedicamento = context.Medicamentos
                                                .AsNoTracking()
                                                .Include(e => e.Variacoes)
                                                .Where(e => e.PrincipioAtivo.Contains(NomeFind))
                                                .OrderBy(e => e.PrincipioAtivo)
                                                .ToList();

                if (ObjModel.objListMedicamento != null && ObjModel.objListMedicamento.Count > 0)
                {
                    Guid LoginuserId = new Guid(userManager.GetUserAsync(HttpContext.User).Result.Id);

                    ProdutoFornecedorHub ObjProdVerHub;
                    List<ProdutoFornecedorHub> ObjListProdVerHub = new List<ProdutoFornecedorHub>();

                    //Verifica se o Medicamento esta cadastrato no HUB
                    foreach (Medicamento MedicamentoItem in ObjModel.objListMedicamento)
                    {

                        //Percorrer as variações apresentadas se ja esta no cadastro
                        foreach (MedicamentoVariacao MedicamentoVarItem in MedicamentoItem.Variacoes)
                        {

                            string lstrApresentacao = "";

                            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                            //Rotina para pegar a Apresentação do Documento
                            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                            //Pega Apresentacao BrasindicePF
                            BrasindicePF ObjBrasPF = new BrasindicePF();
                            ObjBrasPF = context.BrasindicePF
                                        .AsNoTracking()
                                        .Where(e => e.TISS == MedicamentoVarItem.TISS)
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
                                            .Where(e => e.TISS == MedicamentoVarItem.TISS)
                                            .SingleOrDefault();

                                if (ObjBrasPMC != null)
                                {
                                    lstrApresentacao = ObjBrasPMC.ApresentacaoNome;
                                }
                                else
                                {
                                    lstrApresentacao = MedicamentoVarItem.UnApresentacao + " " + MedicamentoVarItem.UnApresentacaoTipo + "/" + MedicamentoVarItem.UnMedida + " " + MedicamentoVarItem.UnMedidaTipo;
                                }
                            }
                            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


                            FornecedoresProduto ObjFornecedorProduto = new FornecedoresProduto();
                            ObjFornecedorProduto = context.FornecedoresProduto
                                                   .AsNoTracking()
                                                   .Where(e => e.MedicamentoVariacaoId == MedicamentoVarItem.Id).FirstOrDefault();

                            if (ObjFornecedorProduto != null)
                            {

                                ObjProdVerHub = new ProdutoFornecedorHub();
                                ObjProdVerHub.ProdutoId = ObjFornecedorProduto.Id;
                                ObjProdVerHub.MedicamentoId = ObjFornecedorProduto.MedicamentoId;
                                ObjProdVerHub.MedicamentoVariacaoId = ObjFornecedorProduto.MedicamentoVariacaoId;
                                ObjProdVerHub.IdFornecedor = ObjFornecedorProduto.IdFornecedor;
                                ObjProdVerHub.LoginUserId = LoginuserId;
                                ObjProdVerHub.Status = ObjFornecedorProduto.Status;
                                ObjProdVerHub.Apresentacao = lstrApresentacao;

                                ObjListProdVerHub.Add(ObjProdVerHub);
                            }
                            else
                            {
                                ObjProdVerHub = new ProdutoFornecedorHub();
                                ObjProdVerHub.MedicamentoId = MedicamentoVarItem.MedicamentoId;
                                ObjProdVerHub.MedicamentoVariacaoId = MedicamentoVarItem.Id;
                                ObjProdVerHub.Status = 0;
                                ObjProdVerHub.Apresentacao = lstrApresentacao;

                                ObjListProdVerHub.Add(ObjProdVerHub);
                            }
                        }
                    }

                    ObjModel.objListProdutoVerHub = ObjListProdVerHub;
                }
                else
                {
                    Alerta("Produto não encontrado.", "danger");
                    return null;
                }

                return ObjModel;
            }
            catch (Exception ex)
            {
                Erro("Erro buscando informações de produto.", ex);
                return null;
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Produtos(string NomeFind, string Tipo)
        {
            try
            {
                string lstrTipoProduto = "";

                if (Tipo.Trim() == "1") //Medicamentos
                {
                    //Busca os Medicamentos conforme pesquisa

                    ProdutoHubViewModels ObjModel = new ProdutoHubViewModels();
                    ObjModel = ListaProduto(NomeFind, Tipo);

                    if (ObjModel != null)
                    {
                        if (ObjModel.objListMedicamento != null && ObjModel.objListMedicamento.Count > 0)
                        {

                            lstrTipoProduto = "Medicamento";

                            ViewBag.ListaProduto = ObjModel.objListMedicamento;
                            ViewBag.TipoProduto = lstrTipoProduto;
                            ViewBag.Tipo = "1";

                            return View("Medicamento", ObjModel);
                        }
                        else
                        {
                            lstrTipoProduto = "";
                            ViewBag.TipoProduto = lstrTipoProduto;
                            ViewBag.Tipo = "0";

                            Alerta("Produto não encontrado.", "danger");
                            return RedirectToAction("Index", "Produto", new { });
                        }

                    }
                    else
                    {
                        lstrTipoProduto = "";
                        ViewBag.TipoProduto = lstrTipoProduto;
                        ViewBag.Tipo = "0";

                        return RedirectToAction("Index", "Produto", new { });

                    }
                }
                else if (Tipo.Trim() == "2") //Material
                {
                    Alerta("Pesquisa de Material em desenvolvimento.", "info");
                    return RedirectToAction("Index", "Produto", new { });
                }
                else
                {
                    Alerta("Favor informar o Tipo de Produto.", "danger");
                    return RedirectToAction("Index", "Produto", new { });
                }

            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro ao consultar o Produto, por favor contate o administrador: " + NomeFind, ex);
                return RedirectToAction("Index", "Produto", new { });
            }
           
        }
        public IActionResult Produtos()
        {
            try
            {

                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //Trata variavel temporaria com os valores para remontar a tela.
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                TempData.TryGetValue("Produto", out object ObjProdTemp);
                
                Produto ObjProd = new Produto();
                ObjProd = JsonConvert.DeserializeObject<Produto>((string)ObjProdTemp);

                string Tipo = "";
                string NomeFind = "";

                if (ObjProd != null)
                {
                    Tipo = ObjProd.Tipo;
                    NomeFind = ObjProd.NomeFind;

                }
                else
                {
                    return View();
                }
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                string lstrTipoProduto = "";


                if (Tipo.Trim() == "1") //Medicamentos
                {
                    //Busca os Medicamentos conforme pesquisa

                    ProdutoHubViewModels ObjModel = new ProdutoHubViewModels();
                    ObjModel = ListaProduto(NomeFind, Tipo);

                    if (ObjModel != null)
                    {
                        if (ObjModel.objListMedicamento != null && ObjModel.objListMedicamento.Count > 0)
                        {

                            lstrTipoProduto = "Medicamento";

                            ViewBag.ListaProduto = ObjModel.objListMedicamento;
                            ViewBag.TipoProduto = lstrTipoProduto;
                            ViewBag.Tipo = "1";

                            return View("Medicamento", ObjModel);
                        }
                        else
                        {
                            lstrTipoProduto = "";
                            ViewBag.TipoProduto = lstrTipoProduto;
                            ViewBag.Tipo = "0";

                            Alerta("Produto não encontrado.", "danger");
                            return RedirectToAction("Index", "Produto", new { });
                        }

                    }
                    else
                    {
                        lstrTipoProduto = "";
                        ViewBag.TipoProduto = lstrTipoProduto;
                        ViewBag.Tipo = "0";

                        return RedirectToAction("Index", "Produto", new { });

                    }
                }
                else if (Tipo.Trim() == "2") //Material
                {
                    Alerta("Pesquisa de Material em desenvolvimento.", "info");
                    return RedirectToAction("Index", "Produto", new { });
                }
                else
                {
                    Alerta("Favor informar o Tipo de Produto.", "danger");
                    return RedirectToAction("Index", "Produto", new { });
                }

            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro ao consultar o Produto, por favor contate o administrador: " , ex);
                return RedirectToAction("Index", "Produto", new { });
            }
        }

        private FornecedoresProduto FindProdutoEstoque(Guid Id)
        {
            try
            {
                return context.FornecedoresProduto
                            .AsNoTracking()                            
                            .Where(e => e.Id == Id)
                            .SingleOrDefault();
            }
            catch (Exception ex)
            {
                Erro("Produto Fornecedor não encontrado.", ex);
                return null;
            }
        }

        [HttpGet]
        public IActionResult ProdutoEstoque(string id)
        {
            FornecedoresProduto ObjFornecedorProd = FindProdutoEstoque(new Guid(id));                        
            return PartialView(ObjFornecedorProd);
        }
    }
}

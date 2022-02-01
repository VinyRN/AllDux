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

using Newtonsoft.Json;

namespace alldux_plataforma.Controllers
{
    public class FornecedorProdutoController : Controller
    {
        private ContentDbContext context;
        private UserManager<ApplicationUser> userManager;

        public FornecedorProdutoController(ContentDbContext _context, UserManager<ApplicationUser> _userManager)
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
        private void AdicionarProdutoMedicamento(FornecedoresProduto ObjFornecedorProduto)
        {
            try
            {


                MedicamentoVariacao ObjMedicamentoVar = context.MedicamentoVariacao
                                                        .AsNoTracking()
                                                        //.Include(e => e.Medicamento)
                                                        .Where(e => e.Id == ObjFornecedorProduto.Id)
                                                        .SingleOrDefault();

                if (ObjMedicamentoVar != null)
                {
                    ObjFornecedorProduto.CreateDate = DateTime.Now;
                    ObjFornecedorProduto.CreateUser = new Guid(userManager.GetUserAsync(HttpContext.User).Result.Id);

                    if (!string.IsNullOrEmpty(ObjFornecedorProduto.UserId))
                    {
                        if (ObjFornecedorProduto.UserId.Trim() != "")
                        {
                            ObjFornecedorProduto.IdFornecedor = new Guid(ObjFornecedorProduto.UserId);
                        }
                    }

                    ObjFornecedorProduto.Status = 1;
                    ObjFornecedorProduto.MedicamentoId = ObjMedicamentoVar.MedicamentoId;
                    ObjFornecedorProduto.MedicamentoVariacaoId = ObjMedicamentoVar.Id;
                    ObjFornecedorProduto.Id = new Guid();

                    context.FornecedoresProduto.Add(ObjFornecedorProduto);
                    context.SaveChanges();
                }
                else
                {
                    Alerta("Erro ao adicionar produto para o fornecedor (dados do medicamento não encontrado)" + ObjFornecedorProduto.Id, "danger");
                }

                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            }
            catch (Exception ex)
            {
                Erro("Erro ao adicionar produto versão" + ObjFornecedorProduto.Id, ex);

            }
        }
        private void EditarProdutoMedicamento(FornecedoresProduto ObjFornecedorProduto)
        {
            try
            {
                if (ObjFornecedorProduto != null)
                {

                    if (!string.IsNullOrEmpty(ObjFornecedorProduto.UserId))
                    {
                        if (ObjFornecedorProduto.UserId.Trim() != "")
                        {
                            ObjFornecedorProduto.IdFornecedor = new Guid(ObjFornecedorProduto.UserId);
                        }
                    }

                    if (ObjFornecedorProduto.Status == 0)
                    {
                        ObjFornecedorProduto.Status = ObjFornecedorProduto.StatusEdit;
                    }
                    
                    ObjFornecedorProduto.LastUpdateUser = new Guid(userManager.GetUserAsync(HttpContext.User).Result.Id);

                    context.FornecedoresProduto.Update(ObjFornecedorProduto);
                    context.SaveChanges();
                }
                else
                {
                    Alerta("Erro ao editar produto para o fornecedor (dados do medicamento não encontrado)" + ObjFornecedorProduto.Id, "danger");
                }

                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            }
            catch (Exception ex)
            {
                Erro("Erro ao adicionar produto fornecedor" + ObjFornecedorProduto.Id, ex);

            }
        }
        private FornecedoresProduto FindProdutoMedicamento(Guid Id)
        {
            try
            {

                MedicamentoVariacao ObjMedicamentoVar = context.MedicamentoVariacao
                                                        .AsNoTracking()
                                                        .Include(e => e.Medicamento)
                                                        .Where(e => e.Id == Id)
                                                        .Single();

                if (ObjMedicamentoVar != null)
                {
                    FornecedoresProduto objFornecedorProduto = new FornecedoresProduto();

                    objFornecedorProduto.PrincipioAtivo = ObjMedicamentoVar.Medicamento.PrincipioAtivo;
                    objFornecedorProduto.Nome = ObjMedicamentoVar.Nome;
                    objFornecedorProduto.Laboratorio = ObjMedicamentoVar.Laboratorio;
                    objFornecedorProduto.Apresentacao = ObjMedicamentoVar.UnApresentacao + " " + ObjMedicamentoVar.UnApresentacaoTipo + "/" + ObjMedicamentoVar.UnMedida + " " + ObjMedicamentoVar.UnMedidaTipo;
                    objFornecedorProduto.Tipo = "1"; //MEDICAMENTO


                    //Pega o Codigo do user logado
                    Guid UserId = new Guid(userManager.GetUserAsync(HttpContext.User).Result.Id);

                    //Verifica se é um fornecedor
                    Fornecedores ObjFornecedor = new Fornecedores();

                    ObjFornecedor = context.Fornecedores
                                    .AsNoTracking()
                                    .Where(e => e.IdFornecedor == UserId)
                                    .SingleOrDefault();

                    objFornecedorProduto.UserId = "";
                    objFornecedorProduto.NomeFornecedor = "";

                    if (ObjFornecedor != null)
                    {
                        objFornecedorProduto.UserId = ObjFornecedor.IdFornecedor.ToString();
                        objFornecedorProduto.NomeFornecedor = ObjFornecedor.NomeFantasia;
                    }



                    return objFornecedorProduto;
                }

                return null;
            }
            catch (Exception ex)
            {
                Erro("Erro ao buscar dados do medicamento.", ex);
                return null;
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdicionarMedicamento(Guid Id)
        {

            try
            {
                FornecedoresProduto ObjFornecedorProd = FindProdutoMedicamento(Id);

                if (ObjFornecedorProd != null)
                {
                    List<Fornecedores> ObjListFornecedor = new List<Fornecedores>();

                    ObjListFornecedor = context.Fornecedores
                                        .AsNoTracking()
                                        .OrderBy(e => e.NomeFantasia)
                                        .ToList();

                    ViewBag.ListaFornecedores = ObjListFornecedor;

                    return View("AdicionarMedicamento", ObjFornecedorProd);
                }
                else
                {
                    return View();
                }

            }
            catch (Exception ex)
            {

                Erro("Ocorreu um erro ao incluir o Produto no Marketplace, por favor contate o administrador.", ex);
                return RedirectToAction("Index", "Produto", new { });
            }


        }

        [HttpPost]
        public IActionResult AdicionarMedicamento(FornecedoresProduto ObjFornecedorProduto)
        {

            try
            {
                AdicionarProdutoMedicamento(ObjFornecedorProduto);

                Produto ObjProduto = new Produto();
                ObjProduto.Tipo = "1"; //MEDICAMENTO
                ObjProduto.NomeFind = ObjFornecedorProduto.PrincipioAtivo;

                
                TempData["Produto"] = JsonConvert.SerializeObject(ObjProduto);
                return RedirectToAction("Produtos", "Produto");

            }
            catch (Exception ex)
            {

                Erro("Ocorreu um erro ao incluir o Produto para o fornecedor, por favor contate o administrador.", ex);
                return RedirectToAction("Index", "Produto", new { });
            }


        }
        public IActionResult EditarMedicamento(Guid Id)
        {

            try
            {
                FornecedoresProduto ObjFornecedorProdEdit = new FornecedoresProduto();

                ObjFornecedorProdEdit = context.FornecedoresProduto
                                        .AsNoTracking()
                                        .Where(e => e.Id == Id)
                                        .SingleOrDefault();

                if (ObjFornecedorProdEdit != null)
                {

                    FornecedoresProduto ObjFornecedorProd = FindProdutoMedicamento(ObjFornecedorProdEdit.MedicamentoVariacaoId);

                    if (ObjFornecedorProd != null)
                    {
                        List<Fornecedores> ObjListFornecedor = new List<Fornecedores>();

                        ObjListFornecedor = context.Fornecedores
                                            .AsNoTracking()
                                            .OrderBy(e => e.NomeFantasia)
                                            .ToList();

                        ViewBag.ListaFornecedores = ObjListFornecedor;
                        

                        ObjFornecedorProd.Id = ObjFornecedorProdEdit.Id;
                        ObjFornecedorProd.MedicamentoId = ObjFornecedorProdEdit.MedicamentoId;
                        ObjFornecedorProd.MedicamentoVariacaoId = ObjFornecedorProdEdit.MedicamentoVariacaoId;
                        ObjFornecedorProd.IdFornecedor = ObjFornecedorProdEdit.IdFornecedor;
                        ObjFornecedorProd.DataDisponivel = ObjFornecedorProdEdit.DataDisponivel;
                        ObjFornecedorProd.DataIndisponivel = ObjFornecedorProdEdit.DataIndisponivel;
                        ObjFornecedorProd.Status = ObjFornecedorProdEdit.Status;
                        ObjFornecedorProd.StatusEdit = ObjFornecedorProdEdit.Status;
                        ObjFornecedorProd.Estoque = ObjFornecedorProdEdit.Estoque;
                        ObjFornecedorProd.CreateDate = ObjFornecedorProdEdit.CreateDate;
                        ObjFornecedorProd.CreateUser = ObjFornecedorProdEdit.CreateUser;
                        ObjFornecedorProd.DataDisponivelAllDux = ObjFornecedorProdEdit.DataDisponivelAllDux;
                        ObjFornecedorProd.Lote = ObjFornecedorProdEdit.Lote;

                        ObjFornecedorProd.LastUpdateUser = new Guid(userManager.GetUserAsync(HttpContext.User).Result.Id);

                        return View("EditarMedicamento", ObjFornecedorProd);
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }                
            }
            catch (Exception ex)
            {

                Erro("Ocorreu um erro ao editar o Produto . por favor contate o administrador.", ex);
                return RedirectToAction("Index", "Produto", new { });
            }


        }

        [HttpPost]
        public IActionResult EditarMedicamento(FornecedoresProduto ObjFornecedorProduto)
        {

            try
            {

                EditarProdutoMedicamento(ObjFornecedorProduto);

                Produto ObjProduto = new Produto();
                ObjProduto.Tipo = "1"; //MEDICAMENTO
                ObjProduto.NomeFind = ObjFornecedorProduto.PrincipioAtivo;


                TempData["Produto"] = JsonConvert.SerializeObject(ObjProduto);
                return RedirectToAction("Produtos", "Produto");

            }
            catch (Exception ex)
            {

                Erro("Ocorreu um erro ao incluir o Produto para o fornecedor, por favor contate o administrador.", ex);
                return RedirectToAction("Index", "Produto", new { });
            }


        }
        public IActionResult DeletarMedicamento(Guid Id)
        {
           
            try
            {
                FornecedoresProduto ObjFornecedorProdDel = new FornecedoresProduto();

                ObjFornecedorProdDel = context.FornecedoresProduto
                                        .AsNoTracking()
                                        .Where(e => e.Id == Id)
                                        .SingleOrDefault();

                if (ObjFornecedorProdDel != null)
                {
                    context.FornecedoresProduto.Remove(ObjFornecedorProdDel);
                    context.SaveChanges();
                    Alerta("Produto excluído com sucesso", "success");

                    Medicamento ObjMedicamento = new Medicamento();

                    ObjMedicamento = context.Medicamentos
                                    .AsNoTracking()
                                    .Where(e => e.Id == ObjFornecedorProdDel.MedicamentoId)
                                    .SingleOrDefault();

                    Produto ObjProduto = new Produto();
                    ObjProduto.Tipo = "1"; //MEDICAMENTO
                    ObjProduto.NomeFind = ObjMedicamento.PrincipioAtivo;

                    TempData["Produto"] = JsonConvert.SerializeObject(ObjProduto);
                    return RedirectToAction("Produtos", "Produto");

                }
                else
                {
                    Alerta("Produto não localizado para o fornecedor.", "danger");
                    return RedirectToAction("Index", "Produto", new { });

                }

            }
            catch (Exception ex)
            {
                Erro("Erro ao exluir produto do fornecedor.", ex);
                return RedirectToAction("Index", "Produto", new { });
            }
            
        }
    }
}

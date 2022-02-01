//using System;
//using System.Linq;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;
//using alldux_plataforma.Data;
//using alldux_plataforma.Models;
//using System.Collections.Generic;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using System.IO;
//using Microsoft.AspNetCore.Http;
//using System.Threading.Tasks;

//namespace alldux_plataforma.Controllers
//{
//    [Authorize(Roles = "Admin")]
//    public class ProdutoVersaoController : Controller
//    {
//        private ContentDbContext context;
//        private UserManager<ApplicationUser> userManager;

//        public ProdutoVersaoController(ContentDbContext _context, UserManager<ApplicationUser> _userManager)
//        {
//            context = _context;
//            userManager = _userManager;
//        }

//        private void Erro(string msg, Exception ex)
//        {
//            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
//            Console.WriteLine("ERRO: " + msg);
//            Console.WriteLine("----------------------------------------------------------------------------------------------------");
//            Console.WriteLine(ex.Message);
//            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
//            TempData["alerta"] = msg;
//            TempData["tipo"] = "danger";
//            TempData["exception"] = ex.Message;
//        }
//        private void Alerta(string msg, string type)
//        {
//            TempData["alerta"] = msg;
//            TempData["tipo"] = type;

//            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
//            Console.WriteLine(type.ToUpper() + ": " + msg);
//            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
//        }

//        public IActionResult Index()
//        {
//            return View(ListaProdutoVersao());
//        }

//        private void AdicionarProdutoVersao(ProdutoVersao ObjProdutoVersao)
//        {
//            try
//            {
//                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//                //Adicionando Produto
//                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

//                Guid NewIdProduto;

//                Produto ObjProduto = new Produto();

//                ObjProduto = context.Produto
//                            .AsNoTracking()
//                            .Where(e => e.ReferenciaId == ObjProdutoVersao.ProdutoId)
//                            .FirstOrDefault();

//                if (ObjProduto == null)
//                {

//                    NewIdProduto = Guid.NewGuid();

//                    ObjProduto = new Produto();

//                    ObjProduto.Id = NewIdProduto;
//                    ObjProduto.Tipo = ObjProdutoVersao.Tipo;
//                    ObjProduto.ReferenciaId = ObjProdutoVersao.ProdutoId;

//                    context.Produto.Add(ObjProduto);
//                    context.SaveChanges();

//                }
//                else
//                {
//                    NewIdProduto = ObjProduto.Id;
//                }
//                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//                //Adicionando Produto Versao
//                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

//                ObjProdutoVersao.ProdutoId = NewIdProduto;
//                ObjProdutoVersao.Id = new Guid();
//                ObjProdutoVersao.ReferenciaId = ObjProdutoVersao.Id;
//                ObjProdutoVersao.CreateDate = DateTime.Now;
//                ObjProdutoVersao.CreateUser = new Guid(userManager.GetUserAsync(HttpContext.User).Result.Id);

//                context.ProdutoVersao.Add(ObjProdutoVersao);
//                context.SaveChanges();

//                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//            }
//            catch (Exception ex)
//            {
//                Erro("Erro ao adicionar produto versão" + ObjProdutoVersao.Id, ex);

//            }
//        }
//        private void UpdateProdutoVersao(Guid Id, ProdutoVersao objProdutoVersaoNovo)
//        {
//            try
//            {
//                //var ProdutoVersaoAtualizar = FindProdutoVersao(Id);
//                //context.Medicamentos.Remove(ProdutoAtualizar);
//                context.SaveChanges();

//                objProdutoVersaoNovo.Id = Id;
//                //context.Medicamentos.Add(objProdutoNovo);
//                context.SaveChanges();
//            }
//            catch (Exception ex)
//            {
//                Erro("Erro ao editar produto versão, por favor contate o administrador.", ex);
//            }
//        }
//        private List<ProdutoVersao> ListaProdutoVersao()
//        {
//            try
//            {
//                //return context.Medicamentos
//                //            .AsNoTracking()
//                //            .Include(e => e.Variacoes)
//                //            .OrderBy(e => e.PrincipioAtivo)
//                //            .ToList();

//                return null;
//            }
//            catch (Exception ex)
//            {
//                Erro("Erro buscando informações de produto versão.", ex);
//                return null;
//            }
//        }

//        private Guid GetIdMedicamento(Guid Id)
//        {
//            try
//            {
//                MedicamentoVariacao ObjMedicamentoVar = context.MedicamentoVariacao
//                                                        .AsNoTracking()
//                                                        .Where(e => e.Id == Id)
//                                                        .Single();
//                if (ObjMedicamentoVar != null)
//                {
//                    return ObjMedicamentoVar.MedicamentoId;
//                }
//                else
//                {
//                    return new Guid();
//                }

//            }
//            catch (Exception ex)
//            {
//                Erro("Erro ao buscar ID do medicamento.", ex);
//                return new Guid();
//            }


//        }

//        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//        //Metodos dierecionado para medicamentos
//        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//        private ProdutoVersao FindProdutoMedicamento(Guid Id)
//        {
//            try
//            {
//                MedicamentoVariacao ObjMedicamentoVar = context.MedicamentoVariacao
//                                                        .AsNoTracking()
//                                                        .Include(e => e.Medicamento)
//                                                        .Where(e => e.Id == Id)
//                                                        .Single();

//                if (ObjMedicamentoVar != null)
//                {
//                        ProdutoVersao objProdutoVersao = new ProdutoVersao();

//                        objProdutoVersao.PrincipioAtivo = ObjMedicamentoVar.Medicamento.PrincipioAtivo;
//                        objProdutoVersao.Nome = ObjMedicamentoVar.Nome;
//                        objProdutoVersao.Laboratorio = ObjMedicamentoVar.Laboratorio;
//                        objProdutoVersao.Distribuidor = ObjMedicamentoVar.Distribuidor;
//                        objProdutoVersao.Apresentacao = ObjMedicamentoVar.UnApresentacao + " " + ObjMedicamentoVar.UnApresentacaoTipo + "/" + ObjMedicamentoVar.UnMedida + " " + ObjMedicamentoVar.UnMedidaTipo;
//                        objProdutoVersao.Tipo = "1"; //MEDICAMENTO
//                    objProdutoVersao.ProdutoId = ObjMedicamentoVar.MedicamentoId;

//                        return objProdutoVersao;
//                }

//                return null;
//            }
//            catch (Exception ex)
//            {
//                Erro("Erro ao buscar dados do medicamento.", ex);
//                return null;
//            }
//        }

//        public IActionResult AdicionarMedicamento(Guid Id)
//        {

//            try
//            {
//                ProdutoVersao ObjProdutoVer = FindProdutoMedicamento(Id);

//                if (ObjProdutoVer != null)
//                {
//                    return View("AdicionarMedicamento", ObjProdutoVer);
//                }
//                else
//                {
//                    return View();
//                }

//            }
//            catch (Exception ex)
//            {

//                Erro("Ocorreu um erro ao incluir o Produto no Marketplace, por favor contate o administrador.", ex);
//                return RedirectToAction("Index", "ProdutoVersao", new { });
//            }

            
//        }

//        [HttpPost]
//        public IActionResult AdicionarMedicamento(ProdutoVersao objProdutoVersao)
//        {
//            try
//            {
//                Guid ProdutoId = GetIdMedicamento(objProdutoVersao.Id);
//                objProdutoVersao.ProdutoId = ProdutoId;

//                AdicionarProdutoVersao(objProdutoVersao);                
//                return RedirectToAction("ListaProdutos", "Produto", new {@NomeFind = objProdutoVersao.Nome, @Tipo=1});
//            }
//            catch (Exception ex)
//            {
//                Erro("Erro ao adicionar produto versão, por favor contate o administrador.", ex);
//                return View(objProdutoVersao);
//            }
//        }

//        [HttpPost]
//        public IActionResult EditarMedicamento(Guid Id)
//        {
//            try
//            {
//                ProdutoVersao ObjProdutoVer = FindProdutoMedicamento(Id);

//                if (ObjProdutoVer != null)
//                {                    
//                    return View(ObjProdutoVer);
//                }
//                else
//                {
//                    return View();
//                }

//            }
//            catch (Exception ex)
//            {
//                Erro("Ocorreu um erro ao Editar o Produto no Marketplace, por favor contate o administrador.", ex);
//                return RedirectToAction("Index", "ProdutoVersao", new { });
//            }

//        }

//        [HttpPost]
//        public IActionResult EditarMedicamento(ProdutoVersao objProdutoVersaoEditado)
//        {
//            ProdutoVersao objProdutoVersaoEdit = new ProdutoVersao()
//            {
//                Id = Guid.Empty,
//            };

//            try
//            {
//                UpdateProdutoVersao(objProdutoVersaoEditado.Id, objProdutoVersaoEdit);
//            }
//            catch (Exception ex)
//            {
//                Erro("Ocorreu um erro editando o Produto Versão, por favor contate o administrador", ex);
//                return View();
//            }

//            return RedirectToAction("Index", "ProdutoVersao", new { });
//        }



//        public IActionResult Excluir(string Id)
//        {
//            try
//            {
//                //var medicamentoExcluir = context.Medicamentos.Where(e => e.Id == new Guid(Id)).Single();
//                //context.Medicamentos.Remove(medicamentoExcluir);
//                context.SaveChanges();
//            }
//            catch (Exception ex)
//            {
//                Erro("Ocorreu um erro ao excluir o Produto Versao, por favor contate o administrador: " + Id, ex);
//                return RedirectToAction("Index", "Produto", new { });
//            }

//            Alerta("Produto Versão excluido com sucesso.", "success");
//            return RedirectToAction("Index", "ProdutoVersao", new { });
//        }

//    }
//}

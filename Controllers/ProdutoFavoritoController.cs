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

namespace alldux_plataforma.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProdutoFavoritoController : Controller
    {
        private ContentDbContext context;
        private UserManager<ApplicationUser> userManager;

        public ProdutoFavoritoController(ContentDbContext _context, UserManager<ApplicationUser> _userManager)
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
        private void AdicionarProdutoFavorito(ProdutosFavorito ObjProdutoFavorito)
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Erro("Erro ao adicionar produto " + ObjProdutoFavorito.Id, ex);
            }
        }
        private List<ProdutosFavorito> ListaProdutoFavorito()
        {
            try
            {
                //return context.Medicamentos
                //            .AsNoTracking()
                //            .Include(e => e.Variacoes)
                //            .OrderBy(e => e.PrincipioAtivo)
                //            .ToList();

                return null;
            }
            catch (Exception ex)
            {
                Erro("Erro buscando informações de produto favorito.", ex);
                return null;
            }
        }
        private ProdutosFavorito FindProdutoFavorito(Guid Id)
        {
            try
            {
                //return context.Medicamentos
                //            .AsNoTracking()
                //            .Include(e => e.Variacoes.OrderBy(e => e.Nome))
                //            .Where(e => e.Id == Id)
                //            .Single();

                return null;
            }
            catch (Exception ex)
            {
                Erro("Produto Favorito não encontrado.", ex);
                return null;
            }
        }
        private void UpdateProdutoFavorito(Guid Id, ProdutosFavorito objProdutoFavoritoNovo)
        {
            try
            {
                var ProdutoFavoritoAtualizar = FindProdutoFavorito(Id);
                //context.Medicamentos.Remove(ProdutoAtualizar);
                context.SaveChanges();

                objProdutoFavoritoNovo.Id = Id;
                //context.Medicamentos.Add(objProdutoNovo);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Erro("Erro ao editar produto favorito, por favor contate o administrador.", ex);
            }
        }
        
        public IActionResult Index()
        {
            return View(ListaProdutoFavorito());
        }

        public IActionResult Adicionar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Adicionar(ProdutosFavorito objProdutoFavorito)
        {
            objProdutoFavorito.Id = new Guid();

            try
            {
                AdicionarProdutoFavorito(objProdutoFavorito);
                Alerta("Produto Favorito " + objProdutoFavorito.Id + " adicionado com sucesso.", "success");
                return RedirectToAction("Index", "ProdutoFavorito", new { });
            }
            catch (Exception ex)
            {
                Erro("Erro ao adicionar produto favorito, por favor contate o administrador.", ex);
                return View(objProdutoFavorito);
            }
        }
        public IActionResult Editar(string Id)
        {
            ProdutosFavorito objProdutoFavorito = new ProdutosFavorito();

            try
            {
                //medicamentoEditar = FindMedicamentos(new Guid(Id));
            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro ao Editar o Produto Favorito, por favor contate o administrador.", ex);
                return RedirectToAction("Index", "ProdutoFavorito", new { });
            }

            return View(objProdutoFavorito);
        }
        [HttpPost]
        public IActionResult Editar(Produto objProdutoFavoritoEditado)
        {
            ProdutosFavorito objProdutoFavoritoEdit = new ProdutosFavorito()
            {
                Id = Guid.Empty            
            };

            try
            {
                UpdateProdutoFavorito(objProdutoFavoritoEditado.Id, objProdutoFavoritoEdit);
            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro editando o Produto Favorito, por favor contate o administrador", ex);
                return View();
            }

            return RedirectToAction("Index", "ProdutoFavorito", new { });
        }
        public IActionResult Excluir(string Id)
        {
            try
            {
                //var medicamentoExcluir = context.Medicamentos.Where(e => e.Id == new Guid(Id)).Single();
                //context.Medicamentos.Remove(medicamentoExcluir);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro ao excluir o Produto Favorito, por favor contate o administrador: " + Id, ex);
                return RedirectToAction("Index", "Produto", new { });
            }

            Alerta("Produto Favorito excluido com sucesso.", "success");
            return RedirectToAction("Index", "ProdutoFavorito", new { });
        }
    }
}

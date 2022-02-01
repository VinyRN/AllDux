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
    public class PrecoController : Controller
    {
        private ContentDbContext context;
        private UserManager<ApplicationUser> userManager;

        public PrecoController(ContentDbContext _context, UserManager<ApplicationUser> _userManager)
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

        private void AdicionarPreco(Preco ObjPreco)
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Erro("Erro ao adicionar Preço " + ObjPreco.Id, ex);
            }
        }
        private List<Preco> ListaPreco()
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
                Erro("Erro buscando informações de Preço.", ex);
                return null;
            }
        }
        private Preco FindPreco(Guid Id)
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
                Erro("Preco não encontrado.", ex);
                return null;
            }
        }
        private void UpdatePreco(Guid Id, Preco objPrecoNovo)
        {
            try
            {
                var PrecoAtualizar = FindPreco(Id);
                //context.Medicamentos.Remove(ProdutoAtualizar);
                context.SaveChanges();

                objPrecoNovo.Id = Id;
                //context.Medicamentos.Add(objProdutoNovo);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Erro("Erro ao editar preço, por favor contate o administrador.", ex);
            }
        }

        public IActionResult Index()
        {
            return View(ListaPreco());
        }

        public IActionResult Adicionar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Adicionar(Preco objPreco)
        {
            objPreco.Id = new Guid();

            try
            {
                AdicionarPreco(objPreco);
                Alerta("Preço " + objPreco.Id + " adicionado com sucesso.", "success");
                return RedirectToAction("Index", "Preco", new { });
            }
            catch (Exception ex)
            {
                Erro("Erro ao adicionar preço, por favor contate o administrador.", ex);
                return View(objPreco);
            }
        }
        public IActionResult Editar(string Id)
        {
            Preco objPreco = new Preco();

            try
            {
                //medicamentoEditar = FindMedicamentos(new Guid(Id));
            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro ao Editar o Preco, por favor contate o administrador.", ex);
                return RedirectToAction("Index", "Preco", new { });
            }

            return View(objPreco);
        }
        [HttpPost]
        public IActionResult Editar(Preco objPrecoEditado)
        {
            Preco objPrecoEdit = new Preco()
            {
                Id = Guid.Empty
            };

            try
            {
                UpdatePreco(objPrecoEditado.Id, objPrecoEdit);
            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro editando o Preço, por favor contate o administrador", ex);
                return View();
            }

            return RedirectToAction("Index", "Preco", new { });
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
                Erro("Ocorreu um erro ao excluir o Preço, por favor contate o administrador: " + Id, ex);
                return RedirectToAction("Index", "Produto", new { });
            }

            Alerta("Produto excluido com sucesso.", "success");
            return RedirectToAction("Index", "Preco", new { });
        }
        
    }
}

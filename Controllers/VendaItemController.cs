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
    public class VendaItemController : Controller
    {
        private ContentDbContext context;
        private UserManager<ApplicationUser> userManager;

        public VendaItemController(ContentDbContext _context, UserManager<ApplicationUser> _userManager)
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
        
        private void AdicionarVendaItem(VendaItem ObjVendaItem)
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Erro("Erro ao adicionar Item da Venda " + ObjVendaItem.VendaId + "-" + ObjVendaItem.Id, ex);
            }
        }
        private List<VendaItem> ListaVendaItem()
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
                Erro("Erro buscando informações de Itens da Venda.", ex);
                return null;
            }
        }
        private VendaItem FindVendaItem(Guid Id)
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
                Erro("Item da Venda não encontrado.", ex);
                return null;
            }
        }
        private void UpdateVendaItem(Guid Id, VendaItem objVendaItemNovo)
        {
            try
            {
                var VendaItemAtualizar = FindVendaItem(Id);
                //context.Medicamentos.Remove(ProdutoAtualizar);
                context.SaveChanges();

                objVendaItemNovo.Id = Id;
                //context.Medicamentos.Add(objProdutoNovo);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Erro("Erro ao editar Item da Venda, por favor contate o administrador.", ex);
            }
        }


        public IActionResult Index()
        {
            return View(ListaVendaItem());
        }

        [HttpPost]
        public IActionResult Adicionar(VendaItem objVendaItem)
        {
            objVendaItem.Id = new Guid();

            try
            {
                AdicionarVendaItem(objVendaItem);
                Alerta("Venda Item " + objVendaItem.Id + " adicionado com sucesso.", "success");
                return RedirectToAction("Index", "VendaItem", new { });
            }
            catch (Exception ex)
            {
                Erro("Erro ao adicionar Item da Venda, por favor contate o administrador.", ex);
                return View(objVendaItem);
            }
        }
        public IActionResult Editar(string Id)
        {
            VendaItem objVendaItem = new VendaItem();

            try
            {
                //medicamentoEditar = FindMedicamentos(new Guid(Id));
            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro ao Editar o Item da Venda, por favor contate o administrador.", ex);
                return RedirectToAction("Index", "VendaItem", new { });
            }

            return View(objVendaItem);
        }
        [HttpPost]
        public IActionResult Editar(VendaItem objItemVendaEditado)
        {
            VendaItem objItemVendaEdit = new VendaItem()
            {
                Id = Guid.Empty,
            };

            try
            {
                UpdateVendaItem(objItemVendaEditado.Id, objItemVendaEdit);
            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro editando o Item da Venda, por favor contate o administrador", ex);
                return View();
            }

            return RedirectToAction("Index", "ItemVenda", new { });
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
                Erro("Ocorreu um erro ao excluir o Item da Venda, por favor contate o administrador: " + Id, ex);
                return RedirectToAction("Index", "Produto", new { });
            }

            Alerta("Produto excluido com sucesso.", "success");
            return RedirectToAction("Index", "ItemVenda", new { });
        }
        private VendaItem FindItemVenda(Guid Id)
        {
            try
            {
                //return context.MedicamentoVariacao
                //            .AsNoTracking()
                //            .Include(e => e.Medicamento)
                //            .Where(e => e.Id == Id)
                //            .Single();

                return null;
            }
            catch (Exception ex)
            {
                Erro("Item da Venda não encontrado.", ex);
                return null;
            }
        }
    }
}

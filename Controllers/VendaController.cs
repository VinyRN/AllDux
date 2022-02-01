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
    public class VendaController : Controller
    {

        private ContentDbContext context;
        private UserManager<ApplicationUser> userManager;

        public VendaController(ContentDbContext _context, UserManager<ApplicationUser> _userManager)
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
        private void AdicionarVenda(Venda ObjVenda)
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Erro("Erro ao adicionar Venda " + ObjVenda.Id, ex);
            }
        }
        private List<Venda> ListaVenda()
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
                Erro("Erro buscando informações da Venda.", ex);
                return null;
            }
        }
        private Venda FindVenda(Guid Id)
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
                Erro("Venda não encontrada.", ex);
                return null;
            }
        }
        private void UpdateVenda(Guid Id, Venda objVendaNovo)
        {
            try
            {
                var VendaAtualizar = FindVenda(Id);
                //context.Medicamentos.Remove(ProdutoAtualizar);
                context.SaveChanges();

                objVendaNovo.Id = Id;
                //context.Medicamentos.Add(objProdutoNovo);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Erro("Erro ao editar venda, por favor contate o administrador.", ex);
            }
        }
        private VendaItem FindVendaItem(Guid VendaId)
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

        public IActionResult Index()
        {
            return View(ListaVenda());
        }
        public IActionResult Adicionar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Adicionar(Venda objVenda)
        {
            objVenda.Id = new Guid();

            try
            {
                AdicionarVenda(objVenda);
                Alerta("Venda " + objVenda.Id + " adicionada com sucesso.", "success");
                return RedirectToAction("Index", "Venda", new { });
            }
            catch (Exception ex)
            {
                Erro("Erro ao adicionar venda, por favor contate o administrador.", ex);
                return View(objVenda);
            }
        }
        public IActionResult Editar(string Id)
        {
            Venda objVenda = new Venda();

            try
            {
                //medicamentoEditar = FindMedicamentos(new Guid(Id));
            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro ao Editar a Venda, por favor contate o administrador.", ex);
                return RedirectToAction("Index", "Venda", new { });
            }

            return View(objVenda);
        }
        [HttpPost]
        public IActionResult Editar(Venda objVendaEditado)
        {
            Venda objVendaEdit = new Venda()
            {
                Id = Guid.Empty                
            };

            foreach (var VendaItem in objVendaEdit.VendaItemList)
            {
                //Confirmar o fluxo e ai colocar o codigo
            }

            try
            {
                UpdateVenda(objVendaEditado.Id, objVendaEdit);
            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro editando a venda, por favor contate o administrador", ex);
                return View();
            }

            return RedirectToAction("Index", "Venda", new { });
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
                Erro("Ocorreu um erro ao excluir a venda, por favor contate o administrador: " + Id, ex);
                return RedirectToAction("Index", "Venda", new { });
            }

            Alerta("Venda excluida com sucesso.", "success");
            return RedirectToAction("Index", "Venda", new { });
        }

    }
}

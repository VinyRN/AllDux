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
    public class MedicamentosController : Controller
    {
        private ContentDbContext context;
        private UserManager<ApplicationUser> userManager;

        public MedicamentosController(ContentDbContext _context, UserManager<ApplicationUser> _userManager)
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

        private void AdicionaMedicamento(Medicamento medicamento)
        {
            try
            {
                context.Medicamentos.Add(medicamento);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Erro("Erro ao adicionar medicamento " + medicamento.PrincipioAtivo, ex);
            }
        }

        private List<Medicamento> ListaMedicamentos()
        {
            try
            {
                return context.Medicamentos
                            .AsNoTracking()
                            .Include(e => e.Variacoes)
                            .OrderBy(e => e.PrincipioAtivo)
                            .ToList();
            }
            catch (Exception ex)
            {
                Erro("Erro buscando informações de medicamentos.", ex);
                return null;
            }
        }

        private Medicamento FindMedicamentos(Guid Id)
        {
            try
            {
                return context.Medicamentos
                            .AsNoTracking()
                            .Include(e => e.Variacoes.OrderBy(e => e.Nome))
                            .Where(e => e.Id == Id)
                            .Single();
            }
            catch (Exception ex)
            {
                Erro("Medicamento não encontrado.", ex);
                return null;
            }
        }

        private void UpdateMedicamento(Guid Id, Medicamento medicamentoNovo)
        {
            try
            {
                var medicamentosAtualizar = FindMedicamentos(Id);
                context.Medicamentos.Remove(medicamentosAtualizar);
                context.SaveChanges();

                medicamentoNovo.Id = Id;
                context.Medicamentos.Add(medicamentoNovo);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Erro("Erro ao editar medicamento, por favor contate o administrador.", ex);
            }
        }

        public IActionResult Index()
        {
            return View(ListaMedicamentos());
        }

        public IActionResult Adicionar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Adicionar(Medicamento medicamento)
        {
            medicamento.Id = new Guid();
            medicamento.CreatedDate = DateTime.Now;
            medicamento.LastUpdate = DateTime.Now;
            medicamento.CreatedUser = new Guid(userManager.GetUserAsync(HttpContext.User).Result.Id);

            try
            {
                AdicionaMedicamento(medicamento);
                Alerta("Medicamento " + medicamento.PrincipioAtivo + " adicionado com sucesso.", "success");
                return RedirectToAction("Index", "Medicamentos", new { });
            }
            catch (Exception ex)
            {
                Erro("Erro ao adicionar medicamento, por favor contate o administrador.", ex);
                return View(medicamento);
            }
        }

        public IActionResult Editar(string Id)
        {
            Medicamento medicamentoEditar = new Medicamento();

            try
            {
                medicamentoEditar = FindMedicamentos(new Guid(Id));
            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro ao Editar o Medicamento, por favor contate o administrador.", ex);
                return RedirectToAction("Index", "Medicamentos", new { });
            }

            return View(medicamentoEditar);
        }

        [HttpPost]
        public IActionResult Editar(Medicamento medicamentoEditado)
        {
            Medicamento medicamentoEdit = new Medicamento()
            {
                Id = Guid.Empty,
                PrincipioAtivo = medicamentoEditado.PrincipioAtivo,
                Descricao = medicamentoEditado.Descricao,
                DescricaoCurta = medicamentoEditado.DescricaoCurta,
                Variacoes = medicamentoEditado.Variacoes,
                CreatedDate = medicamentoEditado.CreatedDate,
                CreatedUser = medicamentoEditado.CreatedUser,
                LastUpdate = DateTime.Now,
                LastUpdateUser = new Guid(userManager.GetUserAsync(HttpContext.User).Result.Id)
            };

            foreach (var med in medicamentoEdit.Variacoes)
            {
                if ((med.PrecoMercado != null) || (med.PrecoAlldux != null))
                {
                    med.PrecoMercado = med.PrecoMercado.Replace(" ", "");
                    med.PrecoMercado = med.PrecoMercado.Trim().Replace("R$", "");

                    med.PrecoAlldux = med.PrecoAlldux.Replace(" ", "");
                    med.PrecoAlldux = med.PrecoAlldux.Replace("R$", "");
                }
            }

            try
            {
                UpdateMedicamento(medicamentoEditado.Id, medicamentoEdit);
            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro editando o Medicamento, por favor contate o administrador", ex);
                return View();
            }

            return RedirectToAction("Index", "Medicamentos", new { });
        }

        public IActionResult Excluir(string Id)
        {
            try
            {
                var medicamentoExcluir = context.Medicamentos.Where(e => e.Id == new Guid(Id)).Single();
                context.Medicamentos.Remove(medicamentoExcluir);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro ao excluir o Medicamento, por favor contate o administrador: " + Id, ex);
                return RedirectToAction("Index", "Medicamentos", new { });
            }

            Alerta("Medicamento excluido com sucesso.", "success");
            return RedirectToAction("Index", "Medicamentos", new { });
        }

        public IActionResult BrasindicePMC()
        {
            var medicamentos = context.BrasindicePMC.Select(
                                                x => new BrasindiceLista
                                                {
                                                    MedicamentoNome = x.MedicamentoNome,
                                                    MedicamentoCod = x.MedicamentoCod,
                                                    LaboratorioNome = x.LaboratorioNome,
                                                    ApresentacaoNome = x.ApresentacaoNome,
                                                    TISS = x.TISS
                                                }).ToList();

            return View(medicamentos);
        }

        public IActionResult BrasindicePFB()
        {
            var medicamentos = context.BrasindicePF.Select(
                                                x => new BrasindiceLista
                                                {
                                                    MedicamentoNome = x.MedicamentoNome,
                                                    MedicamentoCod = x.MedicamentoCod,
                                                    LaboratorioNome = x.LaboratorioNome,
                                                    ApresentacaoNome = x.ApresentacaoNome,
                                                    TISS = x.TISS
                                                }).ToList();

            return View(medicamentos);
        }

        public IActionResult BrasindiceImport()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BrasindiceImport(BrasindiceFile Brasindice)
        {
            try
            {
                if (Brasindice.File != null)
                {
                    using (var reader = new StreamReader(Brasindice.File.OpenReadStream()))
                    {
                        string line;
                        string tiss = "";
                        string verbo = "";
                        int count = 0;

                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] linha = line.Replace("\"", "|").Split("|,|");
                            linha = linha.Select(e => e.Replace("|", "")).ToArray();

                            if (linha[8] == "PMC")
                            {
                                BrasindicePMC medicamento = new BrasindicePMC(linha);
                                BrasindicePMC medicamentoExist = new BrasindicePMC();

                                try
                                {
                                    medicamentoExist = context.BrasindicePMC.Where(e => e.TISS == medicamento.TISS).Single();
                                }
                                catch
                                {
                                    medicamentoExist = null;
                                }

                                verbo = "adicionado";

                                if (medicamentoExist != null)
                                {
                                    context.BrasindicePMC.Remove(medicamentoExist);
                                    context.SaveChanges();
                                    verbo = "atualizado";
                                }

                                context.BrasindicePMC.Add(medicamento);
                                context.SaveChanges();

                                tiss = medicamento.TISS;
                            }
                            else if (linha[8] == "PFB")
                            {
                                BrasindicePF medicamento = new BrasindicePF(linha);
                                BrasindicePF medicamentoExist = new BrasindicePF();

                                try
                                {
                                    medicamentoExist = context.BrasindicePF.Where(e => e.TISS == medicamento.TISS).Single();
                                }
                                catch
                                {
                                    medicamentoExist = null;
                                }

                                if (medicamentoExist != null)
                                {
                                    context.BrasindicePF.Remove(medicamentoExist);
                                    context.SaveChanges();
                                }

                                context.BrasindicePF.Add(medicamento);
                                context.SaveChanges();
                                tiss = medicamento.TISS;
                            }

                            count++;
                            Console.WriteLine(count + " Medicamento cod " + linha[14] + " " + verbo);
                            Console.WriteLine("====================================================================");
                        }
                    }
                }
                else
                {
                    Alerta("Nenhum arquivo enviado.", "danger");
                    return View();
                }
            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro lendo o arquivo: " + ex.Message, ex);
                return View();
            }

            //gravar log de importacao
            //retornar o tempo que demorou e registros importados em outra view
            Alerta("Arquivo importado com sucesso.", "success");
            return View();
        }

        public IActionResult TNUMM()
        {
            var medicamentos = context.TNUMM.Select(
                                                x => new TNUMMLista
                                                {
                                                    MedicamentoNome = x.NomeApresentacao,
                                                    MedicamentoCod = x.Codigo,
                                                    LaboratorioNome = x.DetentorRegistroAnvisa,
                                                    TISS = x.TISS
                                                }).ToList();

            return View(medicamentos);
        }

        public IActionResult TNUMMImport()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TNUMMImport(TNUMMFile tnumm)
        {
            try
            {
                if (tnumm.File != null)
                {
                    using (var reader = new StreamReader(tnumm.File.OpenReadStream()))
                    {
                        string line;
                        string verbo = "";
                        int count = 0;

                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] linha = line.Split(";");
                            //linha = linha.Select(e => e.Replace("|", "")).ToArray();

                            TNUMM medicamento = new TNUMM(linha);
                            TNUMM medicamentoExist = new TNUMM();

                            try
                            {
                                medicamentoExist = context.TNUMM.Where(e => e.TISS == medicamento.TISS).Single();
                            }
                            catch
                            {
                                medicamentoExist = null;
                            }

                            verbo = "adicionado";

                            if (medicamentoExist != null)
                            {
                                context.TNUMM.Remove(medicamentoExist);
                                context.SaveChanges();
                                verbo = "atualizado";
                            }

                            context.TNUMM.Add(medicamento);
                            context.SaveChanges();

                            count++;
                            Console.WriteLine(count + " Medicamento cod " + linha[14] + " " + verbo);
                            Console.WriteLine("====================================================================");
                        }
                    }
                }
                else
                {
                    Alerta("Nenhum arquivo enviado.", "danger");
                    return View();
                }
            }
            catch (Exception ex)
            {
                Erro("Ocorreu um erro lendo o arquivo: " + ex.Message, ex);
                return View();
            }

            //gravar log de importacao
            //retornar o tempo que demorou e registros importados em outra view
            Alerta("Arquivo importado com sucesso.", "success");
            return View();
        }

        private MedicamentoVariacao FindFichaTecnicaMedicamentos(Guid Id)
        {
            try
            {
                return context.MedicamentoVariacao
                            .AsNoTracking()
                            .Include(e => e.Medicamento)
                            .Where(e => e.Id == Id)
                            .Single();
            }
            catch (Exception ex)
            {
                Erro("Medicamento não encontrado.", ex);
                return null;
            }
        }

        [Authorize(Policy = "Negociacoes")]
        [HttpGet]
        public IActionResult FichaTecnicaMedicamento(string id)
        {
            MedicamentoVariacao ficha = FindFichaTecnicaMedicamentos(new Guid(id));
            if(ficha.Medicamento.Descricao == "<br>") ficha.Medicamento.Descricao = "";
            return PartialView(ficha);
        }
    }
}
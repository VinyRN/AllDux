using System;
using System.Collections.Generic;
using System.Linq;
using alldux_plataforma.Data;
using alldux_plataforma.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace alldux_plataforma.Controllers
{
    [Authorize(Roles="Admin")]
    public class TesteController : Controller
    {
        private ContentDbContext context;
        private UserManager<ApplicationUser> userManager;

        public TesteController(ContentDbContext _context, UserManager<ApplicationUser> _userManager)
        {
            context = _context;
            userManager = _userManager;
        }

        public string Index(string Id)
        {
            //https://localhost:5001/Teste/?Id=

            var Guid = new Guid(Id);
            var diretriz = context.DiretrizPrecificadas
                                        .AsNoTracking()
                                        .Where(e => e.Id == Guid)
                                        .Include(e => e.DiretrizPrecificadaTabela)
                                        .ThenInclude(e => e.DiretrizPrecificadaRegistro)
                                        .Single();

            string NomeCurto = "";
            string NomeLongo = "";
            Guid TempGuid = Guid.Empty;

            foreach(var tabela in diretriz.DiretrizPrecificadaTabela){
                Console.WriteLine("TABELA: "+tabela.Id);
                Console.WriteLine("Nome curto: "+ tabela.ChaveTabelaReduzida);
                Console.WriteLine("Nome longo: "+ tabela.ChaveTabela);
                
                if(!String.IsNullOrEmpty(tabela.ChaveTabela) && !String.IsNullOrEmpty(tabela.ChaveTabelaReduzida)){
                    if(tabela.ChaveTabela.Length < tabela.ChaveTabelaReduzida.Length){
                        Console.WriteLine("Nome curto > longo");
                        NomeCurto = tabela.ChaveTabela;
                        NomeLongo = tabela.ChaveTabelaReduzida;
                        tabela.ChaveTabela = NomeLongo;
                        tabela.ChaveTabelaReduzida = NomeCurto;
                    
                        Console.WriteLine("=====================================================================================");
                        Console.WriteLine("TABELA: "+tabela.Id);
                        Console.WriteLine("Nome curto: "+ tabela.ChaveTabelaReduzida);
                        Console.WriteLine("Nome longo: "+ tabela.ChaveTabela);
                        Console.WriteLine("=====================================================================================");

                        UpdateTable(tabela);
                    }
                }

            }

            return "Teste "+Id;
        }

        private void UpdateTable(DiretrizPrecificadaTabela tabela){
            DiretrizPrecificadaTabela tabelaNova = new DiretrizPrecificadaTabela(){
                Id = new Guid(),
                Index = tabela.Index,
                Titulo = tabela.Titulo,
                ChaveTabela = tabela.ChaveTabela,
                ChaveTabelaReduzida = tabela.ChaveTabelaReduzida,
                Observacoes = tabela.Observacoes,
                Linha = tabela.Linha,
                Finalidade = tabela.Finalidade,
                DiretrizPrecificadaId = tabela.DiretrizPrecificadaId,
                DiretrizPrecificadaRegistro = tabela.DiretrizPrecificadaRegistro
            };
            UpdateDiretrizPrecTabela(tabela.Id, tabelaNova);
        }

        private void UpdateDiretrizPrecTabela(Guid Id, DiretrizPrecificadaTabela tabelaNova)
        {   
            var tabelaAtualizar = FindDiretrizesPrecTabela(Id);
            context.DiretrizPrecificadaTabela.Remove(tabelaAtualizar);
            context.SaveChanges();

            tabelaNova.Id = Id;
            context.DiretrizPrecificadaTabela.Add(tabelaNova);
            context.SaveChanges();
        }

        private DiretrizPrecificadaTabela FindDiretrizesPrecTabela(Guid? Id)
        {
            try{
                var tabela = new DiretrizPrecificadaTabela();
                tabela = context.DiretrizPrecificadaTabela
                            .AsNoTracking()
                            .Include(e => e.DiretrizPrecificadaRegistro.OrderBy(e => e.Index))
                            .Where(d => d.Id == Id)
                            .Single();

                //AssociaMedicamento(tabela);
                return tabela;
            }catch  (Exception ex){
                Erro("Erro listando detalhes da diretriz precificada, por favor contate o administrador.", ex);
                return null;
            }
        }

        public DiretrizPrecificadaTabela AssociaMedicamento(DiretrizPrecificadaTabela tabela)
        {
            foreach(var registro in tabela.DiretrizPrecificadaRegistro){
                if(!String.IsNullOrEmpty(registro.TISS)){
                    var medicamento = context.MedicamentoVariacao
                                                .AsNoTracking()
                                                .Where(e => e.TISS == registro.TISS)
                                                .FirstOrDefault();

                    registro.MedicamentoNome = medicamento.Nome;
                    registro.ValorCpMgAlldux = medicamento.PrecoAllduxMg();
                }else{
                    registro.ValorCpMgAlldux = 0;
                    Console.WriteLine("Medicamento não encontrado: "+registro.Id);
                }
            }
            return tabela;
        }

        private void Erro(string msg, Exception ex){
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
            Console.WriteLine("ERRO: "+msg);
            Console.WriteLine("----------------------------------------------------------------------------------------------------");
            Console.WriteLine(ex.Message);
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
            TempData["alerta"] = msg;
            TempData["tipo"] = "danger";
            TempData["exception"] = ex.Message;
        }

        public IActionResult Medicamentos(){
            List<Medicamento> pendencias = new List<Medicamento>();
            var medicamentos = context.Medicamentos
                                            .AsNoTracking()
                                            .Include(e => e.Variacoes.Where(i => i.UnMedida == null || i.UnApresentacao == null))
                                            .ToList();
            
            Console.WriteLine(medicamentos.Count());
            foreach(var item in medicamentos){
                foreach(var variacao in item.Variacoes){
                    Console.WriteLine(item.PrincipioAtivo+" - "+variacao.Nome+" : "+variacao.UnMedida+" / "+variacao.UnApresentacao);
                }
                
            }
            return View();
        }

        public string AdicionaTabela(string TabelaId, string DiretrizId){
            //https://localhost:5001/Teste/AdicionaTabela/?TabelaId=[]&DiretrizId=[]

            string retorno = "";
            
            if(!String.IsNullOrEmpty(TabelaId) && !String.IsNullOrEmpty(DiretrizId)){

                
                try{
                    var tabela = context.DiretrizPrecificadaTabela
                                            .AsNoTracking()
                                            .Where(e => e.Id == new Guid(TabelaId))
                                            .Include(e => e.DiretrizPrecificadaRegistro)
                                            .Single();
                    
                    retorno += $"Tabela encontrada. {tabela.Id} Index: {tabela.Index} Registros: {tabela.DiretrizPrecificadaRegistro.Count()} \n";

                    var Diretriz = context.DiretrizPrecificadas
                                            .AsNoTracking()
                                            .Where(e => e.Id == new Guid(DiretrizId))
                                            .Include(e => e.DiretrizPrecificadaTabela)
                                            .ThenInclude(e => e.DiretrizPrecificadaRegistro)
                                            .Single();

                    retorno += $"Diretriz encontrada. {Diretriz.Id} // Tabelas: {Diretriz.DiretrizPrecificadaTabela.Count()} \n";

                    //troca id da tabela para evitar conflito no banco e o index para ela ser a ultima
                    Guid NewId = Guid.NewGuid();
                    tabela.Id = NewId;
                    tabela.Index = Diretriz.DiretrizPrecificadaTabela.Count() + 1;
                    retorno += $"Tabela Atualizada para: {tabela.Id} Index: {tabela.Index} \n";

                    //troca id dos registros para evitar conflito no banco
                    foreach(var registro in tabela.DiretrizPrecificadaRegistro){ 
                        retorno += $"Atualizado registro: {registro.Id} > ";
                        registro.Id = Guid.NewGuid();
                        retorno += $"Registro atualizado: {registro.Id} \n";
                    }

                    //adiciona a tabela na diretriz
                    Diretriz.DiretrizPrecificadaTabela.Add(tabela);
                    retorno += $"Tabela adicinonada na Diretriz. {Diretriz.Id} // Tabelas: {Diretriz.DiretrizPrecificadaTabela.Count()} \n";

                    //troca id da diretriz e atualiza no banco
                    Diretriz.Id = Guid.NewGuid();
                    UpdateDiretrizPrec(new Guid(DiretrizId), Diretriz);
                    retorno += $"Tabela adicinonada na Diretriz com sucesso. \n";

                }catch{
                    retorno += $"ERRO!. \n";
                    return retorno;
                }

            }else{
                retorno += $"Informe os valores de TabelaId e DiretrizId. \n";
            }
            retorno += $"FIM! \n";
            return retorno;
        }

        private void UpdateDiretrizPrec(Guid Id, DiretrizPrecificada diretrizNova)
        {   
            try{
                var diretrizAtualizar = FindDiretrizesPrec(Id);
                context.DiretrizPrecificadas.Remove(diretrizAtualizar);
                context.SaveChanges();

                diretrizNova.Id = Id;
                context.DiretrizPrecificadas.Add(diretrizNova);
                context.SaveChanges();
            }catch(Exception ex){
                Console.WriteLine("Erro salvando tabela, por favor contate o administrador. "+ex);
                RedirectToAction("Index", "Diretrizes", new {});
            }
        }

        private DiretrizPrecificada FindDiretrizesPrec(Guid Id)
        {
            try{
                var Diretriz = context.DiretrizPrecificadas
                                .AsNoTracking()
                                .Where(e => e.Id == Id)
                                .Include(e => e.DiretrizPrecificadaTabela.OrderBy(e => e.Index))
                                .ThenInclude(e => e.DiretrizPrecificadaRegistro.OrderBy(e => e.Index))
                                .Single();
                return Diretriz;
            }catch  (Exception ex){
                Console.WriteLine("FindDiretrizesPrec" +ex);
                return null;
            }
        }

        public string recuperaDiretriz(string Id)
        {
            //https://localhost:5001/Teste/recuperaDiretriz/?Id=

            // var diretriz = contextBKP.DiretrizPrecificadas
            //                             .AsNoTracking()
            //                             .Where(e => e.Id == new Guid(Id))
            //                             .Include(e => e.DiretrizPrecificadaTabela)
            //                             .ThenInclude(e => e.DiretrizPrecificadaRegistro)
            //                             .Single();

            // context.DiretrizPrecificadas.Add(diretriz);
            // context.SaveChanges();

            return $"Diretriz = {Id} encontrada";
        }
    }
}
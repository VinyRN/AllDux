using System;
using System.Linq;
using Microsoft.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using alldux_plataforma.Data;
using alldux_plataforma.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace alldux_plataforma.Controllers
{
    [Authorize(Policy = "FerramentasAnalises")]
    public class FerramentasAnaliseController : Controller
    {
        private ContentDbContext context;
        private UserManager<ApplicationUser> userManager;

        public FerramentasAnaliseController(ContentDbContext _context, UserManager<ApplicationUser> _userManager)
        {
            context = _context;
            userManager = _userManager;
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

        private List<DiretrizesClinicas> ListaProtocolos(){
            var appUserRoles = userManager.GetRolesAsync(userManager.GetUserAsync(HttpContext.User).Result).Result.FirstOrDefault();
            List<DiretrizesClinicas> protocolos = new List<DiretrizesClinicas>();
            List<string> protocolosTrial = new List<string>();
                protocolosTrial.Add("5ef278e8-a2bd-48cd-bf28-0120e64eb4d7"); //Mama
                protocolosTrial.Add("85316b30-6fd2-41e2-9569-467e56db1678"); //Colon
                protocolosTrial.Add("556a08a3-e742-491d-8e38-3577a44ee959"); //Pulmao

            try{
            
                if(appUserRoles == "TestUser"){
                    protocolos = context.DiretrizesClinicas
                                                .AsNoTracking()
                                                .Where(r => protocolosTrial.Contains(r.Id.ToString()))
                                                .OrderBy(e => e.Titulo)
                                                .Include(e => e.DiretrizPrecificada)
                                                .ThenInclude(e => e.DiretrizPrecificadaTabela.OrderBy(e => e.ChaveTabela))
                                                .ToList();
                }else{
                    protocolos = context.DiretrizesClinicas
                                                .AsNoTracking()
                                                .OrderBy(e => e.Titulo)
                                                .Include(e => e.DiretrizPrecificada)
                                                .ThenInclude(e => e.DiretrizPrecificadaTabela.OrderBy(e => e.ChaveTabela))
                                                .ToList();
                };
            
            }catch(Exception ex){
                Erro("Erro listando protocolos", ex);
                protocolos = null;
            }
            return protocolos;
        }

        private double PrecoComparacaoFrac(string preco, string unMedida, string unApresentacao){
            double dPreco = Convert.ToDouble(preco.Replace(".", ","));
            double DunMedida = Convert.ToDouble(unMedida);
            double DunApresentacao = Convert.ToDouble(unApresentacao);
            double resultado = (dPreco/DunMedida)/DunApresentacao;
            return resultado;
        }

        private DiretrizPrecificadaTabela GetTabela(string Id, string compara, double SC, int Peso, string AUC){
            DiretrizPrecificadaTabela tabela = new DiretrizPrecificadaTabela();
            
            try{
                tabela = context.DiretrizPrecificadaTabela
                                                        .AsNoTracking()
                                                        .Where(e => e.Id == new Guid(Id))
                                                        .Include(e => e.DiretrizPrecificadaRegistro.OrderBy(e => e.Index))
                                                        .Single();
            }catch(Exception ex){
                Erro("Erro procurando tabelas", ex);
                return null;
            }

            foreach(var registro in tabela.DiretrizPrecificadaRegistro){
                MedicamentoVariacao medicamento = new MedicamentoVariacao();

                try{
                    if(!String.IsNullOrEmpty(registro.TISS)){
                        //carrega dados do medicamento
                        medicamento = context.MedicamentoVariacao
                                                    .AsNoTracking()
                                                    .Where(e => e.TISS == registro.TISS)
                                                    .Include(r => r.Medicamento)
                                                    .FirstOrDefault();
                        registro.MedicamentoNome = medicamento.Nome;
                        registro.MedicamentoPrincipioAtivo = medicamento.Medicamento.PrincipioAtivo;
                        registro.ValorCpMgAlldux = medicamento.PrecoAllduxMg();
                        
                        //trocando o SC e Peso dos registros pelo calculado pela ferramenta
                        if(registro.ScPeso == "1.7") registro.ScPeso = SC.ToString();
                        if(registro.ScPeso == "70") registro.ScPeso = Peso.ToString();
                        if(registro.AUCAlvo() != 0 && !String.IsNullOrEmpty(AUC)){ //acerto na Mgm2 de Carboplatina
                            registro.Mgm2 = AUC;
                        }
                        
                        if(compara == "PMC"){
                            var comparacao = context.BrasindicePMC.AsNoTracking().Where(e => e.TISS == registro.TISS).FirstOrDefault();
                            if(comparacao == null || Convert.ToDouble(comparacao.PrecoFracionado) == 0){
                                //nao encontrou medicamento
                                registro.PrecoComparacao = MedicamentoPadrao(
                                                                medicamento.PrecoMercado, 
                                                                medicamento.UnMedida,
                                                                medicamento.UnApresentacao,
                                                                medicamento.Nome,
                                                                medicamento.TISS,
                                                                tabela);
                            }else{
                                registro.PrecoComparacao = PrecoComparacaoFrac(comparacao.PrecoFracionado.Replace(".", ","), "1", medicamento.UnApresentacao);
                            }
                        }else if(compara == "PFB"){
                            var comparacao = context.BrasindicePF.AsNoTracking().Where(e => e.TISS == registro.TISS).FirstOrDefault();
                            if(comparacao == null || Convert.ToDouble(comparacao.PrecoFracionado) == 0){
                                //nao encontrou medicamento
                                registro.PrecoComparacao = MedicamentoPadrao(
                                                                medicamento.PrecoMercado, 
                                                                medicamento.UnMedida,
                                                                medicamento.UnApresentacao,
                                                                medicamento.Nome,
                                                                medicamento.TISS,
                                                                tabela);
                            }else{
                                registro.PrecoComparacao = PrecoComparacaoFrac(comparacao.PrecoFracionado.Replace(".", ","), "1", medicamento.UnApresentacao);
                            }
                        }else if(compara == "TNUMM"){
                            var comparacao = context.TNUMM.AsNoTracking().Where(e => e.TISSBrasindice == registro.TISS).FirstOrDefault();
                            if(comparacao == null || Convert.ToDouble(comparacao.PrecoMaxNacional) == 0){
                                //nao encontrou medicamento
                                registro.PrecoComparacao = MedicamentoPadrao(
                                                                medicamento.PrecoMercado, 
                                                                medicamento.UnMedida,
                                                                medicamento.UnApresentacao,
                                                                medicamento.Nome,
                                                                medicamento.TISS,
                                                                tabela);                                
                            }else{
                                registro.PrecoComparacao = PrecoComparacaoFrac(comparacao.PrecoMaxNacional.Replace(".", ","), "1", medicamento.UnApresentacao);
                                var diferenca = registro.ValorCpMgAlldux / registro.PrecoComparacao;
                                if(diferenca > 2) registro.PrecoComparacao = Convert.ToDouble(comparacao.PrecoMaxNacional); //checa se o valor de TNUMM ja veio fracionado
                            }
                        }
                    }else{
                        if(registro.Ordem == "#quebra#"){ 
                            Console.WriteLine("Quebra encontrada");
                        }else{
                            Erro("Medicamento sem TISS cadastrado: "+registro.Id, null);
                        }
                    }

                }catch(Exception ex){
                    Erro("Erro preenchendo informacoes sobre medicamento:"+registro.TISS+" - Tabela: "+compara, ex);
                }

            }//for
            
            return tabela;
        }

        private double MedicamentoPadrao(string preco, string UnMedida, string UnApresentacao, string Nome, string TISS, DiretrizPrecificadaTabela tabela){
            //executado quando a busca pelo medicamento na tabela de comparacao nao encontra o medicamento.
            var precoMercado = Convert.ToDouble(preco) * 1.30; //ajusta o valor para PrecoMercado + 30%
            var precoFracionado = PrecoComparacaoFrac(precoMercado.ToString().Replace(".", ","), UnMedida, UnApresentacao);
            //coloca o medicamento na lista de nao encontrados
            if(!tabela.MedicamentosNaoEncontrados.Contains(new KeyValuePair<string, string>(Nome,TISS))) tabela.MedicamentosNaoEncontrados.Add(new KeyValuePair<string, string>(Nome, TISS));

            Console.WriteLine("Medicamento de comparação nao encontrado, utilizando o valor de PrecoMercado: "+ Nome + " TISS: "+TISS); 
            return precoFracionado;
        }

        public IActionResult Index()
        {
            // var appUserRoles = userManager.GetRolesAsync(userManager.GetUserAsync(HttpContext.User).Result).Result.FirstOrDefault();
            // if(appUserRoles == "TestUser") return RedirectToAction("AcessoRestrito", "Home");
            
            return RedirectToAction("FerramentaAnaliseCusto");
        }

        public IActionResult FerramentaAnaliseCusto()
        {
            ViewBag.ListaProtocolos = ListaProtocolos();
            FerramentaAnaliseCusto modelo = new FerramentaAnaliseCusto();
            return View(modelo);
        }

        [HttpPost]
        public KeyValuePair<string, string>[] CarregaSelectTabela(string Id){
            Guid DiretrizId = new Guid(Id);
            List<KeyValuePair<string, string>> resposta = new List<KeyValuePair<string, string>>();
            List<DiretrizPrecificadaTabela> listaTabelas = new List<DiretrizPrecificadaTabela>();
            var appUserRoles = userManager.GetRolesAsync(userManager.GetUserAsync(HttpContext.User).Result).Result.FirstOrDefault();
            List<string> protocolosTrial = new List<string>();
            protocolosTrial.Add("67a45b35-70fd-4e01-8fa3-cc4cfd76b809");// mama
            protocolosTrial.Add("653cfda9-e76c-4f98-aac3-84f4ff34d444");// mama
            protocolosTrial.Add("3c67c6fc-4ed7-4609-87df-31cf600a28ec");// mama
            protocolosTrial.Add("953811f9-4028-42c6-bd87-a67ba8881dd8");//pulmao
            protocolosTrial.Add("720d0c3b-662c-4ff8-9690-dbf30b438f62");//pulmao
            protocolosTrial.Add("449084a7-27fd-45ff-a92c-20e610c22a77");//colon
            protocolosTrial.Add("2e792772-28eb-4fc1-aa38-a541a922a3ae");//colon
            protocolosTrial.Add("a5ba215f-da06-4d86-b8a1-e587aa27f36b");//colon


            if(appUserRoles == "TestUser"){
                listaTabelas =  context.DiretrizPrecificadaTabela
                                                        .AsNoTracking()
                                                        .Where(e => e.DiretrizPrecificadaId ==  DiretrizId)
                                                        .Where(e => protocolosTrial.Contains(e.Id.ToString()))
                                                        .OrderBy(e => e.ChaveTabelaReduzida)
                                                        .ToList();
            }else{
                listaTabelas =  context.DiretrizPrecificadaTabela
                                                        .AsNoTracking()
                                                        .Where(e => e.DiretrizPrecificadaId ==  DiretrizId)
                                                        .OrderBy(e => e.ChaveTabelaReduzida)
                                                        .ToList();
            }

            foreach(var item in listaTabelas){
                resposta.Add(new KeyValuePair<string, string>(item.Id.ToString(),item.ChaveTabelaReduzida));
            }
            return resposta.ToArray();
        }

        [HttpPost]
        public IActionResult FerramentaAnaliseCusto(FerramentaAnaliseCusto model)
        {
            if(!ModelState.IsValid){
                ViewBag.ListaProtocolos = ListaProtocolos();
                return View(model);
            }

            return RedirectToAction("FerramentaAnaliseCusto_Relatorio", model);
        }

        public IActionResult FerramentaAnaliseCusto_Relatorio(FerramentaAnaliseCusto model){
            DiretrizPrecificadaTabela tabela = GetTabela(model.DiretrizPrecificadaTabelaId.ToString(), model.Brasindice, model.SC(), model.Peso, model.AUC);
            model.tabela = tabela;

            return View(model);
        }
    }
}
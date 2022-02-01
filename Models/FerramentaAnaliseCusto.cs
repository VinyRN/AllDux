using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using alldux_plataforma.Models.Enums;

namespace alldux_plataforma.Models
{
    public class FerramentaAnaliseCusto
    {  
        [Required(ErrorMessage = "Selecione um Protocolo.")]
        public Guid? DiretrizPrecificadaId { get; set; }

        public string DiretrizNome { get; set; }

        [Required(ErrorMessage = "Selecione uma Tabela.")]
        public Guid? DiretrizPrecificadaTabelaId { get; set; }

        [Required(ErrorMessage = "Informe o peso (kg) do paciente.")]
        public int Peso { get; set; }
        
        [Required(ErrorMessage = "Informe a altura (cm) do pacinete.")]
        public int Altura { get; set; }
        public Guid TipoAcesso { get; set; }
        public List<TiposDeAcesso> ListaTiposAcesso = new List<TiposDeAcesso>(){
            new TiposDeAcesso("896b2efc-a6ea-418e-933f-3e528ec1cb95", "Kit punção periférica", 8.64, 44.94),
            new TiposDeAcesso("ab6e60b4-119b-4e45-9bb6-5a5d4225bdb5", "Kit PORT A CATH", 43.18, 480.18),
            new TiposDeAcesso("8770c5de-9c26-4eee-bdb0-f5df8b2242d8", "Kit intravesical", 11.22, 65.50),
            new TiposDeAcesso("659d0a70-6ebe-487c-bb61-80a57ce0378d", "Kit Subcutânea", 1.6, 5.88),
            new TiposDeAcesso("28644eb9-7d3a-487b-bf58-4137b8d0dd72", "Via Oral", 0, 0),
        };
        
        public double? CustoOperacionalPercent { get; set; }
        public double? CustoOperacionalCalculado { get; set; }

        [Required(ErrorMessage = "Informe o estado (UF).")]
        public string UF { get; set; }
        public string Brasindice { get; set; }
        public string PercentPF { get; set; }
        public string AUC { get; set; }

        public DiretrizPrecificadaTabela tabela { get; set; }

        public double SC() {
            return 0.007184 * Math.Pow(Altura, 0.725) * Math.Pow(Peso, 0.425);
        }

        public double TipoAcessoCustoAlldux(Guid Id){
            TiposDeAcesso tipoAcesso = ListaTiposAcesso.Where(e => e.Id == Id).FirstOrDefault();
            return tipoAcesso.PrecoAlldux;
        }

        public double TipoAcessoCustoComparacao(Guid Id){
            TiposDeAcesso tipoAcesso = ListaTiposAcesso.Where(e => e.Id == Id).FirstOrDefault();
            return tipoAcesso.PrecoComparacao;
        }
        public string TipoAcessoNome(Guid Id){
            TiposDeAcesso tipoAcesso = ListaTiposAcesso.Where(e => e.Id == Id).FirstOrDefault();
            return tipoAcesso.Nome;
        }

        public string ComparativoNome(string tipo){
            if(tipo == "PFB") return "Brasíndice PFB";
            if(tipo == "PMC") return "Brasíndice PMC";
            if(tipo == "TNUMM") return "TNUMM";
            return "";
        }

        public double PrecoCiclosAlldux(){
            double preco = tabela.ValorCicloTotal();
            double uf = 0; //CalculoImposto(preco);
            double custoOpe = CustoOperacional(preco);
            double tipoAcessoAlldux = tabela.CalculaTiposAcessoCiclo() * TipoAcessoCustoAlldux(TipoAcesso);

            double result = preco + uf + custoOpe + tipoAcessoAlldux;
            return result;
        }

        public double PrecoFinalAlldux(){
            double preco = tabela.ValorTotalTotal();
            double uf = 0; //CalculoImposto(preco);
            double custoOpe = CustoOperacional(preco);
            double tipoAcessoAlldux = tabela.CalculaTipoAcessoTotal() * TipoAcessoCustoAlldux(TipoAcesso);

            double result = preco + uf + custoOpe + tipoAcessoAlldux;
            return result;
        }

        public double PrecoCiclosComparacao(){
            double preco = tabela.ValorCicloTotal_Comparacao();
            double uf = CalculoImposto(preco);
            double custoOpe = CustoOperacional(preco);
            double tipoAcessoAlldux = tabela.CalculaTiposAcessoCiclo() * TipoAcessoCustoComparacao(TipoAcesso);
            
            double result = preco + uf + custoOpe + tipoAcessoAlldux;
            return result;
        }

        public double PrecoFinalComparacao(){
            double preco = tabela.ValorTotalTotal_Comparacao();
            double uf = CalculoImposto(preco);
            double custoOpe = CustoOperacional(preco);
            double tipoAcessoAlldux = tabela.CalculaTipoAcessoTotal() * TipoAcessoCustoComparacao(TipoAcesso);

            double result = preco + uf + custoOpe + tipoAcessoAlldux;
            return result;
        }

        public double ImpostoUF(){
            //imposto
            string[] imp1 = {"AC", "AL", "DF", "ES", "GO", "MT", "MS", "PA", "RO", "SC"}; //17
            string[] imp2 = {"RO", "RS"}; //17,5
            string[] imp3 = {"AP", "AM", "BA", "CE", "MA", "MG", "PA", "PR", "PE", "PI", "RN", "SP", "SE", "TO"}; //18
            string[] imp4 = {"RJ"}; //20

            if(imp1.Contains(UF)) return 17;
            if(imp2.Contains(UF)) return 17.5;
            if(imp3.Contains(UF)) return 18;
            if(imp4.Contains(UF)) return 20;

            return 0;
        }

        public double CalculoImposto(double valor){
            double uf = ImpostoUF()/100;
            return valor * uf;
        }

        public double MargemCiclos(){
            double ciclosComparacao = PrecoCiclosComparacao();
            double ciclosAlldux = PrecoCiclosAlldux();
            double total =  ciclosComparacao - ciclosAlldux;

            return total;
        }

        public double MargemTotal(){
            double total = PrecoFinalComparacao() - PrecoFinalAlldux();
            return total;
        }
        
        public double MargemTotalPercent(){
            double total = PrecoFinalComparacao() - PrecoFinalAlldux();
            total = (total * 100)/PrecoFinalComparacao();

            return total;
        }

        public double CustoOperacional(double preco){
            if(CustoOperacionalPercent != null && CustoOperacionalPercent != 0) {
                var custo = Convert.ToDouble(CustoOperacionalPercent)/100;
                return preco * custo;
            }

            if(CustoOperacionalCalculado != null && CustoOperacionalCalculado != 0) {
                return Convert.ToDouble(CustoOperacionalCalculado);
            }
                
            return 0;
        }

        public string MostraAUC(){
            if(!String.IsNullOrEmpty(AUC)){
                //Dose de Carbo Preenchida
                return AUC;
            }
            var doseCarbo = tabela.MostraAUC() * 100;
            return doseCarbo.ToString();
        }
    }
}
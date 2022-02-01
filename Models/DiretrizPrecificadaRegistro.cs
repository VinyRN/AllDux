using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace alldux_plataforma.Models
{
    public class DiretrizPrecificadaRegistro
    {
        [Key]
        public Guid Id { get; set; }

        public int Index { get; set; }

        [MaxLength(20)]
        public string Ordem { get; set; }

        [MaxLength(15)]
        public string TISS { get; set; } //Id da variacao de medicamento
        
        [MaxLength(150)]
        public string Medicamento { get; set; } //Apresentação do medicamento cadastrado nas diretrizes

        [MaxLength(10)]
        public string Mgm2 { get; set; }

        [MaxLength(3)]
        public string DiasCiclo { get; set; }

        [MaxLength(4)]
        public string ScPeso { get; set; }

        [MaxLength(5)]
        public string CicloTotal { get; set; }
        
        [NotMapped] public string MedicamentoNome { get; set; } //vindo do banco de medicamentos no FindDiretrizesPrec();
        [NotMapped] public string MedicamentoPrincipioAtivo { get; set; } //vindo do banco de medicamentos no FindDiretrizesPrec();
        [NotMapped] public double PrecoComparacao { get; set; } //preenchido quando a entidade é carregada
        [NotMapped] public double ValorCpMgAlldux { get; set; } //vindo calculado do banco de medicamentos no FindDiretrizesPrec();
        
        public Guid DiretrizPrecificadaTabelaId { get; set; }
        public virtual DiretrizPrecificadaTabela DiretrizPrecificadaTabela { get; set; }
        
        //valores descontinuados porque foram substituidos por metodos.
        //public string DoseFinal { get; set; } //CalcDoseFinal()
        //public string ValorCiclo { get; set; } //CalcValorCiclo()
        //public string ValorTotal { get; set; } //CalcValorTotal()

        public double CalcDoseFinal() //Calcula a dose final
        {
            try{  
                if(Ordem == "#quebra#") return 0;
                double mgm2 = Double.Parse(Mgm2, CultureInfo.InvariantCulture);
                double diasCiclo = Double.Parse(DiasCiclo, CultureInfo.InvariantCulture);
                double scPeso = Double.Parse(ScPeso.Replace(",", "."), CultureInfo.InvariantCulture);
                double total = mgm2 * diasCiclo * scPeso;
                return Math.Round(total, 2);
             }catch(Exception ex){
                Console.WriteLine("Erro calculando valores (CalcDoseFinal): "+ex.Message);
                return 0;
            }
        }
        public double CalcValorCiclo() //valor do ciclo alldux
        {
            try{
                if(Ordem == "#quebra#") return 0;
                double doseFinal = CalcDoseFinal();
                double total = doseFinal * ValorCpMgAlldux;
                return Math.Round(total, 2);
            }catch(Exception ex){
                Console.WriteLine("Erro calculando valores (CalcValorCiclo): "+ex.Message);
                return 0;
            }
        }
        public double CalcValorTotal() //valor total alldux
        {
            try{
                if(Ordem == "#quebra#") return 0;
                double cicloTotal = Double.Parse(CicloTotal, CultureInfo.InvariantCulture);
                double total = cicloTotal * CalcValorCiclo();
                return Math.Round(total, 2);
            }catch(Exception ex){
                Console.WriteLine("Erro calculando valores (CalcValorTotal): "+ex.Message);
                return 0;
            }
        }

        public double Comparacao_CalcValorCiclo() // valor do ciclo com o preco de comparacao
        {
            try{
                if(Ordem == "#quebra#") return 0;
                double doseFinal = CalcDoseFinal();
                double total = doseFinal * PrecoComparacao;
                return Math.Round(total, 2);
             }catch(Exception ex){
                Console.WriteLine("Erro calculando valores: "+ex.Message);
                return 0;
            }
        }

        public double Comparacao_CalcValorTotal()//valor total com o preco de comparacao
        {
            try{
                if(Ordem == "#quebra#") return 0;
                double cicloTotal = Double.Parse(CicloTotal, CultureInfo.InvariantCulture);
                double total = cicloTotal * Comparacao_CalcValorCiclo();
                return Math.Round(total, 2);
            }catch(Exception ex){
                Console.WriteLine("Erro calculando valores (Comparacao_CalcValorTotal): "+ex.Message);
                return 0;
            }
        }

        public double AUCAlvo(){
            if(!String.IsNullOrEmpty(MedicamentoPrincipioAtivo) && !String.IsNullOrEmpty(Mgm2)){
                if(MedicamentoPrincipioAtivo == "Carboplatina" && ((Mgm2 == "200") || (Mgm2 == "600"))){
                    return (Convert.ToDouble(Mgm2)/100);
                } 
            }
            return 0;
        }

    }
}
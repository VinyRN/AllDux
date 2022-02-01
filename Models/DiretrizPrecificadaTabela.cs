using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alldux_plataforma.Models
{
    public class DiretrizPrecificadaTabela
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        public int Index { get; set; }

        [MaxLength(250)]
        public string Titulo { get; set; }

        [MaxLength(800)]
        public string ChaveTabela { get; set; }

        [MaxLength(500)]
        public string ChaveTabelaReduzida { get; set; }

        [MaxLength(6000)]
        public string Observacoes { get; set; }

        [MaxLength(40)]
        public string Linha { get; set; }

        [MaxLength(40)]
        public string Finalidade { get; set; }

        [NotMapped] //Lista de Medicamentos não encontrados
        public ICollection<KeyValuePair<string, string>> MedicamentosNaoEncontrados { get; set; } = new List<KeyValuePair<string, string>>(); 
        
        [ForeignKey("DiretrizPrecificada")]
        public Guid DiretrizPrecificadaId { get; set; }
        public ICollection<DiretrizPrecificadaRegistro> DiretrizPrecificadaRegistro { get; set; } = new List<DiretrizPrecificadaRegistro>();
        
        public virtual DiretrizPrecificada DiretrizPrecificada { get; set; }

        public double ValorCicloTotal(){ //Total por ciclo da tabela
            double total = 0;
            foreach(var item in DiretrizPrecificadaRegistro){
                if(item.TISS != null){
                    var valor = item.CalcValorCiclo();
                    total += valor;
                }
            }
            return total;
        }

        public double ValorTotalTotal(){ //Total geral da tabela
            double total = 0;
            foreach(var item in DiretrizPrecificadaRegistro){
                if(item.TISS != null){
                    var valor = item.CalcValorTotal();
                    total += valor;
                }
            }
            return total;
        }

        public double ValorCicloTotal_Comparacao(){ //Total por ciclo da tabela com preco de comparacao
            double total = 0;
            foreach(var item in DiretrizPrecificadaRegistro){
                if(item.TISS != null){
                    var valor = item.Comparacao_CalcValorCiclo();
                    total += valor;
                }
            }
            return total;
        }

        public double ValorTotalTotal_Comparacao(){ //Total geral da tabela com preco de comparacao
            double total = 0;
            foreach(var item in DiretrizPrecificadaRegistro){
                if(item.TISS != null){
                    var valor = item.Comparacao_CalcValorTotal();
                    total += valor;
                }
            }
            return total;
        }

        public double CalculaTiposAcessoCiclo(){
            double total = 0;
            foreach(var item in DiretrizPrecificadaRegistro){
                if(item.Ordem == "#quebra#"){
                    total ++;
                }
            }

            return total;
        }

        public double CalculaTipoAcessoTotal(){
            double linhaIndex = 0;
            double linhaTotal = 0;
            double total = 0;
            foreach(var item in DiretrizPrecificadaRegistro){
                if(item.Ordem != "#quebra#"){
                    linhaIndex++;
                    linhaTotal += Convert.ToDouble(item.CicloTotal);
                }else{
                    total += linhaTotal/linhaIndex;
                    linhaIndex = 0;
                    linhaTotal = 0;
                }
            }
            return total;
        }

        public double MostraAUC(){
            foreach(var item in DiretrizPrecificadaRegistro){
                if(item.AUCAlvo() != 0){
                    return item.AUCAlvo();
                }
            }
            return 0;
        }

    }
}
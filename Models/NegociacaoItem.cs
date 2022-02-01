using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;

namespace alldux_plataforma.Models
{
    public class NegociacaoItem
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        [MaxLength(36)]
        public string MedicamentoId { get; set; }
        
        [MaxLength(20)]
        public string PrecoDesconto { get; set; }

        [ForeignKey("Negociacao")]
        public Guid NegociacaoId { get; set; }

        public virtual Negociacao Negociacao { get; set; }

        // public string Texto { get; set; }
        // public string PrecoAlternativo { get; set; }

        //dados do medicamento
        [NotMapped] public string Nome { get; set; }
        [NotMapped] public string PrincipioAtivo { get; set; }
        [NotMapped] public string Laboratorio { get; set; }
        [NotMapped] public string Distribuidor { get; set; }
        [NotMapped] public string UnApresentacao { get; set; }
        [NotMapped] public string UnApresentacaoTipo { get; set; }
        [NotMapped] public string UnMedida { get; set; }
        [NotMapped] public string UnMedidaTipo { get; set; }
        [NotMapped] public double PrecoMercado { get; set; }
        [NotMapped] public double PrecoAlldux { get; set; }
        [NotMapped] public double Margem { get; set; }
        [NotMapped] public string TISS { get; set; }

        public void AddMedicamentoInfo(MedicamentoVariacao Medicamento){
            var Variacao = Medicamento;
            
            Nome = Variacao.Nome;
            PrincipioAtivo = Variacao.Medicamento.PrincipioAtivo;
            Laboratorio = Variacao.Laboratorio;
            Distribuidor = Variacao.Distribuidor;
            UnApresentacao = Variacao.UnApresentacao;
            UnApresentacaoTipo = Variacao.UnApresentacaoTipo;
            UnMedida = Variacao.UnMedida;
            UnMedidaTipo = Variacao.UnMedidaTipo;
            Console.WriteLine(MedicamentoId +" - "+Variacao.PrecoMercado);
            Console.WriteLine(MedicamentoId +" - "+Variacao.PrecoAlldux);
            Console.WriteLine("==================================================================");
            try{
                PrecoMercado = Convert.ToDouble(Variacao.PrecoMercado);
                PrecoAlldux = Convert.ToDouble(Variacao.PrecoAlldux);
                Margem = 100 - ((PrecoAlldux * 100)/PrecoMercado);
            }catch{
                PrecoMercado = 0;
                PrecoAlldux = 0;
                Margem = 0;
            }
            TISS = Variacao.TISS;
        }

    }
}
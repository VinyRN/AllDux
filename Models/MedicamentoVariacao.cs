using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace alldux_plataforma.Models
{
    public class MedicamentoVariacao
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        [MaxLength(50)]
        public string Nome { get; set; }

        [MaxLength(50)]
        public string Laboratorio { get; set; }

        [MaxLength(50)]
        public string Distribuidor { get; set; }

        [MaxLength(5)]
        public string UnMedida { get; set; }

        [MaxLength(5)]
        public string UnMedidaTipo { get; set; }

        [MaxLength(15)]
        public string UnApresentacao { get; set; }

        [MaxLength(5)]
        public string UnApresentacaoTipo { get; set; }

        [MaxLength(50)]
        public string Tipo { get; set; }

        [MaxLength(20)]
        public string PrecoMercado { get; set; }

        [MaxLength(20)]
        public string PrecoAlldux { get; set; }

        [MaxLength(50)]
        public string Familia { get; set; }

        [MaxLength(50)]
        public string Classe { get; set; }

        [MaxLength(50)]
        public string Subclasse { get; set; }
        
        public bool Padronizado { get; set; }

        public bool Generico { get; set; }

        public bool Biossimilar { get; set; }

        public bool Referencia { get; set; }

        [MaxLength(20)]
        public string TISS { get; set; }

        [ForeignKey("Medicamento")]
        public Guid MedicamentoId { get; set; }
        
        public virtual Medicamento Medicamento { get; set; }

        public double PrecoAllduxMg(){
            try{
                double precoTotal = Double.Parse(PrecoAlldux.Replace(".", "").Replace(",", "."), CultureInfo.InvariantCulture);
                double unMedida = Double.Parse(UnMedida, CultureInfo.InvariantCulture);
                double unApres = Double.Parse(UnApresentacao, CultureInfo.InvariantCulture);
                double precoMg = (precoTotal / unMedida) / unApres;
                double result = Math.Round(precoMg, 3, MidpointRounding.ToPositiveInfinity);

                return result;
            }catch{
                Console.WriteLine("Erro calculando Preco p/mg do medicamento ("+Nome+" // "+Id);
                return 0;
            }
        }
        
    }
}
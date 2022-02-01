using System;
using System.ComponentModel.DataAnnotations;

namespace alldux_plataforma.Models
{
    public class BrasindicePF
    {   
        public BrasindicePF(){}

        public BrasindicePF(string[] linha){
            LaboratorioCod = linha[0];
            LaboratorioNome = linha[1];
            MedicamentoCod = linha[2];
            MedicamentoNome = linha[3];
            ApresentacaoCod = linha[4];
            ApresentacaoNome = linha[5];
            Preco = linha[6];
            QtdFracionamento = linha[7];
            PrecoTipo = linha[8];
            PrecoFracionado = linha[9];
            EdicaoAltPreco = linha[10];
            IPI = linha[11];
            FlagPisCofins = linha[12];
            EAN = linha[13];
            TISS = linha[14];
            Generico = linha[15];
            TUSS = linha[16];
        }

        [MaxLength(5)]
        public string LaboratorioCod { get; set; }

        [MaxLength(50)]
        public string LaboratorioNome { get; set; }

        [MaxLength(5)]
        public string MedicamentoCod { get; set; }

        [MaxLength(100)]
        public string MedicamentoNome { get; set; }

        [MaxLength(6)]
        public string ApresentacaoCod { get; set; }

        [MaxLength(200)]
        public string ApresentacaoNome { get; set; }

        [MaxLength(12)]
        public string Preco { get; set; }
        
        [MaxLength(5)]
        public string QtdFracionamento { get; set; }

        [MaxLength(5)]
        public string PrecoTipo { get; set; }

        [MaxLength(12)]
        public string PrecoFracionado { get; set; }

        [MaxLength(5)]
        public string EdicaoAltPreco { get; set; }
        
        [MaxLength(5)]
        public string IPI { get; set; }

        [MaxLength(1)]
        public string FlagPisCofins { get; set; }
        
        [MaxLength(20)]
        public string EAN { get; set; }

        [Key] 
        public string TISS { get; set; }

        [MaxLength(1)]
        public string Generico { get; set; }

        [MaxLength(10)]
        public string TUSS { get; set; }

    }
}
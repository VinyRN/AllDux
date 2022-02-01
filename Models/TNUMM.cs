using System.ComponentModel.DataAnnotations;

namespace alldux_plataforma.Models
{
    public class TNUMM
    {
        [MaxLength(10)]
        public string Codigo { get; set; }

        public string TISSTP { get; set; }

        [Key] 
        public string TISS { get; set; }

        [MaxLength(250)]
        public string NomeApresentacao { get; set; }

        [MaxLength(600)]
        public string PrincipioAtivo { get; set; }

        [MaxLength(30)]
        public string Generico { get; set; }

        [MaxLength(150)]
        public string Grupo { get; set; }

        [MaxLength(150)]
        public string Classe { get; set; }

        [MaxLength(150)]
        public string Forma { get; set; }

        [MaxLength(36)]
        public string UnidMinFracao { get; set; }

        [MaxLength(25)]
        public string CNPJRegistro { get; set; }

        [MaxLength(120)]
        public string DetentorRegistroAnvisa { get; set; }

        [MaxLength(75)]
        public string RegistroAnvisa { get; set; }

        [MaxLength(20)]
        public string PrecoMaxNacional { get; set; }

        [MaxLength(20)]
        public string ValorFatorConversao { get; set; }

        [MaxLength(10)]
        public string TaxaCustos { get; set; }

        [MaxLength(350)]
        public string Obs { get; set; }

        [MaxLength(12)]
        public string TISSCodAnterior { get; set; }

        [MaxLength(12)]
        public string CodAnterior { get; set; }

        [MaxLength(20)]
        public string TipoProduto { get; set; }

        [MaxLength(20)]
        public string TipoCodificacao { get; set; }

        [MaxLength(20)]
        public string DataInicioVigencia { get; set; }

        [MaxLength(20)]
        public string DataFimVigencia { get; set; }

        [MaxLength(60)]
        public string MotivoInsercao { get; set; }

        [MaxLength(20)]
        public string DataFimImplantacao { get; set; }

        [MaxLength(36)]
        public string TISSBrasindice { get; set; }

        [MaxLength(120)]
        public string DescBrasindice { get; set; }

        [MaxLength(250)]
        public string ApresentacaoBrasindice { get; set; }

        public TNUMM() {}
        
        public TNUMM(string[] linha) { 
            Codigo = linha[0];
            TISSTP = linha[1];
            TISS = linha[2];
            NomeApresentacao = linha[3];
            PrincipioAtivo = linha[4];
            Generico = linha[5];
            Grupo = linha[6];
            Classe = linha[7];
            Forma = linha[8];
            UnidMinFracao = linha[9];
            CNPJRegistro = linha[10];
            DetentorRegistroAnvisa = linha[11];
            RegistroAnvisa = linha[12];
            PrecoMaxNacional = linha[13];
            ValorFatorConversao = linha[14];
            TaxaCustos = linha[15];
            Obs = linha[16];
            TISSCodAnterior = linha[17];
            CodAnterior = linha[18];
            TipoProduto = linha[19];
            TipoCodificacao = linha[20];
            DataInicioVigencia = linha[21];
            DataFimVigencia = linha[22];
            MotivoInsercao = linha[23];
            DataFimImplantacao = linha[24];
            TISSBrasindice = linha[25];
            DescBrasindice = linha[26];
            ApresentacaoBrasindice = linha[27];
        }
    }
}
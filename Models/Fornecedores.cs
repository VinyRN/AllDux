using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace alldux_plataforma.Models
{
    public class Fornecedores
    {
        [Key]
        public Guid IdFornecedor { get; set; }

        [MaxLength(14)]
        [Required]        
        public string CNPJ { get; set; }

        [MaxLength(200)]
        [Required]
        public string RazaoSocial { get; set; }

        [MaxLength(200)]
        [Required]
        public string NomeFantasia { get; set; }

        [MaxLength(200)]
        [Required]
        public string Endereco { get; set; }

        [MaxLength(12)]
        [Required]
        public string Numero { get; set; }

        [MaxLength(20)]
        public string Complemento { get; set; }

        [MaxLength(200)]
        [Required]
        public string Bairro { get; set; }

        [MaxLength(100)]
        [Required]
        public string Cidade { get; set; }

        [MaxLength(2)]
        [Required]
        public string Estado { get; set; }

        
        [Required]
        public Guid Pais { get; set; }

        [MaxLength(8)]
        [Required]
        public string Cep { get; set; }

        [MaxLength(100)]
        [Required]
        public string ContatoAlldux { get; set; }

        [Required]
        public int Qualificacao { get; set; }

        [MaxLength(10)]
        [Required]
        public string TelefoneAlldux { get; set; }

        [MaxLength(10)]
        [Required]
        public string TelefonePrincipal { get; set; }

        [MaxLength(10)]
        public string TelefoneSecundario { get; set; }

        [Required]
        public int TipoFornecedor { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? LastUpdate { get; set; }

        [Required]
        public Guid CreatedUser { get; set; }

        public Guid? LastUpdateUser { get; set; }

        public ICollection<FornecedoresAnexos> Anexos { get; set; } = new List<FornecedoresAnexos>();
        public ICollection<FornecedoresProduto> Produtos { get; set; } = new List<FornecedoresProduto>();
        public ICollection<FornecedoresFormasPag> FormasPagamento { get; set; } = new List<FornecedoresFormasPag>();

    }
}

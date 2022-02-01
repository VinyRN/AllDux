using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace alldux_plataforma.Models
{
    public class FornecedoresProduto
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Fornecedores")]
        public Guid IdFornecedor { get; set; }                    

        [Required]
        public Guid MedicamentoId { get; set; }

        [Required]
        public Guid MedicamentoVariacaoId { get; set; }

        [Required]
        public int Estoque { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public DateTime DataDisponivel { get; set; } = DateTime.Now;

        public DateTime? DataIndisponivel { get; set; }

        [MaxLength(2000)]
        public string Observacoes { get; set; }

        [Required]
        public Guid FreteId { get; set; }

        public Guid AproveId { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public Guid CreateUser { get; set; }

        public Guid LastUpdateUser { get; set; }

        [Required]
        public string Lote { get; set; }

        public DateTime? DataDisponivelAllDux { get; set; }

        public virtual Fornecedores Fornecedor { get; set; }

        [NotMapped]
        public string Tipo { get; set; }

        [NotMapped]
        public string PrincipioAtivo { get; set; }

        [NotMapped]
        public string Nome { get; set; }

        [NotMapped]
        public string Laboratorio { get; set; }

        [NotMapped]
        public string Distribuidor { get; set; }

        [NotMapped]
        public string Apresentacao { get; set; }

        [NotMapped]
        public string UserId { get; set; }

        [NotMapped]
        public string NomeFornecedor { get; set; }

        [NotMapped]
        public int StatusEdit { get; set; }


    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace alldux_plataforma.Models
{
    public class FornecedoresFormasPag
    {
        [Key]
        public Guid IdForncedorFormaPag { get; set; }

        [Required]
        [ForeignKey("Fornecedores")]
        public Guid IdFornecedor { get; set; }

        [Required]
        [ForeignKey("FormasPagamento")]
        public Guid IdFormaPagamento { get; set; }

        [Required]
        public int QtdParcelas { get; set; }

        [Required]
        public int Status { get; set; }

        public virtual Fornecedores Fornecedor { get; set; }

        [NotMapped]
        public string Descricao { get; set; }

    }
}

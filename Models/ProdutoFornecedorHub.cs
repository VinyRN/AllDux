using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;


namespace alldux_plataforma.Models
{
    public class ProdutoFornecedorHub
    {
        [Required]
        public Guid ProdutoId { get; set; }

        [Required]
        public Guid IdFornecedor { get; set; }

        [Required]
        public Guid MedicamentoId { get; set; }

        [Required]
        public Guid MedicamentoVariacaoId { get; set; }

        [Required]
        public Guid LoginUserId { get; set; }

        public string Apresentacao { get; set; }
        
        public int Status { get; set; }
    }

}

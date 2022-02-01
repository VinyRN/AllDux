using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alldux_plataforma.Models
{
    public class VendaItem
    {
        [Key]
        [MaxLength(36)]
        public Guid Id { get; set; } = new Guid();

        [MaxLength(36)]
        [Required]
        [ForeignKey("Venda")]
        public Guid VendaId { get; set; }

        [MaxLength(36)]
        [Required]
        [ForeignKey("ProdutoVersao")]
        public Guid ProdutoVersaoId { get; set; }

        [MaxLength(36)]
        [Required]
        [ForeignKey("Preco")]
        public Guid PrecoId { get; set; }

        [MaxLength(20)]
        [Required]
        public DateTime DataAdd { get; set; }

        [MaxLength(36)]
        [Required]
        [ForeignKey("Frete")]
        public Guid FreteId { get; set; }

        [MaxLength(36)]
        [Required]
        [ForeignKey("Pagamento")]
        public Guid PagamentoId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace alldux_plataforma.Models
{
    public class Venda
    {
        [Key]
        [MaxLength(36)]
        public Guid Id { get; set; } = new Guid();

        [MaxLength(36)]
        [Required]
        public Guid UserId { get; set; }

        [MaxLength(20)]
        [Required]
        public DateTime DataCriacao { get; set; }

        [MaxLength(20)]
        public DateTime DataConclusao { get; set; }

        [MaxLength(2)]
        [Required]
        public int Status { get; set; }

        public ICollection<VendaItem> VendaItemList { get; set; } = new List<VendaItem>();

    }
}

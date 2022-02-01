using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace alldux_plataforma.Models
{
    public class FormasPagamento
    {
        [Key]
        public Guid IdFormaPagamento { get; set; }

        [Required]
        [MaxLength(50)]
        public string DescrPagamento { get; set; }
    }
}

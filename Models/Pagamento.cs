using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace alldux_plataforma.Models
{
    public class Pagamento
    {
        [Key]
        [MaxLength(36)]
        public Guid Id { get; set; } = new Guid();
    }
}

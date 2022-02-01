using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace alldux_plataforma.Models
{
    public class EstadosFederacao
    {
        [Key]
        public string IdUf { get; set; }

        [Required]
        [MaxLength(50)]
        public string ds_estado { get; set; }
    }
}

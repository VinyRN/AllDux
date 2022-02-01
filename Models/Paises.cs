using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace alldux_plataforma.Models
{
    public class Paises
    {
        [Key]
        public Guid IdPais { get; set; }

        [Required]
        [MaxLength(255)]
        public string NomePais { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace alldux_plataforma.Models
{
    public class Frete
    {
        [Key]
        public int Id_Frete { get; set; }

        [Required]
        [MaxLength(200)]
        public string descricao { get; set; }

    }
}

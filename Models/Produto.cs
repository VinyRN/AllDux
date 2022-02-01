using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace alldux_plataforma.Models
{
    public class Produto
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        [Required]
        public string Tipo { get; set; }

        [Required]
        public Guid ReferenciaId { get; set; }


        [NotMapped]
        public string NomeFind { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace alldux_plataforma.Models
{
    public class Preco
    {

        [Key]
        [MaxLength(36)]
        public Guid Id { get; set; } = new Guid();

        
        [MaxLength(36)]
        [Required]
        public Guid ProdutoVersaoId { get; set; }

        [MaxLength(20)]
        [Required]
        public DateTime Data { get; set; }

        [MaxLength(20)]
        [Required]
        public double Valor { get; set; }

        [MaxLength(36)]
        [Required]
        public Guid UserId { get; set; }

        [MaxLength(2000)]
        public string Titulo { get; set; }

    }
}

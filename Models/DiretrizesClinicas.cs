using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alldux_plataforma.Models
{
    public class DiretrizesClinicas
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        [Required]
        [MaxLength(250)]
        public string Titulo { get; set; }

        [MaxLength(1500)]
        public string Lead { get; set; }

        [MaxLength(30)]
        public string Categoria { get; set; }

        [MaxLength(75)]
        public string Tags { get; set; }
        
        [Required]
        [MaxLength(36)]
        public string CreateId { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public ICollection<DiretrizModulo> DiretrizModulo { get; set; } = new List<DiretrizModulo>();

        public virtual DiretrizPrecificada DiretrizPrecificada { get; set; }
    }
}
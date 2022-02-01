using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace alldux_plataforma.Models
{
    public class Negociacao
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        [Required]
        [MaxLength(60)]
        public string Titulo { get; set; }

        [MaxLength(50)]
        public string Parceiro { get; set; }

        [MaxLength(150)]
        public string DestaqueLongo { get; set; }

        [MaxLength(100)]
        public string DestaqueCurto { get; set; }

        [MaxLength(100)]
        public string Texto { get; set; }

        [MaxLength(1600)]
        public string Obs { get; set; }

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public Guid CreatedId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid LastUpdateId { get; set; }
        public DateTime LastUpdateDate { get; set; }
        
        public ICollection<NegociacaoItem> NegociacaoItem { get; set; } = new List<NegociacaoItem>();
    }
}
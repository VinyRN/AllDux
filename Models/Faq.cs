using System;
using System.ComponentModel.DataAnnotations;

namespace alldux_plataforma.Models
{
    public class Faq
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        [MaxLength(200)]
        public string Pergunta { get; set; }

        [MaxLength(7000)]
        public string Resposta { get; set; }

        [Required]
        [MaxLength(36)]
        public string CreateId { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public DateTime LastUpdate { get; set; }      
    }
}
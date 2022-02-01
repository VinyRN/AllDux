using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alldux_plataforma.Models
{
    public class DiretrizModulo
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        public int Index { get; set; }

        [MaxLength(300)]
        public string Titulo { get; set; }

        public string Conteudo { get; set; }

        public ICollection<DiretrizSecao> DiretrizSecao { get; } = new List<DiretrizSecao>();
        
        [ForeignKey("DiretrizesClinicas")]
        public Guid DiretrizesClinicasId { get; set; }

        public virtual DiretrizesClinicas DiretrizesClinicas { get; set; }
    }
}
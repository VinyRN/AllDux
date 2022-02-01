using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alldux_plataforma.Models
{
    public class DiretrizPrecificada
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Conteudo { get; set; }

        [MaxLength(50)]
        public string GrupoProtocolo { get; set; }
        public ICollection<DiretrizPrecificadaTabela> DiretrizPrecificadaTabela { get; set; } = new List<DiretrizPrecificadaTabela>();

        [ForeignKey("DiretrizesClinicas")]
        public Guid DiretrizesClinicasId { get; set; }
        
        public virtual DiretrizesClinicas DiretrizesClinicas { get; set; }

    }
}
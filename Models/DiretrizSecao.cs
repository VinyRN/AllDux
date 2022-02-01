using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alldux_plataforma.Models
{
    public class DiretrizSecao
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        public int Index { get; set; }

        [MaxLength(300)]
        public string Titulo { get; set; }

        public string Conteudo { get; set; }
        public string Bibliografia { get; set; }
        public string Observacoes { get; set; }

        [ForeignKey("DiretrizModulo")]
        public Guid DiretrizModuloId { get; set; }
        
        public virtual DiretrizModulo DiretrizModulo { get; set; }
    }
}
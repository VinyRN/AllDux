using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace alldux_plataforma.Models
{
    public class Medicamento
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public Guid CreatedUser { get; set; }
        public Guid LastUpdateUser { get; set; }

        [MaxLength(75)]
        public string PrincipioAtivo { get; set; }

        [MaxLength(150)]
        public string DescricaoCurta { get; set; }

        [MaxLength(300)]
        public string Descricao { get; set; }      

        public ICollection<MedicamentoVariacao> Variacoes { get; set; } = new List<MedicamentoVariacao>();

        // public string Nome { get; set; }
        // public string NomeGenerico { get; set; }

    }
}
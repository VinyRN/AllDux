using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace alldux_plataforma.Models
{
    public class TiposDocFornecedores
    {
        [Key]
        public Guid IdTipoDocumento { get; set; }

        [Required]
        public string DescrDocumento { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public Guid CreatedUser { get; set; }

        public Guid? LastUpdate { get; set; }

        public DateTime? LastUpdateUser { get; set; }
    }
}

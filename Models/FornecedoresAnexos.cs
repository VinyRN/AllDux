using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace alldux_plataforma.Models
{
    public class FornecedoresAnexos
    {
        [Key]
        public Guid IdAnexoFornecedor { get; set; }

        [Required]
        [ForeignKey("Fornecedores")]
        public Guid IdFornecedor { get; set; }

        [Required]
        [ForeignKey("TiposDocFornecedores")]
        public Guid IdTipoDocumento { get; set; }

        [Required]
        public string anexo { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public Guid CreatedUser { get; set; }
        public DateTime? LastUpdate { get; set; }
        public Guid?  LastUpdateUser { get; set; }

        public virtual Fornecedores Fornecedor { get; set; }

        [NotMapped]
        public string CNPJ { get; set; }

        [NotMapped]
        public string NomeFantasia { get; set; }

    }
}

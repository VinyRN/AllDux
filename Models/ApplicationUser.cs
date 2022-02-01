using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace alldux_plataforma.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Company { get; set; }
        public string Cargo { get; set; }
        public string NomeGestor { get; set; }
        public string TelGestor { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedId { get; set; }
        public DateTime LastLogin { get; set; }
        
        [NotMapped]
        public string UserType { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alldux_plataforma.Models
{
    public class User
    {
        //Campos básicos
        [Required]
        public string Name { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "O e-mail informado não é válido")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

        //Campos adicionais
        [NotMapped]
        public string PasswordCheck { get; set; }
        [NotMapped]
        public bool aceitesValues { get; set; }
        [Required]
        public string Company { get; set; }
        public string Cargo { get; set; }
        public string NomeGestor { get; set; }
        public string TelGestor { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedId { get; set; }
        public DateTime LastLogin { get; set; }
        public string UserType { get; set; }
        public string Content_PadronizacaoInsumos { get; set; }
        public string Content_DiretrizesPrecificadas { get; set; }
        public string Content_DiretrizesClinicas { get; set; }
        public string Content_Negociacoes { get; set; }
        public string Content_FerramentasAnalises { get; set; }
    }
}
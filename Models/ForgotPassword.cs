using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alldux_plataforma.Models
{
    public class ForgotPassword
    {
        [Display(Name = "E-mail")]        
        [EmailAddress, Required(ErrorMessage = "O campo e-mail é obrigatório!", AllowEmptyStrings = false)]        
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um e-mail válido!")]
        public string Email { get; set; }
    }
}

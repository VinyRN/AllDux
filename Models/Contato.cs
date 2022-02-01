using System.ComponentModel.DataAnnotations;

namespace alldux_plataforma.Models
{
    public class Contato
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo nome é obrigatório!", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Números e caracteres especiais não são permitidos no nome.")]      
        public string FisrtName { get; set; }        
       
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "O campo e-mail é obrigatório!", AllowEmptyStrings = false)]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um e-mail válido!")]        
        public string FromEmail { get; set; }
    }
}

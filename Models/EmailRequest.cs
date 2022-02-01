using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace alldux_plataforma.Models
{
    public class EmailRequest
    {
        /*[Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo nome é obrigatório!", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Números e caracteres especiais não são permitidos no nome.")]      
        public string FisrtName { get; set; }        
        
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "O campo e-mail é obrigatório!", AllowEmptyStrings = false)]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um e-mail válido!")]        
        public string FromEmail { get; set; }*/

        public string ToEmail { get; set; }
        public string CcEmail { get; set; }
        public string BccEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Files { get; set; }
        public bool chkReceiveInformation { get; set; }
        public Contato Contato { get; set; }
    }
}

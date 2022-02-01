using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Linq;


namespace alldux_plataforma.Models
{
    public class BrasindiceFile
    {
        [Required]
        [FileExtensions(Extensions = "txt")]
        public IFormFile File { get; set; }

    }
}
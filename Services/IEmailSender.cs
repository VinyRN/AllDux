using alldux_plataforma.Models;
using System.Threading.Tasks;

namespace alldux_plataforma.Services
{
    public interface IEmailSender
    {        
        Task SendEmailAsync(EmailRequest emailRequest);
    }
}

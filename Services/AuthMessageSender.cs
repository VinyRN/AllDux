using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using alldux_plataforma.Models;
using System.IO;

namespace alldux_plataforma.Services
{
    public class AuthMessageSender : IEmailSender
    {
        public AuthMessageSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        
        private readonly EmailSettings _emailSettings;

        public async Task SendEmailAsync(EmailRequest emailRequest)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                
                string fromEmail = string.Empty;
                string toEmail = string.Empty; 
                string displayName = string.Empty; 

                if (emailRequest.Contato != null)
                {
                    fromEmail = string.IsNullOrEmpty(emailRequest.Contato.FisrtName) ? _emailSettings.Email : emailRequest.Contato.FromEmail;
                    toEmail = string.IsNullOrEmpty(emailRequest.Contato.FisrtName) ? emailRequest.ToEmail : _emailSettings.Email;
                    displayName = string.IsNullOrEmpty(emailRequest.Contato.FisrtName) ? _emailSettings.DisplayName : emailRequest.Contato.FisrtName;
                }
                else
                {
                    fromEmail = _emailSettings.Email;
                    toEmail = emailRequest.ToEmail;
                    displayName = _emailSettings.DisplayName;
                }
                
                string msgInformation = !emailRequest.chkReceiveInformation ?  "" : "<p>Desejo receber informações sobre Alldux.<p>";

                message.From = new MailAddress(fromEmail, displayName);
                message.To.Add(new MailAddress(toEmail));
                message.Subject = emailRequest.Subject;
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                message.Body = emailRequest.Body + msgInformation;

                if (emailRequest.Files != null)
                {
                    foreach (var file in emailRequest.Files)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                var fileBytes = ms.ToArray();
                                Attachment att = new Attachment(new MemoryStream(fileBytes), file.FileName);
                                message.Attachments.Add(att);
                            }
                        }
                    }
                }

                smtp.Port = _emailSettings.Port;
                smtp.Host = _emailSettings.Host;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                
                await smtp.SendMailAsync(message);
            }
            catch (Exception)
            {
                throw;
            }
        }               
    }
}

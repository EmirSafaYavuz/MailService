using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MailService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MailService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [HttpPost]
        public IActionResult SendEmail([FromBody] EmailRequest emailRequest)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");
                var smtpServer = smtpSettings["SmtpServer"];
                var port = int.Parse(smtpSettings["Port"]);
                var userName = smtpSettings["UserName"];
                var password = smtpSettings["Password"];

                using (var smtpClient = new SmtpClient(smtpServer, port))
                {
                    smtpClient.Credentials = new NetworkCredential(userName, password);
                    smtpClient.EnableSsl = false;

                    var mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(userName);
                    mailMessage.To.Add(emailRequest.To);
                    mailMessage.Subject = emailRequest.From;
                    var emailBody = $"İsim: {emailRequest.Name}\n" +
                                    $"E-Posta: {emailRequest.Mail}\n" +
                                    $"Telefon Numarası: {emailRequest.PhoneNumber}\n\n" +
                                    $"Mesaj:\n{emailRequest.Message}";
                    mailMessage.Body = emailBody;

                    smtpClient.Send(mailMessage);

                    return Ok("E-posta başarıyla gönderildi.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"E-posta gönderme hatası: {ex.Message}");
            }
        }
    }
}

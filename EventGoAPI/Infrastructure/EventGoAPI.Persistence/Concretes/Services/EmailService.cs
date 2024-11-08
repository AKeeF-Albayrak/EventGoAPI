using EventGoAPI.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using EventGoAPI.Persistence.Context;

namespace EventGoAPI.Persistence.Concretes.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        public EmailService(IConfiguration configuration)
        {
            _smtpServer = configuration["SmtpSettings:SmtpServer"];
            _smtpPort = int.Parse(configuration["SmtpSettings:SmtpPort"]);
            _smtpUser = configuration["SmtpSettings:SmtpUser"];
            _smtpPass = configuration["SmtpSettings:SmtpPass"];
        }

        public async Task SendEmailAsync(string to, string verificationCode)
        {
            
            string subject = "Doğrulama Kodu";
            string body = $@"
                    <html>
                    <head>
                        <style>
                            body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                            .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                            .header {{ background-color: #f4f4f4; padding: 20px; text-align: center; display: flex; align-items: center; }}
                            .header img {{ width: 50px; height: 50px; border-radius: 8px; margin-right: 10px; }}
                            .content {{ padding: 20px; }}
                            .code {{ font-size: 32px; font-weight: bold; text-align: center; 
                                    letter-spacing: 8px; margin: 20px 0; color: #007bff; }}
                            .footer {{ background-color: #f4f4f4; padding: 10px; text-align: center; font-size: 12px; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <img src='https://i.hizliresim.com/18ltyfm.png?_gl=1*1nnwvve*_ga*MTc5NzUyMjczLjE3MzExMDgwNjM.*_ga_M9ZRXYS2YN*MTczMTEwODA2My4xLjEuMTczMTEwODI4My42MC4wLjA.' alt='EventGo Logo'>
                                <h1>EventGo</h1>
                            </div>
                            <div class='content'>
                                <p>Merhaba,</p>
                                <p>Hesabınızı doğrulamak için aşağıdaki kodu kullanın:</p>
                                <div class='code'>{verificationCode}</div>
                                <p>Bu kod 10 dakika boyunca geçerli olacaktır.</p>
                                <p>Eğer bu işlemi siz başlatmadıysanız, lütfen bu e-postayı dikkate almayın.</p>
                            </div>
                            <div class='footer'>
                                <p>&copy; {DateTime.Now.Year} EventGo. Tüm hakları saklıdır.</p>
                            </div>
                        </div>
                    </body>
                    </html>";


            try
            {
                using var client = new SmtpClient(_smtpServer, _smtpPort)
                {
                    Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpUser),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);
                await client.SendMailAsync(mailMessage);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"SMTP Error: {ex.Message}");
                throw;
            }
        }
    }
}

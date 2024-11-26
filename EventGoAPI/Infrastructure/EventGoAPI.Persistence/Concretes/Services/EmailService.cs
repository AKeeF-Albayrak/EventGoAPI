using EventGoAPI.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Threading.Tasks;
using MailKit.Security;

namespace EventGoAPI.Persistence.Concretes.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public EmailService(IConfiguration configuration)
        {
            _smtpServer = configuration["SmtpSettings:SmtpServer"];
            _smtpUser = configuration["SmtpSettings:SmtpUser"];
            _smtpPass = configuration["SmtpSettings:SmtpPass"];
        }

        public async Task SendEmailAsync(string to, string verificationCode)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("EventGo", _smtpUser));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = "Doğrulama Kodu";

            message.Body = new TextPart("html")
            {
                Text = $@"
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
                                <p>Bu kod 3 dakika boyunca geçerli olacaktır.</p>
                                <p>Eğer bu işlemi siz başlatmadıysanız, lütfen bu e-postayı dikkate almayın.</p>
                            </div>
                            <div class='footer'>
                                <p>&copy; {DateTime.Now.Year} EventGo. Tüm hakları saklıdır.</p>
                            </div>
                        </div>
                    </body>
                    </html>"
            };

            try
            {
                using var client = new SmtpClient();
                await client.ConnectAsync(_smtpServer, 465, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(_smtpUser, _smtpPass);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch
            {
                try
                {
                    using var client = new SmtpClient();
                    await client.ConnectAsync(_smtpServer, 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_smtpUser, _smtpPass);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                catch
                {
                    throw;
                }
            }

        }
    }
}

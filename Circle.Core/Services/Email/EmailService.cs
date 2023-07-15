using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Circle.Shared.Configs;

namespace Circle.Core.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<string> ReadTemplate(string messageType)
        {
            string htmlPath = Path.Combine(AppContext.BaseDirectory, @"wwwroot\html", "_template.html");
            string contentPath = Path.Combine(AppContext.BaseDirectory, @"wwwroot\html", $"{messageType}.txt");
            string html;
            // get global html template
            if (File.Exists(htmlPath))
                html = File.ReadAllText(htmlPath);
            else
                return null;

            string body;
            // get specific message content
            if (File.Exists(contentPath))
                body = File.ReadAllText(contentPath);
            else return null;

            string msgBody = html.Replace("{body}", body);
            return Task.FromResult(msgBody);
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();
            var emailSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();

            email.From.Add(MailboxAddress.Parse(emailSettings.Username));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Username, emailSettings.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public Task SendManyAsync(List<string> to, string subject, string body)
        {
            throw new NotImplementedException();
        }
    }
}

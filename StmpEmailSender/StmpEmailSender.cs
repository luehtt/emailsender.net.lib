using Microsoft.AspNetCore.Identity.UI.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailSender
{
    public class StmpEmailSender : IEmailSender
    {
        public string Host { get; private set; }
        public int Port { get; private set; }
        public bool EnableSSL { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }

        public StmpEmailSender(string host, int port, bool enableSSL, string userName, string password)
        {
            Host = host;
            Port = port;
            EnableSSL = enableSSL;
            UserName = userName;
            Password = password;
        }

        public void SendEmail(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(Host, Port)
            {
                Credentials = new NetworkCredential(UserName, Password),
                EnableSsl = EnableSSL
            };

            var message = new MailMessage(UserName, email, subject, htmlMessage) { IsBodyHtml = true };
            client.Send(message);
        }

        public void SendGroupEmail(List<string> emails, string subject, string htmlMessage)
        {
            var client = new SmtpClient(Host, Port)
            {
                Credentials = new NetworkCredential(UserName, Password),
                EnableSsl = EnableSSL
            };

            var messages = emails.Select(e => new MailMessage(UserName, e, subject, htmlMessage) { IsBodyHtml = true });
            foreach (var message in messages)
            {
                client.Send(message);
            }
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(Host, Port)
            {
                Credentials = new NetworkCredential(UserName, Password),
                EnableSsl = EnableSSL
            };

            var message = new MailMessage(UserName, email, subject, htmlMessage) { IsBodyHtml = true };
            await client.SendMailAsync(message);
        }

        public async Task SendGroupEmailAsync(List<string> emails, string subject, string htmlMessage)
        {
            var client = new SmtpClient(Host, Port)
            {
                Credentials = new NetworkCredential(UserName, Password),
                EnableSsl = EnableSSL
            };

            var messages = emails.Select(e => new MailMessage(UserName, e, subject, htmlMessage) { IsBodyHtml = true });
            var tasks = messages.Select(e => client.SendMailAsync(e));
            await Task.WhenAll(tasks);
        }
    }
}

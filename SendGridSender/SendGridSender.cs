using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailSender
{
    public class SendGridSender
    {
        public string ApiKey { get; private set; }
        public EmailAddress DefaultEmail { get; private set; }

        public SendGridSender(string apiKey)
        {
            ApiKey = apiKey;
        }

        public SendGridSender(string apiKey, string defaultEmail, string defaultName)
        {
            ApiKey = apiKey;
            DefaultEmail = new EmailAddress(defaultEmail, defaultName);
        }

        public async Task SendEmailAsync(string fromEmail, string fromName, string toEmail, string toName, string subject, string htmlMessage)
        {
            var from = new EmailAddress(fromEmail, fromName);
            var to = new EmailAddress(toEmail, toName);
            await SendEmailAsync(from, to, subject, htmlMessage);
        }

        public async Task SendEmailAsync(string toEmail, string toName, string subject, string htmlMessage)
        {
            var to = new EmailAddress(toEmail, toName);
            await SendEmailAsync(DefaultEmail, to, subject, htmlMessage);
        }

        public async Task SendEmailAsync(EmailAddress to, string subject, string htmlMessage)
        {
            await SendEmailAsync(DefaultEmail, to, subject, htmlMessage);
        }

        public async Task SendEmailAsync(EmailAddress from, EmailAddress to, string subject, string htmlMessage)
        {
            var client = new SendGridClient(ApiKey);
            var email = MailHelper.CreateSingleEmail(from, to, subject, htmlMessage, htmlMessage);
            await client.SendEmailAsync(email);
        }

        public async Task SendGroupEmailAsync(List<string> toEmails, List<string> toNames, string subject, string htmlMessage)
        {
            await SendGroupEmailAsync(DefaultEmail, toEmails, toNames, subject, htmlMessage);
        }

        public async Task SendGroupEmailAsync(EmailAddress from, List<string> toEmails, List<string> toNames, string subject, string htmlMessage)
        {
            var tos = CreateEmails(toEmails, toNames);
            await SendGroupEmailAsync(from, tos, subject, htmlMessage);
        }

        public async Task SendGroupEmailAsync(List<EmailAddress> tos, string subject, string htmlMessage)
        {
            await SendGroupEmailAsync(DefaultEmail, tos, subject, htmlMessage);
        }

        public async Task SendGroupEmailAsync(EmailAddress from, List<EmailAddress> tos, string subject, string htmlMessage)
        {
            var client = new SendGridClient(ApiKey);
            var email = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, htmlMessage, htmlMessage);
            await client.SendEmailAsync(email);
        }

        private List<EmailAddress> CreateEmails(List<string> toEmails, List<string> toNames)
        {
            var res = new List<EmailAddress>();
            for (var i = 0; i < toEmails.Count; i++)
            {
                res.Add(new EmailAddress(toEmails[i], toEmails[i]));
            }
            return res;
        }
    }
}

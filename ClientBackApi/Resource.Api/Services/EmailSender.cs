using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Resource.Api.Models;

namespace Resource.Api.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _optionsEmailSettings;

        public EmailSender(IOptions<EmailSettings> optionsAccessor)
        {
            _optionsEmailSettings = optionsAccessor.Value;
        }

        public async Task<Response> SendEmail(string email, string toUsername, string subject, string messageHTML)
        {
            var client = new SendGridClient(_optionsEmailSettings.ApiKey);

            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(_optionsEmailSettings.SenderEmailAddress, _optionsEmailSettings.SenderName));
            msg.AddTo(new EmailAddress(email, toUsername));
            msg.SetSubject(subject);
            msg.AddContent(MimeType.Html, messageHTML);

            return await client.SendEmailAsync(msg);
        }
    }
}

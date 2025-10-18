using System.Net.Mail;
using Demo.PL.Settings;
using Demo.PL.Utilities;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using static MailKit.Telemetry;

namespace Demo.PL.Helpers
{
    public class MailService(IOptions<MailSettings> mailSettings) : IMailService
    {
        public void Send(Email email)
        {
            var Mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(mailSettings.Value.Email),
                Subject = email.Subject,

            };
            Mail.To.Add(MailboxAddress.Parse(email.To));
            Mail.From.Add(new MailboxAddress(mailSettings.Value.Email, mailSettings.Value.DisplayName));

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = email.Body;

            Mail.Body = bodyBuilder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            smtp.Connect(
                mailSettings.Value.Host,
                mailSettings.Value.Port,
                MailKit.Security.SecureSocketOptions.StartTls
            );

            smtp.Authenticate(mailSettings.Value.Email, mailSettings.Value.Password);

            smtp.Send(Mail);

            smtp.Disconnect(true);
        }
    }
}

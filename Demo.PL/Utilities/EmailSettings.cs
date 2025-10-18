using System.Net;
using System.Net.Mail;

namespace Demo.PL.Utilities
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var Client = new SmtpClient("smtp.gmail.com",587);
            Client.EnableSsl = true;
            Client.Credentials = new NetworkCredential("awalidasa@gmail.com", "gbibkpmsjwedxefs");
            Client.Send("awalidasa@gmail.com", email.To , email.Subject,email.Body);
        }
    }
}

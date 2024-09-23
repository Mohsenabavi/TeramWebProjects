using System.Net.Mail;
using Teram.Module.EmailSender.Services.Interfaces;

namespace Teram.Module.EmailSender.Services
{
    public class EmailService : IEmailService
    {
        public bool SendEmail(string title, string message, DateTime date, string address)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(address);
            mail.From = new MailAddress("backup@teram-group.com", title, System.Text.Encoding.UTF8);
            mail.Subject = title;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = "Date: " + date + "<br/><hr/><br/>" + message;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("backup", "B@k1965$(");
            client.Port = 25;
            client.Host = "mail.teram-group.com";
            try
            {
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

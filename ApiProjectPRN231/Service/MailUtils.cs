using ApiProjectPRN231.Constants;
using System.Net;
using System.Net.Mail;

namespace ApiProjectPRN231.Service
{
    public class MailUtils
    {
        public static async Task<bool> SendGmailAsync(string to, string subject, string token)
        {
            string relativePath = Path.Combine("Template", "EmailVerification.html");
            string body = File.ReadAllText(relativePath);

            body = body.Replace("#URLINEMAIL", $"{Config.Domain}/api/Verify/{token}");

            MailMessage message = new MailMessage(Config.Email, to, subject, body);
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            message.ReplyToList.Add(new MailAddress(Config.Email));
            message.Sender = new MailAddress(Config.Email);

            using var smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(Config.Email, Config.EmailPassword);

            try
            {
                await smtpClient.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}

using MimeKit;
using MailKit.Net.Smtp;

namespace Auth.MailService.Controllers
{
    public class MailService : IMailService
    {
        public async Task SendEmailAsync(string reciverEmail, string confirmationLink, string htmlPath, string subject)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Xsu-company", "kseniadva@yandex.ru"));
            message.To.Add(new MailboxAddress("", reciverEmail));
            message.Subject = subject;

            string htmlTemplate = File.ReadAllText(htmlPath);

            string htmlBody = htmlTemplate.Replace("{{confirmation_link}}", confirmationLink);

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlBody
            };

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync("smtp.yandex.ru", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync("kseniadva@yandex.ru", "ustblphgvcwuymyl");
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }

            }
        }
    }
}

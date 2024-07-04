
namespace Auth.MailService.Controllers
{
    public interface IMailService
    {
        Task SendEmailAsync(string reciverEmail, string confirmationLink, string htmlPath, string subject);
    }
}
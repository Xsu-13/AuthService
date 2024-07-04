using Auth.AuthService.Model;

namespace Auth.AuthService.Interfaces
{
    public interface IEmailConfirmationRepository
    {
        Task Create(User user, string token);
        Task<ConfirmationToken> GetByUserId(int id);
        Task UpdateUsed(ConfirmationToken token, bool used);
    }
}
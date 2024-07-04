using Auth.AuthService.Model;

namespace Auth.AuthService.Interfaces
{
    public interface IPasswordResetRepository
    {
        Task Create(User user, string token);
        Task<PasswordReset> GetByUserId(int id);
        Task UpdateUsed(PasswordReset token, bool used);
    }
}
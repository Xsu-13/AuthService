using Auth.AuthService.Model;

namespace Auth.AuthService.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
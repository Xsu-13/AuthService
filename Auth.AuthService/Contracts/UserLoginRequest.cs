using System.Xml.Linq;

namespace Auth.AuthService.Contracts
{
    public class UserLoginRequest(string email, string password)
    {
        public string Email { get; set; } = email;
        public string Password { get; set; } = password;
    }
}

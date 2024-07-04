using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Auth.AuthService.Contracts
{
    public class UserCreateRequest(string name, string email, string password)
    {
        [Required]
        public string Name { get; set; } = name;

        [Required]
        public string Email { get; set; } = email;

        [Required]
        public string Password { get; set; } = password;
    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Auth.AuthService.Entity
{
    public class UserEntity(int id, string name, string email, string password)
    {
        public int Id { get; private set; } = id;

        [Required]
        public string Name { get; private set; } = name;

        [Required]
        public string Email { get; private set; } = email;

        [Required]
        public string Password { get; set; } = password;

        public bool EmailComfirmed { get; set; }

    }
}

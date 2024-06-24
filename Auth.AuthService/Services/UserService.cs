using Auth.AuthService.Contracts;
using Auth.AuthService.Interfaces;
using Auth.AuthService.Model;

namespace Auth.AuthService.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task Login(string email, string password)
        {
            
        }

        public async Task Register(string name, string email, string password)
        {
            var hashedPassword = _passwordHasher.Generate(password);

            var user = new User(name, email, password);

            await _userRepository.Create(user);
        }
    }
}

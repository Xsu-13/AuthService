using Auth.AuthService.Model;

namespace Auth.AuthService.Interfaces
{
    public interface IUserRepository
    {
        public Task Create(User user);
        public Task<User> GetByEmail(string email);
    }
}

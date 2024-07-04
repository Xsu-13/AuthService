using Auth.AuthService.Model;

namespace Auth.AuthService.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> Create(User user);
        public Task<User> GetByEmail(string email);
        public Task<User> ComfirmUserEmail(string email, bool comfirmed);
        public Task<User> UpdatePassword(string email, string password);
    }
}

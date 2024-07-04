using Auth.AuthService.Entity;
using Auth.AuthService.Interfaces;
using Auth.AuthService.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Auth.AuthService.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly UsersDBContext _dbContext;

        public UserRepository(UsersDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<User> Create(User user)
        {
            var userEntity = _mapper.Map<UserEntity>(user);
            await _dbContext.AddAsync(userEntity);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<User>(userEntity);
        }

        public async Task<User> GetByEmail(string email)
        {
            var userEntity = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email) ?? throw new Exception($"No user in DB with email {email}");

            if (userEntity == null) return null;

            return _mapper.Map<User>(userEntity);
        }

        public async Task<User> ComfirmUserEmail(string email, bool comfirmed)
        {
            var userEntity = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email) ?? throw new Exception($"No user in DB with email {email}");

            userEntity.EmailComfirmed = comfirmed;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<User>(userEntity);
        }

        public async Task<User> UpdatePassword(string email, string password)
        {
            var userEntity = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email) ?? throw new Exception($"No user in DB with email {email}");

            userEntity.Password = password;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<User>(userEntity);
        }
    }
}

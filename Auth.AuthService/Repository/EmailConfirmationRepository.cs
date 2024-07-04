using Auth.AuthService.Entity;
using Auth.AuthService.Interfaces;
using Auth.AuthService.Migrations;
using Auth.AuthService.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Auth.AuthService.Repository
{
    public class EmailConfirmationRepository : IEmailConfirmationRepository
    {
        private readonly IMapper _mapper;
        private readonly UsersDBContext _dbContext;

        public EmailConfirmationRepository(UsersDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Create(User user, string token)
        {
            var confirmationToken = new ConfirmationToken()
            {
                Token = token,
                UserId = user.Id
            };

            var tokenEntity = _mapper.Map<ConfirmationTokenEntity>(confirmationToken);

            await _dbContext.ConfirmationTokens.AddAsync(tokenEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ConfirmationToken> GetByUserId(int id)
        {
            var token = await _dbContext.ConfirmationTokens
                .Where(t => t.UserId == id)
                .AsNoTracking()
                .OrderBy(t => t.CreatedAt)
                .LastOrDefaultAsync();

            if (token?.ExpiredAt < DateTime.UtcNow)
            {
                throw new Exception("Token time was over");
            }

            bool used = token?.isUsed ?? false;

            if (used)
            {
                throw new Exception("Token was already used");
            }

            return _mapper.Map<ConfirmationToken>(token);

        }

        public async Task UpdateUsed(ConfirmationToken token, bool used)
        {
            var tokenEntity = await _dbContext.ConfirmationTokens
                .Where(t => t.Id == token.Id)
                .ExecuteUpdateAsync(t => t.SetProperty(p => p.isUsed, used));

            await _dbContext.SaveChangesAsync();
        }

    }
}

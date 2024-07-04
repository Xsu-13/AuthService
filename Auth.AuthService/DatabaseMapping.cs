using Auth.AuthService.Entity;
using Auth.AuthService.Model;
using AutoMapper;

namespace Auth.AuthService
{
    public class DatabaseMapping : Profile
    {
        public DatabaseMapping()
        {
            CreateMap<User, UserEntity>();
            CreateMap<UserEntity, User>();

            CreateMap<ConfirmationToken, ConfirmationTokenEntity>();
            CreateMap<ConfirmationTokenEntity, ConfirmationToken>();

            CreateMap<PasswordReset, PasswordResetEntity>();
            CreateMap<PasswordResetEntity, PasswordReset>();
        }
    }
}

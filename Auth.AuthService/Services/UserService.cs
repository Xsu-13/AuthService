using Auth.AuthService.Contracts;
using Auth.AuthService.Contracts.ProducerContracts;
using Auth.AuthService.Interfaces;
using Auth.AuthService.Migrations;
using Auth.AuthService.Model;
using Microsoft.AspNetCore.Http;

namespace Auth.AuthService.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailConfirmationRepository _emailConfirmationRepository;
        private readonly IPasswordResetRepository _passwordResetRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider, IEmailConfirmationRepository emailConfirmationRepository, IPasswordResetRepository passwordResetRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _emailConfirmationRepository = emailConfirmationRepository;
            _passwordResetRepository = passwordResetRepository;
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _userRepository.GetByEmail(email);

            var result = _passwordHasher.Verify(password, user.Password);

            if (user != null && !result)
            {
                throw new Exception("Incorrect password or login, check it out");
            }

            if (user != null && !user.EmailComfirmed)
            {
                throw new Exception("Email was not comfirmed, check it out");
            }

            var token = _jwtProvider.GenerateToken(user!);

            return token;
        }

        public async Task Register(HttpContext httpContext, string name, string email, string password, MailProducer producer, CancellationToken cancellation)
        {
            //Save user to db
            var hashedPassword = _passwordHasher.Generate(password);
            var user = new User(name, email, hashedPassword);
            var savedUser = await _userRepository.Create(user);

            //Create confirmation email token
            var emailToken = _jwtProvider.GenerateToken(savedUser);
            await _emailConfirmationRepository.Create(savedUser, emailToken);

            //Send Email 
            var confirmationLinkPart = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/User/confirm/{user.Email}&{emailToken}";
            var mailRequest = new MailConfirmationRequest(email, confirmationLinkPart);
            await producer.ProduceConfirmEmailAsync(mailRequest, cancellation);
        }

        public async Task<bool> TryComfirmUserEmail(string email, string token)
        {
            var user = await _userRepository.GetByEmail(email);

            if(user == null) return false;

            var userToken = await _emailConfirmationRepository.GetByUserId(user.Id);

            if(userToken?.Token == token)
            {
                await _emailConfirmationRepository.UpdateUsed(userToken, true);
                await _userRepository.ComfirmUserEmail(email, true);

                return true;
            }

            return false;
        }

        public async Task<bool> ResetPassword(string email, HttpContext httpContext, MailProducer producer, CancellationToken cancellation)
        {
            var user = await _userRepository.GetByEmail(email);

            if (user == null) return false;

            var token = _jwtProvider.GenerateToken(user);
            await _passwordResetRepository.Create(user, token);

            var confirmationLinkPart = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/User/reset/{user.Email}&{token}";
            var mailRequest = new PasswordResetRequest(email, confirmationLinkPart);
            await producer.ProduceResetPasswordEmailAsync(mailRequest, cancellation);

            return true;
        }


        //ДОДЕЛАТЬ ФУНКЦИЮ
        public async Task<bool> CheckUserTokenToResetPassword(string email, string token, string password)
        {
            var user = await _userRepository.GetByEmail(email);

            if (user == null) return false;

            var userToken = await _emailConfirmationRepository.GetByUserId(user.Id);

            if (userToken?.Token == token)
            {
                await _emailConfirmationRepository.UpdateUsed(userToken, true);

                await _userRepository.UpdatePassword(email, password);

                return true;
            }

            return false;
        }
        
    }
}

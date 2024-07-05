using Auth.AuthService.Contracts;
using Auth.AuthService.Model;
using Auth.AuthService.Services;
using Confluent.Kafka;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.AuthService.Endpoints
{

    public static class UserEndpoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder builder)
        {
            var endpoints = builder.MapGroup("User");

            endpoints.MapPost("register", Register);
            endpoints.MapPost("login", Login);
            endpoints.MapPost("logout", Logout);
            endpoints.MapGet("confirm/{email}&{code}", ConfirmEmail);
            endpoints.MapPost("reset-password", ResetPassword);
            endpoints.MapGet("reset/{email}&{code}", TryChangePassword);
            endpoints.MapPost("reset/{email}&{code}", ChangePassword);

            return builder;
        }

        private static async Task<IResult> Register(HttpContext httpContext, UserRegistrationRequest request, UserService service, MailProducer producer, CancellationToken cancellationToken)
        {
            try
            {
                await service.Register(httpContext, request.Name, request.Email, request.Password, producer, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Registration user error: " + ex);
                return Results.StatusCode(500);
            }

            return Results.Ok("User successfully registrated! ");
        }

        private static async Task<IResult> Login(UserLoginRequest request, UserService service, HttpContext httpContext)
        {
            try
            {
                var token = await service.Login(request.Email, request.Password);
                httpContext.Response.Cookies.Append("giga", token);

            }
            catch (Exception ex)
            {
                return Results.Conflict("Creating user error: " + ex);
            }

            return Results.Ok();
        }

        private static async Task<IResult> Logout(HttpResponse httpResponse)
        {
            //Delete cookies token
            httpResponse.Cookies.Delete("giga");

            return Results.Ok();
        }

        private static async Task<IResult> ConfirmEmail([FromRoute]string email, [FromRoute] string code, UserService service, HttpContext httpContext)
        {
            var result = await service.TryComfirmUserEmail(email, code);

            if(result)
                return Results.Ok();

            return Results.Conflict("Broken confirmation link");
        }

        private static async Task<IResult> ResetPassword(string email, UserService service, MailProducer producer, HttpContext httpContext, CancellationToken cancellation)
        {
            await service.ResetPassword(email, httpContext, producer, cancellation);

            return Results.Ok();
        }

        private static async Task<IResult> TryChangePassword([FromRoute] string email, [FromRoute] string code, UserService service, HttpContext httpContext)
        {
            return Results.Ok();
        }

        private static async Task<IResult> ChangePassword([FromRoute] string email, [FromRoute] string code, string password, UserService service, HttpContext httpContext)
        {
            var result = await service.CheckUserTokenToResetPassword(email, code, password);

            if (result)
            {
                return Results.Ok();
            }

            return Results.Conflict("Broken confirmation link");
        }

    }
}

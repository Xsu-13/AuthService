using Auth.AuthService.Contracts;
using Auth.AuthService.Model;
using Auth.AuthService.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{

    public static class UserEndpoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder builder)
        {
            var endpoints = builder.MapGroup("User");

            endpoints.MapPost("{request}", Register);

            return builder;
        }

        private static async Task<IResult> Register(UserRegistrationRequest request, UserService service)
        {
            try
            {
                await service.Register(request.Name, request.Email, request.Password);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Creating user error: " + ex);
                return Results.StatusCode(500);
            }

            return Results.Ok("User successfully registrated! ");
        }

    }
}

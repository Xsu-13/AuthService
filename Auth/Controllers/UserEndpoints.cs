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
            endpoints.MapPost(String.Empty, CreateUser);
            endpoints.MapGet("{email}", GetByEmail);

            return builder;
        }

        private static async Task<IResult> CreateUser(User user, UserService service)
        {
            try
            {
                await service.Create(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Creating user error: " + ex);
                return Results.StatusCode(500);
            }

            return Results.Ok("User successfully created! ");
        }

        private static async Task<IResult> GetByEmail(string email, UserService service)
        {
            User user;

            try
            {
                user = await service.GetByEmail(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Creating user error: " + ex);
                return Results.StatusCode(500);
            }

            return Results.Ok(user);
        }

    }
}

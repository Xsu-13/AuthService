using Auth.AuthService.Endpoints;
using Auth.AuthService.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Auth.AuthService.Extentions
{
    public static class ApiExtention
    {
        public static void AddMappedEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapUserEndpoints();
            builder.MapPostEndpoints();
        }

        public static void AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;

                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
                    };

                    options.Events = new()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["giga"];

                            return Task.CompletedTask;
                        }
                    };
                })
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = configuration["Authentication:Google:ClientId"] ?? throw new Exception("No client ID for google auth");
                    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"] ?? throw new Exception("No client secret for google auth");
                });

            services.AddAuthorization();
        }

    }
}

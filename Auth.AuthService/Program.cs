using Auth.AuthService.Endpoints;
using Auth.AuthService;
using Auth.AuthService.Interfaces;
using Auth.AuthService.Model;
using Auth.AuthService.Repository;
using Auth.AuthService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Auth.AuthService.Extentions;
using Microsoft.AspNetCore.CookiePolicy;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
services.AddApiAuthentication(configuration);

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddDbContext<UsersDBContext>(options => { options.UseNpgsql(configuration.GetConnectionString(nameof(UsersDBContext)));});
services.AddAutoMapper(typeof(DatabaseMapping));

//Kafka
services.AddTransient<MailProducer>();

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IEmailConfirmationRepository, EmailConfirmationRepository>();
services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();

services.AddScoped<IJwtProvider, JwtProvider>();
services.AddScoped<IPasswordHasher, PasswordHasher>();

services.AddScoped<UserService>();

services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5500");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions 
{ 
    HttpOnly = HttpOnlyPolicy.Always,
    MinimumSameSitePolicy = SameSiteMode.Strict,
    Secure = CookieSecurePolicy.Always
});

app.AddMappedEndpoints();

app.Run();

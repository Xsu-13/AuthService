using Auth.Api.Controllers;
using Auth.AuthService;
using Auth.AuthService.Interfaces;
using Auth.AuthService.Repository;
using Auth.AuthService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddDbContext<UsersDBContext>(options => { options.UseNpgsql(configuration.GetConnectionString(nameof(UsersDBContext)));});
services.AddAutoMapper(typeof(DatabaseMapping));

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapUserEndpoints();

app.Run();

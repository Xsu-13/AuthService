using Auth.Api.Controllers;
using Auth.AuthService.Interfaces;
using Auth.AuthService.Repository;
using Auth.AuthService.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;


services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

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

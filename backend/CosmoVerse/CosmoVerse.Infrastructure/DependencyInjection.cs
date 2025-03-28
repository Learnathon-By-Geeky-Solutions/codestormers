﻿using CosmoVerse.Application.Services;
using CosmoVerse.Infrastructure.Services;
using CosmoVerse.Repositories;
using CosmoVerse.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CosmoVerse.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddScoped<IAuthService, AuthService>();
            service.AddTransient<IEmailService, EmailService>();
            service.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            service.AddScoped<IUserService, UserService>();
            return service;
        }
    }
}

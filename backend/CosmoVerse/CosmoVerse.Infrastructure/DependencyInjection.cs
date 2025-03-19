using CosmoVerse.Repositories;
using CosmoVerse.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CosmoVerse.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddScoped<IAuthService, AuthService>();
            service.AddTransient<IEmailService, EmailService>();
            service.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            return service;
        }
    }
}

using CosmoVerse.Application.Interfaces;
using CosmoVerse.Infrastructure.Repositories;
using CosmoVerse.Infrastructure.Services;
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
            service.AddScoped<ICloudinaryService, CloudinaryService>();
            service.AddScoped<IPlanetService, PlanetService>();
            service.AddScoped<ISatelliteService, SatelliteService>();
            service.AddScoped<ICelestialService, CelestialService>();
            return service;
        }
    }
}

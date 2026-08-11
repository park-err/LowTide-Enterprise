using LowTideEnt.Application.Authorization;
using LowTideEnt.Application.Authorization.Dto;
using LowTideEnt.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LowTideEnt.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
        {
            var bleIssuer = config["BLE_Auth:Issuer"];
            var bleAudience = config["BLE_Auth:Audience"];
            var bleSecret = config["BLE_Auth:Secret"];

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production")
            {
                Environment.SetEnvironmentVariable("BLE_ISSUER", bleIssuer);
                Environment.SetEnvironmentVariable("BLE_AUDIENCE", bleAudience);
                Environment.SetEnvironmentVariable("BLE_SECRET", bleSecret);
            }

            services.AddScoped<IAuthService, AuthService>();

            // register services by scanning the assembly
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddServicesFromAssembly(assembly);

            return services;
        }

        private static IServiceCollection AddServicesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var serviceTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.Name.EndsWith("Service"))
                    .Select(i => new { Interface = i, Implementation = t }));

            foreach (var service in serviceTypes)
                services.AddScoped(service.Interface, service.Implementation);

            return services;
        }
    }
}

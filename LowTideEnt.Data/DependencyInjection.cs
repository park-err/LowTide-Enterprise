using LowTideEnt.Infrastructure.Data;
using LowTideEnt.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LowTideEnt.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EnterpriseDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("EnterpriseConnection")));

            services.AddScoped(typeof(IEnterpriseRepository<>), typeof(EnterpriseRepository<>));

            services.AddRepositoriesFromAssembly(typeof(EnterpriseDbContext).Assembly);

            return services;
        }

        private static IServiceCollection AddRepositoriesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var repoTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.GetInterfaces().Any(gi =>
                        gi.IsGenericType && gi.GetGenericTypeDefinition() == typeof(IEnterpriseRepository<>))
                        || (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnterpriseRepository<>)) == false && i != typeof(IEnterpriseRepository<>))
                    .Where(i => i.Name.EndsWith("Repository"))
                    .Select(i => new { Interface = i, Implementation = t }));

            foreach (var repo in repoTypes)
                services.AddScoped(repo.Interface, repo.Implementation);

            return services;
        }
    }
}

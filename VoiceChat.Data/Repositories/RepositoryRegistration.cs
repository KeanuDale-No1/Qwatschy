using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat.Data.Repositories
{
    public static class RepositoryRegistration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            var assembly = typeof(IRepository<>).Assembly;

            var repoTypes = assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Implementation = t,
                    Interfaces = t.GetInterfaces()
                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>))
                })
                .Where(x => x.Interfaces.Any());

            foreach (var type in repoTypes)
            {
                foreach (var iface in type.Interfaces)
                {
                    services.AddScoped(iface, type.Implementation);
                }
            }
        }
    }
}

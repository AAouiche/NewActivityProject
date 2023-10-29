
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class DomainDependencyInjection
    {
        public static IServiceCollection AddDomainDependencies(this IServiceCollection services)
        {
            // Register your domain-specific services, repositories, etc. here.
            // For example:
            // services.AddScoped<IYourRepository, YourRepositoryImplementation>();
            
            return services;
        }
    }
}
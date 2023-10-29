using Microsoft.Extensions.DependencyInjection;
using Application.Activities;

namespace Application
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(ApplicationDI).Assembly;
            services.AddMediatR(conf =>
            conf.RegisterServicesFromAssembly(assembly));
            
            return services;
        }
    }
}
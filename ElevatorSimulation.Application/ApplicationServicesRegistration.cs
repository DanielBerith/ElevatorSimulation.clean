using ElevatorSimulation.Application.Interfaces;
using ElevatorSimulation.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ElevatorSimulation.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<IElevatorService, ElevatorService>();
            services.AddScoped<IDispatchingStrategy, NearestElevatorStrategy>();

            return services;
        }
    }
}

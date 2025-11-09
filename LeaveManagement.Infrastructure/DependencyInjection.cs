using LeaveManagement.Application.Interfaces;
using LeaveManagement.Application.Services;
using LeaveManagement.Domain.Interfaces.Repositories;
using LeaveManagement.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LeaveManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ILeaveRequestRepository, InMemoryLeaveRequestRepository>();
            services.AddScoped<ILeaveRequestService, LeaveRequestService>();

            return services;
        }
    }
}
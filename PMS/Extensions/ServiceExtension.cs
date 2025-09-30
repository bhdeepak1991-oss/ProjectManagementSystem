using PMS.Features.Master.Respositories;
using PMS.Features.Master.Services;

namespace PMS.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRoleService, RoleService>();

            return services;
        }
    }
}

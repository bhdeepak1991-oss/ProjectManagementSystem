using PMS.Features.Master.Respositories;

namespace PMS.Extensions
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRoleMasterRepository, RoleMasterRepository>();

            return services;
        }
    }
}

using PMS.Features.Master.Respositories;
using PMS.Features.UserManagement.Respositories;

namespace PMS.Extensions
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRoleMasterRepository, RoleMasterRepository>();

            services.AddScoped<IDepartmentRepository, DepartmentRepository>();

            services.AddScoped<IDesignationRepository, DesignationRepository>();

            services.AddScoped<ITaskStatusRepository, TaskStatusRepository>();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            return services;
        }
    }
}

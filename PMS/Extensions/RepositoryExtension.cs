using PMS.Features.Master.Respositories;
using PMS.Features.Project.Repositories;
using PMS.Features.ProjectEmployee.Repositories;
using PMS.Features.Sprint.Repositories;
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

            services.AddScoped<IProjectRepository, ProjectRepository>();

            services.AddScoped<IProjectEmployeeRepository, ProjectEmployeeRepository>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ISprintRepository, SprintRepository>();

            return services;
        }
    }
}

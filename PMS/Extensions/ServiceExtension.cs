using PMS.Features.Master.Services;
using PMS.Features.Project.Services;
using PMS.Features.ProjectEmployee.Services;
using PMS.Features.UserManagement.Services;

namespace PMS.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IDepartmentService, DepartmentService>();

            services.AddScoped<IDesignationService, DesignationService>();

            services.AddScoped<ITaskStatusService, TaskStatusService>();

            services.AddScoped<IEmployeeService, EmployeeService>();

            services.AddScoped<IProjectService, ProjectService>();

            services.AddScoped<IProjectEmployeeServices, ProjectEmployeeServices>();

            return services;
        }
    }
}

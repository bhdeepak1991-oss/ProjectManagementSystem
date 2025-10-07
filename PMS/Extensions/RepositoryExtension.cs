using PMS.Features.Master.Respositories;
using PMS.Features.Project.Repositories;
using PMS.Features.EmployeeTimeSheet.Repositories;
using PMS.Features.ProjectEmployee.Repositories;
using PMS.Features.ProjectTask.Repositories;
using PMS.Features.Sprint.Repositories;
using PMS.Features.TaskDetail.Respositories;
using PMS.Features.UserManagement.Respositories;
using PMS.Features.Utilities.Repositories;

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

            services.AddScoped<IProjectTaskRepository, ProjectTaskRepository>();

            services.AddScoped<ISprintRepository, SprintRepository>();

            services.AddScoped<ITaskDetailRepository, TaskDetailRepository>();

            services.AddScoped<IStatusHelperRepository, StatusHelperRepository>();

            services.AddScoped<ITimeSheetRepository, TimeSheetRepository>();

            return services;
        }
    }
}

using PMS.Features.Master.Services;
using PMS.Features.Project.Services;
using PMS.Features.EmployeeTimeSheet.Services;
using PMS.Features.ProjectEmployee.Services;
using PMS.Features.ProjectTask.Services;
using PMS.Features.Sprint.Services;
using PMS.Features.TaskDetail.Services;
using PMS.Features.UserManagement.Services;
using PMS.Features.Utilities.Services;
using PMS.Helpers;
using PMS.Features.Document.Services;
using PMS.Features.Vacation.Services;
using PMS.Features.Notification.Services;
using PMS.Features.Dashboard.Services;
using PMS.Features.KanbanBoard.Services;

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

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ISprintService, SprintService>();

            services.AddScoped<IProjectTaskService, ProjectTaskService>();

            services.AddScoped<ITaskDetailService, TaskDetailService>();

            services.AddScoped<IStatusHelperService, StatusHelperService>();

            services.AddScoped<ITimeSheetService, TimeSheetService>();

            services.AddScoped<IProjectDocumentService, ProjectDocumentService>();

            services.AddScoped<IVacationService, VacationService>();

            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IDashboardService, DashboardService>();

            services.AddTransient<EmailService>();

            services.AddTransient<PasswordService>();

            services.AddTransient<BlobHelper>();

            services.AddScoped<IBoardService, BoardService>();

            return services;
        }
    }
}

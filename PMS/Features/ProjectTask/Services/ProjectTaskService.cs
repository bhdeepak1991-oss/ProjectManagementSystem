using PMS.Domains;
using PMS.Features.Notification.Services;
using PMS.Features.ProjectTask.Repositories;
using PMS.Features.ProjectTask.ViewModels;
using System.Threading.Tasks;

namespace PMS.Features.ProjectTask.Services
{
    public class ProjectTaskService : IProjectTaskService
    {
        private readonly IProjectTaskRepository _projectTaskRepository;
        private readonly INotificationService _notificationService;
        public ProjectTaskService(IProjectTaskRepository projectTaskRepository, INotificationService notificationService)
        {
            _projectTaskRepository = projectTaskRepository;
            _notificationService = notificationService;
        }

        public async Task<(string message, bool isSuccess)> CreateBulkProjectTask(List<Domains.ProjectTask> models, CancellationToken cancellationToken)
        {
            var response = await _projectTaskRepository.CreateBulkProjectTask(models, cancellationToken);
            return response;
        }

        public async Task<(string message, bool isSuccess)> CreateProjectTask(Domains.ProjectTask models, CancellationToken cancellationToken)
        {
            await _notificationService.AddNotification(new NotificationDetail()
            {
                NotificationMessage = $"New Task has been assigned to you ! {models.TaskName}",
                NotifiedUserId = models.EmployeeId,
                NotificationStatus = "New Notification",
                IsActive = true,
                IsDeleted= false
            }, cancellationToken);

            return await _projectTaskRepository.CreateProjectTask(models, cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> DeleteProjectTask(int taskId, CancellationToken cancellationToken)
        {
            return await _projectTaskRepository.DeleteProjectTask(taskId, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<ProjectEmployeeVm> models)> GetProjectEmployee(int projectId, CancellationToken cancellationToken)
        {
            return await _projectTaskRepository.GetProjectEmployee(projectId, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, Domains.ProjectTask models)> GetProjectTaskById(int taskId, CancellationToken cancellationToken)
        {
            return await _projectTaskRepository.GetProjectTaskById(taskId, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<ProjectTaskViewModel> models)> GetProjectTaskList(int projectId, CancellationToken cancellationToken)
        {
            return await _projectTaskRepository.GetProjectTaskList(projectId, cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> UpdateProjectTask(Domains.ProjectTask models, CancellationToken cancellationToken)
        {
            return await _projectTaskRepository.UpdateProjectTask(models, cancellationToken);
        }
    }
}

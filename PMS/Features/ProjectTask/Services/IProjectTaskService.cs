using PMS.Features.ProjectTask.ViewModels;

namespace PMS.Features.ProjectTask.Services
{
    public interface IProjectTaskService
    {
        Task<(string message, bool isSuccess)> CreateProjectTask(Domains.ProjectTask models, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> UpdateProjectTask(Domains.ProjectTask models, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> DeleteProjectTask(int taskId, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, Domains.ProjectTask models)> GetProjectTaskById(int taskId, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<ProjectTaskViewModel> models)> GetProjectTaskList(int projectId, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<ProjectEmployeeVm> models)> GetProjectEmployee(int projectId, CancellationToken cancellationToken);
    }
}

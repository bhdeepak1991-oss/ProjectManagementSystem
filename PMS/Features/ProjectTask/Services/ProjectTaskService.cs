using PMS.Domains;
using PMS.Features.ProjectTask.Repositories;
using PMS.Features.ProjectTask.ViewModels;
using System.Threading.Tasks;

namespace PMS.Features.ProjectTask.Services
{
    public class ProjectTaskService : IProjectTaskService
    {
        private readonly IProjectTaskRepository _projectTaskRepository;

        public ProjectTaskService(IProjectTaskRepository projectTaskRepository)
        {
            _projectTaskRepository = projectTaskRepository;
        }

        public async Task<(string message, bool isSuccess)> CreateProjectTask(Domains.ProjectTask models, CancellationToken cancellationToken)
        {
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

using PMS.Domains;
using PMS.Features.Master.Respositories;

namespace PMS.Features.Master.Services
{
    public class TaskStatusService : ITaskStatusService
    {
        private readonly ITaskStatusRepository _taskStatusRepository;

        public TaskStatusService(ITaskStatusRepository taskStatusRepository)
        {
            _taskStatusRepository = taskStatusRepository;
        }

        public async Task<(string message, bool isSuccess)> CreateTaskStatus(TaskStatusMaster model, CancellationToken cancellationToken)
        {
            return await _taskStatusRepository.CreateTaskStatus(model, cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> DeleteTaskStatus(int id, CancellationToken cancellationToken)
        {
            return await _taskStatusRepository.DeleteTaskStatus(id, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, TaskStatusMaster? model)> GetTaskStatusById(int id, CancellationToken cancellationToken)
        {
            return await _taskStatusRepository.GetTaskStatusById(id, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<TaskStatusMaster> model)> GetTaskStatusDetail(CancellationToken cancellationToken)
        {
            return await _taskStatusRepository.GetTaskStatusDetail(cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> UpdateTaskStatus(TaskStatusMaster model, CancellationToken cancellationToken)
        {
            return await _taskStatusRepository.UpdateTaskStatus(model, cancellationToken);
        }
    }
}

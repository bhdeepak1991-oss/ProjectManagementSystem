using PMS.Domains;

namespace PMS.Features.Master.Services
{
    public interface ITaskStatusService
    {
        Task<(string message, bool isSuccess)> CreateTaskStatus(TaskStatusMaster model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> UpdateTaskStatus(TaskStatusMaster model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> DeleteTaskStatus(int id, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, TaskStatusMaster? model)> GetTaskStatusById(int id, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<TaskStatusMaster> model)> GetTaskStatusDetail(CancellationToken cancellationToken);
    }
}

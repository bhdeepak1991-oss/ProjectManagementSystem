using PMS.Features.Dashboard.ViewModels;
using PMS.Features.TaskDetail.ViewModels;

namespace PMS.Features.TaskDetail.Respositories
{
    public interface ITaskDetailRepository
    {
        Task<(string message, bool isSuccess, IEnumerable<TaskDetailViewModel> models)> GetTaskDetails(int projectId);

        Task<(string message, bool isSuccess, TaskDetailViewModel model)> GetTaskDetail(int taskId);

        Task<(string message, bool isSuccess, IEnumerable<ProjectDiscussBoard> models)> GetDiscussionBoardList(int taskId);

        Task<(string message, bool isSuccess)> CreateDiscussion(TaskDetailViewModel model);
        Task<(string message, bool isSuccess)> ChangeTaskStatus(int taskId, int userId, string status);
        Task<(string message, bool isSuccess)> ChangeTaskPriority(int taskId, int userId, string priority);
        Task<(string message, bool isSuccess)> ChangeTaskStartDate(int taskId, int userId, DateTime startDate);
        Task<(string message, bool isSuccess)> ChangeTaskCompletedDate(int taskId, int userId, DateTime completedDate);

        Task<(string message, bool isSuccess)> AssignToEmployee(int taskId, int userId,int empId);

        Task<(string message, bool isSuccess, IEnumerable<AssignHistoryVm> models)> GetTaskAssignHistory(int taskId);

        Task<(string message, bool isSuccess, IEnumerable<TaskStatusHistoryVm> models)> GetTaskStatusHistory(int taskId);
    }
}

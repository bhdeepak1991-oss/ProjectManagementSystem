using PMS.Features.TaskDetail.ViewModels;

namespace PMS.Features.TaskDetail.Services
{
    public interface ITaskDetailService
    {
        Task<(string message, bool isSuccess, IEnumerable<TaskDetailViewModel> models)> GetTaskDetails(int projectId);
        Task<(string message, bool isSuccess, TaskDetailViewModel model)> GetTaskDetail(int taskId);
        Task<(string message, bool isSuccess)> CreateDiscussion(TaskDetailViewModel model);
        Task<(string message, bool isSuccess, IEnumerable<ProjectDiscussBoard> models)> GetDiscussionBoardList(int taskId);
    }
}

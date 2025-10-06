using PMS.Features.TaskDetail.ViewModels;

namespace PMS.Features.TaskDetail.Respositories
{
    public interface ITaskDetailRepository
    {
        Task<(string message, bool isSuccess, IEnumerable<TaskDetailViewModel> models)> GetTaskDetails(int projectId);

        Task<(string message, bool isSuccess, TaskDetailViewModel model)> GetTaskDetail(int taskId);

        Task<(string message, bool isSuccess, IEnumerable<ProjectDiscussBoard> models)> GetDiscussionBoardList(int taskId);

        Task<(string message, bool isSuccess)> CreateDiscussion(TaskDetailViewModel model);
    }
}

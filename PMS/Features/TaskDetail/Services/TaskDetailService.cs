using PMS.Features.TaskDetail.Respositories;
using PMS.Features.TaskDetail.ViewModels;

namespace PMS.Features.TaskDetail.Services
{
    public class TaskDetailService : ITaskDetailService
    {
        private readonly ITaskDetailRepository _taskDetailRepository;

        public TaskDetailService(ITaskDetailRepository taskDetailRepository)
        {
            _taskDetailRepository = taskDetailRepository;
        }

        public async Task<(string message, bool isSuccess)> CreateDiscussion(TaskDetailViewModel model)
        {
            return await _taskDetailRepository.CreateDiscussion(model);
        }

        public async  Task<(string message, bool isSuccess, IEnumerable<ProjectDiscussBoard> models)> GetDiscussionBoardList(int taskId)
        {
            return await _taskDetailRepository.GetDiscussionBoardList(taskId);
        }

        public async  Task<(string message, bool isSuccess, TaskDetailViewModel model)> GetTaskDetail(int taskId)
        {
            return await _taskDetailRepository.GetTaskDetail(taskId);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<TaskDetailViewModel> models)> GetTaskDetails(int projectId)
        {
            return await _taskDetailRepository.GetTaskDetails(projectId);
        }
    }
}

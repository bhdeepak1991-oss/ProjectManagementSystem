using PMS.Features.Dashboard.ViewModels;
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

        public async Task<(string message, bool isSuccess)> AssignToEmployee(int taskId, int userId, int empId)
        {
            return await _taskDetailRepository.AssignToEmployee(taskId, userId, empId);
        }

        public async Task<(string message, bool isSuccess)> ChangeTaskCompletedDate(int taskId, int userId, DateTime completedDate)
        {
            return await _taskDetailRepository.ChangeTaskCompletedDate(taskId, userId, completedDate);
        }

        public async Task<(string message, bool isSuccess)> ChangeTaskPriority(int taskId, int userId, string priority)
        {
            return await _taskDetailRepository.ChangeTaskPriority(taskId, userId, priority);
        }

        public async  Task<(string message, bool isSuccess)> ChangeTaskStartDate(int taskId, int userId, DateTime startDate)
        {
            return await _taskDetailRepository.ChangeTaskStartDate(taskId, userId, startDate);
        }

        public async Task<(string message, bool isSuccess)> ChangeTaskStatus(int taskId, int userId, string status)
        {
            return await _taskDetailRepository.ChangeTaskStatus(taskId, userId, status);
        }

        public async Task<(string message, bool isSuccess)> CreateDiscussion(TaskDetailViewModel model)
        {
            return await _taskDetailRepository.CreateDiscussion(model);
        }

        public async  Task<(string message, bool isSuccess, IEnumerable<ProjectDiscussBoard> models)> GetDiscussionBoardList(int taskId)
        {
            return await _taskDetailRepository.GetDiscussionBoardList(taskId);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<AssignHistoryVm> models)> GetTaskAssignHistory(int taskId)
        {
            return await _taskDetailRepository.GetTaskAssignHistory(taskId);
        }
        public async  Task<(string message, bool isSuccess, TaskDetailViewModel model)> GetTaskDetail(int taskId)
        {
            return await _taskDetailRepository.GetTaskDetail(taskId);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<TaskDetailViewModel> models)> GetTaskDetails(int projectId)
        {
            return await _taskDetailRepository.GetTaskDetails(projectId);
        }

        public async  Task<(string message, bool isSuccess, IEnumerable<TaskPriorityHistoryVm> models)> GetTaskPriorityHistory(int taskId)
        {
            return await _taskDetailRepository.GetTaskPriorityHistory(taskId);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<TaskStatusHistoryVm> models)> GetTaskStatusHistory(int taskId)
        {
            return await _taskDetailRepository.GetTaskStatusHistory(taskId);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using PMS.Features.TaskDetail.Services;

namespace PMS.Features.TaskDetail
{
    public class TaskDetailController : Controller
    {
        private readonly ITaskDetailService _taskDetailService;

        public TaskDetailController(ITaskDetailService taskDetailService)
        {
            _taskDetailService = taskDetailService;
        }

        public async Task<IActionResult> TaskDetail(int taskId)
        {
            taskId = 2005;
            var response= await _taskDetailService.GetTaskDetail(taskId);
            return View("~/Features/TaskDetail/Views/TaskDetail.cshtml", response.model);
        }
    }
}

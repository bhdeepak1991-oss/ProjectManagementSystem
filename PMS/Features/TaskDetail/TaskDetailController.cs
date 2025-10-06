using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using PMS.Features.ProjectEmployee.Services;
using PMS.Features.TaskDetail.Services;
using PMS.Features.TaskDetail.ViewModels;
using PMS.Helpers;
using PMS.Notification;

namespace PMS.Features.TaskDetail
{
    public class TaskDetailController : Controller
    {
        private readonly ITaskDetailService _taskDetailService;
        private readonly IProjectEmployeeServices _projectEmployeeService;
        private readonly IHubContext<NotificationHub> _hubContext;
        public TaskDetailController(ITaskDetailService taskDetailService,IHubContext<NotificationHub> hub, IProjectEmployeeServices projectEmployeeService)
        {
            _taskDetailService = taskDetailService;
            _projectEmployeeService = projectEmployeeService;
            _hubContext = hub;
        }

        public async Task<IActionResult> TaskDetail(int taskId)
        {
            var response = await _taskDetailService.GetTaskDetail(taskId);

            var projectId = Convert.ToInt32(HttpContext.Session.GetInt32("selectedProjectId") ?? 1);

            var mappedEmployees = await _projectEmployeeService.GetMappedProjectEmployee(projectId);

            ViewBag.Employees = new SelectList(mappedEmployees.models, "Id", "Name");

            return View("~/Features/TaskDetail/Views/TaskDetail.cshtml", response.model);
        }

        public async Task<IActionResult> DiscussionBoard(int taskId)
        {
            var response = await _taskDetailService.GetDiscussionBoardList(taskId);

            return PartialView("~/Features/TaskDetail/Views/TaskDiscussion.cshtml", response.models);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscussion(TaskDetailViewModel model)
        {
            model.EmployeeId =Convert.ToInt32(HttpContext.GetEmployeeId());

            var response= await _taskDetailService.CreateDiscussion(model);

            await _hubContext.Clients.All.SendAsync("DiscussionAdded", model.TaskName, model.EmployeeName);

            return Json(response);
        }
    }
}

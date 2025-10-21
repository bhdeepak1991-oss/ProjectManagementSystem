using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using PMS.Attributes;
using PMS.Features.Dashboard.Services;
using PMS.Features.ProjectEmployee.Services;
using PMS.Features.TaskDetail.Services;
using PMS.Features.TaskDetail.ViewModels;
using PMS.Helpers;
using PMS.Notification;

namespace PMS.Features.TaskDetail
{

    [PmsAuthorize]
    public class TaskDetailController : Controller
    {
        private readonly ITaskDetailService _taskDetailService;
        private readonly IProjectEmployeeServices _projectEmployeeService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IDashboardService _dashboardService;
        public TaskDetailController(ITaskDetailService taskDetailService, IHubContext<NotificationHub> hub,
            IProjectEmployeeServices projectEmployeeService, IDashboardService dashboardService)
        {
            _taskDetailService = taskDetailService;
            _projectEmployeeService = projectEmployeeService;
            _hubContext = hub;
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> TaskDetail(int taskId)
        {
            var response = await _taskDetailService.GetTaskDetail(taskId);

            var projectId = Convert.ToInt32(HttpContext.Session.GetInt32("selectedProjectId") ?? 1);

            var mappedEmployees = await _projectEmployeeService.GetMappedProjectEmployee(projectId);

            ViewBag.Employees = new SelectList(mappedEmployees.models, "Id", "Name");

            var empModels = await _dashboardService.GetProjectEmployee(projectId);
            var taskStatusModels = await _dashboardService.GetTaskStatus(projectId);
            var taskTypeModels = await _dashboardService.GetTaskType(projectId);
            var taskPriority = await _dashboardService.GetTaskPriority(projectId);

            ViewBag.Status = new SelectList(taskStatusModels.Select(x => new SelectListItem { Text = x.Name, Value = x.Name }), "Value", "Text");
            ViewBag.Type = new SelectList(taskTypeModels.Select(x => new SelectListItem { Text = x.Name, Value = x.Name }), "Value", "Text");
            ViewBag.Priority = new SelectList(taskPriority.Select(x => new SelectListItem { Text = x.Name, Value = x.Name }), "Value", "Text");


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
            model.EmployeeId = Convert.ToInt32(HttpContext.GetEmployeeId());

            var response = await _taskDetailService.CreateDiscussion(model);

            await _hubContext.Clients.All.SendAsync("DiscussionAdded", model.TaskName, model.EmployeeName);

            return Json(response);
        }

        public async Task<IActionResult> ChangeStartDate(DateTime startDate, int taskId)
        {
            var response = await _taskDetailService.ChangeTaskStartDate(taskId,Convert.ToInt32(HttpContext.GetEmployeeId()),startDate);
            return Json(new {isSuccess= response.isSuccess, message= response.message });
        }

        public async Task<IActionResult> ChangeCompleteDate(DateTime startDate, int taskId)
        {
            var response = await _taskDetailService.ChangeTaskCompletedDate(taskId, Convert.ToInt32(HttpContext.GetEmployeeId()), startDate);
            return Json(new { isSuccess = response.isSuccess, message = response.message });
        }

        public async Task<IActionResult> ChangeStatus(string status, int taskId)
        {
            var response = await _taskDetailService.ChangeTaskStatus(taskId, Convert.ToInt32(HttpContext.GetEmployeeId()), status);
            return Json(new { isSuccess = response.isSuccess, message = response.message });
        }

        public async Task<IActionResult> AssignToEmployee(int empId, int taskId)
        {
            var response = await _taskDetailService.AssignToEmployee(taskId, Convert.ToInt32(HttpContext.GetEmployeeId()), empId);
            return Json(new { isSuccess = response.isSuccess, message = response.message });
        }
    }
}

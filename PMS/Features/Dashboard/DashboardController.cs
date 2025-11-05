using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenTelemetry.Trace;
using PMS.Attributes;
using PMS.Features.Dashboard.Services;
using PMS.Features.Project.Services;
using PMS.Features.ProjectEmployee.Services;
using PMS.Features.ProjectTask.Services;
using PMS.Features.UserManagement.Services;
using PMS.Helpers;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace PMS.Features.Dashboard
{


    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IProjectService _projectService;
        private readonly IProjectEmployeeServices _projectEmployeeService;
        private readonly IDashboardService _dashBoardService;
        private readonly IProjectTaskService _projectTaskService;
        private readonly IEmployeeService _employeeService;
        public DashboardController(ILogger<DashboardController> logger,
            IProjectService projectService, IProjectEmployeeServices projectEmpService,
            IDashboardService dashboardService, IProjectTaskService projectTaskService, IEmployeeService employeeService)
        {
            _logger = logger;
            _projectService = projectService;
            _projectEmployeeService = projectEmpService;
            _dashBoardService = dashboardService;
            _projectTaskService = projectTaskService;
            _employeeService = employeeService;
        }
        [PmsAuthorize]
        public async Task<IActionResult> Index()
        {
            var projectId = Convert.ToInt32(HttpContext.GetProjectId());

            var empModels = await _dashBoardService.GetProjectEmployee(projectId);
            var taskStatusModels = await _dashBoardService.GetTaskStatus(projectId);
            var taskTypeModels = await _dashBoardService.GetTaskType(projectId);
            var taskPriority = await _dashBoardService.GetTaskPriority(projectId);

            ViewBag.Employee = new SelectList(empModels, "Id", "Name");
            ViewBag.Status = new SelectList(taskStatusModels.Select(x => new SelectListItem { Text = x.Name, Value = x.Name }), "Value", "Text");
            ViewBag.Type = new SelectList(taskTypeModels.Select(x => new SelectListItem { Text = x.Name, Value = x.Name }), "Value", "Text");
            ViewBag.Priority = new SelectList(taskPriority.Select(x => new SelectListItem { Text = x.Name, Value = x.Name }), "Value", "Text");


            var responseModels = await _dashBoardService.GetDashboradDetails(projectId);
            return View(responseModels);
        }


        [AllowAnonymous]
        public async Task<IActionResult> ProjectSelection()
        {
            var response = await _projectService.GetProjectSelectionList(Convert.ToInt32(HttpContext.GetEmployeeId()), Convert.ToInt32(HttpContext.GetRoleId()), default);
            return View("~/Features/Dashboard/Views/ProjectSelection.cshtml", response.models);
        }

        public async Task<IActionResult> SelectProject(int projectId)
        {
            HttpContext.Session.SetInt32("selectedProjectId", projectId);
            var responseModel = await _projectService.GetProjectById(projectId, default);
            HttpContext.Session.SetString("selectedProjectName", responseModel.model?.Name ?? "PMS");
            return RedirectToAction("Index");
        }

        [PmsAuthorize]
        public async Task<IActionResult> GetMappedEmployee()
        {

            var responseModel = await _projectEmployeeService.GetMappedProjectEmployee(Convert.ToInt32(HttpContext.GetProjectId()));

            return PartialView("~/Features/Dashboard/Views/ProjectEmployee.cshtml", responseModel.models);
        }

        [PmsAuthorize]
        public async Task<IActionResult> DirectChat()
        {

            var responseModel = await _projectEmployeeService.GetMappedProjectEmployee(Convert.ToInt32(HttpContext.GetProjectId()));

            return PartialView("~/Features/Dashboard/Views/DirectChat.cshtml", responseModel.models);
        }

        public async Task<IActionResult> GetProjectTaskList(string status)
        {
            int projectId = Convert.ToInt32(HttpContext.GetProjectId());
            var response = await _dashBoardService.GetProjectTaskList(projectId, status, default);
            return PartialView("~/Features/ProjectTask/Views/ProjectTaskList.cshtml", response);
        }

        public async Task<IActionResult> GetProjectTaskListByEmpId(string status, string empCode)
        {
            int projectId = Convert.ToInt32(HttpContext.GetProjectId());

            var response = await _dashBoardService.GetEmpProjectTaskList(projectId, status, empCode, default);

            return PartialView("~/Features/ProjectTask/Views/ProjectTaskList.cshtml", response);
        }

        public async Task<IActionResult> FilterData(string taskType, string taskStatus, string taskPriority, int employeeId)
        {
            int projectId = Convert.ToInt32(HttpContext.GetProjectId());

            var response = await _dashBoardService.GetProjectTaskFilter(projectId, taskStatus, taskPriority, taskType, employeeId);

            return PartialView("~/Features/ProjectTask/Views/ProjectTaskList.cshtml", response);
        }

        public async Task<IActionResult> GetNotificationDetail()
        {
            var response = await _dashBoardService.GetNotificationDetail(Convert.ToInt32(HttpContext.GetEmployeeId()));
            return Json(response);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> GetEmployeeList()
        {
            var response = await _employeeService.GetEmployees(default);

            return PartialView("~/Features/Dashboard/Views/EmployeeList.cshtml", response.models);
        }


    }
}

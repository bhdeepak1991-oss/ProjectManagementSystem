using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS.Attributes;
using PMS.Features.Project.Services;
using PMS.Features.ProjectEmployee.Services;
using PMS.Helpers;

namespace PMS.Features.Dashboard
{

 
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IProjectService _projectService;
        private readonly IProjectEmployeeServices _projectEmployeeService;
        public DashboardController(ILogger<DashboardController> logger,
            IProjectService projectService, IProjectEmployeeServices projectEmpService)
        {
            _logger = logger;
            _projectService = projectService;
            _projectEmployeeService = projectEmpService;
        }
        [PmsAuthorize]
        public IActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        public async Task<IActionResult> ProjectSelection()
        {
            var response = await _projectService.GetProjectList(default);
            return View("~/Features/Dashboard/Views/ProjectSelection.cshtml", response.model);
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


        public IActionResult Privacy()
        {
            return View();
        }


    }
}

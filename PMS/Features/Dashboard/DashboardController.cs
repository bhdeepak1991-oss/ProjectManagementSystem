using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS.Features.Project.Services;

namespace PMS.Features.Dashboard
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IProjectService _projectService;
        public DashboardController(ILogger<DashboardController> logger, IProjectService projectService)
        {
            _logger = logger;
            _projectService = projectService;
        }

        public IActionResult Index()
        {
            return View();
        }

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

        public IActionResult Privacy()
        {
            return View();
        }


    }
}

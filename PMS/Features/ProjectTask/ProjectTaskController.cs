using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PMS.Features.ProjectTask.Services;

namespace PMS.Features.ProjectTask
{
    public class ProjectTaskController : Controller
    {
        private readonly IProjectTaskService _projectTaskService;

        public ProjectTaskController(IProjectTaskService projectTaskService)
        {
            _projectTaskService = projectTaskService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var response = await _projectTaskService.GetProjectTaskById(id, default);

            var projectId = Convert.ToInt32(HttpContext.Session.GetInt32("selectedProjectId") ?? 1);

            var projectEmployees = await _projectTaskService.GetProjectEmployee(projectId, default);

            ViewBag.ProjectEmployee = new SelectList(projectEmployees.models, "Id", "Name");

            return View("~/Features/ProjectTask/Views/CreateProjectTask.cshtml", response.models);
        }
    }
}

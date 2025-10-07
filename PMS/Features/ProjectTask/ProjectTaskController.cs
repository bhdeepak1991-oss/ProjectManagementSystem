using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PMS.Attributes;
using PMS.Features.ProjectTask.Services;
using PMS.Helpers;

namespace PMS.Features.ProjectTask
{

    [PmsAuthorize]
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
        [HttpPost]
        public async Task<IActionResult> CreateProjectTask(Domains.ProjectTask model)
        {
            if (model.Id == 0)
            {
                model.CreatedBy = Convert.ToInt32(HttpContext.GetEmployeeId());

                model.ProjectId = Convert.ToInt32(HttpContext.GetProjectId());

                var response = await _projectTaskService.CreateProjectTask(model, default);

                return Json(response);
            }

            var updateResponse = await _projectTaskService.UpdateProjectTask(model, default);

            return Json(updateResponse);
        }

        public async Task<IActionResult> GetProjectTask()
        {
            var projectId = Convert.ToInt32(HttpContext.Session.GetInt32("selectedProjectId") ?? 1);

            var response = await _projectTaskService.GetProjectTaskList(projectId, default);

            return PartialView("~/Features/ProjectTask/Views/ProjectTaskList.cshtml", response.models);
        }

        public async Task<IActionResult> ProjectTaskList()
        {
            var projectId = Convert.ToInt32(HttpContext.Session.GetInt32("selectedProjectId") ?? 1);

            var response = await _projectTaskService.GetProjectTaskList(projectId, default);

            return View("~/Features/ProjectTask/Views/ProjectTaskList.cshtml", response.models);
        }

        public async Task<IActionResult> DeleteProjectTask(int Id)
        {
            var response = await _projectTaskService.DeleteProjectTask(Id, default);

            return Json(response);
        }
    }
}

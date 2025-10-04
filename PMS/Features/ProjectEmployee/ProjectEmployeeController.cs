using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PMS.Features.Project.Services;
using PMS.Features.ProjectEmployee.Services;

namespace PMS.Features.ProjectEmployee
{
    public class ProjectEmployeeController : Controller
    {
        private readonly IProjectEmployeeServices _projectEmployeeServices;
        private readonly IProjectService _projectService;

        public ProjectEmployeeController(IProjectEmployeeServices projectEmployeeServices,IProjectService projectService)
        {
            _projectEmployeeServices = projectEmployeeServices;
            _projectService = projectService;
        }
        public async Task<IActionResult> Index()
        {
            var responseModels = await _projectService.GetProjectList(default);
            ViewBag.ProjectDetails = new SelectList(responseModels.model,"Id","Name");
            return View("~/Features/ProjectEmployee/Views/CreateProjectEmployee.cshtml");
        }

        public async Task<IActionResult> MappedEmployee(int projectId)
        {
            HttpContext.Session.SetInt32("projectId", projectId);
            var responseModels = await _projectEmployeeServices.GetMappedProjectEmployee(projectId);
            return PartialView("~/Features/ProjectEmployee/Views/MappedProjectEmployee.cshtml", responseModels.models);
        }

        public async Task<IActionResult> UnMappedEmployee(int projectId)
        {
            HttpContext.Session.SetInt32("projectId", projectId);
            var responseModels = await _projectEmployeeServices.GetUnMappedProjectEmployee(projectId);
            return PartialView("~/Features/ProjectEmployee/Views/UnMappedProjectEmployee.cshtml", responseModels.models);
        }

        public async Task<IActionResult> MappedUnMappedEmployee( int empId, bool isMapped)
        {
            int projectId = Convert.ToInt32(HttpContext.Session.GetInt32("projectId"));
            var responseModels = await _projectEmployeeServices.ProjectEmployeeMapping(projectId,empId, isMapped);
            return Json(responseModels);
        }
    }
}

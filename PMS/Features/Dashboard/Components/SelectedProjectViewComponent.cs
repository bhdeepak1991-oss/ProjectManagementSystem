using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Features.Project.Services;

namespace PMS.Features.Dashboard.Components
{
    public class SelectedProjectViewComponent : ViewComponent
    {
        private readonly IProjectService _projectService;

        public SelectedProjectViewComponent(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int projectId)
        {
            var selectedId = Convert.ToInt32(HttpContext.Session.GetInt32("selectedProjectId"));
            var project = await _projectService.GetProjectById(selectedId, default);
            return View("~/Features/Dashboard/Views/ProjectComponent.cshtml", project.model);
        }
    }
}

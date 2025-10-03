using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using PMS.Domains;
using PMS.Features.Project.Services;
using PMS.Features.UserManagement.Services;
using PMS.Notification;
using SelectPdf;

namespace PMS.Features.Project
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IEmployeeService _employeeService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public ProjectController(IProjectService projectService, IEmployeeService employeeService, IHubContext<NotificationHub> hubContext)
        {
            _projectService = projectService;
            _employeeService = employeeService;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index(int id)
        {

            var employeeList = await _employeeService.GetEmployees(default);

            ViewBag.Employee = new SelectList(employeeList.models.Select(e => new
            {
                e.Id,
                DisplayName = $"{e.Name} ({e.EmployeeCode})"
            }), "Id", "DisplayName");

            var projectModel = await _projectService.GetProjectById(id, default);



            return View("~/Features/project/Views/CreateProject.cshtml", projectModel.model);
        }

        [HttpPost]

        public async Task<IActionResult> CreateProject(Domains.Project model)
        {
            if (model.Id == 0)
            {
                var response = await _projectService.CreateProject(model, default);

                await _hubContext.Clients.All.SendAsync("ProjectCreated", model.Id, model.Name);

                return Json(response);
            }

            var updateResponse = await _projectService.UpdateProject(model, default);

            await _hubContext.Clients.All.SendAsync("ProjectCreated", model.Id, model.Name);


            return Json(updateResponse);
        }

        public async Task<IActionResult> GetProjectList()
        {
            var response = await _projectService.GetProjectList(default);
            return PartialView("~/Features/project/Views/ProjectList.cshtml", response.model);
        }

        public async Task<IActionResult> DeleteProject(int id)
        {
            var response = await _projectService.DeleteProject(id, default);
            return Json(response);
        }
        public async Task<IActionResult> GetProjectDetail(int id)
        {
            var dbModel = await _projectService.GetProjectById(id, default);

            string htmlContent = dbModel.model?.Description ?? "No Description Present !";

            // Initialize converter
            HtmlToPdf converter = new HtmlToPdf();

            // Customize header
            PdfHtmlSection headerHtml = new PdfHtmlSection($"<div style='font-size:14px; text-align:center; color:blue;'>Project Description For ({dbModel.model.Name})</div>");

            converter.Header.Add(headerHtml);
            converter.Header.Height = 50; // Header height in points (1 pt = 1/72 inch)

            // Customize footer
            PdfHtmlSection footerHtml = new PdfHtmlSection("<div style='font-size:12px; text-align:center; color:gray;'>Page &nbsp;{page_number} &nbsp; of &nbsp;{total_pages}</div>");

            converter.Footer.Add(footerHtml);
            converter.Footer.Height = 30;

            // Convert HTML to PDF
            PdfDocument doc = converter.ConvertHtmlString(htmlContent);

            byte[] pdf = doc.Save();
            doc.Close();

            return File(pdf, "application/pdf", $"{dbModel.model.Name}.pdf");
        }

        public async Task<IActionResult> UpdateProjectStatus(int id, string reason)
        {

        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using PMS.Attributes;
using PMS.Domains;
using PMS.Features.Document.Services;
using PMS.Features.Project.Services;
using PMS.Features.UserManagement.Services;
using PMS.Helpers;
using PMS.Notification;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using SelectPdf;
using System.Text;

namespace PMS.Features.Project
{
    [PmsAuthorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IEmployeeService _employeeService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IWebHostEnvironment _enviroment;
        private readonly IProjectDocumentService _projDocumentService;

        public ProjectController(IProjectService projectService,
            IEmployeeService employeeService, IHubContext<NotificationHub> hubContext, IWebHostEnvironment enviroment,
            IProjectDocumentService projectDocumentService)
        {
            _projectService = projectService;
            _employeeService = employeeService;
            _hubContext = hubContext;
            _enviroment = enviroment;
            _projDocumentService = projectDocumentService;
        }


        [AllowAnonymous]
        public async Task<IActionResult> Index(int id, bool projectSelection = false)
        {

            var employeeList = await _employeeService.GetEmployees(default);

            ViewBag.Employee = new SelectList(employeeList.models.Select(e => new
            {
                e.Id,
                DisplayName = $"{e.Name} ({e.EmployeeCode})"
            }), "Id", "DisplayName");

            var projectModel = await _projectService.GetProjectById(id, default);

            ViewBag.ProjectSelection = projectSelection;

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

            string templatePath = Path.Combine(_enviroment.WebRootPath, "HtmlTemplates", "ProjectDetailTemplate.html");
            string htmlContent = System.IO.File.ReadAllText(templatePath);

            htmlContent = htmlContent
                .Replace("{{projectShortName}}", GetProjectShortCode(dbModel.model?.Name ?? "Not Available"))
                .Replace("{{ProjectName}}", dbModel.model?.Name ?? "Not Available")
                .Replace("{{status}}", dbModel.model?.ProjectStatus ?? "Not Available")
                .Replace("{{ClientName}}", dbModel.model?.ClientName ?? "Not Available")
                .Replace("{{projectDescription}}", dbModel.model?.Description ?? "Not Available")
                .Replace("{{clientContactPersone}}", dbModel.model?.ClientContactPerson ?? "Not Available")
                .Replace("{{clientWebsite}}", dbModel.model?.ClientUrl ?? "Not Available")
                .Replace("{{clientContactPerson}}", dbModel.model?.ClientContactPerson ?? "Not Available");

            var projectDocuments = await _projDocumentService.GetProjctDocumentDetail(id);

            var documentDetails = new StringBuilder();

            foreach (var data in projectDocuments.models)
            {
                documentDetails.Append(@$"<div class=""doc"">
                            <div class=""label"">{data.DocumentName}</div>
                            <div class=""meta"">{data.DocumentPath}</div>
                        </div>");
            }

            htmlContent = htmlContent.Replace("{{projectDocuments}}", documentDetails.ToString());
            htmlContent = htmlContent.Replace("{{ProjectStartDate}}", dbModel.model?.ProjectStartDate?.ToShortDateString());
            htmlContent = htmlContent.Replace("{{ProjectEndDate}}", dbModel.model?.ProjectEndDate?.ToShortDateString());


            // Download Chromium
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            // Launch browser
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
            });

            var page = await browser.NewPageAsync();

            // Set HTML content
            await page.SetContentAsync(htmlContent);

            // PDF options
            var pdfOptions = new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                //DisplayHeaderFooter = true,
                //HeaderTemplate = @"<div style='font-size:12px;text-align:center;margin-top:5px;'>Project Report</div>",
                //FooterTemplate = @"<div style='font-size:10px;text-align:center;color:gray;padding:4px;'>
                //Page <span class='pageNumber'></span> of <span class='totalPages'></span>
                //</div>",
                MarginOptions = new MarginOptions
                {
                    Top = "60px",
                    Bottom = "40px"
                }
            };

            // Generate PDF
            var pdfBytes = await page.PdfDataAsync(pdfOptions);

            await browser.CloseAsync();

            return File(pdfBytes, "application/pdf", $"{dbModel.model?.Name}.pdf");
        }

        public async Task<IActionResult> UpdateProjectStatus(int id, string reason, string status)
        {
            var response = await _projectService.UpdateProjectStatus(id, reason, status);
            return Json(response);
        }

        public async Task<IActionResult> GetProjectDetailById(int projId)
        {
            projId = projId == 0 ? Convert.ToInt32(HttpContext.GetProjectId()) : projId;

            var response = await _projectService.GetProjectDetailById(projId, default);

            return View("~/Features/Project/Views/ProjectDetail.cshtml", response.models.FirstOrDefault());
        }

        public static string GetProjectShortCode(string projectName)
        {
            if (string.IsNullOrWhiteSpace(projectName))
                return string.Empty;

            // Split by space
            var words = projectName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (words.Length >= 2)
            {
                // Multiple words → take first letter of first 2 words
                return $"{char.ToUpper(words[0][0])}{char.ToUpper(words[1][0])}";
            }
            else
            {
                // Single word
                string word = words[0];

                if (word.Length == 1)
                {
                    return word.ToUpper(); // Return single letter
                }
                else
                {
                    return $"{char.ToUpper(word[0])}{char.ToUpper(word[1])}";
                }
            }
        }

    }
}

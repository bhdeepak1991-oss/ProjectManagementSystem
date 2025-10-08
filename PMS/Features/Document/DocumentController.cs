using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PMS.Features.Document.Services;
using PMS.Features.Document.ViewModels;
using PMS.Features.ProjectEmployee.Services;
using PMS.Helpers;

namespace PMS.Features.Document
{
    public class DocumentController : Controller
    {
        private readonly IProjectDocumentService _projectDocumentService;
        private readonly IProjectEmployeeServices _projectEmployeeService;
        public DocumentController(IProjectDocumentService projectDocumentService, IProjectEmployeeServices empService)
        {
            _projectDocumentService = projectDocumentService;
            _projectEmployeeService = empService;
        }

        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View("~/Features/Document/Views/UploadDocument.cshtml", new ProjectDocumentVm()));
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocument(ProjectDocumentVm model)
        {
            model.ProjectId =Convert.ToInt32(HttpContext.GetProjectId());
            model.CreatedBy = Convert.ToInt32(HttpContext.GetUserId());
            
            var response = await _projectDocumentService.UploadProjectDocument(model, default);

            return Json(response);
        }

        public async Task<IActionResult> GetDocumentDetail()
        {
            var response= await _projectDocumentService.GetProjectDocuments(default);

            var projEmployees = await _projectEmployeeService.GetMappedProjectEmployee(Convert.ToInt32(HttpContext.GetProjectId()));

            ViewBag.Employee = new SelectList(projEmployees.models, "Id", "Name");

            return PartialView("~/Features/Document/Views/DocumentList.cshtml", response.models);
        }

        public async Task<IActionResult> DeleteDocument(int id)
        {
            var response = await _projectDocumentService.DeleteProjectDocument(id,1,default);

            return Json(response);
        }
    }
}

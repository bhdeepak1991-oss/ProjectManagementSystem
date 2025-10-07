using Microsoft.AspNetCore.Mvc;

namespace PMS.Features.ProjectDocument
{
    public class ProjectDocumentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

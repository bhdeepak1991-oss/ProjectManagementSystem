using Microsoft.AspNetCore.Mvc;

namespace PMS.Features.TaskDetail
{
    public class TaskDetailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

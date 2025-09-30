using Microsoft.AspNetCore.Mvc;

namespace PMS.Features.UserManagement
{
    public class UserManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace PMS.Features.Notification
{
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

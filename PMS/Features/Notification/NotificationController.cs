using Microsoft.AspNetCore.Mvc;
using PMS.Attributes;

namespace PMS.Features.Notification
{

    [PmsAuthorize]
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

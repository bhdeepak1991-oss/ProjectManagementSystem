using Microsoft.AspNetCore.Mvc;
using PMS.Features.Notification.Services;

namespace PMS.Features.Dashboard.Components
{
    public class MessageViewComponent: ViewComponent
    {
        private readonly INotificationService _notificationService;

        public MessageViewComponent(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var messageModels = await _notificationService.GetMessage("Bhavesh Deepak");

            return View("~/Features/Dashboard/Views/Message.cshtml", messageModels.models);
        }
    }
}

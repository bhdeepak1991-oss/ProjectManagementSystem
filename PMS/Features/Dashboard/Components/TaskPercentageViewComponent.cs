using Microsoft.AspNetCore.Mvc;
using PMS.Features.Dashboard.Services;
using PMS.Helpers;

namespace PMS.Features.Dashboard.Components
{
    public class TaskPercentageViewComponent : ViewComponent
    {
        private readonly IDashboardService _dashboardService;
        public TaskPercentageViewComponent(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var responseModels = await _dashboardService.GetProjectTaskPercetages(Convert.ToInt32(HttpContext.GetEmployeeId()));

            return View("~/Features/Dashboard/Views/ProjectPercentage.cshtml", responseModels);
        }
    }
}

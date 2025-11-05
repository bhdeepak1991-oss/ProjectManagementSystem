using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.Features.AccessManagement.Services;
using PMS.Helpers;

namespace PMS.Features.AccessManagement.Components
{
    public class SidebarMenuViewComponent : ViewComponent
    {
        private readonly IAccessManagementService _accessManagementService;

        public SidebarMenuViewComponent(IAccessManagementService accessManagementService)
        {
            _accessManagementService = accessManagementService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int roleId = Convert.ToInt32(HttpContext.GetRoleId());

            var response = await _accessManagementService.GetMenuSubmenuByRole(roleId, CancellationToken.None);

            return View("~/Features/AccessManagement/Views/SideBarMenu.cshtml", response.models);
        }
    }
}

using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using PMS.Domains;
using PMS.Features.AccessManagement.Services;
using PMS.Helpers;

namespace PMS.Features.AccessManagement
{
    public class AccessManagementController : Controller
    {
        private readonly IAccessManagementService _accessManagementService;

        public AccessManagementController(IAccessManagementService accessManagementService)
        {
            _accessManagementService = accessManagementService;
        }
        public async Task<IActionResult> Index(int id)
        {
            var responseModel = await _accessManagementService.GetMenuSubmenuById(id, CancellationToken.None);

            return View("~/Features/AccessManagement/Views/MenuSubMenu.cshtml", responseModel.model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenuSubMenu(MenuMaster model)
        {
            model.CreatedBy = Convert.ToInt32(HttpContext.GetEmployeeId());
            model.UpdatedBy= Convert.ToInt32(HttpContext.GetEmployeeId());
            model.CreatedDate = DateTime.Now;
            model.UpdatedDate = DateTime.Now;
            var responseModel = await _accessManagementService.CreateMenuSubmenu(model, CancellationToken.None);

            return Json(responseModel.message);
        }

        public async Task<IActionResult> GetMenuSubMenuList()
        {
            var responseModel = await _accessManagementService.GetMenuSubmenuList(CancellationToken.None);

            return PartialView("~/Features/AccessManagement/Views/MenuSubMenuList.cshtml", responseModel.models);
        }

        public async Task<IActionResult> DeleteMenuSubMenuList(int id)
        {
            var responseModel = await _accessManagementService.DeleteMenuMaster(id,CancellationToken.None);

            return Json(responseModel.message);
        }
    }
}

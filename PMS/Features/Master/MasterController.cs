using Microsoft.AspNetCore.Mvc;
using PMS.Constants;
using PMS.Domains;
using PMS.Features.Master.Services;

namespace PMS.Features.Master
{
    public class MasterController : Controller
    {
        private readonly IRoleService _roleService;

        public MasterController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public async Task<IActionResult> RoleList()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var response = await _roleService.GetRoles(default);
                return PartialView("~/Features/Master/Views/RoleList.cshtml", response);
            }
            return RedirectToAction(nameof(CreateRole),0);
        }

        public async Task<IActionResult> CreateRole(int id)
        {
            var roleModel = await _roleService.GetRoleById(id, default);
            return View("~/Features/Master/Views/CreateRole.cshtml", roleModel ?? new Domains.RoleMaster());
        }

        [HttpPost]
        public async Task<IActionResult> UpsertRole(RoleMaster model)
        {
            if (model.Id != 0)
            {
                var updateResponse = await _roleService.UpdateRole(model, default);

                return Json(new { message = MessageConstant.RoleUpdatedSuccessfully, isSuccess = true });
            }
            var response = await _roleService.CreateRole(model, default);

            return Json(new { message = MessageConstant.RoleCreatedSuccessfully, isSuccess = true });
        }

        public async Task<IActionResult> DeleteRole(int id)
        {
            var deleteResponse = await _roleService.DeleteRole(id, default);

            return Json(new { message = MessageConstant.RoleDeletedSuccessfully, isSuccess = true });
        }
    }
}

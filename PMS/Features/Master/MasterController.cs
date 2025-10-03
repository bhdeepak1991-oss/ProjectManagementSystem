using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PMS.Constants;
using PMS.Domains;
using PMS.Features.Master.Services;

namespace PMS.Features.Master
{
    public class MasterController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IDepartmentService _departmentService;
        private readonly IDesignationService _designationService;
        private readonly ITaskStatusService _taskStatusService;
        public MasterController(IRoleService roleService,
            IDepartmentService departmentService, IDesignationService designationService,
            ITaskStatusService taskStatusService)
        {
            _roleService = roleService;
            _departmentService = departmentService;
            _designationService = designationService;
            _taskStatusService = taskStatusService;
        }

        #region Role Features
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

        #endregion

        #region Department Features

        public async Task<IActionResult> DepartmentList()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var response = await _departmentService.GetDepartments(default);
                return PartialView("~/Features/Master/Views/DepartmentList.cshtml", response.Item3);
            }
            return RedirectToAction(nameof(CreateRole), 0);
        }

        public async Task<IActionResult> CreateDepartment(int id)
        {
            var responseModel = await _departmentService.GetDepartmentById(id, default);
            return View("~/Features/Master/Views/CreateDepartment.cshtml", responseModel.model ?? new Domains.DepartmentMaster());
        }

        [HttpPost]
        public async Task<IActionResult> UpsertDepartment(DepartmentMaster model)
        {
            if (model.Id != 0)
            {
                var updateResponse = await _departmentService.UpdateDepartment(model, default);

                return Json(new { message = MessageConstant.RoleUpdatedSuccessfully, isSuccess = true });
            }
            var response = await _departmentService.CreateDepartment(model, default);

            return Json(new { message = MessageConstant.RoleCreatedSuccessfully, isSuccess = true });
        }

        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var deleteResponse = await _departmentService.DeleteDepartment(id, default);

            return Json(new { message = MessageConstant.RoleDeletedSuccessfully, isSuccess = true });
        }

        #endregion

        #region Designation Features

        public async Task<IActionResult> DeasignationList()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var response = await _designationService.GetAllDesignation(default);
                return PartialView("~/Features/Master/Views/DesignationList.cshtml", response.model);
            }
            return RedirectToAction(nameof(CreateRole), 0);
        }

        public async Task<IActionResult> CreateDesignation(int id)
        {
            var responseModel = await _designationService.GetDesignationById(id, default);
            var departmentList = await _departmentService.GetDepartments(default);
            ViewBag.DepartmentList =new SelectList(departmentList.Item3.ToList(), "Id", "Name");
            return View("~/Features/Master/Views/CreateDesignation.cshtml", responseModel.model ?? new Domains.DesignationMaster());
        }

        [HttpPost]
        public async Task<IActionResult> UpsertDesignation(DesignationMaster model)
        {
            if (model.Id != 0)
            {
                var updateResponse = await _designationService.UpdateDesignation(model, default);

                return Json(new { message = MessageConstant.RoleUpdatedSuccessfully, isSuccess = true });
            }
            var response = await _designationService.CreateDesignation(model, default);

            return Json(new { message = MessageConstant.RoleCreatedSuccessfully, isSuccess = true });
        }

        public async Task<IActionResult> DeleteDesignation(int id)
        {
            var deleteResponse = await _designationService.DeleteDesignation(id, default);

            return Json(new { message = MessageConstant.RoleDeletedSuccessfully, isSuccess = true });
        }

        #endregion

        #region Task Status Features

        public async Task<IActionResult> TaskList()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var response = await _taskStatusService.GetTaskStatusDetail(default);
                return PartialView("~/Features/Master/Views/TaskList.cshtml", response.model);
            }
            return RedirectToAction(nameof(CreateRole), 0);
        }

        public async Task<IActionResult> CreateTask(int id)
        {
            var responseModel = await _taskStatusService.GetTaskStatusById(id, default);
            return View("~/Features/Master/Views/CreateTask.cshtml", responseModel.model ?? new Domains.TaskStatusMaster());
        }

        [HttpPost]
        public async Task<IActionResult> UpsertTask(TaskStatusMaster model)
        {
            if (model.Id != 0)
            {
                var updateResponse = await _taskStatusService.UpdateTaskStatus(model, default);

                return Json(new { message = MessageConstant.RoleUpdatedSuccessfully, isSuccess = true });
            }
            var response = await _taskStatusService.CreateTaskStatus(model, default);

            return Json(new { message = MessageConstant.RoleCreatedSuccessfully, isSuccess = true });
        }

        public async Task<IActionResult> DeleteTaskStatus(int id)
        {
            var deleteResponse = await _taskStatusService.DeleteTaskStatus(id, default);

            return Json(new { message = MessageConstant.RoleDeletedSuccessfully, isSuccess = true });
        }

        #endregion
    }
}

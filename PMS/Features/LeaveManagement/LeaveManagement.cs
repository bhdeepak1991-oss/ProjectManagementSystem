using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PMS.Domains;
using PMS.Features.LeaveManagement.Services;
using PMS.Helpers;

namespace PMS.Features.LeaveManagement
{
    public class LeaveManagement : Controller
    {
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly IEmployeeLeaveService _employeeLeaveService;
        public LeaveManagement(ILeaveTypeService leaveTypeService, IEmployeeLeaveService employeeLeaveService)
        {
            _leaveTypeService = leaveTypeService;
            _employeeLeaveService = employeeLeaveService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var response = await _leaveTypeService.GetLeaveTypeById(id, default);
            return View("~/Features/LeaveManagement/Views/CreateLeaveType.cshtml", response.model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeaveType(LeaveType model)
        {
            if (model.Id == 0)
            {
                var response = await _leaveTypeService.CreateLeaveType(model, default);
                return Json(response.message);
            }

            var updateResponse = await _leaveTypeService.UpdateLeaveType(model, default);
            return Json(updateResponse.message);

        }

        [HttpGet]
        public async Task<IActionResult> GetLeaveType()
        {
            var response = await _leaveTypeService.GetLeaveType(default);

            return PartialView("~/Features/LeaveManagement/Views/LeaveTypeList.cshtml", response.models);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteLeaveType(int id)
        {
            var response = await _leaveTypeService.DeleteLeaveType(id, default);
            return Json(response.message);
        }

        public async Task<IActionResult> LeaveRequest()
        {
            var responseModels = await _leaveTypeService.GetLeaveType(default);
            ViewBag.LeaveType = responseModels.models.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.LeaveTypeName + " (" + x.LeaveTypeCode + ")"
            }).ToList();

            return await Task.Run(() => View("~/Features/LeaveManagement/Views/LeaveRequest.cshtml", new EmployeeLeave()));
        }

        public async Task<IActionResult> GetLeaveTypeDetail(int leaveType)
        {
            var empId = Convert.ToInt32(HttpContext.GetEmployeeId());

            var response = await _employeeLeaveService.GetLeaveCountDetail(leaveType, empId);

            return Json(response.model);
        }

        [HttpPost]
        public async Task<IActionResult> LeaveRequest(EmployeeLeave model)
        {
            var response = await _employeeLeaveService.CreateEmployeeLeave(model, default);

            return Json(response.message);
        }
    }
}

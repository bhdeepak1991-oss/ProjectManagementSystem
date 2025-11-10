using Azure;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using PMS.Attributes;
using PMS.Domains;
using PMS.Features.LeaveManagement.Services;
using PMS.Helpers;
using PMS.Notification;

namespace PMS.Features.LeaveManagement
{

    [PmsAuthorize]
    public class LeaveManagement : Controller
    {
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly IEmployeeLeaveService _employeeLeaveService;
        private readonly IHubContext<NotificationHub> _hubContext;
        public LeaveManagement(ILeaveTypeService leaveTypeService, IEmployeeLeaveService employeeLeaveService, IHubContext<NotificationHub> hubContext)
        {
            _leaveTypeService = leaveTypeService;
            _employeeLeaveService = employeeLeaveService;
            _hubContext = hubContext;
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
            model.EmployeeId = Convert.ToInt32(HttpContext.GetEmployeeId());

            var response = await _employeeLeaveService.CreateEmployeeLeave(model, default);

            await _hubContext.Clients.All.SendAsync("LeaveRequest", "New Leave Request has been created !");

            return Json(response.message);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeLeaveRequest()
        {
            var empId = Convert.ToInt32(HttpContext.GetEmployeeId());

            var response = await _employeeLeaveService.GetEmployeeLeaves(empId, default);

            return PartialView("~/Features/LeaveManagement/Views/LeaveRequestList.cshtml", response.models);
        }

        public async Task<IActionResult> RequestApproval()
        {
            var empId = Convert.ToInt32(HttpContext.GetEmployeeId());

            var response = await _employeeLeaveService.GetEmployeeRequest(empId);

            return View("~/Features/LeaveManagement/Views/RequestApprovalList.cshtml", response.model);
        }

        public async Task<IActionResult> ApproveRejectRequest(int empId, bool isApproved, int requestId)
        {
            var managerId = Convert.ToInt32(HttpContext.GetEmployeeId());

            var response = await _employeeLeaveService.ApprovedReject(empId, managerId, isApproved, requestId);

            await _hubContext.Clients.All.SendAsync("LeaveRequest", $"Leave Request has been {(isApproved? "Approved":"Rejected")}");

            return Json(response.message);
        }
    }
}

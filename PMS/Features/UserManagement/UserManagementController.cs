using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OtpNet;
using PMS.Attributes;
using PMS.Domains;
using PMS.Features.Master.Services;
using PMS.Features.UserManagement.Services;
using PMS.Features.UserManagement.ViewModels;
using PMS.Helpers;
using QRCoder;

namespace PMS.Features.UserManagement
{


    public class UserManagementController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IDesignationService _designationService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserManagementController(IEmployeeService employeeService,
            IDesignationService designationService, IDepartmentService departmentService, IUserService userService, IRoleService roleService)
        {
            _employeeService = employeeService;
            _designationService = designationService;
            _departmentService = departmentService;
            _userService = userService;
            _roleService = roleService;
        }

        [PmsAuthorize]
        public async Task<IActionResult> CreateEmployee(int empId)
        {
            var empModel = await _employeeService.GetEmployeeById(empId);

            var departmentModels = await _departmentService.GetDepartments(default);

            var designationModels = await _designationService.GetAllDesignation(default);

            ViewBag.Departments = new SelectList(departmentModels.Item3, "Id", "Name");

            ViewBag.Designation = new SelectList(designationModels.Item3, "Id", "Name");

            return View("~/Features/UserManagement/Views/CreateEmployee.cshtml", empModel.model ?? new Employee());
        }

        [HttpPost]
        [PmsAuthorize]
        public async Task<IActionResult> CreateEmployeePost(Employee model)
        {
            if (model.Id == 0)
            {
                var response = await _employeeService.CreateEmployee(model);

                return Json(response);
            }

            var updateResponse = await _employeeService.UpdateEmployee(model);

            return Json(updateResponse);
        }

        [PmsAuthorize]
        public async Task<IActionResult> GetEmployeeList()
        {
            var response = await _employeeService.GetEmployees(default);

            return PartialView("~/Features/UserManagement/Views/EmployeeList.cshtml", response.models);
        }

        [PmsAuthorize]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var response = await _employeeService.DeleteEmployee(id);
            return Json(response);
        }

        [PmsAuthorize]
        public async Task<IActionResult> CreateUser(int userId)
        {
            var responseModel = await _userService.GetUserById(userId, default);

            var empModels = await _employeeService.GetEmployees(default);

            var roleModels = await _roleService.GetRoles(default);

            ViewBag.Role = new SelectList(roleModels, "Id", "Name");

            ViewBag.Employee = new SelectList(empModels.models, "Id", "Name");

            return View("~/Features/UserManagement/Views/CreateUser.cshtml", responseModel.model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(Domain.UserManagement model)
        {
            var responseModels = await _userService.CreateUser(model, default);

            return Json(responseModels);
        }


        [AllowAnonymous]
        public async Task<IActionResult> Authenticate()
        {
            return await Task.Run(() => View("~/Features/UserManagement/Views/Login.cshtml", new Domain.UserManagement()));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticatePost(Domain.UserManagement model)
        {
            var response = await _userService.Authenticate(model);

            if (response.Id != 0)
            {
                string issuer = "PMSProject";

                string email = response?.UserName ?? string.Empty;

                string totpUri = $"otpauth://totp/{issuer}:{email}?secret={response?.AuthenticatorKey}&issuer={issuer}&digits=6";

                HttpContext.Session.SetString("authCode", response?.AuthenticatorKey ?? string.Empty);

                HttpContext.Session.SetObject<Domain.UserManagement>("user", response);

                HttpContext.Session.SetInt32("userId", response.Id);

                HttpContext.Session.SetInt32("employeeId", Convert.ToInt32(response.EmployeeId));

                var empModel = await _employeeService.GetEmployeeById(Convert.ToInt32(response.EmployeeId));

                HttpContext.Session.SetObject<Domains.Employee>("employee", empModel.model);



                if (response.IsTempPassword == true)
                {
                    return RedirectToAction("ChangePassword");
                }


                return RedirectToAction("ProjectSelection", "Dashboard");
                // return RedirectToAction("TwoFactorAuth", "UserManagement", new { totpUri = totpUri });

            }

            return await Task.Run(() => View("~/Features/UserManagement/Views/Login.cshtml", new Domain.UserManagement()));
        }
        public async Task<IActionResult> TwoFactorAuth(string totpUri)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(totpUri, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new Base64QRCode(qrCodeData);
            var qrCodeImageAsBase64 = qrCode.GetGraphic(20);

            ViewBag.QRCodeImageAsBase64 = qrCodeImageAsBase64;

            return await Task.Run(() => View("~/Features/UserManagement/Views/TwoFactorAuth.cshtml"));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> TwoFactorAuthPost(AuthenticatorCode model)
        {
            var authCode = Base32Encoding.ToBytes(HttpContext.Session.GetString("authCode"));
            var totp = new Totp(authCode);
            string code = totp.ComputeTotp();
            var userModel = HttpContext.Session.GetObject<Domain.UserManagement>("user");


            bool isValid = totp.VerifyTotp(model.AuthenticatorKey, out long timeStepMatched, new VerificationWindow(2, 2));

            if (isValid)
            {
                return RedirectToAction("ProjectSelection", "Dashboard");
            }

            string issuer = "PMSProject";

            string email = userModel?.UserName ?? string.Empty;

            string totpUri = $"otpauth://totp/{issuer}:{email}?secret={userModel?.AuthenticatorKey}&issuer={issuer}&digits=6";

            HttpContext.Session.SetString("authCode", userModel?.AuthenticatorKey ?? string.Empty);

            return await Task.Run(() => RedirectToAction("TwoFactorAuth", "UserManagement", new { totpUri = totpUri }));
        }

        [PmsAuthorize]
        public async Task<IActionResult> UserListDetail()
        {
            var response = await _userService.GetUserList(default);
            return PartialView("~/Features/UserManagement/Views/UserList.cshtml", response.models);
        }

        [PmsAuthorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await _userService.DeleteUser(id, default);
            return Json(response);
        }


        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword()
        {
            return await Task.Run(() => PartialView("~/Features/UserManagement/Views/ChangePassword.cshtml", new ChangePasswordVm()));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(ChangePasswordVm model)
        {
            model.UserId = Convert.ToInt32(HttpContext.GetUserId());

            var response = await _userService.ChangesPassword(model);

            return RedirectToAction("Authenticate");
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();

            return await Task.Run(() => RedirectToAction("Authenticate"));
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeDetail()
        {
            var empId = Convert.ToInt32(HttpContext.GetEmployeeId());

            var response = await _employeeService.GetEmployeeDetailById(empId);

            return View("~/Features/UserManagement/Views/EmployeeProfile.cshtml", response.model);
        }
    }
}

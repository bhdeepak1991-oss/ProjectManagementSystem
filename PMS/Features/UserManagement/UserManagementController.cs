using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PMS.Domains;
using PMS.Features.Master.Services;
using PMS.Features.UserManagement.Services;

namespace PMS.Features.UserManagement
{
    public class UserManagementController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IDesignationService _designationService;

        public UserManagementController(IEmployeeService employeeService, IDesignationService designationService, IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _designationService = designationService;
            _departmentService = departmentService;
        }

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
        public async Task<IActionResult> CreateEmployeePost(Employee model)
        {
            var response = await _employeeService.CreateEmployee(model);

            return Json(response);
        }

        public async Task<IActionResult> GetEmployeeList()
        {
            var response = await _employeeService.GetEmployees(default);

            return PartialView("~/Features/UserManagement/Views/EmployeeList.cshtml", response.models);
        }
    }
}

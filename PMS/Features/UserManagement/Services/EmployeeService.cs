using PMS.Domains;
using PMS.Features.Master.Respositories;
using PMS.Features.UserManagement.Respositories;
using PMS.Features.UserManagement.ViewModels;

namespace PMS.Features.UserManagement.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDesignationRepository _designationRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IDesignationRepository designationRepository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _designationRepository = designationRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<(string message, bool isSuccess)> CreateEmployee(Employee model)
        {
            return await _employeeRepository.CreateEmployee(model, default);
        }

        public async Task<(string message, bool isSuccess)> DeleteEmployee(int empId)
        {
            return await _employeeRepository.DeleteEmployee(empId, default);
        }

        public async Task<(string message, bool isSuccess, Employee model)> GetEmployeeById(int empId)
        {
            return await _employeeRepository.GetEmployeeById(empId, default);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<EmployeeViewModel> models)> GetEmployees(CancellationToken cancellationToken)
        {
            var departmentModels = await _departmentRepository.GetAllDepartments(default);

            var designationModels = await _designationRepository.GetAllDesignation(default);

            var employeeModels = await _employeeRepository.GetEmployeeList(default);

            var responseModels = employeeModels.empModels.ToList().Select(emp => new EmployeeViewModel()
            {
                Id=emp.Id,
                Name = emp.Name,
                DepartmentName = departmentModels.model.FirstOrDefault(x => x.Id == emp.DepartmentId)?.Name ?? string.Empty,
                DesignationName = designationModels.model.FirstOrDefault(x => x.Id == emp.DesignationId)?.Name ?? string.Empty,
                EmailId = emp.EmailId,
                PhoneNumber = emp.PhoneNumber,
                DateOfBirth = emp.DateOfBirth,
                DateOfJoining = emp.DateOfJoining,
                EmployeeCode = emp.EmployeeCode
            }).ToList();

            return ("Employee Fetched successfully", true, responseModels);
        }

        public async Task<(string message, bool isSuccess)> UpdateEmployee(Employee model)
        {
            return await _employeeRepository.UpdateEmployee(model, default);
        }
    }
}

using PMS.Domains;
using PMS.Features.UserManagement.ViewModels;

namespace PMS.Features.UserManagement.Services
{
    public interface IEmployeeService
    {
        Task<(string message, bool isSuccess, IEnumerable<EmployeeViewModel> models)> GetEmployees(CancellationToken cancellationToken);

        Task<(string message, bool isSuccess, Employee model)> GetEmployeeById(int empId);

        Task<(string message, bool isSuccess)> CreateEmployee(Employee model);

        Task<(string message, bool isSuccess)> UpdateEmployee(Employee model);

        Task<(string message, bool isSuccess)> DeleteEmployee(int empId);

        Task<(string message, bool isSuccess, EmployeeVm model)> GetEmployeeDetailById(int empId);
    }
}

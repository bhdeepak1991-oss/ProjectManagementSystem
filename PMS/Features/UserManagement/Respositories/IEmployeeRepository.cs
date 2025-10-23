using PMS.Domains;
using PMS.Features.UserManagement.ViewModels;

namespace PMS.Features.UserManagement.Respositories
{
    public interface IEmployeeRepository
    {
        Task<(string message, bool isSuccess)> CreateEmployee(Employee empModel, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> UpdateEmployee(Employee empModel, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<Employee> empModels)> GetEmployeeList(CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, Employee model)> GetEmployeeById(int empId, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> DeleteEmployee(int empId, CancellationToken cancellationToken);

        Task<(string message, bool isSuccess, EmployeeVm model)> GetEmployeeDetailById(int empId);


    }
}

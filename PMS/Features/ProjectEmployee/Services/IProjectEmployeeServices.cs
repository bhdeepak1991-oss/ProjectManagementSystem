using PMS.Domains;
using PMS.Features.ProjectEmployee.ViewModels;
using PMS.Features.UserManagement.ViewModels;

namespace PMS.Features.ProjectEmployee.Services
{
    public interface IProjectEmployeeServices
    {
        Task<(string message, bool isSuccess)> ProjectEmployeeMapping(int projectId, int employeeId, bool isMapped);

        Task<(string message, bool isSuccess, IEnumerable<EmployeeViewModel> models)> GetMappedProjectEmployee(int projectId);

        Task<(string message, bool isSuccess, IEnumerable<EmployeeViewModel> models)> GetUnMappedProjectEmployee(int projectId);
    }
}

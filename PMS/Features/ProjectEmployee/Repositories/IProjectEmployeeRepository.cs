using PMS.Domains;

namespace PMS.Features.ProjectEmployee.Repositories
{
    public interface IProjectEmployeeRepository
    {
        Task<(string message, bool isSuccess)> ProjectEmployeeMapping(int projectId, int employeeId, bool isMapped);

        Task<(string message,bool isSuccess, IEnumerable<Employee> MappedProjectEmployee) > GetMappedEmployee(int projectId);

        Task<(string message, bool isSuccess, IEnumerable<Employee> UnMappedProjectEmployee)> GetUnMappedEmployee(int projectId);
    }
}

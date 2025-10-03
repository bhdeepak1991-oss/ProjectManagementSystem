using PMS.Domains;

namespace PMS.Features.Master.Services
{
    public interface IDepartmentService
    {
        Task<(string message, bool isSuccess, IEnumerable<DepartmentMaster?>)> GetDepartments(CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> CreateDepartment(DepartmentMaster model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, DepartmentMaster? model)> GetDepartmentById(int deptId, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> UpdateDepartment(DepartmentMaster model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> DeleteDepartment(int deptId, CancellationToken cancellationToken);
    }
}

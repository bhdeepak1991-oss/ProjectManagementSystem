using PMS.Domains;

namespace PMS.Features.Master.Respositories
{
    public interface IDepartmentRepository
    {
        Task<(string message, bool isSuccess)> CreateDepartment(DepartmentMaster model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> UpdateDepartment(DepartmentMaster model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> DeleteDepartment(int id, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, DepartmentMaster? model)> GetDepartmentById(int id, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<DepartmentMaster> model)> GetAllDepartments(CancellationToken cancellationToken);
    }
}

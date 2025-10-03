
using PMS.Domains;
using PMS.Features.Master.Respositories;

namespace PMS.Features.Master.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public async Task<(string message, bool isSuccess)> CreateDepartment(DepartmentMaster model, CancellationToken cancellationToken)
        {
            return await _departmentRepository.CreateDepartment(model, cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> DeleteDepartment(int deptId, CancellationToken cancellationToken)
        {
            return await _departmentRepository.DeleteDepartment(deptId, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, DepartmentMaster? model)> GetDepartmentById(int deptId, CancellationToken cancellationToken)
        {
            return await _departmentRepository.GetDepartmentById(deptId, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<DepartmentMaster?>)> GetDepartments(CancellationToken cancellationToken)
        {
           return await _departmentRepository.GetAllDepartments(cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> UpdateDepartment(DepartmentMaster model, CancellationToken cancellationToken)
        {
            return await _departmentRepository.UpdateDepartment(model, cancellationToken);
        }
    }
}

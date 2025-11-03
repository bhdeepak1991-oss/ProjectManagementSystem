using PMS.Domains;
using PMS.Features.LeaveManagement.ViewModels;

namespace PMS.Features.LeaveManagement.Repositories
{
    public interface IEmployeeLeaveRepository
    {
        Task<(string message, bool isSuccess)> CreateEmployeeLeave(EmployeeLeave model, CancellationToken cancellationToken);

        Task<(string message, bool isSuccess, IEnumerable<EmployeeLeaveVm> models)>
                    GetEmployeeLeaves(int empId, CancellationToken cancellationToken);

        Task<(string message, bool isSuccess, LeaveCountVm model)> GetLeaveCountDetail(int leaveType, int empId);
    }
}

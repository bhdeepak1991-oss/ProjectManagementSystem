using PMS.Domains;
using PMS.Features.LeaveManagement.ViewModels;

namespace PMS.Features.LeaveManagement.Services
{
    public interface IEmployeeLeaveService
    {
        Task<(string message, bool isSuccess)> CreateEmployeeLeave(EmployeeLeave model, CancellationToken cancellationToken);

        Task<(string message, bool isSuccess, IEnumerable<EmployeeLeaveVm> models)>
                    GetEmployeeLeaves(int empId, CancellationToken cancellationToken);

        Task<(string message, bool isSuccess, LeaveCountVm model)> GetLeaveCountDetail(int leaveType, int empId);
        Task<(string message, bool isSuccess, IEnumerable<EmployeeLeaveVm> model)> GetEmployeeRequest(int managerId);

        Task<(string message, bool isSuccess)> ApprovedReject(int empId, int managerId, bool isApproved, int requestId);
    }
}

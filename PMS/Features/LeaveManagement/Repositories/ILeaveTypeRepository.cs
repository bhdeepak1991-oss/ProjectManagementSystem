using PMS.Domains;

namespace PMS.Features.LeaveManagement.Repositories
{
    public interface ILeaveTypeRepository
    {
        Task<(string message, bool isSuccess)> CreateLeaveType(LeaveType model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<LeaveType> models)> GetLeaveType(CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, LeaveType model)> GetLeaveTypeById(int id,CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, LeaveType model)> DeleteLeaveType(int id, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> UpdateLeaveType(LeaveType model, CancellationToken cancellationToken);
    }
}

using PMS.Domains;
using PMS.Features.LeaveManagement.Repositories;

namespace PMS.Features.LeaveManagement.Services
{
    public class LeaveTypeService : ILeaveTypeService
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public LeaveTypeService(ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
        }

        public async  Task<(string message, bool isSuccess)> CreateLeaveType(LeaveType model, CancellationToken cancellationToken)
        {
            return await _leaveTypeRepository.CreateLeaveType(model, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, LeaveType model)> DeleteLeaveType(int id, CancellationToken cancellationToken)
        {
            return await _leaveTypeRepository.DeleteLeaveType(id, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<LeaveType> models)> GetLeaveType(CancellationToken cancellationToken)
        {
            return await _leaveTypeRepository.GetLeaveType(cancellationToken);
        }

        public async Task<(string message, bool isSuccess, LeaveType model)> GetLeaveTypeById(int id, CancellationToken cancellationToken)
        {
            return await _leaveTypeRepository.GetLeaveTypeById(id, cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> UpdateLeaveType(LeaveType model, CancellationToken cancellationToken)
        {
            return await _leaveTypeRepository.UpdateLeaveType(model, cancellationToken);
        }
    }
}

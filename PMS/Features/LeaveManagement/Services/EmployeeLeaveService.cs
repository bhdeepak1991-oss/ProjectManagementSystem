using PMS.Domains;
using PMS.Features.LeaveManagement.Repositories;
using PMS.Features.LeaveManagement.ViewModels;

namespace PMS.Features.LeaveManagement.Services
{
    public class EmployeeLeaveService : IEmployeeLeaveService
    {
        private readonly IEmployeeLeaveRepository _employeeLeaveRepository;
        public EmployeeLeaveService(IEmployeeLeaveRepository employeeLeaveRepository)
        {
            _employeeLeaveRepository = employeeLeaveRepository;
        }

        public async Task<(string message, bool isSuccess)> ApprovedReject(int empId, int managerId, bool isApproved, int requestId)
        {
            return await _employeeLeaveRepository.ApprovedReject(empId, managerId, isApproved, requestId);
        }

        public async Task<(string message, bool isSuccess)> CreateEmployeeLeave(EmployeeLeave model, CancellationToken cancellationToken)
        {
            return await _employeeLeaveRepository.CreateEmployeeLeave(model, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<EmployeeLeaveVm> models)> GetEmployeeLeaves(int empId, CancellationToken cancellationToken)
        {
            return await _employeeLeaveRepository.GetEmployeeLeaves(empId, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<EmployeeLeaveVm> model)> GetEmployeeRequest(int managerId)
        {
            return await _employeeLeaveRepository.GetEmployeeRequest(managerId);
        }

        public async  Task<(string message, bool isSuccess, LeaveCountVm model)> GetLeaveCountDetail(int leaveType, int empId)
        {
            return await _employeeLeaveRepository.GetLeaveCountDetail(leaveType, empId);
        }
    }
}

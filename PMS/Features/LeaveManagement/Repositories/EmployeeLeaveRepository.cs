using Microsoft.EntityFrameworkCore;
using PMS.Domains;
using PMS.Features.LeaveManagement.ViewModels;

namespace PMS.Features.LeaveManagement.Repositories
{
    public class EmployeeLeaveRepository : IEmployeeLeaveRepository
    {
        private readonly PmsDbContext _dbContext;

        public EmployeeLeaveRepository(PmsDbContext context)
        {
            _dbContext = context;
        }
        public async Task<(string message, bool isSuccess)> CreateEmployeeLeave(EmployeeLeave model, CancellationToken cancellationToken)
        {
            var response = await _dbContext.EmployeeLeaves.AddAsync(model, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Employee Leave created", true);

        }

        public async Task<(string message, bool isSuccess, IEnumerable<EmployeeLeaveVm> models)> GetEmployeeLeaves(int empId, CancellationToken cancellationToken)
        {
            var empLeaveModels = new List<EmployeeLeave>();

            var leaveTypeModels = await _dbContext.LeaveTypes.Where(x => x.IsActive == true && x.IsDeleted == false).ToListAsync();
            var empModels = await _dbContext.Employees.Where(x => x.IsDeleted == false).ToListAsync();

            if (empId == 0)
            {
                empLeaveModels = await _dbContext.EmployeeLeaves.Where(x => x.IsActive == true && x.IsDeleted == false).ToListAsync();
            }
            else
            {
                empLeaveModels = await _dbContext.EmployeeLeaves.Where(x => x.IsActive == true
                        && x.IsDeleted == false && x.EmployeeId == empId).ToListAsync();
            }

            var response = empLeaveModels.Select(x => new EmployeeLeaveVm
            {
                LeaveType = $"{leaveTypeModels.FirstOrDefault(z => z.Id == x.LeaveTypeId)?.LeaveTypeName ?? string.Empty} ({leaveTypeModels.FirstOrDefault(z => z.Id == x.LeaveTypeId)?.LeaveTypeCode ?? string.Empty})",
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                Reason = x.Reason,
                LeaveStatus = x.LeaveStatus,
                ApprovedRejectedBy = $"{empModels.FirstOrDefault(z => z.Id == x.ApprovedRejectBy)?.Name ?? string.Empty} ({empModels.FirstOrDefault(z => z.Id == x.ApprovedRejectBy)?.EmployeeCode ?? string.Empty})",
                ApprovedRejectDate = x.ApprovedRejectDate,
                ApprovedRejectReason = x.ApprovedRejectReason,
                EmployeeName = $"{empModels.FirstOrDefault(z => z.Id == x.EmployeeId)?.Name ?? string.Empty} ({empModels.FirstOrDefault(z => z.Id == x.EmployeeId)?.EmployeeCode ?? string.Empty})",
            }).ToList();

            return ("Employee Leave Featched Succcessfully", true, response);
        }

        public async Task<(string message, bool isSuccess, LeaveCountVm model)> GetLeaveCountDetail(int leaveType, int empId)
        {
            var responseModel = await _dbContext.LeaveTypes.FirstOrDefaultAsync(x => x.Id == leaveType);

            if (responseModel is not null)
            {
                var empLeaveModel = await _dbContext.EmployeeLeaves
                .Where(x => x.IsActive == true
                    && x.IsDeleted == false
                    && x.LeaveStatus == "Approved"
                    && x.FromDate >= responseModel.LeaveCalcullationStartDate
                    && x.ToDate <= responseModel.LeaveCalcullationEndDate)
                .ToListAsync();


                decimal leaveTaken = default;

                empLeaveModel.ForEach(data =>
                {
                    leaveTaken +=Convert.ToDecimal((Convert.ToDateTime(data.ToDate) - Convert.ToDateTime(data.FromDate)).TotalDays);
                });


                var start = Convert.ToDateTime(responseModel.LeaveCalcullationStartDate);
                var end = DateTime.Now;

                var totalMonths = ((end.Year - start.Year) * 12) + end.Month - start.Month;

                if (end.Day >= start.Day)
                {
                    totalMonths++;
                }

                var model = new LeaveCountVm()
                {
                    TotalLeaveCountTillDate = Convert.ToDecimal(totalMonths * responseModel.PermonthAllocatedLeave),
                    LeaveTaken = leaveTaken,
                    RemainingLeaveCout = Convert.ToDecimal(totalMonths * responseModel.PermonthAllocatedLeave) - leaveTaken,
                    PercentageOfLeaveTaken = (leaveTaken / Convert.ToDecimal(totalMonths * responseModel.PermonthAllocatedLeave)) * 100
                };

                return ("Leave Count Detail", true, model);
            }

            return ("Leave Count Detail", false, new());
        }
    }
}

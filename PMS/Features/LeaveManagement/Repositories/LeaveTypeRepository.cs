using Microsoft.EntityFrameworkCore;
using PMS.Domains;

namespace PMS.Features.LeaveManagement.Repositories
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly PmsDbContext _dbContext;
        public LeaveTypeRepository(PmsDbContext context)
        {
            _dbContext = context;
        }
        public async Task<(string message, bool isSuccess)> CreateLeaveType(LeaveType model, CancellationToken cancellationToken)
        {

            if (_dbContext.LeaveTypes.Any(x => x.LeaveTypeCode == model.LeaveTypeCode
                        && x.LeaveTypeName == model.LeaveTypeName && x.IsActive == true && x.IsDeleted == false))
            {
                return ("Leave Type already present", false);
            }

            var responseModel = await _dbContext.LeaveTypes.AddAsync(model);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Leave Type created successfully", true);
        }

        public async Task<(string message, bool isSuccess, LeaveType model)> DeleteLeaveType(int id, CancellationToken cancellationToken)
        {
            var updateModel = await _dbContext.LeaveTypes.FirstOrDefaultAsync(x => x.Id == id);

            if (updateModel is null)
            {
                return ("Leave Type not found", false, new());
            }

            updateModel.IsDeleted = true;
            updateModel.IsActive = false;

            _dbContext.Update(updateModel);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Leave Type Deleted Successfully", true, new());
        }

        public async Task<(string message, bool isSuccess, IEnumerable<LeaveType> models)> GetLeaveType(CancellationToken cancellationToken)
        {
            var response = await _dbContext.LeaveTypes.AsNoTracking().Where(x => x.IsActive == true && x.IsDeleted == false).ToListAsync();

            return ("Fetched successfully", true, response);
        }

        public async Task<(string message, bool isSuccess, LeaveType model)> GetLeaveTypeById(int id, CancellationToken cancellationToken)
        {
            var responseModel = await _dbContext.LeaveTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Id==id);

            return ("Leave Type fectched Successfully", true, responseModel ?? new());
        }

        public async Task<(string message, bool isSuccess)> UpdateLeaveType(LeaveType model, CancellationToken cancellationToken)
        {

            if (_dbContext.LeaveTypes.Any(x => x.LeaveTypeCode == model.LeaveTypeCode
                      && x.LeaveTypeName == model.LeaveTypeName && x.IsActive == true && x.IsDeleted == false && x.Id != model.Id))
            {
                return ("Leave Type already mapped, You can not update the another Leave Type", false);
            }

            var responseModel =  _dbContext.LeaveTypes.Update(model);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Leave Type updated successfully", true);
        }
    }
}

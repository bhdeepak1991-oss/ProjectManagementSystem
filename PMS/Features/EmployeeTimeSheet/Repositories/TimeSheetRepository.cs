using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PMS.Domains;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PMS.Features.EmployeeTimeSheet.Repositories
{
    public class TimeSheetRepository : ITimeSheetRepository
    {
        private readonly PmsDbContext _dbContext;

        public TimeSheetRepository(PmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<(string message, bool isSuccess)> ApproveDenyTimeSheet(int timeSheetId, bool isApproved, CancellationToken cancellationToken)
        {
            var dbModel = await _dbContext.EmployeeTimeSheets.FirstOrDefaultAsync(x => x.Id == timeSheetId);

            if (dbModel is null)
            {
                return ("Time Sheet for the given Id not found", false);
            }

            dbModel.TimeSheetStatus = isApproved ? "Approved" : "Rejected";
            dbModel.ApprovedBy = 1;
            dbModel.ApprovedDate = DateTime.Now;

            _dbContext.EmployeeTimeSheets.Update(dbModel);

            await _dbContext.SaveChangesAsync(cancellationToken);


            return ($"Time Sheet has been {dbModel.TimeSheetStatus}", true);
        }

        public async Task<(string message, bool isSuccess)> CreateTimeSheet(Domains.EmployeeTimeSheet model, CancellationToken cancellationToken)
        {
            model.TimeSheetStatus = "In Progress";
            model.CreatedBy = model.EmployeeId;

            var repsonseModel = await _dbContext.EmployeeTimeSheets.AddAsync(model, cancellationToken);

            var dayDateResponse = GetAllDatesWithDays(Convert.ToInt32(model.TimeSheetYear), Convert.ToInt32(model.TimeSheetMonth));

            await _dbContext.SaveChangesAsync(cancellationToken);

            var timeSheetDetailModels = new List<EmployeeTimeSheetTaskDetail>();

            dayDateResponse.ToList().ForEach(data =>
            {
                var dbModel = new EmployeeTimeSheetTaskDetail();

                dbModel.EmployeeTimeSheetTaskId = repsonseModel.Entity.Id;
                dbModel.CreatedDate = DateTime.Now;
                dbModel.CreatedBy = 1;
                dbModel.TimeSheetDate = data.Date;
                dbModel.DayName = data.DayName;
                dbModel.WorkingHour = (data.Date.DayOfWeek == DayOfWeek.Saturday || data.Date.DayOfWeek == DayOfWeek.Sunday) ? "0" : "9";
                dbModel.TaskDetail = string.Empty;

                timeSheetDetailModels.Add(dbModel);

            });

            await _dbContext.EmployeeTimeSheetTaskDetails.AddRangeAsync(timeSheetDetailModels);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Time Sheet Created Successfully", true);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<EmployeeTimeSheetTaskDetail>)> GetTimeSheetDetail(int timeSheetId, CancellationToken cancellationToken)
        {
            var responseModels = await _dbContext.EmployeeTimeSheetTaskDetails
                            .Where(x => x.EmployeeTimeSheetTaskId == timeSheetId && x.IsDeleted == false).ToListAsync();

            return ("Time Sheet Fetched Successfully", true, responseModels);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<Domains.EmployeeTimeSheet>)> GetTimeSheetList(int empId, CancellationToken cancellationToken)
        {
            var responseModels = await _dbContext.EmployeeTimeSheets.Where(x => x.IsDeleted == false && x.EmployeeId == empId).ToListAsync();

            return ("Employee Time Sheet Fetched Successfully", true, responseModels);
        }

        public async Task<(string message, bool isSuccess)> UploadTimeSheet(List<EmployeeTimeSheetTaskDetail> models, CancellationToken cancellationToken)
        {
            _dbContext.EmployeeTimeSheetTaskDetails.UpdateRange(models);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Time Sheet uploaded successfully", true);
        }
        public static List<(DateOnly Date, string DayName)> GetAllDatesWithDays(int year, int month)
        {
            var result = new List<(DateOnly, string)>();
            int daysInMonth = DateTime.DaysInMonth(year, month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateOnly(year, month, day);
                var dayName = date.DayOfWeek.ToString();

                result.Add((date, dayName));
            }

            return result;
        }

    }
}

using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
using PMS.Domains;

namespace PMS.Features.Notification.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly PmsDbContext _dbContext;

        public NotificationRepository(PmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(string message, bool isSuccess)> AddNotification(NotificationDetail model, CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.NotificationDetails.AddAsync(model);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return ("Notification addedd sucessfully", true);
            }
            catch (Exception ex)
            {
                return (ex.Message, false);
            }

        }

        public async Task<(string message, bool isSuccess)> ChangeStatus(int notificationId, string status, CancellationToken cancellationToken)
        {
            var dbModel = await _dbContext.NotificationDetails.FirstOrDefaultAsync(x => x.Id == notificationId, cancellationToken);

            if (dbModel is null)
            {
                return ("Record not found", false);

            }

            dbModel.NotificationStatus = status;

            dbModel.UpdatedDate = DateTime.Now;

            dbModel.UpdatedBy = 1;

            _dbContext.Update(dbModel);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Notification status changes successfully", true);


        }

        public async Task<(string message, bool isSuccess, IEnumerable<NotificationDetail> models)> GetNotificationList(int userId, CancellationToken cancellationToken)
        {
            var responseModels = await _dbContext.NotificationDetails.Where(x => x.IsActive == true && x.NotifiedUserId == userId
                    && x.IsDeleted == false && x.NotificationStatus != "Deleted").ToListAsync(cancellationToken);

            return ("Notification fetched successfully", true, responseModels);
        }
    }
}

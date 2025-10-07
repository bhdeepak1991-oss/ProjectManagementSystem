using Microsoft.EntityFrameworkCore;
using PMS.Domains;

namespace PMS.Features.Utilities.Repositories
{
    public class StatusHelperRepository : IStatusHelperRepository
    {
        private readonly PmsDbContext _dbContext;

        public StatusHelperRepository(PmsDbContext context)
        {
            _dbContext = context;
        }
        public async Task<(string message, bool isSuccess, IEnumerable<StatusHelper> models)> GetStatusHelperDetail(CancellationToken cancellationToken)
        {
            var responseModels = await _dbContext.StatusHelpers.AsNoTracking().Where(x => x.IsDeleted == false).ToListAsync();

            return ("Status helper fetched successfully", true, responseModels);
        }

        public async Task<(string message, bool isSuccess)> UploadStatusHelper(List<StatusHelper> models, CancellationToken cancellationToken)
        {
            models.ForEach(data =>
            {
                data.IsDeleted = false;
                data.IsActive = true;
                data.CreatedDate = DateTime.Now;
            });

            var dbModels = await _dbContext.StatusHelpers.AsNoTracking().Where(x => x.IsDeleted == false).ToListAsync();

            //delete previous records from the table
            dbModels.ForEach(data =>
            {
                data.IsDeleted = true;
                data.IsActive = false;
                data.UpdateDate = DateTime.Now;
            });

            _dbContext.StatusHelpers.UpdateRange(dbModels);

            // insert new records inside the table
            await _dbContext.StatusHelpers.AddRangeAsync(models);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("data uploaded inside the table", true);
        }
    }
}

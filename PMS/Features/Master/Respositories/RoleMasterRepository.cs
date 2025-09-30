using Microsoft.EntityFrameworkCore;
using PMS.Domains;

namespace PMS.Features.Master.Respositories
{
    public class RoleMasterRepository : IRoleMasterRepository
    {
        private readonly PmsDbContext _dbContext;

        public RoleMasterRepository(PmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateRole(RoleMaster model, CancellationToken cancellationToken)
        {
            await _dbContext.RoleMasters.AddAsync(model, cancellationToken);

            var result = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return result > 0;
        }

        public async Task<bool> DeleteRole(int roleId, CancellationToken cancellationToken)
        {
            var deleteModel = await _dbContext.RoleMasters.FindAsync(roleId);

            if (deleteModel == null)
            {
                return false;
            }
            deleteModel.IsDeleted = true;
            deleteModel.IsActive = false;
            deleteModel.UpdatedDate = DateTime.UtcNow;

            _dbContext.RoleMasters.Update(deleteModel);

            //Disable context switching for better performance
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return true;
        }

        public async Task<RoleMaster> GetRoleById(int roleId, CancellationToken cancellationToken)
        {
            var responseModel = await _dbContext.RoleMasters.AsNoTracking().FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken);

            return responseModel ?? new RoleMaster();
        }

        public async Task<IEnumerable<RoleMaster>> GetRoles(CancellationToken cancellationToken)
        {
            var responseModels = await _dbContext.RoleMasters.AsNoTracking()
                    .Where(x=>x.IsActive==true && x.IsDeleted==false).ToListAsync(cancellationToken);

            return responseModels;
        }

        public async Task<bool> UpdateRole(RoleMaster model, CancellationToken cancellationToken)
        {
            var dbModel = await _dbContext.RoleMasters.FindAsync(model.Id);

            if (dbModel is not null)
            {
                dbModel.Name = model.Name;
                dbModel.Description = model.Description;
                dbModel.UpdatedBy = 1;// for admin
                dbModel.UpdatedDate = DateTime.UtcNow;

                var result = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return result > 0;
            }
            return false;
        }
    }
}

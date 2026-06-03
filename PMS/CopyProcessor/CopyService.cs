using Microsoft.EntityFrameworkCore;
using PMS.Domains;
using PMS.Features.UserManagement.ViewModels;

namespace PMS.CopyProcessor
{
    public class CopyService : ICopyService
    {
        private readonly PmsDbContext _dbContext;

        public CopyService(PmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CopyProjectService(int projectId, string projectName)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            int newProjectId = default;

            try
            {
                var copyProjectModel = _dbContext.Projects.AsNoTracking().FirstOrDefault(p => p.Id == projectId);

                if (copyProjectModel is null)
                    return false;

                copyProjectModel.Id = 0;
                copyProjectModel.Name= projectName;
                copyProjectModel.ProjectStatus = "Copy In Progress";
                _dbContext.Projects.Add(copyProjectModel);
                await _dbContext.SaveChangesAsync();
                newProjectId = copyProjectModel.Id;

                var copyProjectDocuments = _dbContext.ProjectDocuments.AsNoTracking().Where(x => x.ProjectId == projectId).ToList();
                copyProjectDocuments.ForEach(x => { x.Id = 0; x.ProjectId = newProjectId; });
                _dbContext.ProjectDocuments.AddRange(copyProjectDocuments);

                var copyProjectEmployees = _dbContext.ProjectEmployees.AsNoTracking().Where(x => x.ProjectId == projectId).ToList();
                copyProjectEmployees.ForEach(x => { x.Id = 0; x.ProjectId = newProjectId; });
                _dbContext.ProjectEmployees.AddRange(copyProjectEmployees);

                var copyProjectTasks = _dbContext.ProjectTasks.AsNoTracking().Where(x => x.ProjectId == projectId).ToList();
                copyProjectTasks.ForEach(x => { x.Id = 0; x.ProjectId = newProjectId; });
                _dbContext.ProjectTasks.AddRange(copyProjectTasks);
                

                var copySprints = _dbContext.Sprints.AsNoTracking().Where(x => x.ProjectId == projectId).ToList();
                copySprints.ForEach(x => { x.Id = 0; x.ProjectId = newProjectId; });
                _dbContext.Sprints.AddRange(copySprints);

                var copyStatusUpdate = await _dbContext.Projects.FirstOrDefaultAsync(x => x.Id == newProjectId);
                if (copyStatusUpdate is not null)
                {
                    copyStatusUpdate.ProjectStatus = "Copy Completed";
                    _dbContext.Projects.Update(copyStatusUpdate);
                }

                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                var copyStatusUpdate = await _dbContext.Projects.FirstOrDefaultAsync(x => x.Id == newProjectId);
                if (copyStatusUpdate is not null)
                {
                    copyStatusUpdate.ProjectStatus = "Copy Failed";
                    _dbContext.Projects.Update(copyStatusUpdate);
                }

                await transaction.RollbackAsync();
                throw;
            }
            
        }
    }
}


using Microsoft.EntityFrameworkCore;
using PMS.Domains;

namespace PMS.Features.Project.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly PmsDbContext _dbContext;

        public ProjectRepository(PmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(string message, bool isSuccess)> CreateProject(Domains.Project model, CancellationToken cancellationToken)
        {
            var responseModel = await _dbContext.Projects.AddAsync(model, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Project created successfully", true);
        }

        public async Task<(string message, bool isSuccess)> DeleteProject(int projectId, CancellationToken cancellationToken)
        {
            var deleteModel = await _dbContext.Projects.FindAsync(projectId, cancellationToken);

            if (deleteModel is null)
            {
                return ("Project not found", false);
            }

            deleteModel.IsDeleted = true;
            deleteModel.UpdatedBy = 1; //TODO: Need to change
            deleteModel.UpdatedDate = DateTime.UtcNow;

            _dbContext.Projects.Update(deleteModel);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Project deleted successfully", true);
        }

        public async Task<(string message, bool isSuccess, Domains.Project? model)> GetProjectById(int projectId, CancellationToken cancellationToken)
        {
            var response = await _dbContext.Projects.FindAsync(projectId, cancellationToken);

            return ("Project fetched successfully !", true, response);
        }

        public async Task<(string message, bool isSuccess, string content)> GetProjectDetail(int projectId)
        {
            var responseModel = await _dbContext.Projects.FirstOrDefaultAsync(x => x.Id == projectId);

            return ("Fetched Successfully", true, responseModel?.Description ?? string.Empty);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<Domains.Project> models)> GetProjectList(CancellationToken cancellationToken)
        {
            var responseModels = await _dbContext.Projects.AsNoTracking().Where(x => x.IsDeleted == false).ToListAsync(cancellationToken);

            return ("Project fetched successfully", true, responseModels);
        }

        public async Task<(string message, bool isSuccess)> UpdateProject(Domains.Project model, CancellationToken cancellationToken)
        {
            var updateModel = await _dbContext.Projects.FindAsync(model.Id, cancellationToken);

            if (updateModel is null)
            {
                return ("Project not found", false);
            }

            updateModel.ProjectManager = model.ProjectManager;
            updateModel.ProjectHead = model.ProjectHead;
            updateModel.DeliveryHead = model.DeliveryHead;
            updateModel.Name = model.Name;
            updateModel.Description = model.Description;
            updateModel.ProjectStartDate = model.ProjectStartDate;
            updateModel.ProjectEndDate = model.ProjectEndDate;
            updateModel.UpdatedBy = 1; //TODO: Need to change
            updateModel.UpdatedDate = DateTime.UtcNow;

            _dbContext.Projects.Update(updateModel);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Project updated successfully", true);
        }

        public async Task<(string message, bool isSuccess)> UpdateProjectStatus(int id, string reason, string status)
        {
            var dbModel = await _dbContext.Projects.FindAsync(id);

            dbModel.ProjectStatus = status;
            dbModel.Reason = reason;

            _dbContext.Projects.Update(dbModel);

            await _dbContext.SaveChangesAsync();

            return ("Update project status", true);

        }
    }
}

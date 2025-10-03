using Microsoft.EntityFrameworkCore;
using PMS.Domains;

namespace PMS.Features.Master.Respositories
{
    public class TaskStatusRepository : ITaskStatusRepository
    {
        private readonly PmsDbContext _dbContext;

        public TaskStatusRepository(PmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<(string message, bool isSuccess)> CreateTaskStatus(TaskStatusMaster model, CancellationToken cancellationToken)
        {
            var existingStatus = await _dbContext.TaskStatusMasters
                .FirstOrDefaultAsync(ts => ts.Name == model.Name && ts.IsDeleted == false, cancellationToken);

            if (existingStatus is null)
            {
                model.IsActive = true;

                model.IsDeleted = false;

                var createModel= await _dbContext.TaskStatusMasters.AddAsync(model, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return ("Task status created successfully.", true);
            }
            else
            {
                return ("Task status already exists.", false);
            }
        }

        public async Task<(string message, bool isSuccess)> DeleteTaskStatus(int id, CancellationToken cancellationToken)
        {
            var deleteModel= await _dbContext.TaskStatusMasters
                .FirstOrDefaultAsync(ts => ts.Id == id && ts.IsDeleted == false, cancellationToken);

            if(deleteModel is null)
                return ("Task status not found.", false);

            deleteModel.IsDeleted = true;

            deleteModel.UpdatedDate = DateTime.UtcNow;

            deleteModel.UpdatedBy = 1;

            _dbContext.TaskStatusMasters.Update(deleteModel);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Task status deleted successfully.", true);
        }

        public async Task<(string message, bool isSuccess, TaskStatusMaster? model)> GetTaskStatusById(int id, CancellationToken cancellationToken)
        {
            var response= await _dbContext.TaskStatusMasters
                .FirstOrDefaultAsync(ts => ts.Id == id && ts.IsDeleted == false, cancellationToken);

            return  ("Task status retrieved successfully.", true, response);
        }

        public async  Task<(string message, bool isSuccess, IEnumerable<TaskStatusMaster> model)> GetTaskStatusDetail(CancellationToken cancellationToken)
        {
            var response= await _dbContext.TaskStatusMasters.AsNoTracking()
                .Where(ts => ts.IsDeleted == false)
                .ToListAsync(cancellationToken);

            return ("Task status fetched successfully", true, response);
        }

        public async  Task<(string message, bool isSuccess)> UpdateTaskStatus(TaskStatusMaster model, CancellationToken cancellationToken)
        {
            var responseModel = await _dbContext.TaskStatusMasters
                        .Where(x => x.Id != model.Id && x.IsDeleted == false)
                            .ToListAsync(cancellationToken);

            var isExists = responseModel.Any(x => x.Name == model.Name && x.IsActive == true);

            if (isExists)
            {
                return ($"Task status with name {model.Name} already present mapped with another Id", false);
            }

            var updateModel = await _dbContext.TaskStatusMasters.FindAsync(model.Id, cancellationToken);

            if (updateModel is null)
            {
                return ($"Record Not found for the Id {model.Id}", false);
            }

            updateModel.Name = model.Name;

            updateModel.Description = model.Description;

            updateModel.IsActive = true;

            updateModel.UpdatedDate = DateTime.Now;

            _dbContext.TaskStatusMasters.Update(updateModel);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Department updated successfully", true);
        }
    }
}

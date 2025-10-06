using Microsoft.EntityFrameworkCore;
using PMS.Domains;
using PMS.Features.ProjectTask.ViewModels;
using System.Threading.Tasks;

namespace PMS.Features.ProjectTask.Repositories
{
    public class ProjectTaskRepository : IProjectTaskRepository
    {
        private readonly PmsDbContext _dbContext;

        public ProjectTaskRepository(PmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(string message, bool isSuccess)> CreateProjectTask(Domains.ProjectTask models, CancellationToken cancellationToken)
        {
            var responseModel = await _dbContext.ProjectTasks.AddAsync(models);

            var taskId = await _dbContext.ProjectTasks.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefaultAsync();

            int nextId = taskId + 1;

            models.TaskCode = $"Task_{nextId.ToString("D3")}";

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Project Task created successfully", true);
        }

        public async Task<(string message, bool isSuccess)> DeleteProjectTask(int taskId, CancellationToken cancellationToken)
        {
            var deleteModel = await _dbContext.ProjectTasks.FindAsync(taskId);

            if (deleteModel is null)
            {
                return ("Project Task not found", false);
            }

            deleteModel.IsDeleted = true;
            deleteModel.UpdatedDate = DateTime.Now;
            deleteModel.UpdatedBy = 1;

            _dbContext.ProjectTasks.Update(deleteModel);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Project deleted successfully", true);

        }

        public async Task<(string message, bool isSuccess, IEnumerable<ProjectEmployeeVm> models)> GetProjectEmployee(int projectId, CancellationToken cancellationToken)
        {
            var result = await (from pe in _dbContext.ProjectEmployees
                          join emp in _dbContext.Employees on pe.EmployeeId equals emp.Id
                          where emp.IsDeleted == false && pe.ProjectId == projectId
                          select new ProjectEmployeeVm
                          {
                             Id= emp.Id,
                             Name=$"{emp.Name} ({emp.EmployeeCode})",
                          }).ToListAsync();

            return ("Fetched successfully", true, result);
        }

        public async Task<(string message, bool isSuccess, Domains.ProjectTask models)> GetProjectTaskById(int taskId, CancellationToken cancellationToken)
        {
            var responseModel = await _dbContext.ProjectTasks.FindAsync(taskId, cancellationToken);

            return ("Project Task Fetched successfully", true, responseModel ?? new());
        }

        public async Task<(string message, bool isSuccess, IEnumerable<ProjectTaskViewModel> models)> GetProjectTaskList(int projectId, CancellationToken cancellationToken)
        {
            var projectTaskModels = await _dbContext.ProjectTasks.AsNoTracking().Where(x => x.IsDeleted == false && x.ProjectId== projectId).ToListAsync();

            var employeeModels = await _dbContext.Employees.AsNoTracking().Where(x => x.IsDeleted == false ).ToListAsync();

            var sprintModels = await _dbContext.Sprints.AsNoTracking().Where(x => x.IsDeleted == false && x.ProjectId==projectId).ToListAsync();

            var responseModels = projectTaskModels.ToList().Select(x => new ProjectTaskViewModel()
            {
                Id=x.Id,
                TaskName= x.TaskName,
                TaskCode= x.TaskCode,
                TaskDetail= x.TaskDetail,
                TaskPriority= x.TaskPriority,
                TaskType= x.TaskType,
                DueDate= x.DueDate,
                ModuleName=x.ModuleName,
                TaskStatus= x.TaskStatus,
                SprintName= sprintModels.FirstOrDefault(z=>z.Id== x.SprintId)?.SprintName ?? string.Empty,
                EmployeeName=$"{employeeModels.FirstOrDefault(z=>z.Id== x.EmployeeId)?.Name ?? string.Empty} ({employeeModels.FirstOrDefault(z => z.Id == x.EmployeeId)?.EmployeeCode ?? string.Empty})",
                StartDate=x.StartDate,
                CompletedDate=x.CompletedDate,
                EstimatedHour=x.EstimatedHour ?? 0,
                LoggedHour= x.LoggedHour ?? 0
            }).ToList();

            return ("Project Task Fetched successfully", true, responseModels);
        }

        public async  Task<(string message, bool isSuccess)> UpdateProjectTask(Domains.ProjectTask models, CancellationToken cancellationToken)
        {
            _dbContext.Update(models);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Task updated successfully !", true);

        }
    }
}

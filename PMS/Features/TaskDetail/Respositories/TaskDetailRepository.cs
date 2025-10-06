using Microsoft.EntityFrameworkCore;
using PMS.Domains;
using PMS.Features.ProjectTask.ViewModels;
using PMS.Features.TaskDetail.ViewModels;

namespace PMS.Features.TaskDetail.Respositories
{
    public class TaskDetailRepository : ITaskDetailRepository
    {
        private readonly PmsDbContext _dbContext;

        public TaskDetailRepository(PmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(string message, bool isSuccess, TaskDetailViewModel model)> GetTaskDetail(int taskId)
        {
            var responseModels = await (from pt in _dbContext.ProjectTasks
                                        join sp in _dbContext.Sprints on pt.SprintId equals sp.Id
                                        join em in _dbContext.Employees on pt.EmployeeId equals em.Id
                                        join dm in _dbContext.DepartmentMasters on em.DepartmentId equals dm.Id
                                        join ds in _dbContext.DesignationMasters on em.DesignationId equals ds.Id
                                        join ems in _dbContext.Employees on pt.CreatedBy equals ems.Id
                                        where pt.Id == taskId
                                        select new TaskDetailViewModel
                                        {
                                            Id = pt.Id,
                                            TaskName = pt.TaskName,
                                            TaskCode = pt.TaskCode,
                                            TaskDetail = pt.TaskDetail,
                                            TaskPriority = pt.TaskPriority,
                                            TaskType = pt.TaskType,
                                            TaskStatus = pt.TaskStatus,
                                            ModuleName = pt.ModuleName,
                                            EmployeeName = $"{em.Name} ({em.EmployeeCode})",
                                            DueDate = pt.DueDate,
                                            LoggedHour = pt.LoggedHour ?? 0,
                                            EstimatedHour = pt.EstimatedHour ?? 0,
                                            CompletedDate = pt.CompletedDate,
                                            SprintName = sp.SprintName,
                                            ReportedBy= $"{ems.Name} ({ems.EmployeeCode})",
                                        }).FirstOrDefaultAsync();



            return ("Project Task Fetched successfully", true, responseModels ?? new());
        }

        public async Task<(string message, bool isSuccess, IEnumerable<TaskDetailViewModel> models)> GetTaskDetails(int projectId)
        {
            var projectTaskModels = await _dbContext.ProjectTasks.AsNoTracking().Where(x => x.IsDeleted == false && x.ProjectId == projectId).ToListAsync();

            var employeeModels = await _dbContext.Employees.AsNoTracking().Where(x => x.IsDeleted == false).ToListAsync();

            var sprintModels = await _dbContext.Sprints.AsNoTracking().Where(x => x.IsDeleted == false && x.ProjectId == projectId).ToListAsync();

            var responseModels = projectTaskModels.ToList().Select(x => new TaskDetailViewModel()
            {
                Id = x.Id,
                TaskName = x.TaskName,
                TaskCode = x.TaskCode,
                TaskDetail = x.TaskDetail,
                TaskPriority = x.TaskPriority,
                TaskType = x.TaskType,
                DueDate = x.DueDate,
                ModuleName = x.ModuleName,
                TaskStatus = x.TaskStatus,
                SprintName = sprintModels.FirstOrDefault(z => z.Id == x.SprintId)?.SprintName ?? string.Empty,
                EmployeeName = $"{employeeModels.FirstOrDefault(z => z.Id == x.EmployeeId)?.Name ?? string.Empty} ({employeeModels.FirstOrDefault(z => z.Id == x.EmployeeId)?.EmployeeCode ?? string.Empty})",
                StartDate = x.StartDate,
                CompletedDate = x.CompletedDate,
                EstimatedHour = x.EstimatedHour ?? 0,
                LoggedHour = x.LoggedHour ?? 0
            }).ToList();

            return ("Project Task Fetched successfully", true, responseModels);
        }
    }
}

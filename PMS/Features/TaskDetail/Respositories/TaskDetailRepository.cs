using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using PMS.Domains;
using PMS.Features.Dashboard.ViewModels;
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

        public async Task<(string message, bool isSuccess)> AssignToEmployee(int taskId, int userId, int empId)
        {
            var dbModel = new ProjectTaskEmployeeHistory()
            {
                ProjectTaskId= taskId,
                EmployeeId= empId,
                UpdatedBy= userId,
                UpdatedDate= DateTime.Now
            };

            await _dbContext.ProjectTaskEmployeeHistories.AddAsync(dbModel);

            var updateModel = await _dbContext.ProjectTasks.FindAsync(taskId);

            updateModel.EmployeeId = empId;
            updateModel.UpdatedBy = userId;
            updateModel.UpdatedDate = DateTime.Now;

            _dbContext.ProjectTasks.Update(updateModel);

            await _dbContext.SaveChangesAsync();

            return ("Task assign to employee", true);



        }

        public async Task<(string message, bool isSuccess)> ChangeTaskCompletedDate(int taskId, int userId, DateTime completedDate)
        {
            var model = await _dbContext.ProjectTasks.FindAsync(taskId);

            model.CompletedDate = completedDate;
            model.UpdatedBy = userId;
            model.UpdatedDate = DateTime.Now;

            _dbContext.ProjectTasks.Update(model);

            await _dbContext.SaveChangesAsync();

            return ("Task completed date updated !", true);

        }

        public async Task<(string message, bool isSuccess)> ChangeTaskPriority(int taskId, int userId, string priority)
        {
            var dbModel = new ProjectTaskPriorityHistory()
            {
                ProjectTaskId = taskId,
                TaskPriority = priority,
                UpdatedDate = DateTime.Now,
                UpdatedBy = userId,
            };

            await _dbContext.ProjectTaskPriorityHistories.AddAsync(dbModel);

            var updateModel = await _dbContext.ProjectTasks.FindAsync(taskId);

            updateModel.TaskPriority = priority;

            updateModel.UpdatedBy = userId;

            updateModel.UpdatedDate = DateTime.Now;

            _dbContext.ProjectTasks.Update(updateModel);

            await _dbContext.SaveChangesAsync();

            return ("Task Pririty has been updated !", true);
        }

        public async Task<(string message, bool isSuccess)> ChangeTaskStartDate(int taskId, int userId, DateTime startDate)
        {
            var dbModel = await _dbContext.ProjectTasks.FindAsync(taskId);

            dbModel.StartDate = startDate;
            dbModel.UpdatedDate = startDate;
            dbModel.UpdatedBy = userId;

            _dbContext.ProjectTasks.Update(dbModel);

            await _dbContext.SaveChangesAsync();

            return ("Project start date has been modified", true);
        }

        public async Task<(string message, bool isSuccess)> ChangeTaskStatus(int taskId, int userId, string status)
        {
            var dbModel = new ProjectTaskStatusHistory()
            {
                ProjectTaskId = taskId,
                TaskStatus = status,
                UpdatedDate = DateTime.Now,
                UpdatedBy = userId,
            };

            await _dbContext.ProjectTaskStatusHistories.AddAsync(dbModel);

            var updateModel = await _dbContext.ProjectTasks.FindAsync(taskId);

            updateModel.TaskStatus = status;

            updateModel.UpdatedBy = userId;

            updateModel.UpdatedDate = DateTime.Now;

            _dbContext.ProjectTasks.Update(updateModel);

            await _dbContext.SaveChangesAsync();

            return ("Task status has been updated !", true);
        }

        public async Task<(string message, bool isSuccess)> CreateDiscussion(TaskDetailViewModel model)
        {
            try
            {
                var discussionModel = new TaskDiscussionBoard()
                {
                    ProjectTaskId = model.TaskId,
                    Discusion = model.Discussion,
                    DocumentPath = string.Empty,
                    EmployeeId = model.EmployeeId,
                    CreatedBy = model.EmployeeId,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    UpdatedBy = model.EmployeeId,
                    UpdatedDate = DateTime.Now
                };

                await _dbContext.TaskDiscussionBoards.AddAsync(discussionModel);

                await _dbContext.SaveChangesAsync();

                return new("Task Discussion updated successfully", true);
            }
            catch (Exception ex)
            {
                return new(ex.Message, true);
            }

        }

        public async Task<(string message, bool isSuccess, IEnumerable<ProjectDiscussBoard> models)> GetDiscussionBoardList(int taskId)
        {
            var result = await (from td in _dbContext.TaskDiscussionBoards
                                join emp in _dbContext.Employees
                                    on td.EmployeeId equals emp.Id
                                where td.ProjectTaskId == taskId
                                select new ProjectDiscussBoard
                                {
                                    Id = td.Id,
                                    Discussion = td.Discusion ?? string.Empty,
                                    DocumentPath = td.DocumentPath ?? string.Empty,
                                    CreatedDate = td.CreatedDate,
                                    EmployeeName = emp.Name ?? string.Empty,
                                    EmployeeCode = emp.EmployeeCode ?? string.Empty
                                }).OrderByDescending(x => x.CreatedDate).ToListAsync();

            return ("Task discussion board detail fetched", true, result);

        }

        public async Task<(string message, bool isSuccess, IEnumerable<AssignHistoryVm> models)> GetTaskAssignHistory(int taskId)
        {
            var responseModels = _dbContext.ProjectTaskEmployeeHistories.Where(x=>x.ProjectTaskId== taskId)
                        .Join(_dbContext.Employees,
                              pteh => pteh.EmployeeId,
                              emp => emp.Id,
                              (pteh, emp) => new { pteh, AssignTo = emp })
                        .Join(_dbContext.Employees,
                              temp => temp.pteh.UpdatedBy,
                              emps => emps.Id,
                              (temp, emps) => new AssignHistoryVm
                              {
                                  AssignTo = $"{temp.AssignTo.Name} ({temp.AssignTo.EmployeeCode})",
                                  AssignBy = $"{emps.Name} ({emps.EmployeeCode})",
                                  AssignDate =Convert.ToDateTime(temp.pteh.UpdatedDate)
                              })
                        .OrderByDescending(x => x.AssignDate)
                        .ToList();

            return ("Employee Task Assign History", true, responseModels);

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
                                            ReportedBy = $"{ems.Name} ({ems.EmployeeCode})",
                                            TaskId = taskId,
                                            EmployeeId = Convert.ToInt32(pt.EmployeeId),
                                            StartDate = pt.StartDate,
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

        public async Task<(string message, bool isSuccess, IEnumerable<TaskStatusHistoryVm> models)> GetTaskStatusHistory(int taskId)
        {
            var responseModels =await( _dbContext.ProjectTaskStatusHistories
                                    .Where(ptsh => ptsh.ProjectTaskId == taskId)
                                        .Join(_dbContext.Employees,
                                              ptsh => ptsh.UpdatedBy,
                                              emp => emp.Id,
                                              (ptsh, emp) => new TaskStatusHistoryVm
                                              {
                                                  TaskStatus = ptsh.TaskStatus ?? "N/A",
                                                  EmpName= $"{emp.Name} ({emp.EmployeeCode})",
                                                  UpdatedDate = ptsh.UpdatedDate
                                              })
                                        .OrderBy(x => x.UpdatedDate))
                                        .ToListAsync();

            return ("Status History detail", true, responseModels);

        }
    }
}

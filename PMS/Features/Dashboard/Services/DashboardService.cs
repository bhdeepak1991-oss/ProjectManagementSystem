using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PMS.Domains;
using PMS.Features.Dashboard.ViewModels;
using PMS.Features.ProjectTask.ViewModels;
using PMS.Helpers;
using System.Data;
using System.Threading.Tasks;

namespace PMS.Features.Dashboard.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly PmsDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;


        public DashboardService(PmsDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public async Task<DashboardVm> GetDashboradDetails(int projectId)
        {
            var result = _dbContext.ProjectTasks
                        .Where(t => t.IsDeleted == false)
                        .GroupBy(t => t.TaskStatus)
                        .Select(g => new TaskStatusVm
                        {
                            TaskStatus = g.Key,
                            RecordCount = g.Count()
                        })
                        .ToList();

            var model = new DashboardVm()
            {
                TaskStatusModels = result
            };

            var projectTaskModels = await _dbContext.ProjectTasks
                        .Where(x => x.IsDeleted == false && x.ProjectId == projectId)
                            .ToListAsync();

            var empModels = await _dbContext.Employees.Where(x => x.IsDeleted == false).ToListAsync();

            var empTaskResponse = projectTaskModels
                 .GroupBy(x => x.EmployeeId)
                 .Select(group =>
                 {
                     var emp = empModels.FirstOrDefault(e => e.Id == group.Key);

                     return new EmployeeTaskVm
                     {
                         EmployeeCode = emp?.EmployeeCode ?? string.Empty,
                         EmployeeName = emp?.Name ?? string.Empty,
                         Email = emp?.EmailId ?? string.Empty,
                         TaskPriority = group.FirstOrDefault()?.TaskPriority ?? string.Empty,

                         TaskTypeCounts = group
                             .GroupBy(task => task.TaskStatus)
                             .ToDictionary(
                                 g => g.Key ?? "N/A",
                                 g => g.Count()
                             )
                     };
                 })
                 .ToList();


            var employeeModels = await _dbContext.Employees.AsNoTracking().Where(x => x.IsDeleted == false).ToListAsync();

            var sprintModels = await _dbContext.Sprints.AsNoTracking().Where(x => x.IsDeleted == false && x.ProjectId == projectId).ToListAsync();

            var responseModels = projectTaskModels.ToList().Select(x => new TaskModel()
            {
                TaskType = x.TaskType ?? string.Empty,
                TaskStatus = x.TaskStatus ?? string.Empty,
                TaskPriority = x.TaskPriority ?? string.Empty,
                EmployeeCode = employeeModels.FirstOrDefault(emp => emp.Id == x.EmployeeId)?.EmployeeCode ?? string.Empty,
                EmployeeName = employeeModels.FirstOrDefault(emp => emp.Id == x.EmployeeId)?.Name ?? string.Empty
            }).ToList();

            model.TaskStatusModels = result;
            model.EmployeeTasks = empTaskResponse.ToList();
            model.TaskModelsDrillDown = responseModels;

            return model;

        }

        public async Task<IEnumerable<ProjectTaskViewModel>> GetProjectTaskList(int projectId, string taskStatus, CancellationToken cancellationToken)
        {
            var projectTaskModels = await _dbContext.ProjectTasks.AsNoTracking()
                .Where(x => x.IsDeleted == false && x.ProjectId == projectId && x.TaskStatus.Trim() == taskStatus.Trim()).ToListAsync();

            var employeeModels = await _dbContext.Employees.AsNoTracking().Where(x => x.IsDeleted == false).ToListAsync();

            var sprintModels = await _dbContext.Sprints.AsNoTracking().Where(x => x.IsDeleted == false && x.ProjectId == projectId).ToListAsync();

            var responseModels = projectTaskModels.ToList().Select(x => new ProjectTaskViewModel()
            {
                Id = x.Id,
                TaskName = x.TaskName.TruncateWithEllipsis(),
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

            return responseModels;
        }

        public async Task<IEnumerable<ProjectTaskViewModel>> GetEmpProjectTaskList(int projectId, string taskStatus, string empCode, CancellationToken cancellationToken)
        {
            var employeeModels = await _dbContext.Employees.AsNoTracking().Where(x => x.IsDeleted == false && x.EmployeeCode == empCode).ToListAsync();

            var projectTaskModels = await _dbContext.ProjectTasks.AsNoTracking()
               .Where(x => x.IsDeleted == false && x.ProjectId == projectId
                        && x.TaskStatus.Trim() == taskStatus.Trim() && x.EmployeeId == employeeModels.FirstOrDefault().Id).ToListAsync();



            var sprintModels = await _dbContext.Sprints.AsNoTracking().Where(x => x.IsDeleted == false && x.ProjectId == projectId).ToListAsync();

            var responseModels = projectTaskModels.ToList().Select(x => new ProjectTaskViewModel()
            {
                Id = x.Id,
                TaskName = x.TaskName.TruncateWithEllipsis(),
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

            return responseModels;
        }

        public async Task<IEnumerable<ProjectTaskViewModel>> GetProjectTaskTypeList(int projectId, string taskStatus, string typeOfTask, CancellationToken cancellationToken)
        {
            var projectTaskModelsQuery = _dbContext.ProjectTasks.AsNoTracking()
               .Where(x => x.IsDeleted == false && x.ProjectId == projectId);

            if (typeOfTask == "TP")
            {
                projectTaskModelsQuery = projectTaskModelsQuery.Where(x => x.TaskPriority.Trim() == taskStatus.Trim());
            }
            if (typeOfTask == "TY")
            {
                projectTaskModelsQuery = projectTaskModelsQuery.Where(x => x.TaskType.Trim() == taskStatus.Trim());
            }
            if (typeOfTask == "MC")
            {
                projectTaskModelsQuery = projectTaskModelsQuery.Where(x => x.ModuleName.Trim() == taskStatus.Trim());
            }
            if (typeOfTask == "TS")
            {
                projectTaskModelsQuery = projectTaskModelsQuery.Where(x => x.TaskStatus.Trim() == taskStatus.Trim());
            }

            var projectTaskModels = await projectTaskModelsQuery.ToListAsync();


            var employeeModels = await _dbContext.Employees.AsNoTracking().Where(x => x.IsDeleted == false).ToListAsync();

            var sprintModels = await _dbContext.Sprints.AsNoTracking().Where(x => x.IsDeleted == false && x.ProjectId == projectId).ToListAsync();

            var responseModels = projectTaskModels.ToList().Select(x => new ProjectTaskViewModel()
            {
                Id = x.Id,
                TaskName = x.TaskName.TruncateWithEllipsis(),
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

            return responseModels;
        }

        public async Task<IEnumerable<MasterModelVm>> GetProjectEmployee(int projectId)
        {
            var result = await (from pe in _dbContext.ProjectEmployees
                                join emp in _dbContext.Employees on pe.EmployeeId equals emp.Id
                                where emp.IsDeleted == false && pe.ProjectId == projectId
                                select new MasterModelVm
                                {
                                    Id = emp.Id,
                                    Name = emp.Name + "(" + emp.EmployeeCode + ")"
                                }).ToListAsync();

            return result;

        }

        public async Task<IEnumerable<MasterModelVm>> GetTaskStatus(int projectId)
        {
            var responseModels = await _dbContext.ProjectTasks
                     .Where(t => t.IsDeleted == false)
                     .Select(t => new MasterModelVm { Name = t.TaskStatus ?? string.Empty })
                     .Distinct()
                     .ToListAsync();

            return responseModels;

        }

        public async Task<IEnumerable<MasterModelVm>> GetTaskType(int projectId)
        {
            var responseModels = await _dbContext.ProjectTasks
                   .Where(t => t.IsDeleted == false)
                   .Select(t => new MasterModelVm { Name = t.TaskType ?? string.Empty })
                   .Distinct()
                   .ToListAsync();

            return responseModels;
        }

        public async Task<IEnumerable<MasterModelVm>> GetTaskPriority(int projectId)
        {
            var responseModels = await _dbContext.ProjectTasks
                .Where(t => t.IsDeleted == false)
                .Select(t => new MasterModelVm { Name = t.TaskPriority ?? string.Empty })
                .Distinct()
                .ToListAsync();

            return responseModels;
        }

        public async Task<IEnumerable<ProjectTaskViewModel>> GetProjectTaskFilter(int projectId, string taskStatus,
            string priority, string taskType, int empId)
        {
            var projectTaskModelsQuery = _dbContext.ProjectTasks.AsNoTracking()
               .Where(x => x.IsDeleted == false && x.ProjectId == projectId);

            if (!string.IsNullOrEmpty(taskStatus))
            {
                projectTaskModelsQuery = projectTaskModelsQuery.Where(x => x.TaskStatus.Trim() == taskStatus.Trim());
            }
            if (!string.IsNullOrEmpty(priority))
            {
                projectTaskModelsQuery = projectTaskModelsQuery.Where(x => x.TaskPriority.Trim() == priority.Trim());
            }
            if (!string.IsNullOrEmpty(taskStatus))
            {
                projectTaskModelsQuery = projectTaskModelsQuery.Where(x => x.TaskType.Trim() == taskType.Trim());
            }
            if (empId>0)
            {
                projectTaskModelsQuery = projectTaskModelsQuery.Where(x => x.EmployeeId == empId);
            }

            var projectTaskModels = await projectTaskModelsQuery.ToListAsync();


            var employeeModels = await _dbContext.Employees.AsNoTracking().Where(x => x.IsDeleted == false).ToListAsync();

            var sprintModels = await _dbContext.Sprints.AsNoTracking().Where(x => x.IsDeleted == false && x.ProjectId == projectId).ToListAsync();

            var responseModels = projectTaskModels.ToList().Select(x => new ProjectTaskViewModel()
            {
                Id = x.Id,
                TaskName = x.TaskName.TruncateWithEllipsis(),
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

            return responseModels;
        }

        public async Task<IEnumerable<NotificationDetail>> GetNotificationDetail(int userId)
        {
            var responseModels = await _dbContext.NotificationDetails.Where(x => x.IsDeleted == false
                            && x.NotificationStatus == "New Notification" && x.NotifiedUserId == userId).OrderByDescending(x=>x.CreatedDate).Take(5).ToListAsync();

            return responseModels;
        }
    }
}

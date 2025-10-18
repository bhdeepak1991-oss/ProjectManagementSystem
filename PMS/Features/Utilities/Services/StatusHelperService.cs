using ClosedXML.Excel;
using PMS.Domains;
using PMS.Features.ProjectEmployee.Services;
using PMS.Features.ProjectTask.Services;
using PMS.Features.Sprint.Services;
using PMS.Features.Utilities.Repositories;
using PMS.Features.Utilities.ViewModels;

namespace PMS.Features.Utilities.Services
{
    public class StatusHelperService : IStatusHelperService
    {
        private readonly IStatusHelperRepository _statusHelperRepository;
        private readonly IProjectEmployeeServices _projectEmployeeService;
        private readonly ISprintService _sprintService;
        private readonly IProjectTaskService _projectTaskService;

        public StatusHelperService(IStatusHelperRepository statusHelperRepository, ISprintService sprintService,
                IProjectEmployeeServices projectEmpService, IProjectTaskService projectTaskService)
        {
            _statusHelperRepository = statusHelperRepository;
            _sprintService = sprintService;
            _projectEmployeeService = projectEmpService;
            _projectTaskService = projectTaskService;
        }

        public async Task<(string message, bool isSuccess, IEnumerable<StatusHelper> models)> GetStatusHelperDetail(CancellationToken cancellationToken)
        {
            return await _statusHelperRepository.GetStatusHelperDetail(default);
        }

        public async Task<(string message, bool isSuccess)> UploadStatusHelper(IFormFile uploadFile, CancellationToken cancellationToken)
        {
            if (uploadFile == null || uploadFile.Length == 0)
            {
                return ("Please upload valid file", false);
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, uploadFile.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadFile.CopyToAsync(stream);
            }

            using var workbook = new XLWorkbook(filePath);

            var worksheet = workbook.Worksheet(1);

            var rows = worksheet.RangeUsed().RowsUsed();

            var models = new List<StatusHelper>();

            foreach (var row in rows.Skip(1))
            {
                var model = new StatusHelper();
                model.Name = row.Cell(1).GetValue<string>();
                model.Code = row.Cell(2).GetValue<string>();
                model.StatusType = row.Cell(3).GetValue<string>();
                model.IsActive = true;
                model.IsDeleted = false;
                model.CreatedBy = 1;
                model.CreatedDate = DateTime.Now;

                models.Add(model);
            }

            await _statusHelperRepository.UploadStatusHelper(models, cancellationToken);

            return ("File upload successfully", true);
        }

        public async Task<(string message, bool isSuccess, IList<ErrorListVm> errorResponse)> UploadTaskHelper(IFormFile uploadFile, int projectId, CancellationToken cancellationToken)
        {
            if (uploadFile == null || uploadFile.Length == 0)
            {
                return ("Please upload valid file", false, new List<ErrorListVm>());
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, uploadFile.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadFile.CopyToAsync(stream);
            }

            using var workbook = new XLWorkbook(filePath);

            var worksheet = workbook.Worksheet(1);

            var rows = worksheet.RangeUsed().RowsUsed();

            var models = new List<Domains.ProjectTask>();

            var projectEmpModels = await _projectEmployeeService.GetMappedProjectEmployee(projectId);

            var sprintModels = await _sprintService.GetSprintList(default);

            var errorList = new List<ErrorListVm>();

            bool hasErrors = false;
            int count = 1;

            foreach (var row in rows.Skip(1)) // Skipping header
            {

                count++;
                // Validate Task Name (Cell 1)
                var taskName = row.Cell(1).GetValue<string>();
                if (string.IsNullOrWhiteSpace(taskName))
                {
                    errorList.Add(new ErrorListVm
                    {
                        CellNumber = count,
                        ColumnName = "Task Name",
                        ErrorMessage = "Task Name is required."
                    });
                    hasErrors = true;
                }

                // Validate Task Code (Cell 2)
                var taskCode = row.Cell(2).GetValue<string>();
                if (string.IsNullOrWhiteSpace(taskCode))
                {
                    errorList.Add(new ErrorListVm
                    {
                        CellNumber = count,
                        ColumnName = "Task Code",
                        ErrorMessage = "Task Code is required."
                    });
                    hasErrors = true;
                }

                // Validate Task Detail (Cell 3)
                var taskDetail = row.Cell(3).GetValue<string>();
                if (string.IsNullOrWhiteSpace(taskDetail))
                {
                    errorList.Add(new ErrorListVm
                    {
                        CellNumber = count,
                        ColumnName = "Task Detail",
                        ErrorMessage = "Task Detail is required."
                    });
                    hasErrors = true;
                }

                // Validate Task Priority (Cell 4)
                var taskPriority = row.Cell(4).GetValue<string>();
                if (string.IsNullOrWhiteSpace(taskPriority))
                {
                    errorList.Add(new ErrorListVm
                    {
                        CellNumber = count,
                        ColumnName = "Task Priority",
                        ErrorMessage = "Task Priority is required."
                    });
                    hasErrors = true;
                }

                // Validate Task Type (Cell 5)
                var taskType = row.Cell(5).GetValue<string>();
                if (string.IsNullOrWhiteSpace(taskType))
                {
                    errorList.Add(new ErrorListVm
                    {
                        CellNumber = count,
                        ColumnName = "Task Type",
                        ErrorMessage = "Task Type is required."
                    });
                    hasErrors = true;
                }

                // Validate Module Name (Cell 6)
                var moduleName = row.Cell(6).GetValue<string>();
                if (string.IsNullOrWhiteSpace(moduleName))
                {
                    errorList.Add(new ErrorListVm
                    {
                        CellNumber = count,
                        ColumnName = "Module Name",
                        ErrorMessage = "Module Name is required."
                    });
                    hasErrors = true;
                }

                // Validate Sprint Name → Lookup Sprint ID (Cell 7)
                var sprintName = row.Cell(7).GetValue<string>()?.Trim().ToLower();
                var sprintId = sprintModels.models
                    .FirstOrDefault(x => x.SprintName.Trim().ToLower() == sprintName)?.Id;

                if (string.IsNullOrWhiteSpace(sprintName) || sprintId == null)
                {
                    errorList.Add(new ErrorListVm
                    {
                        CellNumber = count,
                        ColumnName = "Sprint Name",
                        ErrorMessage = $"Invalid or missing Sprint Name: ({sprintName})"
                    });
                    hasErrors = true;
                }

                // Validate Employee Code → Lookup Emp ID (Cell 8)
                var empCode = row.Cell(8).GetValue<string>()?.Trim().ToLower();
                var empId = projectEmpModels.models
                    .FirstOrDefault(x => x.EmployeeCode?.Trim().ToLower() == empCode)?.Id;

                if (string.IsNullOrWhiteSpace(empCode) || empId == null)
                {
                    errorList.Add(new ErrorListVm
                    {
                        CellNumber = count,
                        ColumnName = "Employee Code",
                        ErrorMessage = $"Invalid or missing Employee Code: ({empCode})"
                    });
                    hasErrors = true;
                }

                // Validate Due Date (Cell 9)
                DateTime dueDate;
                if (!DateTime.TryParse(row.Cell(9).GetValue<string>(), out dueDate))
                {
                    errorList.Add(new ErrorListVm
                    {
                        CellNumber = count,
                        ColumnName = "Due Date",
                        ErrorMessage = "Invalid or missing Due Date."
                    });
                    hasErrors = true;
                }

                // Validate Task Status (Cell 10)
                var taskStatus = row.Cell(10).GetValue<string>();
                if (string.IsNullOrWhiteSpace(taskStatus))
                {
                    errorList.Add(new ErrorListVm
                    {
                        CellNumber = count,
                        ColumnName = "Task Status",
                        ErrorMessage = "Task Status is required."
                    });
                    hasErrors = true;
                }

                // If any errors found, skip adding the model
                if (hasErrors)
                    continue;

                // If all validation passed, create the model
                var model = new Domains.ProjectTask
                {
                    TaskName = taskName,
                    TaskCode = taskCode,
                    TaskDetail = taskDetail,
                    TaskPriority = taskPriority,
                    TaskType = taskType,
                    ModuleName = moduleName,
                    SprintId = Convert.ToInt32(sprintId),
                    EmployeeId = empId,
                    DueDate = dueDate,
                    TaskStatus = taskStatus,
                    IsDeleted = false,
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now
                };

                models.Add(model);
            }

            if (hasErrors)
            {
                return ("Please upload valid File and Please clear all the mentioned error display on Tables", false, errorList);
            }

            await _projectTaskService.CreateBulkProjectTask(models, default);

            return ("File Upload Successfully", true, new List<ErrorListVm>());
        }
    }
}


using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PMS.Domains;
using PMS.Features.Project.ViewModels;
using PMS.Features.UserManagement.ViewModels;
using System.Data;

namespace PMS.Features.Project.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly PmsDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public ProjectRepository(PmsDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
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

        public async  Task<(string message, bool isSuccess, IEnumerable<ProjectDetailVm> models)> GetProjectDetailById(int id, CancellationToken cancellationToken)
        {
            var projModel = await _dbContext.Projects.FirstOrDefaultAsync(x => x.Id == id);

            var projEmpModels = await (from pe in _dbContext.ProjectEmployees
                                join e in _dbContext.Employees on pe.EmployeeId equals e.Id
                                where pe.ProjectId == id && pe.IsDeleted==false && e.IsDeleted==false
                                 select e).ToListAsync(cancellationToken);

            var empModel= await _dbContext.Employees.AsNoTracking().ToListAsync(cancellationToken);

            var departmentModel= await _dbContext.DepartmentMasters.AsNoTracking().ToListAsync(cancellationToken);

            var designationModel= await _dbContext.DesignationMasters.AsNoTracking().ToListAsync(cancellationToken);    

            var responseModel = new ProjectDetailVm
            {
                Id = projModel?.Id ?? 0,
                ProjectName = projModel?.Name ?? string.Empty,
                Description = projModel?.Description ?? string.Empty,
                ProjectStartDate = projModel?.ProjectStartDate ?? DateTime.MinValue,
                ProjectEndDate = projModel?.ProjectEndDate ??DateTime.MinValue,
                ClientName = projModel?.ClientName ?? "N/A",
                ContactPerson = projModel?.ClientContactPerson ?? "N/A",
                ClientUrl = projModel?.ClientUrl ?? "N/A",
                ProjectManager = empModel.FirstOrDefault(x => x.Id == projModel?.ProjectManager)?.Name ?? string.Empty,
                ProjectHead = empModel.FirstOrDefault(x => x.Id == projModel?.ProjectHead)?.Name ?? string.Empty,
                DeliveryManager = empModel.FirstOrDefault(x => x.Id == projModel?.DeliveryHead)?.Name ?? string.Empty,
                ContactNumber = projModel?.ClientContactNumber ?? "N/A",
                ProjectStatus = projModel?.ProjectStatus ?? "N/A",
                Reason = projModel?.Reason ?? "N/A",
                EmployeeCount = projEmpModels.Count
            };

            responseModel.ProjectEmployees= projEmpModels.Select(e=> new PMS.Features.Project.ViewModels.ProjectEmployee
            {
                EmployeeId= e.Id,
                EmployeeName= e.Name ?? string.Empty,
                DepartmentName= departmentModel.FirstOrDefault(x=> x.Id== e.DepartmentId)?.Name ?? string.Empty,
                DesignationName= designationModel.FirstOrDefault(x=> x.Id== e.DesignationId)?.Name ?? string.Empty,
                Email= e.EmailId ?? string.Empty,
                Phone= e.PhoneNumber ?? string.Empty,
                ManagerName= empModel.FirstOrDefault(x=> x.Id== e.ManagerId)?.Name ?? string.Empty,
            }).ToList();

            responseModel.ProjectDocument= await _dbContext.ProjectDocuments.Where(x=> x.ProjectId== id).Select(doc=> new PMS.Features.Project.ViewModels.ProjectDocument
            {
                DocumentName= doc.DocumentName ?? string.Empty,
                //UploadByName = empModel.FirstOrDefault(x => x.Id == doc.UpdatedBy).Name ?? string.Empty,
            }).ToListAsync(cancellationToken);

            return ("Project detail fetched successfully", true, new List<ProjectDetailVm> { responseModel });
        }

        public async Task<(string message, bool isSuccess, IEnumerable<Domains.Project> models)> GetProjectList(CancellationToken cancellationToken)
        {
            var responseModels = await _dbContext.Projects.AsNoTracking().Where(x => x.IsDeleted == false).ToListAsync(cancellationToken);

            return ("Project fetched successfully", true, responseModels);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<ProjectViewModel> models)> GetProjectSelectionList(int empId, int roleId, CancellationToken cancellationToken)
        {
            var roleModel = await _dbContext.RoleMasters.FindAsync(roleId);

            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            var parameters = roleModel?.Name == "Employee"
                ? new { EmployeeId = empId }
                : null; 

            var result = await connection.QueryAsync<ProjectViewModel>(
                "usp_GetEmployeeProjectSelection",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return ("Project selection list fetched", true, result.ToList());

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
            updateModel.ClientContactNumber = model.ClientContactNumber;
            updateModel.ClientContactPerson = model.ClientContactPerson;
            updateModel.ClientName = model.ClientName;
            updateModel.ClientUrl = model.ClientUrl;

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

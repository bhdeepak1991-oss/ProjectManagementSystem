using PMS.Features.Project.Repositories;
using PMS.Features.Project.ViewModels;
using PMS.Features.UserManagement.Respositories;

namespace PMS.Features.Project.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public ProjectService(IProjectRepository projectRepository, IEmployeeRepository employeeRepository)
        {
            _projectRepository = projectRepository;
            _employeeRepository= employeeRepository;
        }
        public async  Task<(string message, bool isSuccess)> CreateProject(Domains.Project model, CancellationToken cancellationToken)
        {
            return await _projectRepository.CreateProject(model, cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> DeleteProject(int projectId, CancellationToken cancellationToken)
        {
            return await _projectRepository.DeleteProject(projectId, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, Domains.Project? model)> GetProjectById(int project, CancellationToken cancellationToken)
        {
            return await _projectRepository.GetProjectById(project, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, string content)> GetProjectDetail(int projectId)
        {
            return await _projectRepository.GetProjectDetail(projectId);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<ProjectDetailVm> models)> GetProjectDetailById(int id, CancellationToken cancellationToken)
        {
            return await _projectRepository.GetProjectDetailById(id, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<ProjectViewModel> model)> GetProjectList(CancellationToken cancellationToken)
        {
            var empModels= await _employeeRepository.GetEmployeeList(cancellationToken);

            var projectModels = await _projectRepository.GetProjectList(cancellationToken);

            var responseModel= projectModels.models.ToList().Select(proj=> new ProjectViewModel
            {
                Id=proj.Id,
                Name=proj.Name,
                Description=proj.Description,
                ProjectStartDate=proj.ProjectStartDate,
                ProjectEndDate=proj.ProjectEndDate,
                ProjectManager= empModels.empModels.FirstOrDefault(x=>x.Id== proj.ProjectManager)?.Name ?? string.Empty,
                DeliveryHead= empModels.empModels.FirstOrDefault(x=>x.Id== proj.DeliveryHead)?.Name ?? string.Empty,
                ProjectHead= empModels.empModels.FirstOrDefault(x=>x.Id== proj.ProjectHead)?.Name ?? string.Empty,
                ClientUrl= proj.ClientUrl,
                ClientContactNumber= proj.ClientContactNumber,
                ClientContactPerson= proj.ClientContactPerson,
                ClientName= proj.ClientName,
                ProjectStatus= proj.ProjectStatus,
                Reason= proj.Reason,
                EmployeeCount =0 // Todo
            }).ToList();

            return ("Project Detail fetched", true, responseModel);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<ProjectViewModel> models)> GetProjectSelectionList(int empId, int roleId, CancellationToken cancellationToken)
        {
           return await _projectRepository.GetProjectSelectionList(empId, roleId, cancellationToken);
        }

        public async  Task<(string message, bool isSuccess)> UpdateProject(Domains.Project model, CancellationToken cancellationToken)
        {
            return await _projectRepository.UpdateProject(model, cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> UpdateProjectStatus(int id, string reason, string status)
        {
            return await _projectRepository.UpdateProjectStatus(id, reason, status);
        }
    }
}

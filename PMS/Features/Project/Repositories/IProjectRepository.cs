using Microsoft.AspNetCore.Mvc;
using PMS.Features.Project.ViewModels;

namespace PMS.Features.Project.Repositories
{
    public interface IProjectRepository
    {
        Task<(string message, bool isSuccess)> CreateProject(Domains.Project model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> UpdateProject(Domains.Project model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> DeleteProject(int projectId, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, Domains.Project? model)> GetProjectById(int projectId, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<Domains.Project> models)> GetProjectList(CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, string content)> GetProjectDetail(int projectId);
        Task<(string message, bool isSuccess)> UpdateProjectStatus(int id, string reason, string status);
        Task<(string message, bool isSuccess, IEnumerable<ProjectViewModel> models)> GetProjectSelectionList(int empId, int roleId, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<ProjectDetailVm> models)> GetProjectDetailById(int id, CancellationToken cancellationToken);
    }
}

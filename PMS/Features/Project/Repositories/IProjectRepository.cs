using Microsoft.AspNetCore.Mvc;

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
    }
}

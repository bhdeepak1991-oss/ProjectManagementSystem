using PMS.Domains;

namespace PMS.Features.Sprint.Repositories
{
    public interface ISprintRepository
    {
        Task<(string message, bool isSuccess)> CreateSprint(Domains.Sprint model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> UpdateSprint(Domains.Sprint model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, Domains.Sprint model)> GetSprintById(int id, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<Domains.Sprint> models)> GetSprintList(int projectId,CancellationToken cancellationToken);
    }
}

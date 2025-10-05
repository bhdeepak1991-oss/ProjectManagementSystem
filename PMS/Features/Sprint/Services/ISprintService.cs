using PMS.Features.Sprint.ViewModels;

namespace PMS.Features.Sprint.Services
{
    public interface ISprintService
    {
        Task<(string message, bool isSuccess)> CreateSprint(Domains.Sprint model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> UpdateSprint(Domains.Sprint model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, Domains.Sprint model)> GetSprintById(int id, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<SprintViewModel> models)> GetSprintList(CancellationToken cancellationToken);
        Task<string> GetSprintBusinessGoal(int sprintId, CancellationToken cancellationToken);
        Task<string> GetSprintGoal(int sprintId, CancellationToken cancellationToken);

    }
}

using PMS.Features.Vacation.ViewModels;

namespace PMS.Features.Vacation.Services
{
    public interface IVacationService
    {
        Task<(string message, bool isSuccess)> CreateVacation(Domains.VacationDetail model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, Domains.VacationDetail model)> GetVacationModel(int id, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<VacationVm> models)> GetVacationDetail(CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> DeleteVacationById(int id, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> UpdateVacation(Domains.VacationDetail model, CancellationToken cancellationToken);

        Task<(string message, bool isSuccess, IEnumerable<EventModel> models)> GetEventModels(CancellationToken cancellationToken);

    }
}

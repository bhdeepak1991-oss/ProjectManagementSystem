using PMS.Domains;
using PMS.Features.Vacation.Repositories;
using PMS.Features.Vacation.ViewModels;

namespace PMS.Features.Vacation.Services
{
    public class VacationService : IVacationService
    {
        private readonly IVacationRepository _vacationRepository;

        public VacationService(IVacationRepository vacationRepository)
        {
            _vacationRepository = vacationRepository;
        }

        public async Task<(string message, bool isSuccess)> CreateVacation(VacationDetail model, CancellationToken cancellationToken)
        {
            return await _vacationRepository.CreateVacation(model, cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> DeleteVacationById(int id, CancellationToken cancellationToken)
        {
            return await _vacationRepository.DeleteVacationById(id, cancellationToken);
        }

        public async  Task<(string message, bool isSuccess, IEnumerable<VacationVm> models)> GetVacationDetail(CancellationToken cancellationToken)
        {
            return await _vacationRepository.GetVacationDetail(cancellationToken);
        }

        public async  Task<(string message, bool isSuccess, VacationDetail model)> GetVacationModel(int id, CancellationToken cancellationToken)
        {
            return await _vacationRepository.GetVacationModel(id, cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> UpdateVacation(VacationDetail model, CancellationToken cancellationToken)
        {
            return await _vacationRepository.UpdateVacation(model, cancellationToken);
        }
    }
}

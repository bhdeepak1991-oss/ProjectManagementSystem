using PMS.Domains;
using PMS.Features.Master.ViewModels;

namespace PMS.Features.Master.Services
{
    public interface IDesignationService
    {
        Task<(string message, bool isSuccess)> CreateDesignation(DesignationMaster model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> UpdateDesignation(DesignationMaster model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> DeleteDesignation(int id, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, DesignationMaster? model)> GetDesignationById(int id, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<DesignationViewModel> model)> GetAllDesignation(CancellationToken cancellationToken);
    }
}

using PMS.Domains;

namespace PMS.Features.Master.Respositories
{
    public interface IDesignationRepository
    {
        Task<(string message, bool isSuccess)> CreateDesignation(DesignationMaster model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> UpdateDesignation(DesignationMaster model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> DeleteDesignation(int id, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, DesignationMaster? model)> GetDesignationById(int id, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<DesignationMaster> model)> GetAllDesignation(CancellationToken cancellationToken);
    }
}

namespace PMS.Features.Utilities.Repositories
{
    public interface IStatusHelperRepository
    {
        Task<(string message, bool isSuccess)> UploadStatusHelper(List<Domains.StatusHelper> models, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<Domains.StatusHelper> models)> GetStatusHelperDetail(CancellationToken cancellationToken);
    }
}

namespace PMS.Features.Utilities.Services
{
    public interface IStatusHelperService
    {
        Task<(string message, bool isSuccess)> UploadStatusHelper(IFormFile uploadFile, CancellationToken cancellationToken);

        Task<(string message, bool isSuccess, IEnumerable<Domains.StatusHelper> models)> GetStatusHelperDetail(CancellationToken cancellationToken);
    }
}

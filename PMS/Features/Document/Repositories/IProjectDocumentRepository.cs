using PMS.Domains;

namespace PMS.Features.Document.Repositories
{
    public interface IProjectDocumentRepository
    {
        Task<(string message, bool isSuccess)> UploadProjectDocument(ProjectDocument model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> DeleteProjectDocument(int documentId, int userId, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<ProjectDocument> models)> GetProjectDocuments(CancellationToken cancellationToken);
    }
}

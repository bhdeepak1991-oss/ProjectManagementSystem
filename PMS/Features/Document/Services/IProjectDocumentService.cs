using PMS.Domains;
using PMS.Features.Document.ViewModels;

namespace PMS.Features.Document.Services
{
    public interface IProjectDocumentService
    {
        Task<(string message, bool isSuccess)> UploadProjectDocument(ProjectDocumentVm model, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess)> DeleteProjectDocument(int documentId, int userId, CancellationToken cancellationToken);
        Task<(string message, bool isSuccess, IEnumerable<ProjectDocument> models)> GetProjectDocuments(int empId,CancellationToken cancellationToken);

        Task<(string message, bool isSuccess)> DocumentRequestAccess(DocumentRequestVm model);

        Task<(string message, bool isSuccess, IEnumerable<ProjectDocument> models)> GetProjctDocumentDetail(int projectId);
    }
}

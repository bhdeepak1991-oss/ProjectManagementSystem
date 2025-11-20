using Microsoft.AspNetCore.Mvc;
using PMS.Domains;
using PMS.Features.Document.Repositories;
using PMS.Features.Document.ViewModels;
using PMS.Helpers;

namespace PMS.Features.Document.Services
{
    public class ProjectDocumentService : IProjectDocumentService
    {
        private readonly IProjectDocumentRepository _projectDocumentRepository;
        private readonly BlobHelper _blobHelper;

        public ProjectDocumentService(IProjectDocumentRepository projectDocumentRepository, BlobHelper blobHelper)
        {
            _projectDocumentRepository = projectDocumentRepository;
            _blobHelper = blobHelper;
        }
        public async Task<(string message, bool isSuccess)> DeleteProjectDocument(int documentId, int userId, CancellationToken cancellationToken)
        {
            return await _projectDocumentRepository.DeleteProjectDocument(documentId, userId, cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> DocumentRequestAccess(DocumentRequestVm model)
        {
            var projectDocumentRequest = new ProjectDocumentsRequest()
            {
                DocumentId = model.DocumentId,
                RequestById = model.RequestById,
                RequestReason = model.RequestReason,
                RequestStatus = model.RequestStatus,
                CreatedBy = model.RequestById,
                CreatedDate = DateTime.Now
            };

            return await _projectDocumentRepository.DocumentRequestAccess(projectDocumentRequest);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<ProjectDocument> models)> GetProjctDocumentDetail(int projectId)
        {
            return await _projectDocumentRepository.GetProjctDocumentDetail(projectId);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<ProjectDocument> models)> GetProjectDocuments(int empId, CancellationToken cancellationToken)
        {
            return await _projectDocumentRepository.GetProjectDocuments(empId,cancellationToken);
        }
        public async Task<(string message, bool isSuccess)> UploadProjectDocument(ProjectDocumentVm model, CancellationToken cancellationToken)
        {
            var uploadFilePath = await _blobHelper.UploadFileAsync(model.UploadDocument);

            var dbModel = new ProjectDocument()
            {
                ProjectId = model.ProjectId,
                DocumentDetail = model.DocumentDetail,
                DocumentName = model.DocumentName,
                DocumentPath = uploadFilePath,
                IsDeleted = false,
                CreatedBy = model.CreatedBy,
                CreatedDate = DateTime.Now,
                UploadFileName = model.UploadDocument.FileName ?? "PmsDocument"
            };

            var respsonse = await _projectDocumentRepository.UploadProjectDocument(dbModel, cancellationToken);

            return respsonse;
        }
    }
}

using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using PMS.Domains;

namespace PMS.Features.Document.Repositories
{
    public class ProjectDocumentRepository : IProjectDocumentRepository
    {
        private readonly PmsDbContext _dbContext;

        public ProjectDocumentRepository(PmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(string message, bool isSuccess)> DeleteProjectDocument(int documentId, int userId, CancellationToken cancellationToken)
        {
            var deleteModel = await _dbContext.ProjectDocuments.FindAsync(documentId, cancellationToken);

            if (deleteModel is null)
            {
                return ($"Project document not found for Id {documentId}", false);
            }

            deleteModel.IsDeleted = true;
            deleteModel.UpdatedDate = DateTime.Now;
            deleteModel.UpdatedBy = userId;

            _dbContext.Update(deleteModel);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Project document deleted successfully !", true);
        }

        public async  Task<(string message, bool isSuccess)> DocumentRequestAccess(ProjectDocumentsRequest model)
        {
            var repsonse = await _dbContext.ProjectDocumentsRequests.AddAsync(model);

            await _dbContext.SaveChangesAsync();

            return ("Document request has been addedd", true);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<ProjectDocument> models)> GetProjctDocumentDetail(int projectId)
        {
            var responseModels = await _dbContext.ProjectDocuments
                        .Where(x => x.IsDeleted == false && x.ProjectId == projectId).ToListAsync();

            return ("Project Document fetched successfully", true, responseModels);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<ProjectDocument> models)> GetProjectDocuments(int empId, CancellationToken cancellationToken)
        {
            var requestDocumentIds = await _dbContext.ProjectDocumentsRequests
                          .Where(x => x.RequestById == empId && x.RequestStatus == "Approved")
                          .Select(x => x.DocumentId) 
                          .ToListAsync();

            var responseModels = await _dbContext.ProjectDocuments
                        .Where(x => x.IsDeleted==false && x.CreatedBy == empId)
                        .ToListAsync(cancellationToken);


            var empModel = await _dbContext.Employees.ToListAsync();

            responseModels.ForEach(data =>
            {
                data.CreatedByName = empModel.FirstOrDefault(x => x.Id == data.CreatedBy)?.Name ?? string.Empty;
            });


            return ("Project document fetched successfully", true, responseModels);
        }

        public async Task<(string message, bool isSuccess)> UploadProjectDocument(ProjectDocument model, CancellationToken cancellationToken)
        {
            var uploadDocument = await _dbContext.ProjectDocuments.AddAsync(model, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ("Project document uploaded successfully", true);
        }
    }
}

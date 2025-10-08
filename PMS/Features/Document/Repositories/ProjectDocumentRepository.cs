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

        public async Task<(string message, bool isSuccess, IEnumerable<ProjectDocument> models)> GetProjectDocuments(CancellationToken cancellationToken)
        {
            var responseModels = await _dbContext.ProjectDocuments.Where(x => x.IsDeleted == false).ToListAsync(cancellationToken);

            var result = from um in _dbContext.UserManagements
                         join emp in _dbContext.Employees
                         on um.EmployeeId equals emp.Id
                         where um.IsDeleted ==false && emp.IsDeleted== false
                         select new
                         {
                             emp.Name,
                             emp.EmployeeCode,
                             um.Id
                         };

            responseModels.ForEach(data =>
            {
                data.CreatedByName = result.FirstOrDefault(x => x.Id == data.CreatedBy)?.Name ?? string.Empty;
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

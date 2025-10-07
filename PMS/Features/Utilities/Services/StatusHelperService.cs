using ClosedXML.Excel;
using PMS.Domains;
using PMS.Features.Utilities.Repositories;

namespace PMS.Features.Utilities.Services
{
    public class StatusHelperService : IStatusHelperService
    {
        private readonly IStatusHelperRepository _statusHelperRepository;

        public StatusHelperService(IStatusHelperRepository statusHelperRepository)
        {
            _statusHelperRepository = statusHelperRepository;
        }

        public async Task<(string message, bool isSuccess, IEnumerable<StatusHelper> models)> GetStatusHelperDetail(CancellationToken cancellationToken)
        {
            return await _statusHelperRepository.GetStatusHelperDetail(default);
        }

        public async Task<(string message, bool isSuccess)> UploadStatusHelper(IFormFile uploadFile, CancellationToken cancellationToken)
        {
            if (uploadFile == null || uploadFile.Length == 0)
            {
                return ("Please upload valid file", false);
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, uploadFile.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadFile.CopyToAsync(stream);
            }

            using var workbook = new XLWorkbook(filePath);

            var worksheet = workbook.Worksheet(1);

            var rows = worksheet.RangeUsed().RowsUsed();

            var models = new List<StatusHelper>();

            foreach (var row in rows.Skip(1))
            {
                var model = new StatusHelper();
                model.Name = row.Cell(1).GetValue<string>();
                model.Code = row.Cell(2).GetValue<string>();
                model.StatusType = row.Cell(3).GetValue<string>();
                model.IsActive = true;
                model.IsDeleted = false;
                model.CreatedBy = 1;
                model.CreatedDate = DateTime.Now;

                models.Add(model);
            }

            await _statusHelperRepository.UploadStatusHelper(models, cancellationToken);

            return ("File upload successfully", true);
        }
    }
}

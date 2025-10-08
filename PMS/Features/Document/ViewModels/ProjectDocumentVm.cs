namespace PMS.Features.Document.ViewModels
{
    public class ProjectDocumentVm
    {
        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public string? DocumentName { get; set; }
        public string? DocumentDetail { get; set; }
        public IFormFile UploadDocument { get; set; }
        public int CreatedBy { get; set; }
    }
}

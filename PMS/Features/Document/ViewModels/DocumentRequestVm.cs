namespace PMS.Features.Document.ViewModels
{
    public class DocumentRequestVm
    {
        public int DocumentId { get; set; }
        public int RequestById { get; set; }
        public string RequestReason { get; set; }
        public string RequestStatus { get; set; } = "Pending";
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace PMS.Domains
{

    [Table("ProjectDocumentsRequest")]
    public class ProjectDocumentsRequest
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int DocumentId { get; set; }
        public int RequestById { get; set; }
        public string? RequestReason { get; set; }
        public string? RequestStatus { get; set; }
        public int? RequestApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }
}

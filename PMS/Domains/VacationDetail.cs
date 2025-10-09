using System.ComponentModel.DataAnnotations.Schema;

namespace PMS.Domains
{

    [Table("VacationDetail")]
    public class VacationDetail
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? VacationType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string VacationName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string ProjectInvolved { get; set; } = string.Empty;
    }
}

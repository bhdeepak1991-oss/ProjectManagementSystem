using System.ComponentModel.DataAnnotations.Schema;

namespace PMS.Domains
{

    [Table("LeaveType")]
    public class LeaveType
    {
        public int Id { get; set; }
        public string LeaveTypeCode { get; set; } = string.Empty;
        public string LeaveTypeName { get; set; } = string.Empty;
        public decimal? PermonthAllocatedLeave { get; set; }
        public DateTime? LeaveCalcullationStartDate { get; set; }
        public DateTime? LeaveCalcullationEndDate { get; set; }
        public bool IsCarryForward { get; set; }
        public decimal? CarryForwardLeaveCount { get; set; }
        public string EmployementStatus { get; set; } = "Confirmed";
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
    }
}

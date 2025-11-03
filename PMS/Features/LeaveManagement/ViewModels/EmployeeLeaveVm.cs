using DocumentFormat.OpenXml.Office2013.Excel;

namespace PMS.Features.LeaveManagement.ViewModels
{
    public class EmployeeLeaveVm
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string LeaveType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Reason { get; set; }
        public string LeaveStatus { get; set; } = "In Progress";
        public int? ApprovedRejectBy { get; set; }
        public DateTime? ApprovedRejectDate { get; set; }
        public string ApprovedRejectReason { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public string ApprovedRejectedBy { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
    }
}

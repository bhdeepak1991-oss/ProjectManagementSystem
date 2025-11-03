namespace PMS.Features.LeaveManagement.ViewModels
{
    public class LeaveCountVm
    {
        public decimal RemainingLeaveCout { get; set; }
        public decimal TotalLeaveCountTillDate { get; set; }
        public decimal LeaveTaken { get; set; }
        public decimal PercentageOfLeaveTaken { get; set; }
    }
}

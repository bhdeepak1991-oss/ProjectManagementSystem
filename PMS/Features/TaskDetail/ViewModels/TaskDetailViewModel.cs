namespace PMS.Features.TaskDetail.ViewModels
{
    public class TaskDetailViewModel
    {
        public int Id { get; set; }
        public string? TaskName { get; set; }
        public string? TaskDetail { get; set; }
        public string? TaskPriority { get; set; }
        public string? TaskType { get; set; }
        public DateTime? DueDate { get; set; }
        public string? TaskCode { get; set; }
        public int? ProjectId { get; set; }
        public string? ModuleName { get; set; }
        public string EmployeeName { get; set; }
        public string? TaskStatus { get; set; }
        public string SprintName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public decimal EstimatedHour { get; set; }
        public decimal LoggedHour { get; set; }
    }
}

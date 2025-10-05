
namespace PMS.Features.Sprint.ViewModels
{
    public class SprintViewModel
    {
        public int Id { get; set; }
        public string SprintName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int DurationDays { get; set; }
        public string DepartmentName { get; set; }
        public string SprintGoal { get; set; }
        public string BusinessGoal { get; set; }
        public DateTime? DemoDate { get; set; }
        public string Status { get; set; }
    }
}

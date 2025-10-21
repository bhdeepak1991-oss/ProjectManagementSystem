using Microsoft.Identity.Client;

namespace PMS.Features.Dashboard.ViewModels
{
    public class DashboardVm
    {
        public IEnumerable<TaskStatusVm> TaskStatusModels { get; set; }
        public IEnumerable<EmployeeTaskVm> EmployeeTasks { get; set; }


        public IEnumerable<TaskModel> TaskModelsDrillDown { get; set; }
    }

    public class TaskModel
    {
        public string TaskType { get; set; }      // Bug, CR, etc.
        public string TaskStatus { get; set; }    // Open, Closed, etc.
        public string TaskPriority { get; set; }  // High, Medium, Low
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
    }

    public class MasterModelVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TaskStatusVm
    {
        public int RecordCount { get; set; }
        public string? TaskStatus { get; set; }
        public int TotalCount { get; set; }
    }

    public class EmployeeTaskVm
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string TaskPriority { get; set; }
        public Dictionary<string, int> TaskTypeCounts { get; set; } = new();
    }
}

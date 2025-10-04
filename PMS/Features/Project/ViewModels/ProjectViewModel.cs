namespace PMS.Features.Project.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime? ProjectStartDate { get; set; }

        public DateTime? ProjectEndDate { get; set; }

        public string? ProjectManager { get; set; }

        public string? ProjectHead { get; set; }

        public string? DeliveryHead { get; set; }

        public int? EmployeeCount { get; set; }

        public string? ClientName { get; set; }
        public string? ClientContactNumber { get; set; }
        public string? ClientContactPerson { get; set; }
        public string? ClientUrl { get; set; }

        public string? ProjectStatus { get; set; }
        public string? Reason { get; set; }
    }
}

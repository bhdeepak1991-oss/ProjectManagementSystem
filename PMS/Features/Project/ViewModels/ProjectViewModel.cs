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

        public string? VersionControlURL { get; set; }
        public string? DevelopmentUIURL { get; set; }
        public string? QAUIURL { get; set; }
        public string? ProductionURL { get; set; }
    }
}

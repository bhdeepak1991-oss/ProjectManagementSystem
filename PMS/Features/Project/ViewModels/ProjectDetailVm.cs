namespace PMS.Features.Project.ViewModels
{
    public class ProjectDetailVm
    {
        public int Id { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string ClientUrl { get; set; } = string.Empty;
        public string ProjectManager { get; set; } = string.Empty;
        public string ProjectHead { get; set; } = string.Empty;
        public string DeliveryManager { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string ProjectStatus { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public int EmployeeCount { get; set; }
        public IEnumerable<ProjectEmployee> ProjectEmployees { get; set; } = Array.Empty<ProjectEmployee>();
        public IEnumerable<ProjectDocument> ProjectDocument { get; set; } = Array.Empty<ProjectDocument>();
    }
    public class ProjectEmployee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string DesignationName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
    }

    public class ProjectDocument
    {
        public string DocumentName { get; set; } = string.Empty;
        public string UploadByName { get; set; }
    }
}

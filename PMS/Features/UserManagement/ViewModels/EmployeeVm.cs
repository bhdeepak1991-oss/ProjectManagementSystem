namespace PMS.Features.UserManagement.ViewModels
{
    public class EmployeeVm
    {
        public string EmployeeName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string DesignationName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string EmailId { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string ManagerName { get; set; }
        public IEnumerable<ProjectVm> AssignProjectList { get; set; } = new List<ProjectVm>();
    }

    public class ProjectVm
    {
        public string ProjectName { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string ProjectStatus { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

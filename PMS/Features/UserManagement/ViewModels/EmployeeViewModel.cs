namespace PMS.Features.UserManagement.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? DepartmentName { get; set; }

        public string? DesignationName { get; set; }

        public string? EmployeeCode { get; set; }

        public string? EmailId { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public string? PhoneNumber { get; set; }
    }
}

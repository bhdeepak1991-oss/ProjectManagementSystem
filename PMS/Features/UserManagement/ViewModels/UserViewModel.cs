namespace PMS.Features.UserManagement.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string RoleName { get; set; }
        public bool  IsLocked { get; set; }
        public string EmpCode { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
    }
}

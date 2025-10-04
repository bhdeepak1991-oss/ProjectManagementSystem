namespace PMS.Features.ProjectEmployee.ViewModels
{
    public class MappedUnMappedEmployeeVm
    {
        public List<MappedEmployeeVm> MappedEmployee { get; set; }
        public List<MappedEmployeeVm> UnMappedEmployee { get; set; }
    }

    public class MappedEmployeeVm
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
    }
}

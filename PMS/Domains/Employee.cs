using System;
using System.Collections.Generic;

namespace PMS.Domains;

public partial class Employee
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? DepartmentId { get; set; }

    public int? DesignationId { get; set; }

    public string? EmployeeCode { get; set; }

    public string? EmailId { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public DateTime? DateOfJoining { get; set; }

    public string? PhoneNumber { get; set; }

    public bool? IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

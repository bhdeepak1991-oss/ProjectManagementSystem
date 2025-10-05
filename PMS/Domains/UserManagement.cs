using System;
using System.Collections.Generic;

namespace PMS.Domain;

public partial class UserManagement
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public bool? IsLocked { get; set; }

    public int? RoleId { get; set; }

    public bool? IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? EmployeeId { get; set; }

    public bool? IsTempPassword { get; set; }
    public string? AuthenticatorKey { get; set; }
}

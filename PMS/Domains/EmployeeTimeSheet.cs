using System;
using System.Collections.Generic;

namespace PMS.Domains;

public partial class EmployeeTimeSheet
{
    public int Id { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? EmployeeId { get; set; }

    public string? TimeSheetStatus { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public int? TimeSheetYear { get; set; }

    public int? TimeSheetMonth { get; set; }

    public string? TimeSheetName { get; set; }
}

using System;
using System.Collections.Generic;

namespace PMS.Domains;

public partial class EmployeeTimeSheetTaskDetail
{
    public int Id { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? EmployeeTimeSheetTaskId { get; set; }

    public DateOnly? TimeSheetDate { get; set; }

    public string? WorkingHour { get; set; }

    public string? TaskDetail { get; set; }

    public string? DayName { get; set; }
}

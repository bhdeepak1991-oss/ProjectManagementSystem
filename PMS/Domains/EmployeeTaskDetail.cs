using System;
using System.Collections.Generic;

namespace PMS.Domains;

public partial class EmployeeTaskDetail
{
    public int Id { get; set; }

    public int? ProjectTaskId { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? TaskStatus { get; set; }

    public string? Comment { get; set; }

    public bool? IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

using System;
using System.Collections.Generic;

namespace PMS.Domains;

public partial class ProjectTask
{
    public int Id { get; set; }

    public string? TaskName { get; set; }

    public string? TaskDetail { get; set; }

    public string? TaskPriority { get; set; }

    public string? TaskType { get; set; }

    public bool? IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public DateTime? DueDate { get; set; }

    public string? TaskCode { get; set; }

    public int? ProjectId { get; set; }

    public string? ModuleName { get; set; }

    public int? EmployeeId { get; set; }

    public string? TaskStatus { get; set; }
}

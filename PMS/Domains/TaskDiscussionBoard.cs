using System;
using System.Collections.Generic;

namespace PMS.Domains;

public partial class TaskDiscussionBoard
{
    public int Id { get; set; }

    public int? ProjectTaskId { get; set; }

    public string? Discusion { get; set; }

    public string? DocumentPath { get; set; }

    public bool? IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? EmployeeId { get; set; }
}

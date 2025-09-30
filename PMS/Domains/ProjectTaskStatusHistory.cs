using System;
using System.Collections.Generic;

namespace PMS.Domains;

public partial class ProjectTaskStatusHistory
{
    public int Id { get; set; }

    public int? ProjectTaskId { get; set; }

    public string? TaskStatus { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

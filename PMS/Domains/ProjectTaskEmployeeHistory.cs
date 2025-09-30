using System;
using System.Collections.Generic;

namespace PMS.Domains;

public partial class ProjectTaskEmployeeHistory
{
    public int Id { get; set; }

    public int? ProjectTaskId { get; set; }

    public int? EmployeeId { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

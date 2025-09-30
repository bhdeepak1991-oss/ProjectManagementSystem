using System;
using System.Collections.Generic;

namespace PMS.Domains;

public partial class ProjectEmployee
{
    public int Id { get; set; }

    public int? ProjectId { get; set; }

    public int? EmployeeId { get; set; }

    public int? EmployeeDesignation { get; set; }

    public bool? IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

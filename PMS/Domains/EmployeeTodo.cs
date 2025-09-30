using System;
using System.Collections.Generic;

namespace PMS.Domains;

public partial class EmployeeTodo
{
    public int Id { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? TaskDetail { get; set; }

    public string? TaskStatus { get; set; }
}

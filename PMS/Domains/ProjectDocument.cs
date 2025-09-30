using System;
using System.Collections.Generic;

namespace PMS.Domains;

public partial class ProjectDocument
{
    public int Id { get; set; }

    public int? ProjectId { get; set; }

    public string? DocumentName { get; set; }

    public string? DocumentDetail { get; set; }

    public string? DocumentPath { get; set; }

    public bool? IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

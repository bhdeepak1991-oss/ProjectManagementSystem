using System;
using System.Collections.Generic;

namespace PMS.Domains;

public partial class Project
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime? ProjectStartDate { get; set; }

    public DateTime? ProjectEndDate { get; set; }

    public int? ProjectManager { get; set; }

    public int? ProjectHead { get; set; }

    public int? DeliveryHead { get; set; }

    public bool? IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? VersionControlURL { get; set; }
    public string? DevelopmentUIURL { get; set; }
    public string? QAUIURL { get; set; }
    public string? ProductionURL { get; set; }

    public string? ProjectStatus { get; set; }
    public string? Reason { get; set; }
}

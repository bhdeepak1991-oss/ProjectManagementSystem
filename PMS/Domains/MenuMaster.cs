using System;
using System.Collections.Generic;

namespace PMS.Domains;

public partial class MenuMaster
{
    public int Id { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? MenuName { get; set; }

    public string? ControllerName { get; set; }

    public string? ActionName { get; set; }

    public int? DisplayOrder { get; set; }

    public string? SubMenuName { get; set; }
}

using System;
using System.Collections.Generic;

namespace PMS.Domains;

public partial class RoleMenuMapping
{
    public int Id { get; set; }

    public int? MenuId { get; set; }

    public int? RoleId { get; set; }

    public int? CreatedBy { get; set; }

    public int? CreatedDate { get; set; }
}

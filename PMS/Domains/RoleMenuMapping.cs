using System;
using System.Collections.Generic;

namespace PMS.Domains;

public partial class RoleMenuMapping
{
    public int Id { get; set; }

    public int? MenuId { get; set; }

    public int? RoleId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }= DateTime.Now;

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }
    public int DisplayOrder { get; set; }
}

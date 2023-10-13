using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewAssignViewP
{
    public Guid? LUid { get; set; }

    public Guid ClUid { get; set; }

    public string? CName { get; set; }

    public string? DName { get; set; }

    public Guid? LevId { get; set; }

    public Guid? LUidEv { get; set; }

    public int? LevType { get; set; }

    public int? LevYear { get; set; }

    public string? Remark { get; set; }

    public Guid? FMatchKey { get; set; }
}

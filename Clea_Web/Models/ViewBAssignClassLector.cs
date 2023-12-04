using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewBAssignClassLector
{
    public Guid EId { get; set; }

    public int EYear { get; set; }

    public Guid CUid { get; set; }

    public string? CId { get; set; }

    public string? CName { get; set; }

    public Guid EsId { get; set; }

    public Guid ClUid { get; set; }

    public string? DId { get; set; }

    public string? DName { get; set; }

    public int? DHour { get; set; }

    public Guid? LUid { get; set; }

    public string? LId { get; set; }

    public string? LName { get; set; }

    public int Status { get; set; }

    public bool IsSche { get; set; }

    public int ScheNum { get; set; }

    public bool? IsPass { get; set; }

    public bool IsClose { get; set; }

    public long ChkNum { get; set; }

    public DateTime Credate { get; set; }
}

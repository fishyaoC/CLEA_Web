using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewPLectorEvaluate
{
    public Guid EId { get; set; }

    public int EType { get; set; }

    public string? MName { get; set; }

    public string? CName { get; set; }

    public Guid EsId { get; set; }

    public string? BpName { get; set; }

    public string? DName { get; set; }

    public Guid EdId { get; set; }

    public string? LName { get; set; }

    public int Status { get; set; }

    public Guid? Evaluate { get; set; }

    public bool? IsPass { get; set; }

    public bool IsClose { get; set; }

    public DateTime Credate { get; set; }

    public int? EScoreA { get; set; }
}

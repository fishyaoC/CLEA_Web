using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewBAssignClassScore
{
    public Guid EId { get; set; }

    public Guid EsId { get; set; }

    public Guid EdId { get; set; }

    public Guid? Evaluate { get; set; }

    public string? LName { get; set; }

    public int? EScoreA { get; set; }

    public int? EScoreB { get; set; }

    public int? EScoreC { get; set; }

    public int? EScoreD { get; set; }

    public int? EScoreE { get; set; }

    public string? ERemark { get; set; }

    public int Status { get; set; }

    public bool IsClose { get; set; }

    public DateTime? EFirstScoreDate { get; set; }
}

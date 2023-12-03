using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewBAssignBookEvaluateTeacher
{
    public Guid EdId { get; set; }

    public Guid EId { get; set; }

    public Guid EsId { get; set; }

    public Guid? Evaluate { get; set; }

    public int? EScoreA { get; set; }

    public bool IsClose { get; set; }

    public int Status { get; set; }

    public Guid? MId { get; set; }

    public string? MIndex { get; set; }

    public string? MName { get; set; }

    public Guid? LUid { get; set; }

    public string? LId { get; set; }

    public string? LName { get; set; }

    public Guid? BpId { get; set; }

    public string? BpName { get; set; }
}

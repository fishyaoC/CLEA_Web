using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewBAssignClassEvaluate
{
    public Guid CUid { get; set; }

    public string? CId { get; set; }

    public string? CName { get; set; }

    public Guid EId { get; set; }

    public int EType { get; set; }

    public int EYear { get; set; }
}

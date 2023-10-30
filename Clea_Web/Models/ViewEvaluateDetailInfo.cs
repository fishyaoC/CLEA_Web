using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewEvaluateDetailInfo
{
    public Guid EdId { get; set; }

    public Guid EId { get; set; }

    public int? EScoreA { get; set; }

    public string? MName { get; set; }

    public Guid? MdPublish { get; set; }

    public Guid? FMatchKey { get; set; }
}

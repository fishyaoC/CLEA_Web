using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewBookEvaluate
{
    public Guid EId { get; set; }

    public int EType { get; set; }

    public int EYear { get; set; }

    public Guid MId { get; set; }

    public int? MIndex { get; set; }

    public string? MName { get; set; }
}

using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewClassSubLectorV
{
    public Guid? CKey { get; set; }

    public string? ClassName { get; set; }

    public Guid SubKey { get; set; }

    public string? SubId { get; set; }

    public string? SubName { get; set; }

    public string? LName { get; set; }

    public int LevYear { get; set; }

    public int LevType { get; set; }
}

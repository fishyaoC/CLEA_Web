using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewClassSubLectorP
{
    public Guid? CKey { get; set; }

    public string? ClassName { get; set; }

    public Guid SubKey { get; set; }

    public string? SubId { get; set; }

    public string? SubName { get; set; }

    public string? LName { get; set; }
}

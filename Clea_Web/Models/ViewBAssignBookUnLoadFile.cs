using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewBAssignBookUnLoadFile
{
    public Guid EId { get; set; }

    public Guid EsId { get; set; }

    public Guid? FMatchKey { get; set; }

    public string? LName { get; set; }

    public string? CName { get; set; }

    public string? DName { get; set; }
}

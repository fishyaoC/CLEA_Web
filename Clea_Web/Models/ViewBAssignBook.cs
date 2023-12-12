using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewBAssignBook
{
    public Guid EId { get; set; }

    public Guid? MId { get; set; }

    public string? MIndex { get; set; }

    public string? MName { get; set; }

    public bool IsClose { get; set; }
}

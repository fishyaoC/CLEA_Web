using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewBBookEvaluateTeacher
{
    public Guid EId { get; set; }

    public Guid? Evaluate { get; set; }

    public string? LName { get; set; }

    public string? LId { get; set; }
}

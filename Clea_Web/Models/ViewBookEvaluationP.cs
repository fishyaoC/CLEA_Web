using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewBookEvaluationP
{
    public Guid MId { get; set; }

    public int? MIndex { get; set; }

    public string? MName { get; set; }

    public string? MPublish { get; set; }

    public string? MNumber { get; set; }

    public DateTime? MVersion { get; set; }

    public Guid? FMatchKey { get; set; }
}

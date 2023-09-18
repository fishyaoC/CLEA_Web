using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CClassLector
{
    public string ClId { get; set; } = null!;

    public string? CId { get; set; }

    public string? LId { get; set; }

    public string? ClIndex { get; set; }

    public string? ClOrder { get; set; }

    public string? ClHourlyRate { get; set; }

    public string? ClQualify { get; set; }

    public string? ClIsActive { get; set; }

    public string? Creuser { get; set; }

    public string? Credate { get; set; }

    public string? Upduser { get; set; }

    public string? Upddate { get; set; }
}

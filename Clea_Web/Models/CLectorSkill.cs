using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CLectorSkill
{
    public string SId { get; set; } = null!;

    public string? LId { get; set; }

    public string? SIndex { get; set; }

    public string? SClass { get; set; }

    public string? SHourlRate { get; set; }

    public string? STeachOrder { get; set; }

    public string? SQualify { get; set; }

    public string? SIsActive { get; set; }

    public string? FMatchKey { get; set; }

    public string? Creuser { get; set; }

    public string? Credate { get; set; }

    public string? Upduser { get; set; }

    public string? Upddate { get; set; }
}

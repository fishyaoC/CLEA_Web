using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CLectorHistory
{
    public string HId { get; set; } = null!;

    public string? LId { get; set; }

    public string? HIndex { get; set; }

    public string? HHistroy { get; set; }

    public string? FMatchKey { get; set; }

    public string? Creuser { get; set; }

    public string? Credate { get; set; }

    public string? Upduser { get; set; }

    public string? Upddate { get; set; }
}

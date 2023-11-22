using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewBridgeCClass
{
    public string CId { get; set; } = null!;

    public string? CName { get; set; }

    public string? CType { get; set; }

    public string? CBookNum { get; set; }

    public string Creuser { get; set; } = null!;

    public DateTime Credate { get; set; }

    public string? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

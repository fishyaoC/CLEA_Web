using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewRole
{
    public Guid RUid { get; set; }

    public string RId { get; set; } = null!;

    public string RName { get; set; } = null!;

    public byte ROrder { get; set; }

    public bool RStatus { get; set; }

    public string? Creuser { get; set; }

    public DateTime Credate { get; set; }

    public string? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

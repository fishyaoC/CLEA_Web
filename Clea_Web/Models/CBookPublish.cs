using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CBookPublish
{
    public Guid BpId { get; set; }

    public string BpNumber { get; set; } = null!;

    public string BpName { get; set; } = null!;

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class PNewsReadLog
{
    /// <summary>
    /// PK
    /// </summary>
    public long Sn { get; set; }

    /// <summary>
    /// NEWS ID
    /// </summary>
    public Guid NewsId { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }

    public virtual PNews News { get; set; } = null!;
}

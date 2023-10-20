using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class EClassUploadLog
{
    public long Sn { get; set; }

    public Guid EdId { get; set; }

    public string FileFullName { get; set; } = null!;

    public bool IsUpdate { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

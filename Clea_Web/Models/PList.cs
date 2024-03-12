using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class PList
{
    public Guid Uid { get; set; }

    public Guid? LParentUid { get; set; }

    public string? LTitle { get; set; }

    public string? LMemo { get; set; }

    public int LOrder { get; set; }

    public bool LStatus { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

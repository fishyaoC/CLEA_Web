using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CLectorDateBook
{
    public string DId { get; set; } = null!;

    public string? LId { get; set; }

    public string? HIndex { get; set; }

    public string? HMemo { get; set; }

    public string? Creuser { get; set; }

    public string? Credate { get; set; }

    public string? Upduser { get; set; }

    public string? Upddate { get; set; }
}

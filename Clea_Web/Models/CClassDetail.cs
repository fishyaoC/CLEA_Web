using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CClassDetail
{
    public string DUid { get; set; } = null!;

    public string? CId { get; set; }

    public string? DId { get; set; }

    public string? DName { get; set; }

    public string? DHour { get; set; }

    public string? DHourlyRate { get; set; }

    public string? DType { get; set; }

    public string? DIsTest { get; set; }

    public string? DMemo { get; set; }

    public string? Creuser { get; set; }

    public string? Credate { get; set; }

    public string? Upduser { get; set; }

    public string? Upddate { get; set; }
}

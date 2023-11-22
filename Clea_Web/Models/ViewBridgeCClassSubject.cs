using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewBridgeCClassSubject
{
    public long Sn { get; set; }

    public string? CUid { get; set; }

    public string? DId { get; set; }

    public string? DName { get; set; }

    public decimal? DHour { get; set; }

    public decimal? DHourlyRate { get; set; }

    public string? DType { get; set; }

    public string? DIsTest { get; set; }

    public string? DMemo { get; set; }

    public string? Creuser { get; set; }

    public DateTime? Credate { get; set; }

    public string? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

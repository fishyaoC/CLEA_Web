using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewBridgeCClassLector
{
    public Guid ClUid { get; set; }

    public string? CUid { get; set; }

    public string? DUid { get; set; }

    public string? LUid { get; set; }

    public int? ClOrder { get; set; }

    public decimal? ClHourlyRate { get; set; }

    public string? ClQualify { get; set; }

    public string? ClIsActive { get; set; }

    public string? Creuser { get; set; }

    public DateTime? Credate { get; set; }

    public string? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

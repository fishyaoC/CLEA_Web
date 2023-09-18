using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CCourseDetail
{
    public string CdId { get; set; } = null!;

    public string? CId { get; set; }

    public string? CdIndex { get; set; }

    public string? CdClass { get; set; }

    public string? CdClassName { get; set; }

    public string? CdDate { get; set; }

    public string? CdStartTime { get; set; }

    public string? CdEndTime { get; set; }

    public string? CdType { get; set; }

    public string? CdIsTest { get; set; }

    public string? CdHour { get; set; }

    public string? CdLector { get; set; }

    public string? Creuser { get; set; }

    public string? Credate { get; set; }

    public string? Upduser { get; set; }

    public string? Upddate { get; set; }
}

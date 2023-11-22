using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewBridgeCBook
{
    public string MIndex { get; set; } = null!;

    public string? MName { get; set; }

    public string? MPublish { get; set; }

    public string? MNumber { get; set; }

    public string? MVersion { get; set; }

    public int? MOrder { get; set; }

    public string? MMemo { get; set; }

    public string? Creuser { get; set; }

    public DateTime? Credate { get; set; }

    public string? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

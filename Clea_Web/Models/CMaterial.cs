using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CMaterial
{
    public string MId { get; set; } = null!;

    public string? MIndex { get; set; }

    public string? MPublish { get; set; }

    public string? MNumber { get; set; }

    public string? MVersion { get; set; }

    public string? MOrder { get; set; }

    public string? MMemo { get; set; }

    public string? Creuser { get; set; }

    public string? Credate { get; set; }

    public string? Upduser { get; set; }

    public string? Upddate { get; set; }
}

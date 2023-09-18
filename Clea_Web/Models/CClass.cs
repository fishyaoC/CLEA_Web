using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CClass
{
    public string CId { get; set; } = null!;

    public string? CType { get; set; }

    public string? CName { get; set; }

    public string? COrder { get; set; }

    public string? CIsActive { get; set; }

    public string? CMemo { get; set; }

    public string? Creuser { get; set; }

    public string? Credate { get; set; }

    public string? Upduser { get; set; }

    public string? Upddate { get; set; }
}

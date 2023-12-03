using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewPLectorClass
{
    public Guid EId { get; set; }

    public string? CName { get; set; }

    public DateTime Credate { get; set; }

    public Guid EsId { get; set; }

    public Guid MatchKey { get; set; }

    public string? DName { get; set; }

    public int Status { get; set; }

    public int ScheNum { get; set; }

    public bool IsSche { get; set; }

    public bool IsClose { get; set; }

    public Guid? Reception { get; set; }

    public string? FileName { get; set; }
}

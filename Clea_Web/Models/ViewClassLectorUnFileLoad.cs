using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewClassLectorUnFileLoad
{
    public Guid EId { get; set; }

    public int EType { get; set; }

    public int EYear { get; set; }

    public Guid EdId { get; set; }

    public Guid MatchKey2 { get; set; }

    public Guid? FMatchKey { get; set; }

    public string? CName { get; set; }

    public string? DName { get; set; }

    public string? LName { get; set; }
}

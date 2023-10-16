using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class EEvaluate
{
    public Guid EId { get; set; }

    public int EType { get; set; }

    public int EYear { get; set; }

    public Guid MatchKey { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

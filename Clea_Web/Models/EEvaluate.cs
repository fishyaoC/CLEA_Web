using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class EEvaluate
{
    public Guid EId { get; set; }

    /// <summary>
    /// 0:課程 1:教材
    /// </summary>
    public int EType { get; set; }

    /// <summary>
    /// 評核年度
    /// </summary>
    public int EYear { get; set; }

    /// <summary>
    /// BOOK OR CLASS PK
    /// </summary>
    public Guid MatchKey { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

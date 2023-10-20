using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class EEvaluateDetail
{
    public Guid EdId { get; set; }

    public Guid EId { get; set; }

    public Guid MatchKey2 { get; set; }

    public Guid? Reception { get; set; }

    public Guid? Evaluate { get; set; }

    public int? EScoreA { get; set; }

    public int? EScoreB { get; set; }

    public int? EScoreC { get; set; }

    public int? EScoreD { get; set; }

    public int? EScoreE { get; set; }

    public string? ERemark { get; set; }

    public string? ETeachSyllabus { get; set; }

    public string? ETeachObject { get; set; }

    public string? ETeachAbstract { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

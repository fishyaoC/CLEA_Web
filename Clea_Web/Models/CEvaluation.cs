using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CEvaluation
{
    public Guid LevId { get; set; }

    /// <summary>
    /// 0:課程 1:教材
    /// </summary>
    public int LevType { get; set; }

    public int LevYear { get; set; }

    /// <summary>
    /// 被評鑑教師
    /// </summary>
    public Guid LUid { get; set; }

    /// <summary>
    /// 評鑑人ID
    /// </summary>
    public Guid LUidEv { get; set; }

    /// <summary>
    /// 開課ID
    /// </summary>
    public Guid CUid { get; set; }

    public Guid DUid { get; set; }

    /// <summary>
    /// 課程ID
    /// </summary>
    public Guid ClUid { get; set; }

    public int? ScoreA { get; set; }

    public int? ScoreB { get; set; }

    public int? ScoreC { get; set; }

    public int? ScoreD { get; set; }

    public int? ScoreE { get; set; }

    public int? ScoreF { get; set; }

    public int? ScoreG { get; set; }

    public int? ScroeH { get; set; }

    public int? ScoreI { get; set; }

    public int? ScoreJ { get; set; }

    public string? Remark { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

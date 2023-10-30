using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class EEvaluateDetail
{
    public Guid EdId { get; set; }

    public Guid EId { get; set; }

    /// <summary>
    /// CL OR BOOKDETAIL PK
    /// </summary>
    public Guid MatchKey2 { get; set; }

    /// <summary>
    /// 受評人員
    /// </summary>
    public Guid? Reception { get; set; }

    /// <summary>
    /// 評核人員
    /// </summary>
    public Guid? Evaluate { get; set; }

    public int? EScoreA { get; set; }

    public int? EScoreB { get; set; }

    public int? EScoreC { get; set; }

    public int? EScoreD { get; set; }

    public int? EScoreE { get; set; }

    /// <summary>
    /// 審核意見
    /// </summary>
    public string? ERemark { get; set; }

    /// <summary>
    /// 課程大綱
    /// </summary>
    public string? ETeachSyllabus { get; set; }

    /// <summary>
    /// 課程目標
    /// </summary>
    public string? ETeachObject { get; set; }

    /// <summary>
    /// 教學內容
    /// </summary>
    public string? ETeachAbstract { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

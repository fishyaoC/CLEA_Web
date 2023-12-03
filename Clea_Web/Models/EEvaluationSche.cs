using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class EEvaluationSche
{
    public Guid EsId { get; set; }

    public Guid EId { get; set; }

    public Guid MatchKey { get; set; }

    /// <summary>
    /// 講師
    /// </summary>
    public Guid? Reception { get; set; }

    public string? ETeachSyllabus { get; set; }

    public string? ETeachObject { get; set; }

    public string? ETeachAbstract { get; set; }

    public long ChkNum { get; set; }

    public int Status { get; set; }

    public bool IsMail { get; set; }

    /// <summary>
    /// 評鑑序
    /// </summary>
    public int ScheNum { get; set; }

    /// <summary>
    /// 最優先評鑑
    /// </summary>
    public bool IsSche { get; set; }

    public bool? IsPass { get; set; }

    /// <summary>
    /// 退回重評
    /// </summary>
    public bool IsClose { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

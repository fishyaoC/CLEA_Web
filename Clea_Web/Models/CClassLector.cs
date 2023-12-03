using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CClassLector
{
    public Guid ClUid { get; set; }

    /// <summary>
    /// 課程種類ID
    /// </summary>
    public Guid? CUid { get; set; }

    /// <summary>
    /// 課程明細ID
    /// </summary>
    public Guid? DUid { get; set; }

    /// <summary>
    /// 講師ID
    /// </summary>
    public Guid? LUid { get; set; }

    /// <summary>
    /// 編號
    /// </summary>
    public int? ClId { get; set; }

    /// <summary>
    /// 上課順序
    /// </summary>
    public int? ClOrder { get; set; }

    /// <summary>
    /// 授課鐘點費
    /// </summary>
    public int? ClHourlyRate { get; set; }

    /// <summary>
    /// 講師資格
    /// </summary>
    public string? ClQualify { get; set; }

    /// <summary>
    /// 是否停止授課
    /// </summary>
    public bool? ClIsActive { get; set; }

    public bool? IsEvaluate { get; set; }

    public Guid? Creuser { get; set; }

    public DateTime? Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

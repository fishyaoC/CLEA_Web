using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CClassSubject
{
    public Guid DUid { get; set; }

    public Guid? CUid { get; set; }

    /// <summary>
    /// 課程編號
    /// </summary>
    public string? DId { get; set; }

    /// <summary>
    /// 課程名稱
    /// </summary>
    public string? DName { get; set; }

    /// <summary>
    /// 時數
    /// </summary>
    public int? DHour { get; set; }

    /// <summary>
    /// 鐘點費
    /// </summary>
    public int? DHourlyRate { get; set; }

    /// <summary>
    /// 學/術科
    /// </summary>
    public string? DType { get; set; }

    /// <summary>
    /// 是否測驗
    /// </summary>
    public bool? DIsTest { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string? DMemo { get; set; }

    public Guid? Creuser { get; set; }

    public DateTime? Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

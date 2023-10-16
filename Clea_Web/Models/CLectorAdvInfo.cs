using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CLectorAdvInfo
{
    public Guid LaUid { get; set; }

    /// <summary>
    /// 講師UID
    /// </summary>
    public Guid? LUid { get; set; }

    /// <summary>
    /// 進修年度
    /// </summary>
    public int? LaYear { get; set; }

    /// <summary>
    /// 標題
    /// </summary>
    public string? LaTitle { get; set; }

    /// <summary>
    /// 建立者
    /// </summary>
    public Guid Creuser { get; set; }

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime Credate { get; set; }

    /// <summary>
    /// 更新者
    /// </summary>
    public Guid? Upduser { get; set; }

    /// <summary>
    /// 更新時間
    /// </summary>
    public DateTime? Upddate { get; set; }
}

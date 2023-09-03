using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class SysMenu
{
    /// <summary>
    /// 選單ID
    /// </summary>
    public long MId { get; set; }

    /// <summary>
    /// 選單名稱
    /// </summary>
    public string MName { get; set; } = null!;

    public long MParentId { get; set; }

    /// <summary>
    /// 選單層級
    /// </summary>
    public int MLevel { get; set; }

    public string? MUrl { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int MOrder { get; set; }

    /// <summary>
    /// 啟用狀態
    /// </summary>
    public bool MIsActice { get; set; }

    /// <summary>
    /// 前台顯示狀態
    /// </summary>
    public bool MIsShow { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

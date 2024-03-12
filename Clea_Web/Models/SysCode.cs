using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class SysCode
{
    public Guid Uid { get; set; }

    /// <summary>
    /// 父層Uid
    /// </summary>
    public Guid? CParentUid { get; set; }

    /// <summary>
    /// 父層代號
    /// </summary>
    public string? CParentCode { get; set; }

    /// <summary>
    /// 代號
    /// </summary>
    public string CItemCode { get; set; } = null!;

    /// <summary>
    /// 代碼名稱
    /// </summary>
    public string CItemName { get; set; } = null!;

    /// <summary>
    /// 排序
    /// </summary>
    public int CItemOrder { get; set; }

    /// <summary>
    /// 開啟狀態
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// 顯示狀態
    /// </summary>
    public bool? IsShow { get; set; }

    /// <summary>
    /// 是否可在後台編輯
    /// </summary>
    public bool? IsEdit { get; set; }

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

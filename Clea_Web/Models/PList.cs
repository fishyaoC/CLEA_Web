using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class PList
{
    public Guid Uid { get; set; }

    /// <summary>
    /// 功能模組
    /// </summary>
    public int? LType { get; set; }

    /// <summary>
    /// 子項目UID
    /// </summary>
    public Guid? LParentUid { get; set; }

    /// <summary>
    /// 標題
    /// </summary>
    public string? LTitle { get; set; }

    /// <summary>
    /// 說明
    /// </summary>
    public string? LMemo { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int LOrder { get; set; }

    /// <summary>
    /// 是否上架
    /// </summary>
    public bool LStatus { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

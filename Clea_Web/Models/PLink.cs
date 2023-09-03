using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class PLink
{
    public Guid LinkId { get; set; }

    /// <summary>
    /// 功能模組代碼
    /// </summary>
    public int LType { get; set; }

    /// <summary>
    /// 連結分類
    /// </summary>
    public int LClass { get; set; }

    /// <summary>
    /// 標題名稱
    /// </summary>
    public string LTitle { get; set; } = null!;

    /// <summary>
    /// 連結網址
    /// </summary>
    public string LUrl { get; set; } = null!;

    /// <summary>
    /// 排序
    /// </summary>
    public int? LOrder { get; set; }

    /// <summary>
    /// 是否置頂
    /// </summary>
    public bool LIsTop { get; set; }

    /// <summary>
    /// 啟用狀態
    /// </summary>
    public bool LStatus { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

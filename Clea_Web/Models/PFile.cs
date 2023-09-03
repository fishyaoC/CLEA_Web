using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class PFile
{
    public Guid FileId { get; set; }

    /// <summary>
    /// 檔案模組代碼
    /// </summary>
    public int FType { get; set; }

    /// <summary>
    /// 標題名稱
    /// </summary>
    public string FTitle { get; set; } = null!;

    /// <summary>
    /// 備註
    /// </summary>
    public string? FMemo { get; set; }

    /// <summary>
    /// 啟用狀態
    /// </summary>
    public bool FStatus { get; set; }

    /// <summary>
    /// 是否置頂
    /// </summary>
    public bool FIsTop { get; set; }

    public int? FOrder { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

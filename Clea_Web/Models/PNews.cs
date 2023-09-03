using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class PNews
{
    public Guid NewsId { get; set; }

    /// <summary>
    /// 功能模組代碼
    /// </summary>
    public int NType { get; set; }

    /// <summary>
    /// 標題名稱
    /// </summary>
    public string NTitle { get; set; } = null!;

    /// <summary>
    /// 分類
    /// </summary>
    public int NClass { get; set; }

    /// <summary>
    /// 起始日
    /// </summary>
    public DateTime? NStartDate { get; set; }

    /// <summary>
    /// 結束日
    /// </summary>
    public DateTime? NEndDate { get; set; }

    /// <summary>
    /// 是否置頂
    /// </summary>
    public bool NIsTop { get; set; }

    /// <summary>
    /// 前台是否顯示
    /// </summary>
    public bool NIsShow { get; set; }

    /// <summary>
    /// 啟用狀態
    /// </summary>
    public bool NStatus { get; set; }

    /// <summary>
    /// 內文
    /// </summary>
    public string? NContent { get; set; }

    /// <summary>
    /// 角色代碼
    /// </summary>
    public string RId { get; set; } = null!;

    public Guid Creuser { get; set; }

    public DateTime Curdate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }

    public virtual ICollection<PNewsReadLog> PNewsReadLogs { get; set; } = new List<PNewsReadLog>();

    public virtual SysRole RIdNavigation { get; set; } = null!;
}

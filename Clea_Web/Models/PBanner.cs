using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class PBanner
{
    public Guid BannerId { get; set; }

    /// <summary>
    /// 模組型別 0:首頁輪播
    /// </summary>
    public int BannerType { get; set; }

    /// <summary>
    /// 圖片名稱
    /// </summary>
    public string BannerName { get; set; } = null!;

    /// <summary>
    /// 連結
    /// </summary>
    public string? BannerUrl { get; set; }

    /// <summary>
    /// 輪播啟用日
    /// </summary>
    public DateTime? BannerStart { get; set; }

    /// <summary>
    /// 輪播結束日
    /// </summary>
    public DateTime? BannerEnd { get; set; }

    public int? BannerOrder { get; set; }

    /// <summary>
    /// 啟用狀態
    /// </summary>
    public bool BannerStatus { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

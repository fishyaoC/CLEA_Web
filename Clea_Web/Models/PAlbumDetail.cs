using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class PAlbumDetail
{
    public Guid? AdId { get; set; }

    public Guid? AlbumId { get; set; }

    public string? AdMemo { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int AdOrder { get; set; }

    /// <summary>
    /// 是否置頂
    /// </summary>
    public bool? AdIsTop { get; set; }

    /// <summary>
    /// 啟用狀態
    /// </summary>
    public bool AdStatus { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

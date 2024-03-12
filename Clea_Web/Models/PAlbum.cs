using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class PAlbum
{
    public Guid? AlbumId { get; set; }

    public string AName { get; set; } = null!;

    public string? AMemo { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int AOrder { get; set; }

    /// <summary>
    /// 是否置頂
    /// </summary>
    public bool? AIsTop { get; set; }

    /// <summary>
    /// 啟用狀態
    /// </summary>
    public bool AStatus { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

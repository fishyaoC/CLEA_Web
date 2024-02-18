using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class PNav
{
    public Guid Uid { get; set; }

    /// <summary>
    /// 標題
    /// </summary>
    public string NTitle { get; set; } = null!;

    /// <summary>
    /// 住址
    /// </summary>
    public string NAddress { get; set; } = null!;

    /// <summary>
    /// 電話
    /// </summary>
    public string NPhone { get; set; } = null!;

    /// <summary>
    /// 傳真
    /// </summary>
    public string NFax { get; set; } = null!;

    /// <summary>
    /// googleMap
    /// </summary>
    public string NEmbed { get; set; } = null!;

    /// <summary>
    /// 介紹
    /// </summary>
    public string? NMemo { get; set; }

    public int NOrder { get; set; }

    /// <summary>
    /// 是否上架
    /// </summary>
    public bool NStatus { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

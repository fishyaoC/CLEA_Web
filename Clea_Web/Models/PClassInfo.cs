using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class PClassInfo
{
    public Guid Uid { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string CName { get; set; } = null!;

    /// <summary>
    /// 負責業務
    /// </summary>
    public string CWork { get; set; } = null!;

    /// <summary>
    /// 聯絡方式
    /// </summary>
    public string CPhone { get; set; } = null!;

    /// <summary>
    /// 負責地區
    /// </summary>
    public string CWorkPlace { get; set; } = null!;

    /// <summary>
    /// LINE好友連結
    /// </summary>
    public string CLinelink { get; set; } = null!;

    /// <summary>
    /// 是否上架
    /// </summary>
    public bool CStatus { get; set; }

    public int COrder { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

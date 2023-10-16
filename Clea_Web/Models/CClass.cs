using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CClass
{
    public Guid CUid { get; set; }

    /// <summary>
    /// 課程ID
    /// </summary>
    public string? CId { get; set; }

    /// <summary>
    /// 課程名稱
    /// </summary>
    public string? CName { get; set; }

    /// <summary>
    /// 課程類別
    /// </summary>
    public string? CType { get; set; }

    /// <summary>
    /// 教材編號
    /// </summary>
    public string? CBookNum { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

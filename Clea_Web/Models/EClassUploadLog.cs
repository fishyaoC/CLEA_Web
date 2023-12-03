using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class EClassUploadLog
{
    public long Sn { get; set; }

    /// <summary>
    /// 評核PK
    /// </summary>
    public Guid EsId { get; set; }

    /// <summary>
    /// 檔案全名
    /// </summary>
    public string FileFullName { get; set; } = null!;

    /// <summary>
    /// 是否為重大更新
    /// </summary>
    public bool IsUpdate { get; set; }

    public int? Status { get; set; }

    public string? Other { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

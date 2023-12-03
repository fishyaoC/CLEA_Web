using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CBook
{
    public Guid MId { get; set; }

    /// <summary>
    /// 教材編號
    /// </summary>
    public string MIndex { get; set; } = null!;

    /// <summary>
    /// 教材名稱
    /// </summary>
    public string MName { get; set; } = null!;

    /// <summary>
    /// 優先順序
    /// </summary>
    public int? MOrder { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

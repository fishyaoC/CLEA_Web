using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class SysUnit
{
    /// <summary>
    /// 單位代碼
    /// </summary>
    public string UnId { get; set; } = null!;

    /// <summary>
    /// 單位名稱
    /// </summary>
    public string UnName { get; set; } = null!;

    /// <summary>
    /// 單位狀態
    /// </summary>
    public bool UnStatus { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }

    public virtual ICollection<SysUser> SysUsers { get; set; } = new List<SysUser>();
}

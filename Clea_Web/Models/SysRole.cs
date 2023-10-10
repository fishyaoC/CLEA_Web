using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class SysRole
{
    public Guid RUid { get; set; }

    /// <summary>
    /// 角色代碼
    /// </summary>
    public string RId { get; set; } = null!;

    /// <summary>
    /// 是否為後台帳號
    /// </summary>
    public bool RBackEnd { get; set; }

    /// <summary>
    /// 角色名稱
    /// </summary>
    public string RName { get; set; } = null!;

    /// <summary>
    /// 角色排序
    /// </summary>
    public byte ROrder { get; set; }

    /// <summary>
    /// 角色啟用狀態
    /// </summary>
    public bool RStatus { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }

    public virtual ICollection<SysPower> SysPowers { get; set; } = new List<SysPower>();

    public virtual ICollection<SysUser> SysUsers { get; set; } = new List<SysUser>();
}

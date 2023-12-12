using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class SysUser
{
    /// <summary>
    /// PK
    /// </summary>
    public Guid UId { get; set; }

    /// <summary>
    /// 角色UID
    /// </summary>
    public Guid RUid { get; set; }

    /// <summary>
    /// 帳號
    /// </summary>
    public string UAccount { get; set; } = null!;

    /// <summary>
    /// 密碼
    /// </summary>
    public string UPassword { get; set; } = null!;

    /// <summary>
    /// 使用者名稱
    /// </summary>
    public string UName { get; set; } = null!;

    /// <summary>
    /// 使用者電子郵件
    /// </summary>
    public string UEmail { get; set; } = null!;

    /// <summary>
    /// 使用者連絡電話
    /// </summary>
    public string UPhone { get; set; } = null!;

    /// <summary>
    /// 使用者地址
    /// </summary>
    public string? UAddress { get; set; }

    /// <summary>
    /// 使用者性別:0女、1男
    /// </summary>
    public byte? USex { get; set; }

    /// <summary>
    /// 使用者生日
    /// </summary>
    public DateTime? UBirthday { get; set; }

    /// <summary>
    /// 使用者單位
    /// </summary>
    public string? UnId { get; set; }

    /// <summary>
    /// 狀態
    /// </summary>
    public bool UStatus { get; set; }

    public bool IsOutSide { get; set; }

    /// <summary>
    /// 建立者
    /// </summary>
    public Guid Creuser { get; set; }

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime Credate { get; set; }

    /// <summary>
    /// 更新者
    /// </summary>
    public Guid? Upduser { get; set; }

    /// <summary>
    /// 更新時間
    /// </summary>
    public DateTime? Upddate { get; set; }

    public virtual SysRole RU { get; set; } = null!;
}

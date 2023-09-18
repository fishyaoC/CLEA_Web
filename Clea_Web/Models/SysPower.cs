using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class SysPower
{
    /// <summary>
    /// PK
    /// </summary>
    public long Sn { get; set; }

    /// <summary>
    /// 角色代碼UID
    /// </summary>
    public Guid RUid { get; set; }

    /// <summary>
    /// 頁面代碼
    /// </summary>
    public long MId { get; set; }

    /// <summary>
    /// 新增資料
    /// </summary>
    public bool CreateData { get; set; }

    /// <summary>
    /// 查詢資料
    /// </summary>
    public bool SearchData { get; set; }

    /// <summary>
    /// 編輯資料
    /// </summary>
    public bool ModifyData { get; set; }

    /// <summary>
    /// 刪除資料
    /// </summary>
    public bool DeleteData { get; set; }

    /// <summary>
    /// 匯入資料
    /// </summary>
    public bool ImportData { get; set; }

    /// <summary>
    /// 匯出資料
    /// </summary>
    public bool Exportdata { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }

    public virtual SysRole RU { get; set; } = null!;
}

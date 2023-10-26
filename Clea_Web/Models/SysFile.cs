using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class SysFile
{
    /// <summary>
    /// 檔案ID
    /// </summary>
    public Guid FileId { get; set; }

    /// <summary>
    /// 功能模組代碼
    /// </summary>
    public string FModule { get; set; } = null!;

    /// <summary>
    /// 功能主KEY
    /// </summary>
    public Guid FMatchKey { get; set; }

    public string? FMatchKey2 { get; set; }

    /// <summary>
    /// 檔案全名
    /// </summary>
    public string FFullName { get; set; } = null!;

    /// <summary>
    /// 檔案實體名稱
    /// </summary>
    public string FNameReal { get; set; } = null!;

    /// <summary>
    /// 儲存後名稱
    /// </summary>
    public string FNameDl { get; set; } = null!;

    /// <summary>
    /// 檔案類型
    /// </summary>
    public string FMimeType { get; set; } = null!;

    /// <summary>
    /// 副檔名
    /// </summary>
    public string FExt { get; set; } = null!;

    /// <summary>
    /// 檔案路徑
    /// </summary>
    public string FPath { get; set; } = null!;

    /// <summary>
    /// 檔案描述
    /// </summary>
    public string? FDescription { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int? FOrder { get; set; }

    /// <summary>
    /// 檔案備註
    /// </summary>
    public string? FRemark { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

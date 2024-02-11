using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class PCompanyCv
{
    public Guid CvUid { get; set; }

    /// <summary>
    /// 工作地點
    /// </summary>
    public string CvPlace { get; set; } = null!;

    /// <summary>
    /// 徵才人數
    /// </summary>
    public string CvNum { get; set; } = null!;

    /// <summary>
    /// 薪資待遇
    /// </summary>
    public string CvPay { get; set; } = null!;

    /// <summary>
    /// 求才機構(公司名稱)
    /// </summary>
    public string CvCompanyName { get; set; } = null!;

    /// <summary>
    /// 聯絡人
    /// </summary>
    public string CvContact { get; set; } = null!;

    /// <summary>
    /// 電話
    /// </summary>
    public string CvPhone { get; set; } = null!;

    /// <summary>
    /// 地址
    /// </summary>
    public string CvAddress { get; set; } = null!;

    /// <summary>
    /// 電子信箱
    /// </summary>
    public string CvEmail { get; set; } = null!;

    /// <summary>
    /// 應徵方式
    /// </summary>
    public string CvWay { get; set; } = null!;

    /// <summary>
    /// 應徵內容
    /// </summary>
    public string CvContent { get; set; } = null!;

    /// <summary>
    /// 職務要求
    /// </summary>
    public string CvRequire { get; set; } = null!;

    /// <summary>
    /// 核准狀態
    /// </summary>
    public bool IsApprove { get; set; }

    /// <summary>
    /// 核准備註
    /// </summary>
    public string? ApproveMemo { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

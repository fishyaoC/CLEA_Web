using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CLector
{
    public Guid LUid { get; set; }

    /// <summary>
    /// 身分證
    /// </summary>
    public string? LId { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string? LName { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    public DateTime? LBrithday { get; set; }

    /// <summary>
    /// 學歷
    /// </summary>
    public string? LEdu { get; set; }

    /// <summary>
    /// 畢業學校
    /// </summary>
    public string? LEduSchool { get; set; }

    /// <summary>
    /// 聯絡地址郵遞區號
    /// </summary>
    public int? LPosCode { get; set; }

    /// <summary>
    /// 聯絡地址
    /// </summary>
    public string? LAddress { get; set; }

    /// <summary>
    /// 戶籍地址郵遞區號
    /// </summary>
    public int? LCposCode { get; set; }

    /// <summary>
    /// 戶籍地址
    /// </summary>
    public string? LCaddress { get; set; }

    /// <summary>
    /// 聯絡電話
    /// </summary>
    public string? LPhone { get; set; }

    /// <summary>
    /// 行動電話
    /// </summary>
    public string? LCellPhone { get; set; }

    /// <summary>
    /// 電子郵件
    /// </summary>
    public string? LEmail { get; set; }

    /// <summary>
    /// 講師類別
    /// </summary>
    public string? LType { get; set; }

    /// <summary>
    /// 差旅費
    /// </summary>
    public int? LTravelExpenses { get; set; }

    /// <summary>
    /// 總會是否核准
    /// </summary>
    public bool? LIsCheck { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    public bool? LActive { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string? LMemo { get; set; }

    public Guid? Creuser { get; set; }

    public DateTime? Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

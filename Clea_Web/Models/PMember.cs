using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class PMember
{
    public Guid Uid { get; set; }

    /// <summary>
    /// 姓名/公司名稱
    /// </summary>
    public string? MName { get; set; }

    /// <summary>
    /// 身分證/統一編號
    /// </summary>
    public string? MId { get; set; }

    /// <summary>
    /// 密碼
    /// </summary>
    public string? MPassword { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    public DateTime? MBrithday { get; set; }

    /// <summary>
    /// 畢業學校
    /// </summary>
    public string? MGraduatedSchool { get; set; }

    /// <summary>
    /// 廠商：聯絡人
    /// </summary>
    public string? MContact { get; set; }

    /// <summary>
    /// 連絡電話(市話)/連絡電話(市話)
    /// </summary>
    public string? MPhone { get; set; }

    /// <summary>
    /// 手機號碼/傳真
    /// </summary>
    public string? MCellPhone { get; set; }

    /// <summary>
    /// 戶籍住址/聯絡地址
    /// </summary>
    public string? MAddress { get; set; }

    /// <summary>
    /// 服務單位
    /// </summary>
    public string? MWorkPlace { get; set; }

    /// <summary>
    /// 電子郵件/電子郵件
    /// </summary>
    public string? MEmail { get; set; }

    /// <summary>
    /// LineID
    /// </summary>
    public string? MLineId { get; set; }

    /// <summary>
    /// 性別
    /// </summary>
    public byte? MSex { get; set; }

    /// <summary>
    /// 會員等級
    /// </summary>
    public string? MLevel { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    public bool? MStatus { get; set; }

    /// <summary>
    /// 會員/廠商
    /// </summary>
    public int? MType { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

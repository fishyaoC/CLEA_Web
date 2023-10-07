using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CBook
{
    public Guid MId { get; set; }

    /// <summary>
    /// 教材編號
    /// </summary>
    public int? MIndex { get; set; }

    /// <summary>
    /// 教材名稱
    /// </summary>
    public string? MName { get; set; }

    /// <summary>
    /// 出版者
    /// </summary>
    public string? MPublish { get; set; }

    /// <summary>
    /// 證號
    /// </summary>
    public string? MNumber { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public DateTime? MVersion { get; set; }

    /// <summary>
    /// 優先順序
    /// </summary>
    public int? MOrder { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string? MMemo { get; set; }

    public Guid? Creuser { get; set; }

    public DateTime? Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

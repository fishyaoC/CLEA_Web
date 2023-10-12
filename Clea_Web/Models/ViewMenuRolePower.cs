using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewMenuRolePower
{
    public Guid RUid { get; set; }

    public long? MId { get; set; }

    public string? MName { get; set; }

    public int? MLevel { get; set; }

    public int? MOrder { get; set; }

    public string? MType { get; set; }

    public string? MUrl { get; set; }

    public bool CreateData { get; set; }

    public bool SearchData { get; set; }

    public bool ModifyData { get; set; }

    public bool DeleteData { get; set; }

    public bool ImportData { get; set; }

    public bool Exportdata { get; set; }
}

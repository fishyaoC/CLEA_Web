using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewRolePower
{
    public string RId { get; set; } = null!;

    public string RName { get; set; } = null!;

    public byte ROrder { get; set; }

    public bool RStatus { get; set; }

    public long? Sn { get; set; }

    public long? MId { get; set; }

    public bool? CreateData { get; set; }

    public bool? SearchData { get; set; }

    public bool? ModifyData { get; set; }

    public bool? DeleteData { get; set; }

    public bool? ImportData { get; set; }

    public bool? Exportdata { get; set; }

    public string Creuser { get; set; }

    public DateTime Credate { get; set; }

    public string? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

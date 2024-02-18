using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class PClassMain
{
    public Guid ClassMainId { get; set; }

    public string CmClassName { get; set; } = null!;

    public string CmClassCode { get; set; } = null!;

    public int CmPlace { get; set; }

    public bool CmStatus { get; set; }

    public Guid Creuser { get; set; }

    public DateTime Credate { get; set; }

    public Guid? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

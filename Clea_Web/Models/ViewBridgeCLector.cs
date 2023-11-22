using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewBridgeCLector
{
    public string LId { get; set; } = null!;

    public string? LName { get; set; }

    public DateTime? LBrithday { get; set; }

    public string? LEdu { get; set; }

    public string? LEduSchool { get; set; }

    public string? LPosCode { get; set; }

    public string? LAddress { get; set; }

    public string? LCposCode { get; set; }

    public string? LCaddress { get; set; }

    public string? LPhone { get; set; }

    public string? LCellPhone { get; set; }

    public string? LEmail { get; set; }

    public string? LType { get; set; }

    public int? LTravelExpenses { get; set; }

    public string? LIsCheck { get; set; }

    public string? LActive { get; set; }

    public string? LMemo { get; set; }

    public string? Creuser { get; set; }

    public DateTime? Credate { get; set; }

    public string? Upduser { get; set; }

    public DateTime? Upddate { get; set; }
}

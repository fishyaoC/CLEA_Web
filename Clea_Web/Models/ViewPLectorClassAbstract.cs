using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewPLectorClassAbstract
{
    public Guid EId { get; set; }

    public Guid Am { get; set; }

    public Guid EsId { get; set; }

    public Guid Bm { get; set; }

    public int Status { get; set; }

    public bool IsClose { get; set; }

    public string? ETeachAbstract { get; set; }

    public string? ETeachObject { get; set; }

    public string? ETeachSyllabus { get; set; }

    public Guid? DUid { get; set; }

    public string? DName { get; set; }

    public string? CName { get; set; }

    public string? LName { get; set; }
}

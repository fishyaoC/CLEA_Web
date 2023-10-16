using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class ViewClassLectorEvaluate
{
    public Guid ClUid { get; set; }

    public Guid? CUid { get; set; }

    public string? CId { get; set; }

    public string? CName { get; set; }

    public Guid? DUid { get; set; }

    public string? DId { get; set; }

    public string? DName { get; set; }

    public Guid? LUid { get; set; }

    public string? LId { get; set; }

    public string? LName { get; set; }

    public Guid? Expr1 { get; set; }

    public string? Expr2 { get; set; }

    public string? Expr3 { get; set; }

    public int? LevYear { get; set; }

    public int? LevType { get; set; }
}

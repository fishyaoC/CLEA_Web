using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CTrainingCourse
{
    public string TId { get; set; } = null!;

    public string? CId { get; set; }

    public string? TIndex { get; set; }

    public string? TName { get; set; }

    public string? TDate { get; set; }

    public string? TTimeS { get; set; }

    public string? TTimeE { get; set; }

    public string? TType { get; set; }

    public string? TTest { get; set; }

    public string? THour { get; set; }

    public string? TTeacher1 { get; set; }

    public string? TTeacher2 { get; set; }

    public string? TTeacher3 { get; set; }

    public string? TTeacher4 { get; set; }

    public string? Creuser { get; set; }

    public string? Credate { get; set; }

    public string? Upduser { get; set; }

    public string? Upddate { get; set; }
}

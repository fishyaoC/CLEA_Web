using System;
using System.Collections.Generic;

namespace Clea_Web.Models;

public partial class CCourseMain
{
    public Guid CId { get; set; }

    /// <summary>
    /// 內部編號
    /// </summary>
    public int? CInum { get; set; }

    public string? CClassCode { get; set; }

    public string? CBookNum { get; set; }

    public string? CClassDateS { get; set; }

    public string? CClassDateE { get; set; }

    public string? CPhase { get; set; }

    public string? CSubjectP { get; set; }

    public string? CSurgeryP { get; set; }

    public string? CTestSchedule { get; set; }

    public string? CNumOfPeople { get; set; }

    public string? CIsShow { get; set; }

    public string? CReportingAuthority { get; set; }

    public string? COpenNum { get; set; }

    public string? COpenDate { get; set; }

    public string? CApprovalNum { get; set; }

    public string? CApprovalDate { get; set; }

    public string? CAmount { get; set; }

    public string? CHeaderCheck { get; set; }

    public string? CBreakTime { get; set; }

    public string? CReportingDate { get; set; }

    public string? CUnderTaker { get; set; }

    public string? CHour { get; set; }

    public string? CMemo { get; set; }

    public string? CIsWebSignUp { get; set; }

    public string? CWebSignUpDeadLine { get; set; }

    public string? CStatus { get; set; }

    public string? Creuser { get; set; }

    public string? Credate { get; set; }

    public string? Upduser { get; set; }

    public string? Upddate { get; set; }
}

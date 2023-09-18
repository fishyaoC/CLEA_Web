using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Clea_Web.Models;

public partial class DbContextCLEA : DbContext
{
    public DbContextCLEA()
    {
    }

    public DbContextCLEA(DbContextOptions<DbContextCLEA> options)
        : base(options)
    {
    }

    public virtual DbSet<CClass> CClasses { get; set; }

    public virtual DbSet<CClassDetail> CClassDetails { get; set; }

    public virtual DbSet<CClassLector> CClassLectors { get; set; }

    public virtual DbSet<CCourseDetail> CCourseDetails { get; set; }

    public virtual DbSet<CCourseMain> CCourseMains { get; set; }

    public virtual DbSet<CLector> CLectors { get; set; }

    public virtual DbSet<CLectorDateBook> CLectorDateBooks { get; set; }

    public virtual DbSet<CLectorHistory> CLectorHistories { get; set; }

    public virtual DbSet<CLectorSkill> CLectorSkills { get; set; }

    public virtual DbSet<CMaterial> CMaterials { get; set; }

    public virtual DbSet<CTrainingCourse> CTrainingCourses { get; set; }

    public virtual DbSet<PBanner> PBanners { get; set; }

    public virtual DbSet<PFile> PFiles { get; set; }

    public virtual DbSet<PLink> PLinks { get; set; }

    public virtual DbSet<PNews> PNews { get; set; }

    public virtual DbSet<PNewsReadLog> PNewsReadLogs { get; set; }

    public virtual DbSet<SysFile> SysFiles { get; set; }

    public virtual DbSet<SysMenu> SysMenus { get; set; }

    public virtual DbSet<SysPower> SysPowers { get; set; }

    public virtual DbSet<SysRole> SysRoles { get; set; }

    public virtual DbSet<SysUnit> SysUnits { get; set; }

    public virtual DbSet<SysUser> SysUsers { get; set; }

    public virtual DbSet<ViewMenuRolePower> ViewMenuRolePowers { get; set; }

    public virtual DbSet<ViewRole> ViewRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=210.61.116.244;Database=CELA_WEBDB;Persist Security Info=True;TrustServerCertificate=true;User ID=clea_Web;Password=!qaz2wsX3edc");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CClass>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("C_Class");

            entity.Property(e => e.CId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_ID");
            entity.Property(e => e.CIsActive)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_IsActive");
            entity.Property(e => e.CMemo)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_Memo");
            entity.Property(e => e.CName)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_Name");
            entity.Property(e => e.COrder)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_Order");
            entity.Property(e => e.CType)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_Type");
            entity.Property(e => e.Credate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREUSER");
            entity.Property(e => e.Upddate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<CClassDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("C_ClassDetail");

            entity.Property(e => e.CId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_ID");
            entity.Property(e => e.Credate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREUSER");
            entity.Property(e => e.DHour)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("D_Hour");
            entity.Property(e => e.DHourlyRate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("D_HourlyRate");
            entity.Property(e => e.DId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("D_ID");
            entity.Property(e => e.DIsTest)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("D_IsTest");
            entity.Property(e => e.DMemo)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("D_Memo");
            entity.Property(e => e.DName)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("D_Name");
            entity.Property(e => e.DType)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("D_Type");
            entity.Property(e => e.DUid)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("D_UID");
            entity.Property(e => e.Upddate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<CClassLector>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("C_ClassLector");

            entity.Property(e => e.CId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_ID");
            entity.Property(e => e.ClHourlyRate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CL_HourlyRate");
            entity.Property(e => e.ClId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CL_ID");
            entity.Property(e => e.ClIndex)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CL_Index");
            entity.Property(e => e.ClIsActive)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CL_IsActive");
            entity.Property(e => e.ClOrder)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CL_Order");
            entity.Property(e => e.ClQualify)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CL_Qualify");
            entity.Property(e => e.Credate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREUSER");
            entity.Property(e => e.LId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_ID");
            entity.Property(e => e.Upddate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<CCourseDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("C_CourseDetail");

            entity.Property(e => e.CId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_ID");
            entity.Property(e => e.CdClass)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CD_Class");
            entity.Property(e => e.CdClassName)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CD_ClassName");
            entity.Property(e => e.CdDate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CD_Date");
            entity.Property(e => e.CdEndTime)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CD_EndTime");
            entity.Property(e => e.CdHour)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CD_Hour");
            entity.Property(e => e.CdId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CD_ID");
            entity.Property(e => e.CdIndex)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CD_Index");
            entity.Property(e => e.CdIsTest)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CD_IsTest");
            entity.Property(e => e.CdLector)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CD_Lector");
            entity.Property(e => e.CdStartTime)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CD_StartTime");
            entity.Property(e => e.CdType)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CD_Type");
            entity.Property(e => e.Credate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREUSER");
            entity.Property(e => e.Upddate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<CCourseMain>(entity =>
        {
            entity.HasKey(e => e.CId).HasName("PK_P_CourseMain");

            entity.ToTable("C_CourseMain");

            entity.Property(e => e.CId)
                .ValueGeneratedNever()
                .HasColumnName("C_ID");
            entity.Property(e => e.CAmount)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_Amount");
            entity.Property(e => e.CApprovalDate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_ApprovalDate");
            entity.Property(e => e.CApprovalNum)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_ApprovalNum");
            entity.Property(e => e.CBookNum)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_BookNum");
            entity.Property(e => e.CBreakTime)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_BreakTime");
            entity.Property(e => e.CClassCode)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_ClassCode");
            entity.Property(e => e.CClassDateE)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_ClassDateE");
            entity.Property(e => e.CClassDateS)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_ClassDateS");
            entity.Property(e => e.CHeaderCheck)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_HeaderCheck");
            entity.Property(e => e.CHour)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_Hour");
            entity.Property(e => e.CInum)
                .HasComment("內部編號")
                .HasColumnName("C_INum");
            entity.Property(e => e.CIsShow)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_IsShow");
            entity.Property(e => e.CIsWebSignUp)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_IsWebSignUp");
            entity.Property(e => e.CMemo)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_Memo");
            entity.Property(e => e.CNumOfPeople)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_NumOfPeople");
            entity.Property(e => e.COpenDate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_OpenDate");
            entity.Property(e => e.COpenNum)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_OpenNum");
            entity.Property(e => e.CPhase)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_Phase");
            entity.Property(e => e.CReportingAuthority)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_ReportingAuthority");
            entity.Property(e => e.CReportingDate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_ReportingDate");
            entity.Property(e => e.CStatus)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_Status");
            entity.Property(e => e.CSubjectP)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_SubjectP");
            entity.Property(e => e.CSurgeryP)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_SurgeryP");
            entity.Property(e => e.CTestSchedule)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_TestSchedule");
            entity.Property(e => e.CUnderTaker)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_UnderTaker");
            entity.Property(e => e.CWebSignUpDeadLine)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_WebSignUpDeadLine");
            entity.Property(e => e.Credate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREUSER");
            entity.Property(e => e.Upddate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<CLector>(entity =>
        {
            entity.HasKey(e => e.LUid);

            entity.ToTable("C_Lector");

            entity.Property(e => e.LUid)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_UID");
            entity.Property(e => e.Credate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREUSER");
            entity.Property(e => e.LActive)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_Active");
            entity.Property(e => e.LAddress)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_Address");
            entity.Property(e => e.LBrithday)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_BRITHDAY");
            entity.Property(e => e.LCaddress)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_CAddress");
            entity.Property(e => e.LCellPhone)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_CellPhone");
            entity.Property(e => e.LCposCode)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_CPosCode");
            entity.Property(e => e.LEdu)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_Edu");
            entity.Property(e => e.LEduSchool)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_EduSchool");
            entity.Property(e => e.LEmail)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_Email");
            entity.Property(e => e.LId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_ID");
            entity.Property(e => e.LIsCheck)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_IsCheck");
            entity.Property(e => e.LMemo)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_Memo");
            entity.Property(e => e.LName)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_NAME");
            entity.Property(e => e.LPhone)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_Phone");
            entity.Property(e => e.LPosCode)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_PosCode");
            entity.Property(e => e.LTravelExpenses)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_TravelExpenses");
            entity.Property(e => e.LType)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_Type");
            entity.Property(e => e.Upddate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<CLectorDateBook>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("C_LectorDateBook");

            entity.Property(e => e.Credate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREUSER");
            entity.Property(e => e.DId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("D_ID");
            entity.Property(e => e.HIndex)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("H_Index");
            entity.Property(e => e.HMemo)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("H_Memo");
            entity.Property(e => e.LId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_ID");
            entity.Property(e => e.Upddate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<CLectorHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("C_LectorHistory");

            entity.Property(e => e.Credate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREUSER");
            entity.Property(e => e.FMatchKey)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("F_MatchKey");
            entity.Property(e => e.HHistroy)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("H_Histroy");
            entity.Property(e => e.HId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("H_ID");
            entity.Property(e => e.HIndex)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("H_Index");
            entity.Property(e => e.LId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_ID");
            entity.Property(e => e.Upddate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<CLectorSkill>(entity =>
        {
            entity.HasKey(e => e.SId);

            entity.ToTable("C_LectorSkill");

            entity.Property(e => e.SId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("S_ID");
            entity.Property(e => e.Credate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREUSER");
            entity.Property(e => e.FMatchKey)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("F_MatchKey");
            entity.Property(e => e.LId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("L_ID");
            entity.Property(e => e.SClass)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("S_Class");
            entity.Property(e => e.SHourlRate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("S_HourlRate");
            entity.Property(e => e.SIndex)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("S_Index");
            entity.Property(e => e.SIsActive)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("S_IsActive");
            entity.Property(e => e.SQualify)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("S_Qualify");
            entity.Property(e => e.STeachOrder)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("S_TeachOrder");
            entity.Property(e => e.Upddate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<CMaterial>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("C_Material");

            entity.Property(e => e.Credate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREUSER");
            entity.Property(e => e.MId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("M_ID");
            entity.Property(e => e.MIndex)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("M_Index");
            entity.Property(e => e.MMemo)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("M_Memo");
            entity.Property(e => e.MNumber)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("M_Number");
            entity.Property(e => e.MOrder)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("M_Order");
            entity.Property(e => e.MPublish)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("M_Publish");
            entity.Property(e => e.MVersion)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("M_Version");
            entity.Property(e => e.Upddate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<CTrainingCourse>(entity =>
        {
            entity.HasKey(e => e.TId).HasName("PK_P_TrainingCourses");

            entity.ToTable("C_TrainingCourses");

            entity.Property(e => e.TId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("T_ID");
            entity.Property(e => e.CId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("C_ID");
            entity.Property(e => e.Credate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CREUSER");
            entity.Property(e => e.TDate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("T_Date");
            entity.Property(e => e.THour)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("T_Hour");
            entity.Property(e => e.TIndex)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("T_Index");
            entity.Property(e => e.TName)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("T_Name");
            entity.Property(e => e.TTeacher1)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("T_Teacher1");
            entity.Property(e => e.TTeacher2)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("T_Teacher2");
            entity.Property(e => e.TTeacher3)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("T_Teacher3");
            entity.Property(e => e.TTeacher4)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("T_Teacher4");
            entity.Property(e => e.TTest)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("T_Test");
            entity.Property(e => e.TTimeE)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("T_TimeE");
            entity.Property(e => e.TTimeS)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("T_TimeS");
            entity.Property(e => e.TType)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("T_Type");
            entity.Property(e => e.Upddate)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<PBanner>(entity =>
        {
            entity.HasKey(e => e.BannerId);

            entity.ToTable("P_Banner");

            entity.Property(e => e.BannerId)
                .ValueGeneratedNever()
                .HasColumnName("Banner_ID");
            entity.Property(e => e.BannerEnd)
                .HasComment("輪播結束日")
                .HasColumnType("date");
            entity.Property(e => e.BannerName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("圖片名稱");
            entity.Property(e => e.BannerStart)
                .HasComment("輪播啟用日")
                .HasColumnType("date");
            entity.Property(e => e.BannerStatus).HasComment("啟用狀態");
            entity.Property(e => e.BannerType).HasComment("模組型別 0:首頁輪播");
            entity.Property(e => e.BannerUrl)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasComment("連結")
                .HasColumnName("BannerURL");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<PFile>(entity =>
        {
            entity.HasKey(e => e.FileId);

            entity.ToTable("P_File");

            entity.Property(e => e.FileId)
                .ValueGeneratedNever()
                .HasColumnName("File_ID");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.FIsTop)
                .HasComment("是否置頂")
                .HasColumnName("F_IsTop");
            entity.Property(e => e.FMemo)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasComment("備註")
                .HasColumnName("F_Memo");
            entity.Property(e => e.FOrder).HasColumnName("F_Order");
            entity.Property(e => e.FStatus)
                .HasComment("啟用狀態")
                .HasColumnName("F_Status");
            entity.Property(e => e.FTitle)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("標題名稱")
                .HasColumnName("F_Title");
            entity.Property(e => e.FType)
                .HasComment("檔案模組代碼")
                .HasColumnName("F_Type");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<PLink>(entity =>
        {
            entity.HasKey(e => e.LinkId);

            entity.ToTable("P_Link");

            entity.Property(e => e.LinkId)
                .ValueGeneratedNever()
                .HasColumnName("Link_ID");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.LClass)
                .HasComment("連結分類")
                .HasColumnName("L_Class");
            entity.Property(e => e.LIsTop)
                .HasComment("是否置頂")
                .HasColumnName("L_IsTop");
            entity.Property(e => e.LOrder)
                .HasComment("排序")
                .HasColumnName("L_Order");
            entity.Property(e => e.LStatus)
                .HasComment("啟用狀態")
                .HasColumnName("L_Status");
            entity.Property(e => e.LTitle)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("標題名稱")
                .HasColumnName("L_Title");
            entity.Property(e => e.LType)
                .HasComment("功能模組代碼")
                .HasColumnName("L_Type");
            entity.Property(e => e.LUrl)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("連結網址")
                .HasColumnName("L_URL");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<PNews>(entity =>
        {
            entity.HasKey(e => e.NewsId);

            entity.ToTable("P_News");

            entity.Property(e => e.NewsId)
                .ValueGeneratedNever()
                .HasColumnName("News_ID");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.Curdate)
                .HasColumnType("datetime")
                .HasColumnName("CURDATE");
            entity.Property(e => e.NClass)
                .HasComment("分類")
                .HasColumnName("N_Class");
            entity.Property(e => e.NContent)
                .HasMaxLength(1000)
                .HasComment("內文")
                .HasColumnName("N_Content");
            entity.Property(e => e.NEndDate)
                .HasComment("結束日")
                .HasColumnType("date")
                .HasColumnName("N_EndDate");
            entity.Property(e => e.NIsShow)
                .HasComment("前台是否顯示")
                .HasColumnName("N_IsShow");
            entity.Property(e => e.NIsTop)
                .HasComment("是否置頂")
                .HasColumnName("N_IsTop");
            entity.Property(e => e.NStartDate)
                .HasComment("起始日")
                .HasColumnType("date")
                .HasColumnName("N_StartDate");
            entity.Property(e => e.NStatus)
                .HasComment("啟用狀態")
                .HasColumnName("N_Status");
            entity.Property(e => e.NTitle)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("標題名稱")
                .HasColumnName("N_Title");
            entity.Property(e => e.NType)
                .HasComment("功能模組代碼")
                .HasColumnName("N_Type");
            entity.Property(e => e.RId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("角色代碼")
                .HasColumnName("R_ID");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<PNewsReadLog>(entity =>
        {
            entity.HasKey(e => e.Sn);

            entity.ToTable("P_NewsReadLog");

            entity.Property(e => e.Sn)
                .ValueGeneratedNever()
                .HasComment("PK")
                .HasColumnName("SN");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.NewsId)
                .HasComment("NEWS ID")
                .HasColumnName("News_ID");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");

            entity.HasOne(d => d.News).WithMany(p => p.PNewsReadLogs)
                .HasForeignKey(d => d.NewsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_P_NewsReadLog_P_News");
        });

        modelBuilder.Entity<SysFile>(entity =>
        {
            entity.HasKey(e => e.FileId);

            entity.ToTable("SYS_File");

            entity.Property(e => e.FileId)
                .ValueGeneratedNever()
                .HasComment("檔案ID")
                .HasColumnName("File_ID");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.FDescription)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("檔案描述")
                .HasColumnName("F_Description");
            entity.Property(e => e.FMatchKey)
                .HasComment("功能主KEY")
                .HasColumnName("F_MatchKey");
            entity.Property(e => e.FModule)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("功能模組代碼")
                .HasColumnName("F_Module");
            entity.Property(e => e.FNameDl)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("儲存後名稱")
                .HasColumnName("F_NameDL");
            entity.Property(e => e.FNameReal)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("檔案實體名稱")
                .HasColumnName("F_NameReal");
            entity.Property(e => e.FOrder)
                .HasComment("排序")
                .HasColumnName("F_Order");
            entity.Property(e => e.FPath)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("檔案路徑")
                .HasColumnName("F_Path");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<SysMenu>(entity =>
        {
            entity.HasKey(e => e.MId);

            entity.ToTable("SYS_Menu");

            entity.Property(e => e.MId)
                .ValueGeneratedNever()
                .HasComment("選單ID")
                .HasColumnName("M_ID");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.MIsActice)
                .HasComment("啟用狀態")
                .HasColumnName("M_IsActice");
            entity.Property(e => e.MIsShow)
                .HasComment("前台顯示狀態")
                .HasColumnName("M_IsShow");
            entity.Property(e => e.MLevel)
                .HasComment("選單層級")
                .HasColumnName("M_Level");
            entity.Property(e => e.MName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("選單名稱")
                .HasColumnName("M_Name");
            entity.Property(e => e.MOrder)
                .HasComment("排序")
                .HasColumnName("M_Order");
            entity.Property(e => e.MParentId).HasColumnName("M_ParentID");
            entity.Property(e => e.MType)
                .HasMaxLength(1)
                .HasComment("P=前台，B=後台")
                .HasColumnName("M_Type");
            entity.Property(e => e.MUrl)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("M_Url");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<SysPower>(entity =>
        {
            entity.HasKey(e => e.Sn);

            entity.ToTable("SYS_Power");

            entity.Property(e => e.Sn)
                .HasComment("PK")
                .HasColumnName("SN");
            entity.Property(e => e.CreateData).HasComment("新增資料");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.DeleteData).HasComment("刪除資料");
            entity.Property(e => e.Exportdata).HasComment("匯出資料");
            entity.Property(e => e.ImportData).HasComment("匯入資料");
            entity.Property(e => e.MId)
                .HasComment("頁面代碼")
                .HasColumnName("M_ID");
            entity.Property(e => e.ModifyData).HasComment("編輯資料");
            entity.Property(e => e.RUid)
                .HasComment("角色代碼UID")
                .HasColumnName("R_UID");
            entity.Property(e => e.SearchData).HasComment("查詢資料");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");

            entity.HasOne(d => d.RU).WithMany(p => p.SysPowers)
                .HasForeignKey(d => d.RUid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SYS_Power_SYS_Power");
        });

        modelBuilder.Entity<SysRole>(entity =>
        {
            entity.HasKey(e => e.RUid).HasName("PK_SYS_Role_1");

            entity.ToTable("SYS_Role");

            entity.Property(e => e.RUid)
                .ValueGeneratedNever()
                .HasColumnName("R_UID");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.RId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("角色代碼")
                .HasColumnName("R_ID");
            entity.Property(e => e.RName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("角色名稱")
                .HasColumnName("R_Name");
            entity.Property(e => e.ROrder)
                .HasComment("角色排序")
                .HasColumnName("R_Order");
            entity.Property(e => e.RStatus)
                .HasComment("角色啟用狀態")
                .HasColumnName("R_Status");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<SysUnit>(entity =>
        {
            entity.HasKey(e => e.UnId).HasName("PK_SYS_UDT");

            entity.ToTable("SYS_Unit");

            entity.Property(e => e.UnId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("單位代碼")
                .HasColumnName("Un_ID");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.UnName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("單位名稱")
                .HasColumnName("Un_Name");
            entity.Property(e => e.UnStatus)
                .HasComment("單位狀態")
                .HasColumnName("Un_Status");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<SysUser>(entity =>
        {
            entity.HasKey(e => e.UId);

            entity.ToTable("SYS_User");

            entity.Property(e => e.UId)
                .ValueGeneratedNever()
                .HasComment("PK")
                .HasColumnName("U_ID");
            entity.Property(e => e.Credate)
                .HasComment("建立時間")
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasComment("建立者")
                .HasColumnName("CREUSER");
            entity.Property(e => e.RUid)
                .HasComment("角色UID")
                .HasColumnName("R_UID");
            entity.Property(e => e.UAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("帳號")
                .HasColumnName("U_Account");
            entity.Property(e => e.UAddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("使用者地址")
                .HasColumnName("U_Address");
            entity.Property(e => e.UBirthday)
                .HasComment("使用者生日")
                .HasColumnType("date")
                .HasColumnName("U_Birthday");
            entity.Property(e => e.UEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("使用者電子郵件")
                .HasColumnName("U_Email");
            entity.Property(e => e.UName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("使用者名稱")
                .HasColumnName("U_Name");
            entity.Property(e => e.UPassword)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("密碼")
                .HasColumnName("U_Password");
            entity.Property(e => e.UPhone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("使用者連絡電話")
                .HasColumnName("U_Phone");
            entity.Property(e => e.USex)
                .HasComment("使用者性別:0女、1男")
                .HasColumnName("U_Sex");
            entity.Property(e => e.UStatus).HasColumnName("U_Status");
            entity.Property(e => e.UnId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("使用者單位")
                .HasColumnName("Un_ID");
            entity.Property(e => e.Upddate)
                .HasComment("更新時間")
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasComment("更新者")
                .HasColumnName("UPDUSER");

            entity.HasOne(d => d.RU).WithMany(p => p.SysUsers)
                .HasForeignKey(d => d.RUid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SYS_User_SYS_User");
        });

        modelBuilder.Entity<ViewMenuRolePower>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_MenuRolePower");

            entity.Property(e => e.MId).HasColumnName("M_ID");
            entity.Property(e => e.MLevel).HasColumnName("M_Level");
            entity.Property(e => e.MName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("M_Name");
            entity.Property(e => e.MOrder).HasColumnName("M_Order");
            entity.Property(e => e.MType)
                .HasMaxLength(1)
                .HasColumnName("M_Type");
            entity.Property(e => e.MUrl)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("M_Url");
            entity.Property(e => e.RUid).HasColumnName("R_UID");
        });

        modelBuilder.Entity<ViewRole>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_Role");

            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CREUSER");
            entity.Property(e => e.RId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("R_ID");
            entity.Property(e => e.RName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("R_Name");
            entity.Property(e => e.ROrder).HasColumnName("R_Order");
            entity.Property(e => e.RStatus).HasColumnName("R_Status");
            entity.Property(e => e.RUid).HasColumnName("R_Uid");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UPDUSER");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

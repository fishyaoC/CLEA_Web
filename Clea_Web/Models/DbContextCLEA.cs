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

    public virtual DbSet<CBook> CBooks { get; set; }

    public virtual DbSet<CBookDetail> CBookDetails { get; set; }

    public virtual DbSet<CBookPublish> CBookPublishes { get; set; }

    public virtual DbSet<CClass> CClasses { get; set; }

    public virtual DbSet<CClassLector> CClassLectors { get; set; }

    public virtual DbSet<CClassSubject> CClassSubjects { get; set; }

    public virtual DbSet<CLector> CLectors { get; set; }

    public virtual DbSet<CLectorAdvInfo> CLectorAdvInfos { get; set; }

    public virtual DbSet<EClassUploadLog> EClassUploadLogs { get; set; }

    public virtual DbSet<EEvaluate> EEvaluates { get; set; }

    public virtual DbSet<EEvaluateDetail> EEvaluateDetails { get; set; }

    public virtual DbSet<EEvaluationSche> EEvaluationSches { get; set; }

    public virtual DbSet<PBanner> PBanners { get; set; }

    public virtual DbSet<PFile> PFiles { get; set; }

    public virtual DbSet<PLink> PLinks { get; set; }

    public virtual DbSet<PNews> PNews { get; set; }

    public virtual DbSet<PNewsReadLog> PNewsReadLogs { get; set; }

    public virtual DbSet<SysCode> SysCodes { get; set; }

    public virtual DbSet<SysFile> SysFiles { get; set; }

    public virtual DbSet<SysMenu> SysMenus { get; set; }

    public virtual DbSet<SysPower> SysPowers { get; set; }

    public virtual DbSet<SysRole> SysRoles { get; set; }

    public virtual DbSet<SysUnit> SysUnits { get; set; }

    public virtual DbSet<SysUser> SysUsers { get; set; }

    public virtual DbSet<ViewBAssignBook> ViewBAssignBooks { get; set; }

    public virtual DbSet<ViewBAssignBookEvaluateTeacher> ViewBAssignBookEvaluateTeachers { get; set; }

    public virtual DbSet<ViewBAssignBookScore> ViewBAssignBookScores { get; set; }

    public virtual DbSet<ViewBAssignBookUnLoadFile> ViewBAssignBookUnLoadFiles { get; set; }

    public virtual DbSet<ViewBAssignClassEvaluate> ViewBAssignClassEvaluates { get; set; }

    public virtual DbSet<ViewBAssignClassLector> ViewBAssignClassLectors { get; set; }

    public virtual DbSet<ViewBAssignClassScore> ViewBAssignClassScores { get; set; }

    public virtual DbSet<ViewBBookEvaluateTeacher> ViewBBookEvaluateTeachers { get; set; }

    public virtual DbSet<ViewBookEvaluate> ViewBookEvaluates { get; set; }

    public virtual DbSet<ViewBridgeCBook> ViewBridgeCBooks { get; set; }

    public virtual DbSet<ViewBridgeCBookPublish> ViewBridgeCBookPublishes { get; set; }

    public virtual DbSet<ViewBridgeCClass> ViewBridgeCClasses { get; set; }

    public virtual DbSet<ViewBridgeCClassLector> ViewBridgeCClassLectors { get; set; }

    public virtual DbSet<ViewBridgeCClassSubject> ViewBridgeCClassSubjects { get; set; }

    public virtual DbSet<ViewBridgeCLector> ViewBridgeCLectors { get; set; }

    public virtual DbSet<ViewBridgePublishT> ViewBridgePublishTs { get; set; }

    public virtual DbSet<ViewClassEvaluate> ViewClassEvaluates { get; set; }

    public virtual DbSet<ViewClassLector> ViewClassLectors { get; set; }

    public virtual DbSet<ViewClassLectorEvaluate> ViewClassLectorEvaluates { get; set; }

    public virtual DbSet<ViewEvaluateDetailInfo> ViewEvaluateDetailInfos { get; set; }

    public virtual DbSet<ViewMenuRolePower> ViewMenuRolePowers { get; set; }

    public virtual DbSet<ViewPLectorClass> ViewPLectorClasses { get; set; }

    public virtual DbSet<ViewPLectorClassAbstract> ViewPLectorClassAbstracts { get; set; }

    public virtual DbSet<ViewPLectorEvaluate> ViewPLectorEvaluates { get; set; }

    public virtual DbSet<ViewRole> ViewRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Database=CELA_WEBDB;Persist Security Info=True;TrustServerCertificate=true;User ID=sa;Password=sa123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CBook>(entity =>
        {
            entity.HasKey(e => e.MId);

            entity.ToTable("C_Book");

            entity.Property(e => e.MId)
                .ValueGeneratedNever()
                .HasColumnName("M_ID");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.MIndex)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("教材編號")
                .HasColumnName("M_Index");
            entity.Property(e => e.MName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("教材名稱")
                .HasColumnName("M_Name");
            entity.Property(e => e.MOrder)
                .HasComment("優先順序")
                .HasColumnName("M_Order");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<CBookDetail>(entity =>
        {
            entity.HasKey(e => e.MdId);

            entity.ToTable("C_BookDetail");

            entity.Property(e => e.MdId)
                .ValueGeneratedNever()
                .HasColumnName("MD_ID");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.MId).HasColumnName("M_ID");
            entity.Property(e => e.MdPublish).HasColumnName("MD_Publish");
            entity.Property(e => e.RDate)
                .HasComment("備查時間")
                .HasColumnType("date")
                .HasColumnName("R_Date");
            entity.Property(e => e.RNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("備查文號")
                .HasColumnName("R_Number");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<CBookPublish>(entity =>
        {
            entity.HasKey(e => e.BpId);

            entity.ToTable("C_BookPublish");

            entity.Property(e => e.BpId)
                .ValueGeneratedNever()
                .HasColumnName("BP_ID");
            entity.Property(e => e.BpName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BP_Name");
            entity.Property(e => e.BpNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BP_Number");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<CClass>(entity =>
        {
            entity.HasKey(e => e.CUid);

            entity.ToTable("C_Class");

            entity.Property(e => e.CUid)
                .ValueGeneratedNever()
                .HasColumnName("C_UID");
            entity.Property(e => e.CBookNum)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("教材編號")
                .HasColumnName("C_BookNum");
            entity.Property(e => e.CId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("課程ID")
                .HasColumnName("C_ID");
            entity.Property(e => e.CName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("課程名稱")
                .HasColumnName("C_Name");
            entity.Property(e => e.CType)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("課程類別")
                .HasColumnName("C_Type");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<CClassLector>(entity =>
        {
            entity.HasKey(e => e.ClUid);

            entity.ToTable("C_ClassLector");

            entity.Property(e => e.ClUid)
                .ValueGeneratedNever()
                .HasColumnName("CL_UID");
            entity.Property(e => e.CUid)
                .HasComment("課程種類ID")
                .HasColumnName("C_UID");
            entity.Property(e => e.ClHourlyRate)
                .HasComment("授課鐘點費")
                .HasColumnName("CL_HourlyRate");
            entity.Property(e => e.ClId)
                .HasComment("編號")
                .HasColumnName("CL_ID");
            entity.Property(e => e.ClIsActive)
                .HasComment("是否停止授課")
                .HasColumnName("CL_IsActive");
            entity.Property(e => e.ClOrder)
                .HasComment("上課順序")
                .HasColumnName("CL_Order");
            entity.Property(e => e.ClQualify)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("講師資格")
                .HasColumnName("CL_Qualify");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.DUid)
                .HasComment("課程明細ID")
                .HasColumnName("D_UID");
            entity.Property(e => e.LUid)
                .HasComment("講師ID")
                .HasColumnName("L_UID");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<CClassSubject>(entity =>
        {
            entity.HasKey(e => e.DUid);

            entity.ToTable("C_ClassSubject");

            entity.Property(e => e.DUid)
                .ValueGeneratedNever()
                .HasColumnName("D_UID");
            entity.Property(e => e.CUid).HasColumnName("C_UID");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.DHour)
                .HasComment("時數")
                .HasColumnName("D_Hour");
            entity.Property(e => e.DHourlyRate)
                .HasComment("鐘點費")
                .HasColumnName("D_HourlyRate");
            entity.Property(e => e.DId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("課程編號")
                .HasColumnName("D_ID");
            entity.Property(e => e.DIsTest)
                .HasComment("是否測驗")
                .HasColumnName("D_IsTest");
            entity.Property(e => e.DMemo)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("備註")
                .HasColumnName("D_Memo");
            entity.Property(e => e.DName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("課程名稱")
                .HasColumnName("D_Name");
            entity.Property(e => e.DType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("學/術科")
                .HasColumnName("D_Type");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<CLector>(entity =>
        {
            entity.HasKey(e => e.LUid);

            entity.ToTable("C_Lector");

            entity.Property(e => e.LUid)
                .ValueGeneratedNever()
                .HasColumnName("L_UID");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.LActive)
                .HasComment("是否啟用")
                .HasColumnName("L_Active");
            entity.Property(e => e.LAddress)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasComment("聯絡地址")
                .HasColumnName("L_Address");
            entity.Property(e => e.LBrithday)
                .HasComment("生日")
                .HasColumnType("date")
                .HasColumnName("L_BRITHDAY");
            entity.Property(e => e.LCaddress)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasComment("戶籍地址")
                .HasColumnName("L_CAddress");
            entity.Property(e => e.LCellPhone)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("行動電話")
                .HasColumnName("L_CellPhone");
            entity.Property(e => e.LCposCode)
                .HasComment("戶籍地址郵遞區號")
                .HasColumnName("L_CPosCode");
            entity.Property(e => e.LEdu)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasComment("學歷")
                .HasColumnName("L_Edu");
            entity.Property(e => e.LEduSchool)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasComment("畢業學校")
                .HasColumnName("L_EduSchool");
            entity.Property(e => e.LEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("電子郵件")
                .HasColumnName("L_Email");
            entity.Property(e => e.LId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("身分證")
                .HasColumnName("L_ID");
            entity.Property(e => e.LIsCheck)
                .HasComment("總會是否核准")
                .HasColumnName("L_IsCheck");
            entity.Property(e => e.LMemo)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("備註")
                .HasColumnName("L_Memo");
            entity.Property(e => e.LName)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasComment("姓名")
                .HasColumnName("L_NAME");
            entity.Property(e => e.LPhone)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("聯絡電話")
                .HasColumnName("L_Phone");
            entity.Property(e => e.LPosCode)
                .HasComment("聯絡地址郵遞區號")
                .HasColumnName("L_PosCode");
            entity.Property(e => e.LTravelExpenses)
                .HasComment("差旅費")
                .HasColumnName("L_TravelExpenses");
            entity.Property(e => e.LType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("講師類別")
                .HasColumnName("L_Type");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<CLectorAdvInfo>(entity =>
        {
            entity.HasKey(e => e.LaUid);

            entity.ToTable("C_LectorAdvInfo");

            entity.Property(e => e.LaUid)
                .ValueGeneratedNever()
                .HasColumnName("LA_UID");
            entity.Property(e => e.Credate)
                .HasComment("建立時間")
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasComment("建立者")
                .HasColumnName("CREUSER");
            entity.Property(e => e.LUid)
                .HasComment("講師UID")
                .HasColumnName("L_UID");
            entity.Property(e => e.LaTitle)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("標題")
                .HasColumnName("LA_Title");
            entity.Property(e => e.LaYear)
                .HasComment("進修年度")
                .HasColumnName("LA_Year");
            entity.Property(e => e.Upddate)
                .HasComment("更新時間")
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasComment("更新者")
                .HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<EClassUploadLog>(entity =>
        {
            entity.HasKey(e => e.Sn);

            entity.ToTable("E_ClassUploadLog");

            entity.Property(e => e.Sn).HasColumnName("SN");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.EsId)
                .HasComment("評核PK")
                .HasColumnName("ES_ID");
            entity.Property(e => e.FileFullName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasComment("檔案全名");
            entity.Property(e => e.IsUpdate).HasComment("是否為重大更新");
            entity.Property(e => e.Other)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<EEvaluate>(entity =>
        {
            entity.HasKey(e => e.EId);

            entity.ToTable("E_Evaluate");

            entity.Property(e => e.EId)
                .ValueGeneratedNever()
                .HasColumnName("E_ID");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.EType)
                .HasComment("0:課程 1:教材")
                .HasColumnName("E_Type");
            entity.Property(e => e.EYear)
                .HasComment("評核年度")
                .HasColumnName("E_Year");
            entity.Property(e => e.MatchKey)
                .HasComment("BOOK OR CLASS PK")
                .HasColumnName("matchKey");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<EEvaluateDetail>(entity =>
        {
            entity.HasKey(e => e.EdId).HasName("PK_E_E_EvaluateDetail");

            entity.ToTable("E_EvaluateDetail");

            entity.Property(e => e.EdId)
                .ValueGeneratedNever()
                .HasColumnName("ED_ID");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.EId).HasColumnName("E_ID");
            entity.Property(e => e.ERemark)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("審核意見")
                .HasColumnName("E_Remark");
            entity.Property(e => e.EScoreA).HasColumnName("E_ScoreA");
            entity.Property(e => e.EScoreB).HasColumnName("E_ScoreB");
            entity.Property(e => e.EScoreC).HasColumnName("E_ScoreC");
            entity.Property(e => e.EScoreD).HasColumnName("E_ScoreD");
            entity.Property(e => e.EScoreE).HasColumnName("E_ScoreE");
            entity.Property(e => e.ETeachAbstract)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasComment("教學內容")
                .HasColumnName("E_TeachAbstract");
            entity.Property(e => e.ETeachObject)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasComment("課程目標")
                .HasColumnName("E_TeachObject");
            entity.Property(e => e.ETeachSyllabus)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasComment("課程大綱")
                .HasColumnName("E_TeachSyllabus");
            entity.Property(e => e.EsId).HasColumnName("ES_ID");
            entity.Property(e => e.Evaluate).HasComment("評核人員");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<EEvaluationSche>(entity =>
        {
            entity.HasKey(e => e.EsId);

            entity.ToTable("E_EvaluationSche");

            entity.Property(e => e.EsId)
                .ValueGeneratedNever()
                .HasColumnName("ES_ID");
            entity.Property(e => e.ChkNum).ValueGeneratedOnAdd();
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
            entity.Property(e => e.EId).HasColumnName("E_ID");
            entity.Property(e => e.ETeachAbstract)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("E_TeachAbstract");
            entity.Property(e => e.ETeachObject)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("E_TeachObject");
            entity.Property(e => e.ETeachSyllabus)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("E_TeachSyllabus");
            entity.Property(e => e.IsClose).HasComment("退回重評");
            entity.Property(e => e.IsSche).HasComment("最優先評鑑");
            entity.Property(e => e.MatchKey).HasColumnName("matchKey");
            entity.Property(e => e.Reception).HasComment("講師");
            entity.Property(e => e.ScheNum).HasComment("評鑑序");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");
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
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
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
            entity.Property(e => e.NRole)
                .HasComment("true=群發，false=個人")
                .HasColumnName("N_Role");
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
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComment("功能模組代碼")
                .HasColumnName("N_Type");
            entity.Property(e => e.RId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("角色代碼(觀看權限)")
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

        modelBuilder.Entity<SysCode>(entity =>
        {
            entity.HasKey(e => e.Uid);

            entity.ToTable("SYS_Code");

            entity.Property(e => e.Uid).ValueGeneratedNever();
            entity.Property(e => e.CItemCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("代號")
                .HasColumnName("C_itemCode");
            entity.Property(e => e.CItemName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("代碼名稱")
                .HasColumnName("C_itemName");
            entity.Property(e => e.CItemOrder)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("排序")
                .HasColumnName("C_itemOrder");
            entity.Property(e => e.CParentCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("父層代號")
                .HasColumnName("C_ParentCode");
            entity.Property(e => e.CParentUid)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("父層Uid")
                .HasColumnName("C_ParentUid");
            entity.Property(e => e.Credate)
                .HasComment("建立時間")
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasComment("建立者")
                .HasColumnName("CREUSER");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasComment("開啟狀態")
                .HasColumnName("isActive");
            entity.Property(e => e.IsShow)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasComment("顯示狀態")
                .HasColumnName("isShow");
            entity.Property(e => e.Upddate)
                .HasComment("更新時間")
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasComment("更新者")
                .HasColumnName("UPDUSER");
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
            entity.Property(e => e.FExt)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("副檔名")
                .HasColumnName("F_Ext");
            entity.Property(e => e.FFullName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("檔案全名")
                .HasColumnName("F_FullName");
            entity.Property(e => e.FMatchKey)
                .HasComment("功能主KEY")
                .HasColumnName("F_MatchKey");
            entity.Property(e => e.FMatchKey2)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("F_MatchKey2");
            entity.Property(e => e.FMimeType)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("檔案類型")
                .HasColumnName("F_MimeType");
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
                .HasMaxLength(200)
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
            entity.Property(e => e.FRemark)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasComment("檔案備註")
                .HasColumnName("F_Remark");
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
            entity.Property(e => e.RBackEnd)
                .HasComment("是否為後台帳號")
                .HasColumnName("R_BackEnd");
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
            entity.Property(e => e.IsOutSide).HasColumnName("isOutSide");
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
                .HasMaxLength(100)
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
            entity.Property(e => e.UStatus)
                .HasComment("狀態")
                .HasColumnName("U_Status");
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

        modelBuilder.Entity<ViewBAssignBook>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_B_AssignBook");

            entity.Property(e => e.EId).HasColumnName("E_ID");
            entity.Property(e => e.MId).HasColumnName("M_ID");
            entity.Property(e => e.MIndex)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("M_Index");
            entity.Property(e => e.MName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("M_Name");
        });

        modelBuilder.Entity<ViewBAssignBookEvaluateTeacher>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_B_AssignBookEvaluateTeacher");

            entity.Property(e => e.BpId).HasColumnName("BP_ID");
            entity.Property(e => e.BpName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BP_Name");
            entity.Property(e => e.EId).HasColumnName("E_ID");
            entity.Property(e => e.EScoreA).HasColumnName("E_ScoreA");
            entity.Property(e => e.EdId).HasColumnName("ED_ID");
            entity.Property(e => e.EsId).HasColumnName("ES_ID");
            entity.Property(e => e.LId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("L_ID");
            entity.Property(e => e.LName)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("L_NAME");
            entity.Property(e => e.LUid).HasColumnName("L_UID");
            entity.Property(e => e.MId).HasColumnName("M_ID");
            entity.Property(e => e.MIndex)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("M_Index");
            entity.Property(e => e.MName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("M_Name");
        });

        modelBuilder.Entity<ViewBAssignBookScore>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_B_AssignBookScore");

            entity.Property(e => e.BpName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BP_Name");
            entity.Property(e => e.EId).HasColumnName("E_ID");
            entity.Property(e => e.ERemark)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("E_Remark");
            entity.Property(e => e.EScoreA).HasColumnName("E_ScoreA");
            entity.Property(e => e.EScoreB).HasColumnName("E_ScoreB");
            entity.Property(e => e.EScoreC).HasColumnName("E_ScoreC");
            entity.Property(e => e.EScoreD).HasColumnName("E_ScoreD");
            entity.Property(e => e.EScoreE).HasColumnName("E_ScoreE");
            entity.Property(e => e.EdId).HasColumnName("ED_ID");
            entity.Property(e => e.EsId).HasColumnName("ES_ID");
            entity.Property(e => e.LName)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("L_NAME");
            entity.Property(e => e.MName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("M_Name");
        });

        modelBuilder.Entity<ViewBAssignBookUnLoadFile>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_B_AssignBookUnLoadFile");

            entity.Property(e => e.CName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("C_Name");
            entity.Property(e => e.DName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("D_Name");
            entity.Property(e => e.EId).HasColumnName("E_ID");
            entity.Property(e => e.EsId).HasColumnName("ES_ID");
            entity.Property(e => e.FMatchKey).HasColumnName("F_MatchKey");
            entity.Property(e => e.LName)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("L_NAME");
        });

        modelBuilder.Entity<ViewBAssignClassEvaluate>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_B_AssignClassEvaluate");

            entity.Property(e => e.CId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("C_ID");
            entity.Property(e => e.CName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("C_Name");
            entity.Property(e => e.CUid).HasColumnName("C_UID");
            entity.Property(e => e.EId).HasColumnName("E_ID");
            entity.Property(e => e.EType).HasColumnName("E_Type");
            entity.Property(e => e.EYear).HasColumnName("E_Year");
        });

        modelBuilder.Entity<ViewBAssignClassLector>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_B_AssignClassLector");

            entity.Property(e => e.CId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("C_ID");
            entity.Property(e => e.CName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("C_Name");
            entity.Property(e => e.CUid).HasColumnName("C_UID");
            entity.Property(e => e.ClUid).HasColumnName("CL_UID");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.DHour).HasColumnName("D_Hour");
            entity.Property(e => e.DId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("D_ID");
            entity.Property(e => e.DName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("D_Name");
            entity.Property(e => e.EId).HasColumnName("E_ID");
            entity.Property(e => e.EYear).HasColumnName("E_Year");
            entity.Property(e => e.EsId).HasColumnName("ES_ID");
            entity.Property(e => e.LId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("L_ID");
            entity.Property(e => e.LName)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("L_NAME");
            entity.Property(e => e.LUid).HasColumnName("L_UID");
        });

        modelBuilder.Entity<ViewBAssignClassScore>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_B_AssignClassScore");

            entity.Property(e => e.EId).HasColumnName("E_ID");
            entity.Property(e => e.ERemark)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("E_Remark");
            entity.Property(e => e.EScoreA).HasColumnName("E_ScoreA");
            entity.Property(e => e.EScoreB).HasColumnName("E_ScoreB");
            entity.Property(e => e.EScoreC).HasColumnName("E_ScoreC");
            entity.Property(e => e.EScoreD).HasColumnName("E_ScoreD");
            entity.Property(e => e.EScoreE).HasColumnName("E_ScoreE");
            entity.Property(e => e.EdId).HasColumnName("ED_ID");
            entity.Property(e => e.EsId).HasColumnName("ES_ID");
            entity.Property(e => e.LName)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("L_NAME");
        });

        modelBuilder.Entity<ViewBBookEvaluateTeacher>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_B_BookEvaluateTeacher");

            entity.Property(e => e.EId).HasColumnName("E_ID");
            entity.Property(e => e.LId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("L_ID");
            entity.Property(e => e.LName)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("L_NAME");
        });

        modelBuilder.Entity<ViewBookEvaluate>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_BookEvaluate");

            entity.Property(e => e.EId).HasColumnName("E_ID");
            entity.Property(e => e.EType).HasColumnName("E_Type");
            entity.Property(e => e.EYear).HasColumnName("E_Year");
            entity.Property(e => e.MId).HasColumnName("M_ID");
            entity.Property(e => e.MIndex).HasColumnName("M_Index");
            entity.Property(e => e.MName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("M_Name");
        });

        modelBuilder.Entity<ViewBridgeCBook>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_Bridge_C_Book");

            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasMaxLength(20)
                .HasColumnName("CREUSER");
            entity.Property(e => e.MIndex)
                .HasMaxLength(10)
                .HasColumnName("M_Index");
            entity.Property(e => e.MMemo)
                .HasMaxLength(50)
                .HasColumnName("M_Memo");
            entity.Property(e => e.MName)
                .HasMaxLength(80)
                .HasColumnName("M_Name");
            entity.Property(e => e.MNumber)
                .HasMaxLength(40)
                .HasColumnName("M_Number");
            entity.Property(e => e.MOrder).HasColumnName("M_Order");
            entity.Property(e => e.MPublish)
                .HasMaxLength(20)
                .HasColumnName("M_Publish");
            entity.Property(e => e.MVersion)
                .HasMaxLength(20)
                .HasColumnName("M_Version");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasMaxLength(20)
                .HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<ViewBridgeCBookPublish>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_Bridge_C_BookPublish");

            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasMaxLength(20)
                .HasColumnName("CREUSER");
            entity.Property(e => e.MIndex)
                .HasMaxLength(10)
                .HasColumnName("M_Index");
            entity.Property(e => e.MMemo)
                .HasMaxLength(50)
                .HasColumnName("M_Memo");
            entity.Property(e => e.MName)
                .HasMaxLength(80)
                .HasColumnName("M_Name");
            entity.Property(e => e.MNumber)
                .HasMaxLength(40)
                .HasColumnName("M_Number");
            entity.Property(e => e.MOrder).HasColumnName("M_Order");
            entity.Property(e => e.MPublish)
                .HasMaxLength(20)
                .HasColumnName("M_Publish");
            entity.Property(e => e.MVersion)
                .HasMaxLength(20)
                .HasColumnName("M_Version");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasMaxLength(20)
                .HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<ViewBridgeCClass>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_Bridge_C_Class");

            entity.Property(e => e.CBookNum)
                .HasMaxLength(10)
                .HasColumnName("C_BookNum");
            entity.Property(e => e.CId)
                .HasMaxLength(50)
                .HasColumnName("C_ID");
            entity.Property(e => e.CName)
                .HasMaxLength(200)
                .HasColumnName("C_Name");
            entity.Property(e => e.CType)
                .HasMaxLength(20)
                .HasColumnName("C_Type");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasMaxLength(20)
                .HasColumnName("CREUSER");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasMaxLength(20)
                .HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<ViewBridgeCClassLector>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_Bridge_C_ClassLector");

            entity.Property(e => e.CUid)
                .HasMaxLength(10)
                .HasColumnName("C_UID");
            entity.Property(e => e.ClHourlyRate)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("CL_HourlyRate");
            entity.Property(e => e.ClIsActive)
                .HasMaxLength(10)
                .HasColumnName("CL_IsActive");
            entity.Property(e => e.ClOrder).HasColumnName("CL_Order");
            entity.Property(e => e.ClQualify)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("CL_Qualify");
            entity.Property(e => e.ClUid).HasColumnName("CL_UID");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasMaxLength(20)
                .HasColumnName("CREUSER");
            entity.Property(e => e.DUid)
                .HasMaxLength(10)
                .HasColumnName("D_UID");
            entity.Property(e => e.LUid)
                .HasMaxLength(20)
                .HasColumnName("L_UID");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasMaxLength(20)
                .HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<ViewBridgeCClassSubject>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_Bridge_C_ClassSubject");

            entity.Property(e => e.CUid)
                .HasMaxLength(10)
                .HasColumnName("C_UID");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasMaxLength(20)
                .HasColumnName("CREUSER");
            entity.Property(e => e.DHour)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("D_Hour");
            entity.Property(e => e.DHourlyRate)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("D_HourlyRate");
            entity.Property(e => e.DId)
                .HasMaxLength(10)
                .HasColumnName("D_ID");
            entity.Property(e => e.DIsTest)
                .HasMaxLength(50)
                .HasColumnName("D_IsTest");
            entity.Property(e => e.DMemo)
                .HasMaxLength(50)
                .HasColumnName("D_Memo");
            entity.Property(e => e.DName)
                .HasMaxLength(200)
                .HasColumnName("D_Name");
            entity.Property(e => e.DType)
                .HasMaxLength(1)
                .HasColumnName("D_Type");
            entity.Property(e => e.Sn)
                .ValueGeneratedOnAdd()
                .HasColumnName("SN");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasMaxLength(20)
                .HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<ViewBridgeCLector>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_Bridge_C_Lector");

            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser)
                .HasMaxLength(20)
                .HasColumnName("CREUSER");
            entity.Property(e => e.LActive)
                .HasMaxLength(50)
                .HasColumnName("L_Active");
            entity.Property(e => e.LAddress)
                .HasMaxLength(80)
                .HasColumnName("L_Address");
            entity.Property(e => e.LBrithday)
                .HasColumnType("datetime")
                .HasColumnName("L_BRITHDAY");
            entity.Property(e => e.LCaddress)
                .HasMaxLength(80)
                .HasColumnName("L_CAddress");
            entity.Property(e => e.LCellPhone)
                .HasMaxLength(30)
                .HasColumnName("L_CellPhone");
            entity.Property(e => e.LCposCode)
                .HasMaxLength(8)
                .HasColumnName("L_CPosCode");
            entity.Property(e => e.LEdu)
                .HasMaxLength(4)
                .HasColumnName("L_Edu");
            entity.Property(e => e.LEduSchool)
                .HasMaxLength(30)
                .HasColumnName("L_EduSchool");
            entity.Property(e => e.LEmail)
                .HasMaxLength(50)
                .HasColumnName("L_Email");
            entity.Property(e => e.LId)
                .HasMaxLength(20)
                .HasColumnName("L_ID");
            entity.Property(e => e.LIsCheck)
                .HasMaxLength(50)
                .HasColumnName("L_IsCheck");
            entity.Property(e => e.LMemo)
                .HasMaxLength(50)
                .HasColumnName("L_Memo");
            entity.Property(e => e.LName)
                .HasMaxLength(20)
                .HasColumnName("L_NAME");
            entity.Property(e => e.LPhone)
                .HasMaxLength(30)
                .HasColumnName("L_Phone");
            entity.Property(e => e.LPosCode)
                .HasMaxLength(8)
                .HasColumnName("L_PosCode");
            entity.Property(e => e.LTravelExpenses).HasColumnName("L_TravelExpenses");
            entity.Property(e => e.LType)
                .HasMaxLength(4)
                .HasColumnName("L_Type");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser)
                .HasMaxLength(20)
                .HasColumnName("UPDUSER");
        });

        modelBuilder.Entity<ViewBridgePublishT>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_Bridge_Publish_T");

            entity.Property(e => e.Publish).HasMaxLength(20);
        });

        modelBuilder.Entity<ViewClassEvaluate>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_ClassEvaluate");

            entity.Property(e => e.CId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("C_ID");
            entity.Property(e => e.CName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("C_Name");
            entity.Property(e => e.CUid).HasColumnName("C_UID");
            entity.Property(e => e.EId).HasColumnName("E_ID");
            entity.Property(e => e.EType).HasColumnName("E_Type");
            entity.Property(e => e.EYear).HasColumnName("E_Year");
        });

        modelBuilder.Entity<ViewClassLector>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_ClassLector");

            entity.Property(e => e.CName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("C_Name");
            entity.Property(e => e.CUid).HasColumnName("C_UID");
            entity.Property(e => e.ClUid).HasColumnName("CL_UID");
            entity.Property(e => e.DName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("D_Name");
            entity.Property(e => e.EdId).HasColumnName("ED_ID");
            entity.Property(e => e.LName)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("L_NAME");
        });

        modelBuilder.Entity<ViewClassLectorEvaluate>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_ClassLectorEvaluate");

            entity.Property(e => e.CId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("C_ID");
            entity.Property(e => e.CName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("C_Name");
            entity.Property(e => e.CUid).HasColumnName("C_UID");
            entity.Property(e => e.ClUid).HasColumnName("CL_UID");
            entity.Property(e => e.DId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("D_ID");
            entity.Property(e => e.DName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("D_Name");
            entity.Property(e => e.DUid).HasColumnName("D_UID");
            entity.Property(e => e.Expr2)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Expr3)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.LId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("L_ID");
            entity.Property(e => e.LName)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("L_NAME");
            entity.Property(e => e.LUid).HasColumnName("L_UID");
            entity.Property(e => e.LevType).HasColumnName("LEv_Type");
            entity.Property(e => e.LevYear).HasColumnName("LEv_Year");
        });

        modelBuilder.Entity<ViewEvaluateDetailInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_EvaluateDetailInfo");

            entity.Property(e => e.EId).HasColumnName("E_ID");
            entity.Property(e => e.EScoreA).HasColumnName("E_ScoreA");
            entity.Property(e => e.EdId).HasColumnName("ED_ID");
            entity.Property(e => e.FMatchKey).HasColumnName("F_MatchKey");
            entity.Property(e => e.MName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("M_Name");
            entity.Property(e => e.MdPublish).HasColumnName("MD_Publish");
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

        modelBuilder.Entity<ViewPLectorClass>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_P_LectorClass");

            entity.Property(e => e.CName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("C_Name");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.DName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("D_Name");
            entity.Property(e => e.EId).HasColumnName("E_ID");
            entity.Property(e => e.EsId).HasColumnName("ES_ID");
            entity.Property(e => e.FileName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fileName");
            entity.Property(e => e.MatchKey).HasColumnName("matchKey");
        });

        modelBuilder.Entity<ViewPLectorClassAbstract>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_P_LectorClassAbstract");

            entity.Property(e => e.Am).HasColumnName("AM");
            entity.Property(e => e.Bm).HasColumnName("BM");
            entity.Property(e => e.CName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("C_Name");
            entity.Property(e => e.DName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("D_Name");
            entity.Property(e => e.DUid).HasColumnName("D_UID");
            entity.Property(e => e.EId).HasColumnName("E_ID");
            entity.Property(e => e.ETeachAbstract)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("E_TeachAbstract");
            entity.Property(e => e.ETeachObject)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("E_TeachObject");
            entity.Property(e => e.ETeachSyllabus)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("E_TeachSyllabus");
            entity.Property(e => e.EsId).HasColumnName("ES_ID");
            entity.Property(e => e.LName)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("L_NAME");
        });

        modelBuilder.Entity<ViewPLectorEvaluate>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_P_LectorEvaluate");

            entity.Property(e => e.BpName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BP_Name");
            entity.Property(e => e.CName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("C_Name");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.DName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("D_Name");
            entity.Property(e => e.EId).HasColumnName("E_ID");
            entity.Property(e => e.EScoreA).HasColumnName("E_ScoreA");
            entity.Property(e => e.EType).HasColumnName("E_Type");
            entity.Property(e => e.EdId).HasColumnName("ED_ID");
            entity.Property(e => e.EsId).HasColumnName("ES_ID");
            entity.Property(e => e.Expr1)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.LName)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("L_NAME");
            entity.Property(e => e.MName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("M_Name");
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
            entity.Property(e => e.RBackEnd).HasColumnName("R_BackEnd");
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
            entity.Property(e => e.RUid).HasColumnName("R_UID");
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

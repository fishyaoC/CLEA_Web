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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=210.61.116.244;Database=CELA_WEBDB;Persist Security Info=True;TrustServerCertificate=true;User ID=clea_Web;Password=!qaz2wsX3edc");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

            entity.HasOne(d => d.RIdNavigation).WithMany(p => p.PNews)
                .HasForeignKey(d => d.RId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_P_News_SYS_Role");
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
            entity.Property(e => e.RId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("角色代碼")
                .HasColumnName("R_ID");
            entity.Property(e => e.SearchData).HasComment("查詢資料");
            entity.Property(e => e.Upddate)
                .HasColumnType("datetime")
                .HasColumnName("UPDDATE");
            entity.Property(e => e.Upduser).HasColumnName("UPDUSER");

            entity.HasOne(d => d.RIdNavigation).WithMany(p => p.SysPowers)
                .HasForeignKey(d => d.RId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SYS_Power_SYS_Role");
        });

        modelBuilder.Entity<SysRole>(entity =>
        {
            entity.HasKey(e => e.RId);

            entity.ToTable("SYS_Role");

            entity.Property(e => e.RId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("角色代碼")
                .HasColumnName("R_ID");
            entity.Property(e => e.Credate)
                .HasColumnType("datetime")
                .HasColumnName("CREDATE");
            entity.Property(e => e.Creuser).HasColumnName("CREUSER");
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

            entity.HasOne(d => d.Un).WithMany(p => p.SysUsers)
                .HasForeignKey(d => d.UnId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SYS_User_SYS_Unit");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clea_Web.ViewModels
{
    public class BtnViewModel : BaseViewModel
    {
        public Modify modify { get; set; }
        public SchModel schModel { get; set; }

        #region 選單
        public class UserRole
        {
            public List<SysMenu> lst_sysMenu { get; set; }
        }
        #endregion

        public class FileModel {
            [DisplayName("檔案路徑")]
            public String? FilePath { get; set; }
            [DisplayName("檔案名稱")]
            public String? FileName { get; set; }
            [DisplayName("檔案PK")]
            public Guid? FileID { get; set; }
        }

        #region 編輯頁面
        public class Modify
        {
            public List<FileModel> fileModels { get; set; } = new List<FileModel>();
            public List<ViewMenuRolePower> rolePowerListB { get; set; }
            public List<ViewMenuRolePower> rolePowerListP { get; set; }
            public List<SelectListItem> DropDownList { get; set; }
            public List<SelectListItem> DropDownListLevel { get; set; }
            public List<SelectListItem> DropDownListType { get; set; }


            public Guid NewsId { get; set; }

            /// <summary>
            /// 功能模組代碼
            /// </summary>
            [DisplayName("功能模組代碼")]
            [Required(ErrorMessage = "必填項目!")]
            public int? NType { get; set; } = null!;

            /// <summary>
            /// 標題名稱
            /// </summary>
            [DisplayName("公告標題")]
            [Required(ErrorMessage = "必填項目!")]
            public string NTitle { get; set; } = null!;

            /// <summary>
            /// 分類
            /// </summary>
            [DisplayName("公告類型")]
            public int? NClass { get; set; }

            /// <summary>
            /// 起始日
            /// </summary>
            [DisplayName("公告起始日")]
            [Required(ErrorMessage = "必填項目!")]
            public DateTime NStartDate { get; set; }

            /// <summary>
            /// 結束日
            /// </summary>
            [DisplayName("公告結束日")]
            public DateTime? NEndDate { get; set; }

            /// <summary>
            /// 是否置頂
            /// </summary>
            [DisplayName("是否置頂")]
            public bool NIsTop { get; set; } =false;

            /// <summary>
            /// 前台是否顯示
            /// </summary>
            [DisplayName("是否顯示")]
            public bool NIsShow { get; set; } = true;

            /// <summary>
            /// 啟用狀態
            /// </summary>
            [DisplayName("啟用狀態")]
            public bool NStatus { get; set; } = true;

            /// <summary>
            /// 內文
            /// </summary>
            [DisplayName("公告內容")]
            [Required(ErrorMessage = "必填項目!")]
            public string NContent { get; set; } = null!;

            /// <summary>
            /// true=群發，false=個人
            /// </summary>
            [DisplayName("發布對象")]
            [Required(ErrorMessage = "必填項目!")]
            public bool? NRole { get; set; } = false;

            /// <summary>
            /// 角色代碼(觀看權限)
            /// </summary>
            [DisplayName("角色代碼(觀看權限)")]
            public string RId { get; set; } = null!;

            /// <summary>
            /// 會員等級
            /// </summary>
            [DisplayName("會員等級")]
            public string Level { get; set; } = null!;

            /// <summary>
            /// 可調整之點閱次數
            /// </summary>
            [DisplayName("可調整之點閱次數")]
            public int? Click { get; set; }

            public Guid Creuser { get; set; }

            public DateTime Credate { get; set; }

            public Guid? Upduser { get; set; }

            public DateTime? Upddate { get; set; }

            [DisplayName("上傳檔案")]            
            public List<IFormFile>? file { get; set; }
            
            /// <summary>
            /// 發布對象(群組)
            /// </summary>
            [DisplayName("公告ID")]
            public Guid? GroupID { get; set; }
            /// <summary>
            /// 發布對象(個人)
            /// </summary>
            [DisplayName("公告ID")]
            public Guid? PersonID { get; set; }
            /// <summary>
            /// 編輯狀態 True = 編輯 ; False = 新增
            /// </summary>
            [DisplayName("編輯狀態")]
            public Boolean IsEdit { get; set; } = false;

        }
        #endregion
        #region Index
        public class SchModel
        {
            public SchItem schItem { get; set; }
            public List<schPageList> schPageList { get; set; }
            public IPagedList<schPageList> schPageList2 { get; set; }
            /// <summary>
            /// 分類名稱
            /// </summary>
            [DisplayName("分類名稱")]
            public string NTypeName { get; set; }
            /// <summary>
            /// Uid
            /// </summary>
            [DisplayName("Uid")]
            public Guid R_ID { get; set; }
            public List<SelectListItem> DropDownList { get; set; }
            public List<SelectListItem> DropDownListType { get; set; }
        }
        #endregion

        #region 搜尋條件
        public class SchItem
        {
            //公告類型
            public String? NClass { get; set; }
            //開始日期
            public DateTime? NStartDate { get; set; }
            //結束日期
            public DateTime? NEndDate { get; set; }
            //公告標題
            public String? NTitle { get; set; }

        }
        #endregion

        #region 列表
        public class schPageList : PNews
        {

            /// <summary>
            /// 分類名稱
            /// </summary>
            [DisplayName("分類名稱")]
            public string NClassName { get; set; }

            [DisplayName("公告時間")]
            public string NStartDateStr { get; set; }

            [DisplayName("時間排序")]
            public DateTime Date { get; set; }

            [DisplayName("觀看次數")]
            public int? ViewCount { get; set; }

        }
        #endregion
    }
}

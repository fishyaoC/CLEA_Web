using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clea_Web.ViewModels
{
    public class P_LectorBtnViewModel : BaseViewModel
    {
        public Modify modify { get; set; }
        public SchModel schModel { get; set; }

        #region 選單
        public class UserRole
        {
            public List<SysMenu> lst_sysMenu { get; set; }
        }
        #endregion

        #region 編輯頁面
        public class Modify :PNews
        {
            public List<ViewMenuRolePower> rolePowerListB { get; set; }
            public List<ViewMenuRolePower> rolePowerListP { get; set; }
            public List<SelectListItem> DropDownList { get; set; }
            public List<SelectListItem> DropDownListUser { get; set; }
            public List<SelectListItem> DropDownListType { get; set; }
            /// <summary>
            /// Uid
            /// </summary>
            [DisplayName("Uid")]
            public Guid R_ID { get; set; }

            /// <summary>
            /// 公告ID
            /// </summary>
            [DisplayName("公告ID")]
            public String? News_ID { get; set; }

            /// <summary>
            /// 公告類型
            /// </summary>
            [DisplayName("分類")]
            public string NType { get; set; }
            /// <summary>
            /// 主旨
            /// </summary>
            [DisplayName("主旨")]
            public String NTitle { get; set; }

            /// <summary>
            /// 公告類別
            /// </summary>
            [DisplayName("分類")]
            public Int16 N_Class { get; set; }

            /// <summary>
            /// 開始日期
            /// </summary>
            [DisplayFormat(DataFormatString = "{0:yyyy/mm/dd}")]
            [DisplayName("開始日期")]
            public DateTime N_StartDate { get; set; }
            /// <summary>
            /// 分類名稱
            /// </summary>
            [DisplayName("分類名稱")]
            public string NTypeName { get; set; }
            /// <summary>
            /// 是否置頂
            /// </summary>
            [DisplayName("是否置頂")]
            public Boolean N_IsTop { get; set; } = false;
            /// <summary>
            /// 是否顯示
            /// </summary>
            [DisplayName("是否顯示")]
            public Boolean N_IsShow { get; set; } = false;
            /// 公告狀態
            /// </summary>
            [DisplayName("公告狀態")]
            public Boolean N_Status { get; set; } = false;
            /// <summary>
            /// 公告內容
            /// </summary>
            [DisplayName("公告內容")]
            public String NContent { get; set; }
            /// <summary>
            /// 編輯狀態 True = 編輯 ; False = 新增
            /// </summary>
            [DisplayName("編輯狀態")]
            public Boolean IsEdit { get; set; } = false;

            [DisplayName("上傳檔案")]
            [Required(ErrorMessage = "請選擇檔案!")]
            public IFormFile? file { get; set; }
            [DisplayName("檔案路徑")]
            public String? FilePath { get; set; }

            [DisplayName("檔案名稱")]
            public String? FileName { get; set; }
            [DisplayName("檔案PK")]
            public Guid? FileID { get; set; }
            [DisplayName("檔案下載")]
            public String? FileDownLoad { get; set; }
            /// <summary>
            /// 發布日期
            /// </summary>
            [DisplayName("發布日期")]
            public String N_CreateDate { get; set; }
            /// <summary>
            /// 結束日期
            /// </summary>
            [DisplayFormat(DataFormatString = "{0:yyyy/mm/dd}")]
            [DisplayName("結束日期")]
            public DateTime N_EndDate { get; set; }
        }
        #endregion
        #region Index
        public class SchModel
        {
            public SchItem schItem { get; set; }
            public List<schPageList> schPageList { get; set; }
            public IPagedList<schPageList> schPageList2 { get; set; }
        }
        #endregion

        #region 搜尋條件
        public class SchItem
        {

            public String? News_ID { get; set; }
            public String? rId { get; set; }
            public int? s_type { get; set; }
            public String? s_Title { get; set; }
            public DateTime s_StartDate { get; set; }
            public DateTime s_EndDate { get; set; }
            public String updDate { get; set; }
            public Guid? updUser { get; set; }
            /// <summary>
            /// 分類名稱
            /// </summary>
            [DisplayName("分類名稱")]
            public string NTypeName { get; set; }
        }
        #endregion

        #region 列表
        public class schPageList :PNews 
        {
            /// <summary>
            /// 發布日期
            /// </summary>
            [DisplayName("發布日期")]
            public String N_CreateDate { get; set; }
            /// <summary>
            /// 分類名稱
            /// </summary>
            [DisplayName("分類名稱")]
            public string NTypeName { get; set; }

            [DisplayName("公告時間")]
            public string NStartDateStr { get; set; }

            [DisplayName("時間排序")]
            public DateTime Date { get; set; }

        }
        #endregion
    }
}

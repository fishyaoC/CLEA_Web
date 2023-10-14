using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clea_Web.ViewModels
{
    public class B_LectorBtnViewModel : BaseViewModel
    {
        public UserRole userRole { get; set; }
        public Modify modify { get; set; }
        public SchModel schModel { get; set; }

        #region 選單
        public class UserRole
        {
            public List<SysMenu> lst_sysMenu { get; set; }
        }
        #endregion

        #region 編輯頁面
        public class Modify
        {
            public List<ViewMenuRolePower> rolePowerListB { get; set; }
            public List<ViewMenuRolePower> rolePowerListP { get; set; }
            public List<SelectListItem> DropDownList { get; set; }
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
            [DisplayName("公告類型")]
            public Int16 N_Type { get; set; }

            /// <summary>
            /// 公告標題
            /// </summary>
            [DisplayName("公告標題")]
            public String? N_Title { get; set; }

            /// <summary>
            /// 公告類別
            /// </summary>
            [DisplayName("公告類別")]
            public Int16? N_Class { get; set; }

            /// <summary>
            /// 開始日期
            /// </summary>
            [DisplayName("開始日期")]
            public DateTime N_StartDate { get; set; }

            /// <summary>
            /// 結束日期
            /// </summary>
            [DisplayName("結束日期")]
            public DateTime N_EndDate { get; set; }
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
            public String? N_Content { get; set; }
            /// <summary>
            /// 編輯狀態 True = 編輯 ; False = 新增
            /// </summary>
            [DisplayName("編輯狀態")]
            public Boolean IsEdit { get; set; } = false;
            /// <summary>
            /// 發布對象
            /// </summary>
            [DisplayName("發布對象")]
            public String? User1 { get; set; }
            /// <summary>
            /// 發布講師
            /// </summary>
            [DisplayName("發布講師")]
            public String? User2 { get; set; }
            [DisplayName("上傳檔案")]
            [Required(ErrorMessage = "請選擇檔案!")]
            public IFormFile? file { get; set; }

            [DisplayName("檔案路徑")]
            public String? FilePath { get; set; }

            [DisplayName("檔案名稱")]
            public String? FileName { get; set; }
            public Guid? File_ID { get; set; }
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

            public String? rId { get; set; }
            public String? rName { get; set; }
            public Int16? rOrder { get; set; }
            public Boolean? rStatus { get; set; }

        }
        #endregion

        #region 列表
        public class schPageList
        {
            public String rUid { get; set; }
            public String rId { get; set; }
            public String? rBackEnd { get; set; }
            public String rName { get; set; }
            public Int16 rOrder { get; set; }
            public String? rStatus { get; set; }
            public String creUser { get; set; }
            public String creDate { get; set; }
            public String updUser { get; set; }
            public String updDate { get; set; }
            //public DateTime mbCredateE { get; set; }


        }
        #endregion
    }
}

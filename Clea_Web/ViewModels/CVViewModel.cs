using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MathNet.Numerics.RootFinding;

namespace Clea_Web.ViewModels
{
    public class CVViewModel : BaseViewModel
    {
        public Modify modify { get; set; }
        public SchModel schModel { get; set; }


        #region 編輯頁面
        public class Modify
        {

            [DisplayName("Uid")]
            public Guid Uid { get; set; }

            /// <summary>
            /// 標題
            /// </summary>
            [DisplayName("標題")]
            [Required(ErrorMessage = "必填項目!")]
            public string Title { get; set; }

            /// <summary>
            /// 工作地點
            /// </summary>
            [DisplayName("工作地點")]
            [Required(ErrorMessage = "必填項目!")]
            public string Place { get; set; }

            /// <summary>
            /// 徵才人數
            /// </summary>
            [DisplayName("徵才人數")]
            [Required(ErrorMessage = "必填項目!")]
            public string Num { get; set; }

            /// <summary>
            /// 有效時間
            /// </summary>
            [DisplayName("有效時間")]
            [Required(ErrorMessage = "必填項目!")]
            public DateTime Exp { get; set; }

            /// <summary>
            /// 薪資待遇
            /// </summary>
            [DisplayName("薪資待遇")]
            [Required(ErrorMessage = "必填項目!")]
            public string Pay { get; set; }

            /// <summary>
            /// 求才機構
            /// </summary>
            [DisplayName("求才機構")]
            [Required(ErrorMessage = "必填項目!")]
            public Guid? Company { get; set; }

            /// <summary>
            /// 求才機構
            /// </summary>
            [DisplayName("求才機構")]
            [Required(ErrorMessage = "必填項目!")]
            public string? CompanyCName { get; set; }

            /// <summary>
            /// 聯絡人
            /// </summary>
            [DisplayName("聯絡人")]
            [Required(ErrorMessage = "必填項目!")]
            public string Contact { get; set; }

            /// <summary>
            /// 連絡電話
            /// </summary>
            [DisplayName("連絡電話")]
            [Required(ErrorMessage = "必填項目!")]
            public string Phone { get; set; }

            /// <summary>
            /// 公司地址
            /// </summary>
            [DisplayName("公司地址")]
            [Required(ErrorMessage = "必填項目!")]
            public string Address { get; set; }

            /// <summary>
            /// 電子郵件
            /// </summary>
            [DisplayName("電子郵件")]
            [Required(ErrorMessage = "必填項目!")]
            public string EMail { get; set; }

            /// <summary>
            /// 應徵方式
            /// </summary>
            [DisplayName("應徵方式")]
            [Required(ErrorMessage = "必填項目!")]
            public string Way { get; set; }

            /// <summary>
            /// 工作內容
            /// </summary>
            [DisplayName("工作內容")]
            [Required(ErrorMessage = "必填項目!")]
            public string Content { get; set; }

            /// <summary>
            /// 職務要求
            /// </summary>
            [DisplayName("職務要求")]
            [Required(ErrorMessage = "必填項目!")]
            public string Require { get; set; }

            /// <summary>
            /// 審核狀態
            /// </summary>
            [DisplayName("審核狀態")]
            public string? Approve { get; set; }


            /// <summary>
            /// 審核備註
            /// </summary>
            [DisplayName("審核備註")]
            public string? Memo { get; set; }

            /// <summary>
            /// 是否上架
            /// </summary>
            [DisplayName("是否上架")]
            [Required(ErrorMessage = "必填項目!")]
            public bool Status { get; set; } = true;

            /// <summary>
            /// 廠商上下架狀態
            /// </summary>
            [DisplayName("廠商上下架狀態")]
            public string? Close { get; set; }

            /// <summary>
            /// 廠商上下架時間
            /// </summary>
            [DisplayName("廠商上下架時間")]
            public string? CloseTime { get; set; }

            /// <summary>
            /// 可調整之點閱次數
            /// </summary>
            [DisplayName("可調整之點閱次數")]
            public int? Click { get; set; }


            [DisplayName("編輯狀態")]
            public Boolean IsEdit { get; set; }

            public List<SelectListItem> DropDownApprove { get; set; }
            public List<SelectListItem> DropDownCompany { get; set; }

        }
        #endregion

        #region Index
        public class SchModel
        {
            public List<SelectListItem> DropDownApprove { get; set; }
            public List<SelectListItem> DropDownCompany { get; set; }
            public SchItem schItem { get; set; }
            public List<schPageList> schPageList { get; set; }
            public IPagedList<schPageList> schPageList2 { get; set; }
        }
        #endregion

        #region 搜尋條件
        public class SchItem
        {
            public String? Company { get; set; }
            public String? isApprove { get; set; }

        }
        #endregion

        #region 列表
        public class schPageList
        {         
            public String? Uid { get; set; }
            public String? ApplyDate { get; set; }
            public String? Company { get; set; }
            public String Title { get; set; }
            public String? Approve { get; set; }
            public String? Status { get; set; }
            public String? Memo { get; set; }
            public int? ViewCount { get; set; }
            public String creDate { get; set; }
            public String updUser { get; set; }
            public String updDate { get; set; }
            public DateTime? StartD { get; set; }


        }
        #endregion
    }
}

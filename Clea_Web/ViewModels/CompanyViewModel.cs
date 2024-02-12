using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MathNet.Numerics.RootFinding;

namespace Clea_Web.ViewModels
{
    public class CompanyViewModel : BaseViewModel
    {
        public Modify modify { get; set; }
        public SchModel schModel { get; set; }


        #region 編輯頁面
        public class Modify
        {
            [DisplayName("Uid")]
            public Guid Uid { get; set; }

            /// <summary>
            /// 公司名稱
            /// </summary>
            [DisplayName("公司名稱")]
            [Required(ErrorMessage = "必填項目!")]
            public string Name { get; set; }

            /// <summary>
            /// 統一編號
            /// </summary>
            [DisplayName("統一編號")]
            [Required(ErrorMessage = "必填項目!")]
            public string ID { get; set; }

            /// <summary>
            /// 密碼
            /// </summary>
            [DisplayName("密碼")]
            [Required(ErrorMessage = "必填項目!")]
            public string? Password { get; set; }

            /// <summary>
            /// 連絡電話(市話)/手機
            /// </summary>
            [DisplayName("連絡電話(市話)/手機")]
            [Required(ErrorMessage = "必填項目!")]
            public string? Phone { get; set; }

            /// <summary>
            /// 傳真
            /// </summary>
            [DisplayName("傳真")]
            [Required(ErrorMessage = "必填項目!")]
            public string CellPhone { get; set; }

            /// <summary>
            /// 聯絡人
            /// </summary>
            [DisplayName("聯絡人")]
            [Required(ErrorMessage = "必填項目!")]
            public string Contact { get; set; }

            /// <summary>
            /// 公司地址
            /// </summary>
            [DisplayName("公司地址")]
            [Required(ErrorMessage = "必填項目!")]
            public string? Address { get; set; }

            /// <summary>
            /// 電子郵件
            /// </summary>
            [DisplayName("電子郵件")]
            public string? EMail { get; set; }

           
            /// <summary>
            /// 是否啟用
            /// </summary>
            [DisplayName("是否啟用")]
            [Required(ErrorMessage = "必填項目!")]
            public bool? Status { get; set; } = true;



            [DisplayName("編輯狀態")]
            public Boolean IsEdit { get; set; }

        }
        #endregion

        #region Index
        public class SchModel
        {
            public List<SelectListItem> DropDownID { get; set; }
            public List<SelectListItem> DropDownCompany { get; set; }

            public SchItem schItem { get; set; }
            public List<schPageList> schPageList { get; set; }
            public IPagedList<schPageList> schPageList2 { get; set; }
        }
        #endregion

        #region 搜尋條件
        public class SchItem
        {
            public String? ID { get; set; }
            public String? Name { get; set; }

        }
        #endregion

        #region 列表
        public class schPageList
        {         
            public String? Uid { get; set; }
            public String? Name { get; set; }
            public String? ID { get; set; }
            public String Password { get; set; }
            public String? Brithday { get; set; }
            public String? GraduatedSchool { get; set; }
            public String? Contact { get; set; }
            public String? Phone { get; set; }
            public String? CellPhone { get; set; }
            public String? Address { get; set; }
            public String? WorkPlace { get; set; }
            public String? EMail { get; set; }
            public String? LineID { get; set; }
            public String? Sex { get; set; }
            public String? Level { get; set; }
            public String? Status { get; set; }
            public String creDate { get; set; }
            public String updUser { get; set; }
            public String updDate { get; set; }
            public DateTime? StartD { get; set; }


        }
        #endregion
    }
}

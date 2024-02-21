using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MathNet.Numerics.RootFinding;

namespace Clea_Web.ViewModels
{
    public class MemberViewModel : BaseViewModel
    {
        public Modify modify { get; set; }
        public SchModel schModel { get; set; }


        #region 編輯頁面 for 會員
        public class Modify
        {
            public List<SelectListItem> DropDownLevel { get; set; }

            [DisplayName("Uid")]
            public Guid Uid { get; set; }

            /// <summary>
            /// 姓名
            /// </summary>
            [DisplayName("姓名")]
            [Required(ErrorMessage = "必填項目!")]
            public string Name { get; set; }

            /// <summary>
            /// 身分證字號
            /// </summary>
            [DisplayName("身分證字號")]
            [Required(ErrorMessage = "必填項目!")]
            public string ID { get; set; }

            /// <summary>
            /// 密碼
            /// </summary>
            [DisplayName("密碼")]
            public string? Password { get; set; }

            /// <summary>
            /// 畢業學校
            /// </summary>
            [DisplayName("畢業學校")]
            public string? School { get; set; }

            /// <summary>
            /// 出生年月日(西元)
            /// </summary>
            [DisplayName("出生年月日(西元)")]
            [Required(ErrorMessage = "必填項目!")]
            public DateTime? Brithday { get; set; }

            /// <summary>
            /// 連絡電話(市話)
            /// </summary>
            [DisplayName("連絡電話(市話)")]
            public string? Phone { get; set; }

            /// <summary>
            /// 手機號碼
            /// </summary>
            [DisplayName("手機號碼")]
            [Required(ErrorMessage = "必填項目!")]
            public string CellPhone { get; set; }

            /// <summary>
            /// 戶籍住址
            /// </summary>
            [DisplayName("戶籍住址")]
            [Required(ErrorMessage = "必填項目!")]
            public string Address { get; set; }

            /// <summary>
            /// 服務單位
            /// </summary>
            [DisplayName("服務單位")]
            public string? WorkPlace { get; set; }

            /// <summary>
            /// 電子郵件
            /// </summary>
            [DisplayName("電子郵件")]
            [Required(ErrorMessage = "必填項目!")]
            public string EMail { get; set; }

            /// <summary>
            /// LineID
            /// </summary>
            [DisplayName("LineID")]
            [Required(ErrorMessage = "必填項目!")]
            public string LineID { get; set; }
           
            /// <summary>
            /// 是否啟用
            /// </summary>
            [DisplayName("是否啟用")]
            [Required(ErrorMessage = "必填項目!")]
            public bool? Status { get; set; } = true;

            /// <summary>
            /// 會員等級
            /// </summary>
            [DisplayName("會員等級")]
            [Required(ErrorMessage = "必填項目!")]
            public string Level { get; set; }


            [DisplayName("編輯狀態")]
            public Boolean IsEdit { get; set; }

        }
        #endregion

        #region Index
        public class SchModel
        {
            public List<SelectListItem> DropDownLevel { get; set; }
            public List<SelectListItem> DropDownMember { get; set; }

            public SchItem schItem { get; set; }
            public List<schPageList> schPageList { get; set; }
            public IPagedList<schPageList> schPageList2 { get; set; }
        }
        #endregion

        #region 搜尋條件
        public class SchItem
        {
            public String? ID { get; set; }
            public String? Level { get; set; }

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

using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using X.PagedList;

namespace Clea_Web.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        public Modify modify { get; set; }
        public SchModel schModel { get; set; }


        #region 編輯頁面
        public class Modify
        {
            public List<SelectListItem> DropDownItem { get; set; }

            /// <summary>
            /// Uid
            /// </summary>
            [DisplayName("Uid")]
            public Guid UId { get; set; }

            /// <summary>
            /// 角色權限
            /// </summary>
            [DisplayName("角色權限")] 
            public Guid RUid { get; set; }

            /// <summary>
            /// 帳號
            /// </summary>
            [DisplayName("帳號")] 
            public string UAccount { get; set; } = null!;

            /// <summary>
            /// 密碼(非必填)
            /// </summary>
            [DisplayName("密碼")] 
            public string? UPassword { get; set; } = null!;

            /// <summary>
            /// 帳號名稱
            /// </summary>
            [DisplayName("帳號名稱")] 
            public string UName { get; set; } = null!;

            /// <summary>
            /// 帳號電子郵件
            /// </summary>
            [DisplayName("帳號電子郵件")] 
            public string UEmail { get; set; } = null!;

            /// <summary>
            /// 帳號連絡電話
            /// </summary>
            [DisplayName("帳號連絡電話")] 
            public string UPhone { get; set; } = null!;

            /// <summary>
            /// 帳號地址
            /// </summary>
            [DisplayName("帳號地址")] 
            public string UAddress { get; set; } = null!;

            /// <summary>
            /// 帳號性別:0女、1男
            /// </summary>
            [DisplayName("帳號性別")] 
            public byte USex { get; set; }

            /// <summary>
            /// 帳號生日
            /// </summary>
            [DisplayName("帳號生日")] 
            public DateTime UBirthday { get; set; }

            /// <summary>
            /// 帳號單位
            /// </summary>
            [DisplayName("帳號單位")] 
            public string UnId { get; set; } = null!;

            /// <summary>
            /// 啟用狀態 True = 啟用 ; False = 停用
            /// </summary>
            [DisplayName("啟用狀態")]
            public bool UStatus { get; set; } = true;

            /// <summary>
            /// 啟用狀態 True = 外聘 ; False = 非外聘
            /// </summary>
            [DisplayName("啟用狀態")]
            public bool isOutSide { get; set; } = false;

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
            public List<SelectListItem> DropDownItem { get; set; }
        }
        #endregion

        #region 搜尋條件
        public class SchItem
        {
            //帳號名稱
            public String? uName { get; set; }
            //帳號權限
            public Guid? urId { get; set; }

        }
        #endregion

        #region 列表
        public class schPageList
        {
            public String uUId { get; set; }
            public String rUId { get; set; }
            public String uAccount { get; set; }
            public String? uPassWord { get; set; }
            public String uName { get; set; }
            public String? uEmail { get; set; }
            public String? uPhone { get; set; }
            public String? uAddress { get; set; }
            public String? uSex { get; set; }
            public String? uBrithday { get; set; }
            public String? uID { get; set; }
            public String? uStatus { get; set; }
            public String creUser { get; set; }
            public String creDate { get; set; }
            public String updUser { get; set; }
            public String updDate { get; set; }
            public DateTime Date { get; set; }
            public String updUserC { get; set; }



        }
        #endregion
    }
}

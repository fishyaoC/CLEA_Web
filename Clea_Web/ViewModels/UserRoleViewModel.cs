using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;

namespace Clea_Web.ViewModels
{
    public class UserRoleViewModel : BaseViewModel
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
            //public List<ViewMenuRolePower> rolePowerListB { get; set; }
            //public List<ViewMenuRolePower> rolePowerListP { get; set; }

            public List<treeView> treeViewList { get; set; }
            public treeView treeView { get; set; }

            /// <summary>
            /// Uid
            /// </summary>
            [DisplayName("Uid")]
            public Guid? RUId { get; set; }

            /// <summary>
            /// 角色代碼
            /// </summary>
            [DisplayName("角色代碼")]
            public String? RId { get; set; }

            /// <summary>
            /// 是否為後台帳號
            /// </summary>
            [DisplayName("是否為後台帳號")]
            public Boolean RBackEnd { get; set; } = true;

            /// <summary>
            /// 角色名稱
            /// </summary>
            [DisplayName("角色名稱")]
            public String? RName { get; set; }

            /// <summary>
            /// 角色排序
            /// </summary>
            [DisplayName("角色排序")]
            public Int16? ROrder { get; set; }

            /// <summary>
            /// 角色啟用狀態
            /// </summary>
            [DisplayName("角色啟用狀態")]
            public Boolean RStatus { get; set; } = true;

            /// <summary>
            /// 編輯狀態 True = 編輯 ; False = 新增
            /// </summary>
            [DisplayName("編輯狀態")]
            public Boolean IsEdit { get; set; } = false;
        }
        #endregion

        #region treeView
        public class checkbox
        {
            public String? value { get; set; }
            public String? text { get; set; }
            public Boolean isChecked { get; set; }
        }
        public class treeView
        {
            //public List<checkbox> checkboxList { get; set; }
            public Guid RUid { get; set; }
            public Boolean MidCheck { get; set; }
            public long? MID { get; set; }
            public String? MType { get; set; }
            public String? MName { get; set; }
            public long? MParentID { get; set; }
            public long? MLevel { get; set; }
            public String? MUrl { get; set; }
            public int? MOrder { get; set; }

            public bool SearchData { get; set; } = false;
            public bool CreateData { get; set; } = false;
            public bool ModifyData { get; set; } = false;
            public bool DeleteData { get; set; } = false;
            public bool ImportData { get; set; } = false;
            public bool Exportdata { get; set; } = false;
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

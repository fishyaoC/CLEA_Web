using Clea_Web.Models;
using System.ComponentModel;

namespace Clea_Web.ViewModels
{
    public class UserRoleViewModel : BaseViewModel
    {
        public UserRole userRole { get; set; }
        public Modify modify { get; set; }

        #region 選單
        public class UserRole
        {
            public List<SysMenu> lst_sysMenu { get; set; }
        }
        #endregion

        #region 編輯頁面
        public class Modify
        {
            /// <summary>
            /// 角色代碼
            /// </summary>
            [DisplayName("角色代碼")]
            public String? RId { get; set; }

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
    }
}

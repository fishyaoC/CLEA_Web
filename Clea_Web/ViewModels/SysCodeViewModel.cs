using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Clea_Web.ViewModels
{
    public class SysCodeViewModel : BaseViewModel
    {
        public SysCode syscode { get; set; }
        public Modify modify { get; set; }
        public ChildList childList { get; set; }
        public SchModel schModel { get; set; }

        public class ChildList 
        {
            public int Order { get; set; }
            public string CItemName { get; set; }


        }

        #region 編輯頁面
        public class Modify
        {
            public List<ChildList>? modifies = new List<ChildList>();
            [DisplayName("Uid")]

            public Guid Uid { get; set; }

            /// <summary>
            /// 代號
            /// </summary>
            [DisplayName("選單代碼")]
            public string CItemCode { get; set; } = null!;

            /// <summary>
            /// 代碼名稱
            /// </summary>
            [DisplayName("選單名稱")]
            public string CItemName { get; set; } = null!;

            /// <summary>
            /// 開啟狀態
            /// </summary>
            [DisplayName("開啟狀態")]
            public bool? IsActive { get; set; }

            /// <summary>
            /// 顯示狀態
            /// </summary>
            [DisplayName("顯示狀態")]
            public bool? IsShow { get; set; }

            [DisplayName("編輯狀態")]
            public Boolean IsEdit { get; set; }

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
            public String? itemName { get; set; }
            public String? itemCode { get; set; }

            //public Int16? itemOrder { get; set; }
            //public Boolean isActive { get; set; }
            //public Boolean isShow { get; set; }


        }
        #endregion

        #region 列表
        public class schPageList
        {
            public String Uid { get; set; }
            public String cParentUid { get; set; }
            public String cParentCode { get; set; }
            public String itemName { get; set; }
            public int itemOrder { get; set; }
            public String? isActive { get; set; }
            public String? isShow { get; set; }
            public String? isEdit { get; set; }
            public String creDate { get; set; }
            public String updUser { get; set; }
            public String updDate { get; set; }
            //public DateTime mbCredateE { get; set; }


        }
        #endregion
    }
}

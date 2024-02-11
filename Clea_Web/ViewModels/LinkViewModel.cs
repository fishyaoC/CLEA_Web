using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MathNet.Numerics.RootFinding;

namespace Clea_Web.ViewModels
{
    public class LinkViewModel : BaseViewModel
    {
        public Modify modify { get; set; }
        public SchModel schModel { get; set; }


        #region 編輯頁面
        public class Modify
        {
            public List<SelectListItem> getTypeItem { get; set; }

            [DisplayName("Uid")]

            public Guid Uid { get; set; }

            /// <summary>
            /// 相關連結分類
            /// </summary>
            [DisplayName("相關連結分類")]
            public string Class { get; set; }

            /// <summary>
            /// 相關連結名稱
            /// </summary>
            [DisplayName("相關連結名稱")]
            public string Title { get; set; }

            /// <summary>
            /// 相關連結網址
            /// </summary>
            [DisplayName("相關連結網址")]
            public string Url { get; set; }

            /// <summary>
            /// 相關連結順序
            /// </summary>
            [DisplayName("相關連結順序")]
            public int? Order { get; set; }

            /// <summary>
            /// 是否上架
            /// </summary>
            [DisplayName("是否上架")]
            public bool Status { get; set; } = true;


            [DisplayName("編輯狀態")]
            public Boolean IsEdit { get; set; }

        }
        #endregion

        #region Index
        public class SchModel
        {
            public List<SelectListItem> DropDownItem { get; set; }
            public SchItem schItem { get; set; }
            public List<schPageList> schPageList { get; set; }
            public IPagedList<schPageList> schPageList2 { get; set; }
        }
        #endregion

        #region 搜尋條件
        public class SchItem
        {
            public String? Title { get; set; }
            public String? Class { get; set; }
        }
        #endregion

        #region 列表
        public class schPageList
        {         
            public String? Uid { get; set; }
            public String? Class { get; set; }
            public String? Title { get; set; }
            public String? Url { get; set; }
            public int? Order { get; set; }
            public String? Status { get; set; }
            public String creDate { get; set; }
            public String updUser { get; set; }
            public String updDate { get; set; }
        }
        #endregion
    }
}

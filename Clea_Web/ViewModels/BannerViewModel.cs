using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MathNet.Numerics.RootFinding;

namespace Clea_Web.ViewModels
{
    public class BannerViewModel : BaseViewModel
    {
        public Modify modify { get; set; }
        public SchModel schModel { get; set; }


        #region 編輯頁面
        public class Modify
        {

            [DisplayName("Uid")]
            public Guid Uid { get; set; }

            [DisplayName("上傳檔案")]
            public IFormFile? file { get; set; }

            /// <summary>
            /// 輪播圖名稱
            /// </summary>
            [DisplayName("輪播圖名稱")]
            [Required(ErrorMessage = "必填項目!")]
            public string Name { get; set; }

            /// <summary>
            /// 輪播圖連結
            /// </summary>
            [DisplayName("輪播圖連結")]
            public string Url { get; set; }

            /// <summary>
            /// 輪播圖起始日
            /// </summary>
            [DisplayName("輪播圖起始日")]
            public DateTime? StartDate { get; set; }

            /// <summary>
            /// 輪播圖結束日
            /// </summary>
            [DisplayName("輪播圖結束日")]
            public DateTime? EndDate { get; set; }

            /// <summary>
            /// 輪播圖排序
            /// </summary>
            [DisplayName("輪播圖排序")]
            [Range(0, 100, ErrorMessage = "範圍為0~100!")]
            public int? Order { get; set; }

            /// <summary>
            /// 是否上架
            /// </summary>
            [DisplayName("是否上架")]
            [Required(ErrorMessage = "必填項目!")]
            public bool Status { get; set; } = true;

            /// <summary>
            /// 輪播圖檔案
            /// </summary>
            [DisplayName("圖片預覽")]
            [Required(ErrorMessage = "必填項目!")]
            public string BannerIMG { get; set; }



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
            public String? BannerName { get; set; }
        }
        #endregion

        #region 列表
        public class schPageList
        {         
            public String? Uid { get; set; }
            public String? BannerName { get; set; }
            public String? BannerURL { get; set; }
            public String BannerStart { get; set; }
            public String BannerEnd { get; set; }
            public int? BannerOrder { get; set; }
            public String? BannerStatus { get; set; }
            public String creDate { get; set; }
            public String updUser { get; set; }
            public String updDate { get; set; }
            public DateTime? BannerStartD { get; set; }
            public String BannerIMG { get; set; }


        }
        #endregion
    }
}

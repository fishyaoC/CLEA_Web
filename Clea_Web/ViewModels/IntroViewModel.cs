using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MathNet.Numerics.RootFinding;

namespace Clea_Web.ViewModels
{
    public class IntroViewModel : BaseViewModel
    {
        public Rate rate { get; set; }

        public Nav nav { get; set; }
        public SchModel schModel { get; set; }


        #region 編輯頁面
        public class Rate
        {

            [DisplayName("Uid")]
            public Guid Uid { get; set; }

            [DisplayName("上傳檔案")]
            public IFormFile? file { get; set; }

            /// <summary>
            /// 標題
            /// </summary>
            [DisplayName("標題")]
            [Required(ErrorMessage = "必填項目!")]
            public string Title { get; set; }

            /// <summary>
            /// 備註文字
            /// </summary>
            [DisplayName("備註文字")]
            [Required(ErrorMessage = "必填項目!")]
            public string Memo { get; set; }

            /// <summary>
            /// 排列順序
            /// </summary>
            [DisplayName("排列順序")]
            [Required(ErrorMessage = "必填項目!")]
            public int? Order { get; set; }

            /// <summary>
            /// 是否上架
            /// </summary>
            [DisplayName("是否上架")]
            [Required(ErrorMessage = "必填項目!")]
            public bool Status { get; set; }

            /// <summary>
            /// 輪播圖檔案
            /// </summary>
            [DisplayName("圖片預覽")]
            [Required(ErrorMessage = "必填項目!")]
            public string IMG { get; set; }



            [DisplayName("編輯狀態")]
            public Boolean IsEdit { get; set; }

        }


        public class Nav
        {

            [DisplayName("Uid")]
            public Guid Uid { get; set; }

            [DisplayName("上傳檔案")]
            public IFormFile? file { get; set; }

            /// <summary>
            /// 標題
            /// </summary>
            [DisplayName("標題")]
            [Required(ErrorMessage = "必填項目!")]
            public string Title { get; set; }

            /// <summary>
            /// 備註文字
            /// </summary>
            [DisplayName("備註文字")]
            [Required(ErrorMessage = "必填項目!")]
            public string Memo { get; set; }

            /// <summary>
            /// 排列順序
            /// </summary>
            [DisplayName("排列順序")]
            [Required(ErrorMessage = "必填項目!")]
            public int Order { get; set; }

            /// <summary>
            /// 是否上架
            /// </summary>
            [DisplayName("是否上架")]
            [Required(ErrorMessage = "必填項目!")]
            public bool Status { get; set; }

            /// <summary>
            /// 輪播圖檔案
            /// </summary>
            [DisplayName("圖片預覽")]
            [Required(ErrorMessage = "必填項目!")]
            public string IMG { get; set; }


            /// <summary>
            /// 地址
            /// </summary>
            [DisplayName("地址")]
            [Required(ErrorMessage = "必填項目!")]
            public string Address { get; set; }

            /// <summary>
            /// 電話
            /// </summary>
            [DisplayName("地址")]
            [Required(ErrorMessage = "必填項目!")]
            public string Phone { get; set; }

            /// <summary>
            /// 傳真
            /// </summary>
            [DisplayName("地址")]
            [Required(ErrorMessage = "必填項目!")]
            public string Fax { get; set; }

            /// <summary>
            /// 內嵌地圖
            /// </summary>
            [DisplayName("內嵌地圖")]
            [Required(ErrorMessage = "必填項目!")]
            public string Embed { get; set; }

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
        }
        #endregion

        #region 列表
        public class schPageList
        {         
            public String? Uid { get; set; }
            public String? Title { get; set; }
            public int? Order { get; set; }
            public String? Status { get; set; }
            public String creDate { get; set; }
            public String updUser { get; set; }
            public String updDate { get; set; }
            public String IMG { get; set; }


        }
        #endregion
    }
}

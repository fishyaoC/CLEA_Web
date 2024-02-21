using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MathNet.Numerics.RootFinding;
using static Clea_Web.ViewModels.BtnViewModel;

namespace Clea_Web.ViewModels
{
    public class FileDownloadViewModel : BaseViewModel
    {
        public Modify modify { get; set; }
        public SchModel schModel { get; set; }

        #region Index
        public class SchModel
        {
            public List<SelectListItem> DropDownLevel { get; set; }
            public List<SelectListItem> DropDownClass { get; set; }

            public SchItem schItem { get; set; }
            public List<schPageList> schPageList { get; set; }
            public IPagedList<schPageList> schPageList2 { get; set; }
        }
        #endregion

        #region 搜尋條件
        public class SchItem
        {
            public String? Class { get; set; }
            public String? Level { get; set; }

        }
        #endregion

        #region 列表
        public class schPageList
        {         
            public String? Uid { get; set; }
            public String? Title { get; set; }
            public String? Class { get; set; }
            public String Level { get; set; }
            public String? Status { get; set; }
            public String creDate { get; set; }
            public String updUser { get; set; }
            public String updDate { get; set; }
            public DateTime? StartD { get; set; }


        }
        #endregion

        #region 編輯頁面
        public class Modify
        {
            public List<SelectListItem> DropDownLevel { get; set; }
            public List<SelectListItem> DropDownClassID { get; set; }
            public List<SelectListItem> DropDownClass { get; set; }


            [DisplayName("Uid")]
            public Guid Uid { get; set; }

            /// <summary>
            /// 標題
            /// </summary>
            [DisplayName("標題")]
            [Required(ErrorMessage = "必填項目!")]
            public string Title { get; set; }

            /// <summary>
            /// 下載分類
            /// </summary>
            [DisplayName("下載分類")]
            [Required(ErrorMessage = "必填項目!")]
            public string Class { get; set; }

            /// <summary>
            /// 選擇權限
            /// </summary>
            [DisplayName("選擇權限")]
            [Required(ErrorMessage = "必填項目!")]
            public int? Level { get; set; }

            /// <summary>
            /// 選擇課程
            /// </summary>
            [DisplayName("選擇課程")]
            public Guid? ClassID { get; set; }
           
            /// <summary>
            /// 是否啟用
            /// </summary>
            [DisplayName("是否啟用")]
            [Required(ErrorMessage = "必填項目!")]
            public bool Status { get; set; } = true;

            /// <summary>
            /// 是否置頂
            /// </summary>
            [DisplayName("是否置頂")]
            [Required(ErrorMessage = "必填項目!")]
            public bool isTop { get; set; } = false;

            [DisplayName("編輯狀態")]
            public Boolean IsEdit { get; set; }

            [DisplayName("上傳檔案")]
            public IFormFile? file { get; set; }
            [DisplayName("檔案路徑")]
            public String? FilePath { get; set; }
            [DisplayName("檔案名稱")]
            public String? FileName { get; set; }
            [DisplayName("檔案PK")]
            public Guid? FileID { get; set; }

            public List<FileModel> fileModels { get; set; }

        }
        #endregion


    }
}

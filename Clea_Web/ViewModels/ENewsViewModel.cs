using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MathNet.Numerics.RootFinding;

namespace Clea_Web.ViewModels
{
    public class ENewsViewModel : BaseViewModel
    {
        public Modify modify { get; set; }
        public SchModel schModel { get; set; }


        #region 編輯頁面
        public class Modify
        {

            [DisplayName("Uid")]
            public Guid Uid { get; set; }

            [DisplayName("上傳檔案")]
            [Required(ErrorMessage = "必填項目!")]
            public IFormFile? file { get; set; }

            [DisplayName("檔案路徑")]
            public String? FilePath { get; set; }
            [DisplayName("檔案名稱")]
            public String? FileName { get; set; }
            [DisplayName("檔案PK")]
            public Guid? FileID { get; set; }

            /// <summary>
            /// 標題
            /// </summary>
            [DisplayName("標題")]
            [Required(ErrorMessage = "必填項目!")]
            public string Title { get; set; }

            /// <summary>
            /// 發布日期
            /// </summary>
            [DisplayName("發布日期")]
            [Required(ErrorMessage = "必填項目!")]
            public DateTime StartDate { get; set; }

            /// <summary>
            /// 是否上架
            /// </summary>
            [DisplayName("是否上架")]
            [Required(ErrorMessage = "必填項目!")]
            public bool Status { get; set; } = true;

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
        }
        #endregion

        #region 列表
        public class schPageList
        {         
            public String? Uid { get; set; }
            public String? Title { get; set; }
            public String? StartDate { get; set; }
            public String Status { get; set; }
            public int? NewsViews { get; set; }
            public String creDate { get; set; }
            public String updUser { get; set; }
            public String updDate { get; set; }
            public DateTime? ENewsStartD { get; set; }
        }
        #endregion
    }
}

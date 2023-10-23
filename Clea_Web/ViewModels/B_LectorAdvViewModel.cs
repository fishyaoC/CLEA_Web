using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Clea_Web.ViewModels.AssignClassViewModel;
using System.Text;

namespace Clea_Web.ViewModels
{
    public class B_LectorAdvViewModel : BaseViewModel
    {
        public Modify modify { get; set; }
        public SchModel schModel { get; set; }

        #region Index
        public class SchModel : CLectorAdvInfo
        {
            public SchItem schItem { get; set; }
            public List<schPageList> schPageList { get; set; }
            public IPagedList<schPageList> schPageList2 { get; set; }
        }

        #region 列表
        public class schPageList
        {
            public String LUid { get; set; }
            public String LName { get; set; }
            public int? LaYear { get; set; }
            public int YearNow { get; set; }
        }
        #endregion

        #region 搜尋條件
        public class SchItem
        {
            public String? LaYear { get; set; }
            public String? LName { get; set; }
        }
        #endregion

        #endregion

        #region D_INDEX
        public class D_Model
        {
            public Guid L_UID { get; set; }
            public String LName { get; set; }

            public int YearNow { get; set; }
            public IPagedList<D_PageList> D_PageList { get; set; }
        }

        public class D_PageList
        {
            public String LaUid { get; set; }

            public String LUid { get; set; }
            public String LName { get; set; }
            public int? LaYear { get; set; }
            public int YearNow { get; set; }
            public String LaTitle { get; set; }
            public String FileName { get; set; }
            public String creUser { get; set; }
            public String creDate { get; set; }
            public String updUser { get; set; }
            public String updDate { get; set; }

        }
        #endregion

        #region Modify
        public class Modify : CLectorAdvInfo
        {
            [DisplayName("上傳時間")]
            public String? UptDate { get; set; }
            [DisplayName("使用者")]
            public String? LName { get; set; }
            [DisplayName("檔案路徑")]
            public String? FilePath { get; set; }

            [DisplayName("檔案名稱")]
            public String? FNameReal { get; set; }
            [DisplayName("檔案副檔名")]
            public String? FExt { get; set; }
            [DisplayName("檔案PK")]
            public Guid? FileID { get; set; }
            /// <summary>
            /// 編輯狀態 True = 編輯 ; False = 新增
            /// </summary>
            [DisplayName("編輯狀態")]
            public Boolean IsEdit { get; set; } = false;

            [DisplayName("上傳檔案")]
            [Required(ErrorMessage = "請選擇上傳檔案!")]
            public IFormFile file { get; set; }
        }
        #endregion



    }
}

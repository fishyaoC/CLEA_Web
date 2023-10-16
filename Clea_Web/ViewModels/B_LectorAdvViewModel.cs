using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clea_Web.ViewModels
{
    public class B_LectorAdvViewModel : BaseViewModel
    {
        public Modify modify { get; set; }
        public SchModel schModel { get; set; }

        #region 選單
        public class UserRole
        {
            public List<SysMenu> lst_sysMenu { get; set; }
        }
        #endregion

        #region 編輯頁面
        public class Modify : CLectorAdvInfo
        {
            public List<SelectListItem> DropDownList { get; set; }
            /// <summary>
            /// 編輯狀態 True = 編輯 ; False = 新增
            /// </summary>
            [DisplayName("編輯狀態")]
            public Boolean IsEdit { get; set; } = false;
            [DisplayName("檔案路徑")]
            public String? LA_Year { get; set; }
            
            [DisplayName("上傳檔案")]
            [Required(ErrorMessage = "請選擇檔案!")]
            public IFormFile? file { get; set; }

            [DisplayName("檔案路徑")]
            public String? FilePath { get; set; }

            [DisplayName("檔案名稱")]
            public String? FileName { get; set; }
            public Guid? File_ID { get; set; }
        }
        #endregion
        #region Index
        public class SchModel : CLectorAdvInfo
        {
            public SchItem schItem { get; set; }
            public List<schPageList> schPageList { get; set; }
            public IPagedList<schPageList> schPageList2 { get; set; }
        }
        #endregion

        #region 搜尋條件
        public class SchItem : CLectorAdvInfo
        {

            public String? News_ID { get; set; }
            public String? rId { get; set; }
            public int? s_type { get; set; }
            public String? s_Title { get; set; }
            public DateTime s_StartDate { get; set; }
            public DateTime s_EndDate { get; set; }
            public String updDate { get; set; }
            public Guid? updUser { get; set; }
        }
        #endregion

        #region 列表
        public class schPageList : CLectorAdvInfo
        {



        }
        #endregion
    }
}

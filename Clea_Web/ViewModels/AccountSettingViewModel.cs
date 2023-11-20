using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Clea_Web.ViewModels.AssignClassViewModel;
using System.Text;

namespace Clea_Web.ViewModels
{
    public class AccountSettingViewModel : BaseViewModel
    {
        public Modify modify { get; set; }


        #region Modify
        public class Modify : SysUser
        {
            [DisplayName("舊密碼")]
            public String? OldPW { get; set; }
            [DisplayName("新密碼")]
            public String? NewPW { get; set; }
            [DisplayName("再次確認新密碼")]
            public String? CheckNewPW { get; set; }
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
            public IFormFile file { get; set; }
            public String? FileName { get; set; }
        }
        #endregion



    }
}

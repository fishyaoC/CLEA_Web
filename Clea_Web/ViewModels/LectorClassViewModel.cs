using Clea_Web.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace Clea_Web.ViewModels
{
    public class LectorClassViewModel : BaseViewModel
    {
		#region Index
		public IPagedList<ClassMenu> classMenuPageList { get; set; }
		public class ClassMenu
		{
			public Guid ES_ID { get; set; }
			public String ClassName { get; set; }
			public String SubName { get; set; }					
			public Boolean IsUpload { get; set; }
			public Int32 Status { get; set; }
			public DateTime CREDATE { get; set; }
			public Boolean IsClose { get; set; }
		}

		#endregion

		#region Modify
		public class ModifyModel
		{
			public Modify modify { get; set; }
			public List<UploadLog> uploadLogs { get; set; }
		}

		public class UploadLog
		{
			public String F_Name { get; set; }
			public Boolean IsUpdate { get; set; }
			public Int32? Status { get; set; }
			public String? Other { get; set; }
			public DateTime CreDate { get; set; }
		}

		public class Modify
		{
			public Guid ES_ID { get; set; }

			public Int32 Year { get; set; }

			public String C_Name { get; set; }

			public String S_Name { get; set; }
			[DisplayName("教學大綱")]
			[Required(ErrorMessage = "必填欄位!")]
			public String Syllabus { get; set; }

			[DisplayName("教學目標")]
			[Required(ErrorMessage = "必填欄位!")]
			public String Object { get; set; }

			[DisplayName("內容摘要")]
			[Required(ErrorMessage = "必填欄位!")]
			public String Abstract { get; set; }

			[DisplayName("上傳檔案")]
			[Required(ErrorMessage ="請選擇上傳檔案!")]
			public IFormFile file { get; set; }

			public Guid? F_ID { get; set; }
			public String? FileName { get; set; }

			[DisplayName("更新教材內容")]
			[Required]
			public Boolean IsUpdate { set; get; } = false;
			[Required(ErrorMessage = "請選擇更新內容!")]
			public Int32? UpdContent { set; get; }
			[Required(ErrorMessage = "請填入其他內容!")]
			public String? Other { get; set; }
			public Int32 Status { get; set; }
			public Boolean IsClose { get; set; }
			
		}
		#endregion
	}
}

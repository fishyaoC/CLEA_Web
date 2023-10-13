using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Clea_Web.ViewModels
{
	//教材評鑑
	public class AssignBookViewModel : BaseViewModel
	{
		#region BackEnd

		#region 查詢列表
		public class schBookModel
		{
			public SchBookItem schBookItem { get; set; }
			public IPagedList<BookInfo> bookInfosPageList { get; set; }
		}

		public class SchBookItem
		{
			[DisplayName("教材編號")]
			public String? B_ID { get; set; }

			[DisplayName("教材名稱")]
			public String? B_Name { get; set; }
		}

		public class BookInfo
		{
			public Guid B_UID { get; set; }

			public String? B_ID { get; set; }

			public String? B_Name { get; set; }

			[DisplayName("版本")]
			public String? B_Version { get; set; }

			[DisplayName("出版人")]
			public String? B_Publish { get; set; }

			[DisplayName("證號")]
			public String? B_CNumber { get; set; }

			[DisplayName("MEMO")]
			public String? B_Memo { get; set; }

			[DisplayName("是否上傳檔案")]
			public Boolean? IsUpload { get; set; }
		}

		public class P_Teacher
		{
			public Guid L_UID_Ev { get; set; }
			public String L_ID_Ev { get; set; }
			public String L_Name_Ev { get; set; }
		}
		#endregion

		#region Modify

		#region GetModel

		public BookInfoModel bookInfoModel { get; set; }
		public class BookInfoModel : BookInfo
		{
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

		#region SaveData
		#endregion

		#endregion

		#region P_Modify
		public class P_Modify_Model
		{
			public BookInfo bookInfo { get; set; }
			public List<SelectListItem> DropDownList { get; set; }

			public IPagedList<P_Teacher> pTeacherPagedList { get; set; }

			[DisplayName("評鑑教師")]
			[Required(ErrorMessage = "請指定評鑑教師!")]
			public Guid L_UID_Ev { get; set; }
		}
		#endregion

		#endregion

		#region Portal
		#endregion
	}
}

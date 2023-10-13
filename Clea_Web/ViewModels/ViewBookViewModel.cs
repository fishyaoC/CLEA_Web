using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using static Clea_Web.ViewModels.AssignClassViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Clea_Web.ViewModels
{
	//課程評鑑成績
	public class ViewBookViewModel : BaseViewModel
	{

		#region 查詢列表		

		#region 查詢列表
		public class schBookModel
		{			
			public SchBookItem schBookItem { get; set; }
			public IPagedList<BookInfo> bookInfosPageList { get; set; }
			public List<SelectListItem> selectList { get; set; }
		}

		public class SchBookItem
		{
			public Int32? Year { get; set; }

			[DisplayName("教材編號")]
			public String? B_ID { get; set; }

			[DisplayName("教材名稱")]
			public String? B_Name { get; set; }
		}

		public class BookInfo
		{
			public Guid CLv_UID { get; set; }
			public Guid B_UID { get; set; }
			public Int32 Year { get; set; }

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
		#endregion

		#region 評鑑教師

		public class P_TeacherInfo
		{
			public BookInfo bookInfo { get; set; }
			public IPagedList<P_Teacher> p_TeachersPageList { get; set; }
		}
		public class P_Teacher
		{			
			public Guid LEv_UID { get; set; }
			public String L_ID_Ev { get; set; }
			public String L_Name_Ev { get; set; }
			public Boolean IsEvaluation { get; set; }
		}
		#endregion

		#endregion




		#region 成績編輯頁		
		public class Modify_Score
		{
			//public CSTinfo_V cSTinfo_V { get; set; }
			public List<String> picPath { get; set; }
			public ScoreModify scoreModify { get; set; }
		}

		public class ScoreModify
		{
			public Guid CLeID { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 100, ErrorMessage = "※分數範圍:0~100")]
			public Int32? Score_A { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 100, ErrorMessage = "※分數範圍:0~100")]
			public Int32? Score_B { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 100, ErrorMessage = "※分數範圍:0~100")]
			public Int32? Score_C { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 100, ErrorMessage = "※分數範圍:0~100")]
			public Int32? Score_D { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 100, ErrorMessage = "※分數範圍:0~100")]
			public Int32? Score_E { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 100, ErrorMessage = "※分數範圍:0~100")]
			public Int32? Score_F { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 100, ErrorMessage = "※分數範圍:0~100")]
			public Int32? Score_G { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 100, ErrorMessage = "※分數範圍:0~100")]
			public Int32? Score_H { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 100, ErrorMessage = "※分數範圍:0~100")]
			public Int32? Score_I { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 100, ErrorMessage = "※分數範圍:0~100")]
			public Int32? Score_J { get; set; }

			public String? Remark { get; set; }
		}

		#endregion

	}
}

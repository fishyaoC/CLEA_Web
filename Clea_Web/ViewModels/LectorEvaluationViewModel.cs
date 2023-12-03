using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Clea_Web.ViewModels
{
	//評鑑模組 
	public class LectorEvaluationViewModel : BaseViewModel
	{
		#region Index
		public IPagedList<schEvInfo> schEvInfosPageLists { get; set; }
		public class schEvInfo
		{
			public Guid ED_ID { get; set; }
			public String mType { get; set; }
			public Int32 Status { get; set; }
			public String ClassName_BookName { get; set; }
			public String SubName_PName { get; set; }
			public Boolean IsUpload { get; set; }
			public DateTime CreDate { get; set; }			
		}

		#endregion

		#region Modify
		public class ScoreModel
		{
			public EvInfo evInfo { get; set; }
			public scoreModify scoreModify { get; set; }
		}

		public class EvInfo
		{
			public Int32 Year { get; set; }
			public String mType { get; set; }
			public String L_Name { get; set; }
			public String C_B_Name { get; set; }
			public String S_P_Name { get; set; }
		}
		public class scoreModify
		{
			public Guid ED_ID { get; set; }

			public Int32 mType { get; set; }

			public List<String> lst_pic { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 60, ErrorMessage = "範圍為0~60!")]
			public Int32? ScoreA { get; set; }
			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 10, ErrorMessage = "範圍為0~10!")]
			public Int32? ScoreB { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 20, ErrorMessage = "範圍為0~20!")]
			public Int32? ScoreBB { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 10, ErrorMessage = "範圍為0~10!")]
			public Int32? ScoreC { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 20, ErrorMessage = "範圍為0~20!")]
			public Int32? ScoreCB { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 10, ErrorMessage = "範圍為0~10!")]
			public Int32? ScoreD { get; set; }
			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 10, ErrorMessage = "範圍為0~10!")]
			public Int32? ScoreE { get; set; }
			public String? Remark { get; set; }
			public Boolean IsClose { get; set; }
			public Int32 Status { get; set; }
		}
		#endregion
	}
}

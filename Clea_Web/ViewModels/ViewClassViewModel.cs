using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using static Clea_Web.ViewModels.AssignClassViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Clea_Web.ViewModels
{
	//課程評鑑成績
	public class ViewClassViewModel : BaseViewModel
	{

		#region 查詢列表		

		#region 課程列表
		public class SchClassModel_V
		{
			public List<SelectListItem> DropDownList_Year { get; set; }
			public SchClassItem_V schClassItem_V { get; set; }
			public IPagedList<GetClassList_V> getClassList_Vs { get; set; }
		}

		public class SchClassItem_V : SchClassItem
		{
			[DisplayName("年度")]
			public Int32? CEv_Year { get; set; }
		}

		public class GetClassList_V : GetClassList
		{
			[DisplayName("年度")]
			public Int32 CEv_Year { get; set; }
		}
		#endregion

		#region 科目列表		
		public class SchSubLecModel_V
		{
			public SchSubLecItem_V schSubLecItem_V { get; set; }
			public IPagedList<GetSubLecList_V> getSubLecPageLists_V { get; set; }
			public EvaluationActonInfo EvaluationActonInfo { get; set; }
		}

		public class SchSubLecItem_V : SchSubLecItem
		{

		}

		public class GetSubLecList_V : GetSubLecList
		{
			public Int32 Year { get; set; }
		}
		#endregion

		#region 授課教師評鑑列表
		public class CL_Model_V
		{
			public ClassInfo_V ClassInfo_V { get; set; }
			public IPagedList<GetClassLector_V> getClassLectorPageLists_V { get; set; }
		}

		public class ClassInfo_V : ClassInfo
		{
			public Int32 Year { get; set; }
			public Guid CL_UID { get; set; }
		}

		public class GetClassLector_V : GetClassLector
		{

		}
		#endregion

		#region 檢視指定教師頁面

		public class Modify_Model_V
		{
			public CSTinfo_V cSTinfo_V { get; set; }
			//public List<SelectListItem> DropDownItem { get; set; }
			//public LModify lModify { get; set; }
			public IPagedList<M_EvTeacher_V> m_EvTeacherPageLists_V { get; set; }
		}
		public class CSTinfo_V : ClassInfo_V
		{
			public Int32 Ev_Year { get; set; }
			public Guid CL_UID { get; set; }
			public Guid L_UID { get; set; }
			public String L_ID { get; set; }
			public String L_Name { get; set; }
		}
		public class M_EvTeacher_V
		{
			public Guid CEv_UID { get; set; }
			public String L_ID_Ev { get; set; }
			public String L_Name_Ev { get; set; }
			public Boolean IsEvaluation { get; set; }
		}
		#endregion

		#endregion

		#region 成績編輯頁		
		public class Modify_Score
		{
			public CSTinfo_V cSTinfo_V { get; set; }
			public List<String> picPath { get; set; }
			public ScoreModify scoreModify { get; set; }
		}

		public class ScoreModify
		{
			public Guid CLeID { get; set; }

			[Required(ErrorMessage ="此欄位為必填!")]
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

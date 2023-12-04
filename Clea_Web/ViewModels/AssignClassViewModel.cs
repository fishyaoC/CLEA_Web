using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Clea_Web.ViewModels
{
	//課程評鑑
	public class AssignClassViewModel : BaseViewModel
	{

		#region NEW

		#region 查詢頁面
		public class SearchModel
		{
			public List<SelectListItem> selectListItems { get; set; }
			public schClassItem schClassItem { get; set; }
			public IPagedList<ClassInfor> classInfoPageLists { get; set; }
		}
		#endregion

		#region 新增頁面
		public class AddModel
		{
			public List<SelectListItem> Year_selectListItems { get; set; }
			public List<SelectListItem> Class_selectListItems { get; set; }
			public addModify addModify { get; set; }

		}

		public class addModify
		{
			[DisplayName("課程")]
			[Required(ErrorMessage = "請選擇課程!")]
			public Guid C_UID { get; set; }

			[DisplayName("年度")]
			[Required(ErrorMessage = "請選擇年度!")]
			public Int32 Year { get; set; }
		}

		#endregion

		#region TC_INDEX
		public class TC_Model
		{
			public IPagedList<CL> cLPageLists { get; set; }
			public CLschItem cLschItem { get; set; }
			public Guid E_ID { get; set; }
			public EvaluationActonInfo evaluationActonInfo { get; set; }
		}

		public class CLschItem
		{
			public String? S_Name { get; set; }
			public String? L_Name { get; set; }
		}

		public class CL
		{
			public Guid E_ID { get; set; }
			public Guid ES_ID { get; set; }
			public String C_Name { get; set; }
			public String S_Name { get; set; }
			public String L_Name { get; set; }
			public Boolean IsEvaluate { get; set; }
			public Int32 Status { get; set; }
			public Int32 ScheNum { get; set; }
			public Int32? D_Hour { get; set; }
		}
		#endregion

		#region TC_Moidify
		public class TC_ModifyModel
		{
			public List<SelectListItem> selectListItems { get; set; }
			public CLInfo cLInfo { get; set; }

			public TCModify tCModify { get; set; }
			public List<EvTeacher> lst_EvTeachers { get; set; }
		}

		public class TCModify
		{
			public Guid E_ID { get; set; }
			public Guid ES_ID { get; set; }

			[DisplayName("指定評鑑教師")]
			[Required(ErrorMessage = "{0} 為必填!")]
			public Guid L_UID_Ev { get; set; }
		}

		public class CLInfo
		{
			public Guid ES_ID { get; set; }
			public Int32 Year { get; set; }
			public Guid L_UID { get; set; }
			public String L_ID { get; set; }
			public String L_Name { get; set; }
			public String S_ID { get; set; }
			public String S_Name { get; set; }
			public String C_ID { get; set; }
			public String C_Name { get; set; }
		}

		public class EvTeacher
		{
			public Guid? E_ID { get; set; }
			public Guid ED_ID { get; set; }
			public Guid? L_UID_Ev { get; set; }
			public String L_Ev_ID { get; set; }
			public String L_Ev_Name { get; set; }
		}
		#endregion

		#region V_Modify
		public class V_ModifyModel
		{
			public CLInfo cLInfo { get; set; }
			public List<String> picPath { get; set; }
			public V_ScoreModel v_ScoreModel { get; set; }
		}

		public class V_ScoreModel
		{
			public Guid ED_ID { get; set; }
			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 60, ErrorMessage = "※分數範圍:0~60")]
			public Int32? Score_A { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 10, ErrorMessage = "※分數範圍:0~10")]
			public Int32? Score_B { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 10, ErrorMessage = "※分數範圍:0~10")]
			public Int32? Score_C { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 10, ErrorMessage = "※分數範圍:0~10")]
			public Int32? Score_D { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			[Range(0, 10, ErrorMessage = "※分數範圍:0~10")]
			public Int32? Score_E { get; set; }

			public String? Remark { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			public String? Syllabus { get; set; }
			[Required(ErrorMessage = "此欄位為必填!")]
			public String? Object { get; set; }
			[Required(ErrorMessage = "此欄位為必填!")]
			public String? Abstract { get; set; }
			public Int32 Status { get; set; }
			public Boolean IsClose { get; set; }
		}
		#endregion

		#region V_Index
		public class vIndexModel
		{
			public Guid E_ID { get; set; }
			public Guid ES_ID { get; set; }
			public List<vIndexList> vIndexLists { get; set; }
		}
		public class vIndexList
		{
			public Guid ED_ID { get; set; }
			public String Lv_Teacher { get; set; }
			public Int32 Status { get; set; }
		}
		#endregion

		#region ScoreView
		public class S_Model
		{
			public Guid E_ID { get; set; }
			public Guid ES_ID { get; set; }
			public String ClassType { get; set; }
			public String ClassName { get; set; }
			public String ClassSub { get; set; }
			public String SubClassTime { get; set; }
			public String Syllabus { get; set; }
			public String Objectives { get; set; }
			public String ClassPlace { get; set; }
			public String ClassTeacher { get; set; }
			public String Abstract { get; set; }
			public String BookNamePublish { get; set; }
			public String BookNumber { get; set; }
			public String Write { get; set; }
			public Boolean? IsPass { get; set; }
		}
		#endregion
		public class schClassItem
		{
			public Int32? Year { get; set; }
			public String? C_Number { get; set; }
			public String? C_Name { get; set; }
		}

		public class ClassInfor
		{
			public Guid E_ID { get; set; }
			public Int32 Year { get; set; }
			public String? C_ID { get; set; }
			public String? C_Name { get; set; }
		}

		#endregion

	}
}

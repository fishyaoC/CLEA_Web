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
			public Guid ED_ID { get; set; }
			public Guid CL_UID { get; set; }
			public String C_Name { get; set; }
			public String S_Name { get; set; }
			public String L_Name { get; set; }
			public Boolean IsEvaluate { get; set; }
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
			public Guid CL_UID { get; set; }

			[DisplayName("指定評鑑教師")]
			[Required(ErrorMessage = "{0} 為必填!")]
			public Guid L_UID_Ev { get; set; }
		}

		public class CLInfo
		{
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

		#region OLD
		#region 查詢列表

		#region 課程列表	
		public class SchClassModel
		{
			public SchClassItem schClassItem { get; set; }
			public IPagedList<GetClassList> getClassPageList { get; set; }
		}

		public class SchClassItem
		{
			[DisplayName("課程代碼")]
			public String? ClassID { get; set; }

			[DisplayName("課程名稱")]
			public String? ClassName { get; set; }

			[DisplayName("課程類別")]
			public String? ClassType { get; set; }

			[DisplayName("教材編號")]
			public String? BookNumber { get; set; }
		}

		public class GetClassList
		{
			[DisplayName("KEY")]
			public Guid ClassUid { get; set; }

			[DisplayName("課程代碼")]
			public String ClassID { get; set; }

			[DisplayName("課程名稱")]
			public String? ClassName { get; set; }

			[DisplayName("課程類別")]
			public String? ClassType { get; set; }

			[DisplayName("教材編號")]
			public String? BookNumber { get; set; }
		}
		#endregion

		#region 科目教師列表		
		public class SchSubLecModel
		{
			public SchSubLecItem schSubLecItem { get; set; }
			public IPagedList<GetSubLecList> getSubLecPageLists { get; set; }
			public EvaluationActonInfo EvaluationActonInfo { get; set; }
		}

		public class SchSubLecItem
		{
			[DisplayName("科目代碼")]
			public String? SubID { get; set; }

			[DisplayName("科目名稱")]
			public String? SubName { get; set; }

			[DisplayName("授課教師編號")]
			public String? SubLectorNumber { get; set; }

			[DisplayName("授課教師")]
			public String? SubLector { get; set; }

		}

		public class GetSubLecList
		{
			[DisplayName("課程PK")]
			public Guid C_UID { get; set; }

			[DisplayName("科目PK")]
			public Guid SUB_UID { get; set; }

			[DisplayName("科目代碼")]
			public String? SubID { get; set; }

			[DisplayName("科目名稱")]
			public String? SubName { get; set; }

			[DisplayName("授課教師編號")]
			public String? SubLectorNumber { get; set; }

			[DisplayName("授課教師")]
			public String? SubLector { get; set; }
		}
		#endregion

		#region 授課教師評鑑列表
		public class CL_Model
		{
			public ClassInfo ClassInfo { get; set; }
			public IPagedList<GetClassLector> getClassLectorPageLists { get; set; }
		}

		public class ClassInfo
		{
			public Guid? C_UID { get; set; }
			public String? ClassID { get; set; }
			public String? ClassName { get; set; }
			public Guid? Sub_UID { get; set; }
			public String? SubID { get; set; }
			public String? SubName { get; set; }
		}
		public class GetClassLector
		{
			public Guid CL_UID { get; set; }

			public String L_ID { get; set; }
			public String L_Name { get; set; }
			public Boolean IsUpload { get; set; }
		}
		#endregion

		#region 未上傳教師列表		

		#endregion

		#endregion

		#region 指定教師頁面
		public class Modify_Model
		{
			public CSTinfo cSTinfo { get; set; }
			public List<SelectListItem> DropDownItem { get; set; }
			public LModify lModify { get; set; }
			public IPagedList<M_EvTeacher> m_EvTeacherPageLists { get; set; }
		}

		public class CSTinfo : ClassInfo
		{
			public Int32 Ev_Year { get; set; }
			public Guid CL_UID { get; set; }
			public Guid L_UID { get; set; }
			public String L_ID { get; set; }
			public String L_Name { get; set; }
		}

		public class LModify
		{
			public Int32 mType { get; set; }

			public Guid C_UID { get; set; }

			public Guid Sub_UID { get; set; }

			public Guid CL_UID { get; set; }

			public Guid L_UID { get; set; }

			[DisplayName("指定評鑑教師")]
			[Required(ErrorMessage = "{0} 為必填!")]
			public Guid L_UID_Ev { get; set; }

		}

		public class M_EvTeacher
		{
			public Guid CEv_UID { get; set; }
			public String L_ID_Ev { get; set; }
			public String L_Name_Ev { get; set; }
		}

		#endregion
		#endregion

	}
}

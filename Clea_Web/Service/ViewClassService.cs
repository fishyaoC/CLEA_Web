using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Clea_Web.Service
{
	//評鑑成績檢視 0:課程 1:教材
	public class ViewClassService : BaseService
	{
		public ViewClassService(dbContext dbContext)
		{
			db = dbContext;
		}

		#region BackEnd

		#region 查詢列表

		#region 課程列表
		public List<SelectListItem> GetSelectListItems_Year()
		{
			List<SelectListItem> result = new List<SelectListItem>();
			Int32 EndYear = DateTime.Now.Year + 1;
			Int32 StartYear = EndYear - 10;

			result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
			for (int x = EndYear; x > StartYear; x--)
			{
				result.Add(new SelectListItem() { Text = x.ToString() + "年", Value = x.ToString() });
			}

			return result;
		}

		public IPagedList<ViewClassViewModel.GetClassList_V> getClassPageList_Vs(ViewClassViewModel.SchClassItem_V data, Int32 page)
		{
			List<ViewClassViewModel.GetClassList_V> result = new List<ViewClassViewModel.GetClassList_V>();
			result = (from Class in db.CClasses
					  join CEv in db.CEvaluations on Class.CUid equals CEv.CUid
					  where
					  (
					  (string.IsNullOrEmpty(data.CEv_Year.ToString()) || CEv.LevYear == data.CEv_Year) &&
					  (string.IsNullOrEmpty(data.ClassID) || Class.CId.Contains(data.ClassID)) &&
					  (string.IsNullOrEmpty(data.ClassName) || Class.CName.Contains(data.ClassName)) &&
					  (string.IsNullOrEmpty(data.ClassType) || Class.CType.Contains(data.ClassType)) &&
					  (string.IsNullOrEmpty(data.BookNumber) || Class.CBookNum.Contains(data.BookNumber))
					  )
					  select new ViewClassViewModel.GetClassList_V()
					  {
						  CEv_Year = CEv.LevYear,
						  ClassUid = Class.CUid,
						  ClassID = Class.CId,
						  ClassName = Class.CName,
						  ClassType = Class.CType,
						  BookNumber = Class.CBookNum
					  }).OrderByDescending(x => x.CEv_Year).ThenBy(x => x.ClassID).ToList();

			return result.ToPagedList(page, pagesize);
		}
		#endregion

		#region 科目列表
		public IPagedList<ViewClassViewModel.GetSubLecList_V> GetSubLecPageLists_V(Guid Class_Uid, Int32 Year, ViewClassViewModel.SchSubLecItem_V data, Int32 page)
		{
			List<ViewClassViewModel.GetSubLecList_V> result = new List<ViewClassViewModel.GetSubLecList_V>();
			result = (from CL in db.ViewClassSubLectorVs
					  where
					  (
					  (CL.CKey == Class_Uid) &&
					  (CL.LevYear == Year) &&
					  (string.IsNullOrEmpty(data.SubID) || CL.SubId.Contains(data.SubID)) &&
					  (string.IsNullOrEmpty(data.SubName) || CL.SubName.Contains(data.SubName)) &&
					  //(string.IsNullOrEmpty(data.SubLectorNumber) || CL.) &&
					  (string.IsNullOrEmpty(data.SubLector) || CL.LName.Contains(data.SubLector))
					  )
					  select new ViewClassViewModel.GetSubLecList_V()
					  {
						  Year = CL.LevYear,
						  C_UID = CL.CKey.Value,
						  SUB_UID = CL.SubKey,
						  SubID = CL.SubId,
						  SubName = CL.SubName,
						  SubLector = CL.LName
					  }).OrderBy(x => x.SubName).ToList();
			return result.ToPagedList(page, pagesize);
		}
		#endregion

		#region 授課教師列表
		public IPagedList<ViewClassViewModel.GetClassLector_V> GetClassLectorPageLists_V(Guid C_UID, Guid Sub_UID, Int32 Year, Int32? Type, Int32 page)
		{
			List<ViewClassViewModel.GetClassLector_V> result = new List<ViewClassViewModel.GetClassLector_V>();

			result = (from CL in db.CClassLectors
					  join lec in db.CLectors on CL.LUid equals lec.LUid
					  join ev in db.CEvaluations on CL.ClUid equals ev.ClUid
					  where (ev.LevYear == Year && CL.CUid == C_UID && CL.DUid == Sub_UID)
					  select new ViewClassViewModel.GetClassLector_V
					  {
						  CL_UID = CL.ClUid,
						  L_ID = lec.LId,
						  L_Name = lec.LName,
						  IsUpload = (from sysFile in db.SysFiles where sysFile.FMatchKey == CL.ClUid select sysFile).FirstOrDefault() != null ? true : false
					  }).ToList();

			return result.ToPagedList(page, pagesize);
		}
		#endregion

		#region 教師指定評鑑列表
		public IPagedList<ViewClassViewModel.M_EvTeacher_V> m_EvTeacher_Vs(Int32 Year, Guid C_UID, Guid Sub_UID, Int32 mType, Int32 page)
		{
			List<ViewClassViewModel.M_EvTeacher_V> result = new List<ViewClassViewModel.M_EvTeacher_V>();

			result = (from ev in db.CEvaluations
					  join lec in db.CLectors on ev.LUid equals lec.LUid
					  where ev.LevYear == Year && ev.CUid == C_UID && ev.DUid == Sub_UID
					  select new ViewClassViewModel.M_EvTeacher_V()
					  {
						  CEv_UID = ev.LevId,
						  L_ID_Ev = lec.LId,
						  L_Name_Ev = lec.LName,
						  IsEvaluation = ev.ScoreA == null ? false : true
					  }).OrderBy(x => x.L_ID_Ev).ToList();

			return result.ToPagedList(page, pagesize);
		}
		#endregion

		#endregion

		#region 授課資訊
		public ViewClassViewModel.CSTinfo_V GetCSTInfo_V(Int32 Year, Guid CL_UID, Guid C_UID, Guid Sub_UID, Guid L_UID)
		{
			ViewClassViewModel.CSTinfo_V result = new ViewClassViewModel.CSTinfo_V();
			CLector? c_Lector = db.CLectors.Find(L_UID) ?? null;
			ViewClassViewModel.ClassInfo_V Cinfo = GetClassInfo_V(Year, C_UID, Sub_UID);

			result.Year = Year;
			result.Ev_Year = Year;
			result.CL_UID = CL_UID;
			result.L_UID = L_UID;
			result.L_ID = c_Lector != null ? c_Lector.LId : null;
			result.L_Name = c_Lector != null ? c_Lector.LName : null;
			result.C_UID = C_UID;
			result.ClassID = Cinfo != null ? Cinfo.ClassID : null;
			result.ClassName = Cinfo != null ? Cinfo.ClassName : null;
			result.Sub_UID = Sub_UID;
			result.SubID = Cinfo != null ? Cinfo.SubID : null;
			result.SubName = Cinfo != null ? Cinfo.SubName : null;
			return result;
		}
		public ViewClassViewModel.ClassInfo_V GetClassInfo_V(Int32 Year, Guid C_UID, Guid Sub_UID)
		{
			ViewClassViewModel.ClassInfo_V result = new ViewClassViewModel.ClassInfo_V();
			CClass? cClass = db.CClasses.Find(C_UID) ?? null;
			CClassSubject? cClassSubject = db.CClassSubjects.Find(Sub_UID) ?? null;

			result.Year = Year;
			result.C_UID = cClass != null ? cClass.CUid : null;
			result.ClassID = cClass != null ? cClass.CId : null;
			result.ClassName = cClass != null ? cClass.CName : null;
			result.SubID = cClassSubject != null ? cClassSubject.DId : null;
			result.SubName = cClassSubject != null ? cClassSubject.DName : null;
			return result;
		}
		#endregion

		#region 取得圖片列表
		public List<String> GetClassSubPic(Guid CL_UID, Int32 mType)
		{
			List<String> result = new List<String>();
			List<SysFile> lst_file = db.SysFiles.Where(x => x.FRemark.Equals(mType.ToString()) && x.FMatchKey == CL_UID).ToList();

			if (lst_file != null && lst_file.Count() > 0)
			{
				foreach (SysFile file in lst_file)
				{
					//Get File Path
					result.Add(file.FPath);
				}				
			}

			return result;
		}
		#endregion

		#region 取得成績資料
		public ViewClassViewModel.ScoreModify Get_EvaData(Guid CEvUID)
		{
			ViewClassViewModel.ScoreModify result = new ViewClassViewModel.ScoreModify();
			
			CEvaluation? cEvaluation = db.CEvaluations.Find(CEvUID) ?? null;

			result.CLeID = cEvaluation.LevId;
			result.Score_A = cEvaluation.ScoreA;
			result.Score_B = cEvaluation.ScoreB;
			result.Score_C = cEvaluation.ScoreC;
			result.Score_D = cEvaluation.ScoreD;
			result.Score_E = cEvaluation.ScoreE;
			result.Score_F = cEvaluation.ScoreF;
			result.Score_G = cEvaluation.ScoreG;
			result.Score_H = cEvaluation.ScroeH;
			result.Score_I = cEvaluation.ScoreI;
			result.Score_J = cEvaluation.ScoreJ;
			result.Remark = cEvaluation.Remark;

			return result;
		}
		#endregion

		#region 儲存成績
		public ViewClassViewModel.errorMsg Savedata_V(ViewClassViewModel.ScoreModify data)
		{
			ViewClassViewModel.errorMsg result = new BaseViewModel.errorMsg();

			CEvaluation? cEvaluation = db.CEvaluations.Find(data.CLeID) ?? null;

			if (cEvaluation != null)
			{
				cEvaluation.ScoreA = data.Score_A;
				cEvaluation.ScoreB = data.Score_B;
				cEvaluation.ScoreC = data.Score_C;
				cEvaluation.ScoreD = data.Score_D;
				cEvaluation.ScoreE = data.Score_E;
				cEvaluation.ScoreF = data.Score_F;
				cEvaluation.ScoreG = data.Score_G;
				cEvaluation.ScroeH = data.Score_H;
				cEvaluation.ScoreI = data.Score_I;
				cEvaluation.ScoreJ = data.Score_J;
				cEvaluation.Remark = data.Remark;
				cEvaluation.Upduser = Guid.Parse(GetUserID(user));
				cEvaluation.Upddate = DateTime.Now;

				result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
			}
			else
			{
				result.CheckMsg = false;
				result.ErrorMsg = "查無此筆資料!";
			}

			return result;
		}
		#endregion

		#endregion

		#region Portal
		#endregion

	}
}


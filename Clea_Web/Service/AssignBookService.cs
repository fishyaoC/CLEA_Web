using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Org.BouncyCastle.Cms;
using X.PagedList;

namespace Clea_Web.Service
{
	//評鑑 mType => 0:課程 1:教材
	public class AssignBookService : BaseService
	{
		public AssignBookService(dbContext dbContext)
		{
			db = dbContext;
		}

		#region BackEnd

		#region NEW

		#region IndexPage
		public IPagedList<AssignBookViewModel.BookInfor> GetSchBookItemPageList(AssignBookViewModel.SchBookItems data, Int32 page)
		{
			List<AssignBookViewModel.BookInfor> result = new List<AssignBookViewModel.BookInfor>();

			result = (from book in db.ViewBookEvaluates
					  where
					  (
					  (book.EType == 1) &&
					  (data.Year == null || book.EYear == data.Year) &&
					  (string.IsNullOrEmpty(data.B_ID) || book.MIndex.ToString().Contains(data.B_ID)) &&
					  (string.IsNullOrEmpty(data.B_Name) || book.MName.Contains(data.B_Name))
					  )
					  select new AssignBookViewModel.BookInfor()
					  {
						  Year = book.EYear,
						  B_UID = book.MId,
						  B_ID = book.MIndex.ToString(),
						  B_Name = book.MName,
						  E_ID = book.EId
					  }).OrderByDescending(x => x.Year).ToList();

			return result.ToPagedList(page, pagesize);
		}
		#endregion

		#region 取得評鑑年度
		public List<SelectListItem> GetYearSelectItems()
		{
			List<SelectListItem> result = new List<SelectListItem>();
			Int32 Start = (DateTime.Now.Year + 1) - 10;
			Int32 End = (DateTime.Now.Year + 1);

			result.Add(new SelectListItem() { Text = "請選擇", Value = null });
			for (int x = End; x > Start; x--)
			{
				result.Add(new SelectListItem() { Text = x.ToString() + "年", Value = x.ToString() });
			}

			return result;
		}
		#endregion

		#region 取得課程列表
		public List<SelectListItem> GetSelectListItemsBook()
		{
			List<SelectListItem> result = new List<SelectListItem>();

			List<CBook> lst_Book = new List<CBook>();
			lst_Book = db.CBooks.ToList();

			if (lst_Book.Count > 0)
			{
				foreach (CBook bk in lst_Book.OrderBy(x => x.MOrder))
				{
					result.Add(new SelectListItem() { Text = bk.MName, Value = bk.MId.ToString() });
				}
			}

			return result;
		}
		#endregion

		#region SaveAddData
		public BaseViewModel.errorMsg SaveAddData(AssignBookViewModel.AddModify data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();

			List<CBookDetail> lst_BD = new List<CBookDetail>();
			lst_BD = db.CBookDetails.Where(x => x.MId == data.B_UID).ToList();

			try
			{
				EEvaluate eEvaluate = new EEvaluate()
				{
					EId = Guid.NewGuid(),
					EType = 1,
					EYear = data.Year,
					MatchKey = data.B_UID,
					Creuser = Guid.Parse(GetUserID(user)),
					Credate = DateTime.Now
				};
				result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
			}
			catch (Exception ex)
			{
				result.CheckMsg = false;
				result.ErrorMsg = ex.Message;
			}

			return result;
		}
		#endregion

		#region 取得教材基本資訊
		public AssignBookViewModel.BookInfor GetBookInfor(Guid E_ID)
		{
			AssignBookViewModel.BookInfor result = new AssignBookViewModel.BookInfor();

			EEvaluate? eEvaluate = db.EEvaluates.Find(E_ID) ?? null;
			CBook? cBook = db.CBooks.Find(eEvaluate.MatchKey) ?? null;

			if (eEvaluate != null)
			{
				result.E_ID = E_ID;
				result.Year = eEvaluate.EYear;
				result.B_ID = cBook.MIndex.ToString();
				result.B_Name = cBook.MName;
			}

			return result;
		}
		#endregion

		#region 取得評鑑教師列表
		public List<AssignClassViewModel.EvTeacher> getEvTeacherList(Guid E_ID, Guid B_UID)
		{
			List<AssignClassViewModel.EvTeacher> result = new List<AssignClassViewModel.EvTeacher>();

			result = (from ed in db.ViewBookEvaluateTeachers

					  where (ed.EId == E_ID && ed.MatchKey2 == B_UID)
					  select new AssignClassViewModel.EvTeacher()
					  {
						  E_ID = ed.EId.Value,
						  L_UID_Ev = ed.LUid,
						  L_Ev_ID = ed.LId,
						  L_Ev_Name = ed.LName
					  }).ToList();

			return result;
		}
		#endregion

		#region 取得評鑑教師item
		public List<SelectListItem> selectListItemsTeacher()
		{
			List<SelectListItem> result = new List<SelectListItem>();
			List<CLector> lst_lec = db.CLectors.ToList();
			result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });

			if (lst_lec != null && lst_lec.Count > 0)
			{
				foreach (CLector lec in lst_lec)
				{
					result.Add(new SelectListItem() { Text = lec.LName, Value = lec.LUid.ToString() });
				}
			}

			return result;
		}
		#endregion

		#region SaveModify
		public BaseViewModel.errorMsg SaveModify(AssignBookViewModel.Modify data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			try
			{
				List<CBookDetail> lst_BD = db.CBookDetails.Where(x => x.MId == data.B_UID).ToList();
				if (lst_BD != null && lst_BD.Count > 0)
				{
					foreach (CBookDetail BD in lst_BD)
					{
						EEvaluateDetail eEvaluateDetail = new EEvaluateDetail()
						{
							EdId = Guid.NewGuid(),
							EId = data.E_ID,
							MatchKey2 = data.B_UID,
							Evaluate = data.L_UID_Ev,
							Creuser = Guid.Parse(GetUserID(user)),
							Credate = DateTime.Now
						};
						db.EEvaluateDetails.Add(eEvaluateDetail);
					}
					result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
				}
				else
				{
					result.CheckMsg = false;
					result.ErrorMsg = "尚無教材版本資料!";
				}
			}
			catch (Exception ex)
			{
				result.CheckMsg = false;
				result.ErrorMsg = ex.Message;
			}

			return result;
		}
		#endregion

		#region V_Index
		public List<AssignBookViewModel.EDInfo> getEDInfoList(Guid E_ID, Guid B_UID)
		{
			List<AssignBookViewModel.EDInfo> result = new List<AssignBookViewModel.EDInfo>();
			List<EEvaluateDetail> lst_BD = db.EEvaluateDetails.Where(x => x.EId == E_ID && x.MatchKey2 == B_UID).ToList();

			result = (from ed in db.EEvaluateDetails
					  where (ed.EId == E_ID && ed.MatchKey2 == B_UID)					  
					  select new AssignBookViewModel.EDInfo()
					  {
						  ED_ID = ed.EdId,
						  B_Name = (from cb in db.CBooks where cb.MId == ed.MatchKey2 select cb).FirstOrDefault().MName,
						  BD_Publish = (from cb in db.CBookDetails where cb.MId == ed.MatchKey2 select cb).FirstOrDefault().MdPublish.ToString(),
						  IsEvaluate = ed.EScoreA == null ? false : true,
						  IsUpload = db.SysFiles.Where(x => x.FMatchKey == ed.EdId).Count() == 0 ? false : true,
					  }).ToList();

			return result;
		}
		#endregion

		#endregion

		#region OLD

		#region Index
		public IPagedList<AssignBookViewModel.BookInfo> GetbookInfosPageList(AssignBookViewModel.SchBookItem data, Int32 page)
		{
			List<AssignBookViewModel.BookInfo> result = new List<AssignBookViewModel.BookInfo>();

			result = (from book in db.ViewBookEvaluationPs
					  where
					  (
					  (string.IsNullOrEmpty(data.B_ID) || book.MIndex.ToString().Contains(data.B_ID)) &&
					  (string.IsNullOrEmpty(data.B_Name) || book.MName.Contains(data.B_Name))
					  )
					  select new AssignBookViewModel.BookInfo()
					  {
						  B_UID = book.MId,
						  B_ID = book.MIndex.ToString(),
						  B_Name = book.MName,
						  B_CNumber = book.MNumber,
						  B_Publish = book.MPublish,
						  B_Version = book.MVersion.Value.ToLongDateString(),
						  IsUpload = book.FMatchKey == null ? false : true
					  }).OrderBy(x => x.B_ID).ToList();

			return result.ToPagedList(page, pagesize);
		}
		#endregion

		#region GetModel
		public AssignBookViewModel.BookInfoModel GetModel(Guid B_UID)
		{
			AssignBookViewModel.BookInfoModel result = new AssignBookViewModel.BookInfoModel();

			CBook? cBook = db.CBooks.Find(B_UID) ?? null;
			SysFile? file = db.SysFiles.Where(x => x.FMatchKey == cBook.MId).FirstOrDefault() ?? null;

			if (cBook != null)
			{
				result.B_UID = cBook.MId;
				result.B_ID = cBook.MIndex.ToString();
				result.B_Name = cBook.MName;
				result.B_CNumber = cBook.MNumber;
				result.B_Publish = cBook.MPublish;
				result.B_Version = cBook.MVersion.ToString();
				result.File_ID = file is null ? null : file.FileId;
				result.FileName = file is null ? null : file.FNameReal + "." + file.FExt;
				result.FilePath = file is null ? null : file.FPath;
			}

			return result;
		}
		#endregion

		#region SaveData Modify
		//public AssignBookViewModel.errorMsg SaveData(AssignBookViewModel.P_Modify_Model data, Int32 mType)
		//{
		//	AssignBookViewModel.errorMsg result = new BaseViewModel.errorMsg();

		//	CBook? cBook = db.CBooks.Find(data.bookInfo.B_UID) ?? null;

		//	if (cClassLector != null)
		//	{
		//		CEvaluation cEvaluation = new CEvaluation();
		//		cEvaluation.LevId = Guid.NewGuid();
		//		cEvaluation.LevYear = DateTime.Now.Year + 1;
		//		cEvaluation.LUid = cClassLector.LUid.Value;
		//		cEvaluation.LUidEv = data.lModify.L_UID_Ev;
		//		cEvaluation.LevType = mType; // 0:課程 1:教材
		//		cEvaluation.CUid = cClassLector.CUid.Value;
		//		cEvaluation.DUid = cClassLector.DUid.Value;
		//		cEvaluation.ClUid = cClassLector.ClUid;
		//		cEvaluation.Creuser = Guid.Parse(GetUserID(user));
		//		cEvaluation.Credate = DateTime.Now;

		//		db.CEvaluations.Add(cEvaluation);
		//		result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
		//	}
		//	else
		//	{
		//		result.CheckMsg = false;
		//		result.ErrorMsg = "查無授課資訊!";
		//	}

		//	return result;
		//}
		#endregion

		#region 教師下拉選單
		public List<SelectListItem> getTeacherItem()
		{
			List<SelectListItem> result = new List<SelectListItem>();
			result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
			List<CLector> lst_cLectors = db.CLectors.ToList();
			if (lst_cLectors != null && lst_cLectors.Count() > 0)
			{
				foreach (CLector L in lst_cLectors)
				{
					result.Add(new SelectListItem() { Text = L.LName, Value = L.LUid.ToString() });
				}
			}
			return result;
		}
		#endregion

		#region 取得評鑑教師列表
		public IPagedList<AssignBookViewModel.P_Teacher> Getp_Teachers(Guid B_UID, Int32 page)
		{
			List<AssignBookViewModel.P_Teacher> result = new List<AssignBookViewModel.P_Teacher>();

			result = (from Ev in db.CEvaluations
					  where Ev.BUid == B_UID && Ev.LevYear == DateTime.Now.Year + 1 && Ev.LevType == 1
					  join lec in db.CLectors on Ev.LUidEv equals lec.LUid
					  select new AssignBookViewModel.P_Teacher
					  {
						  L_UID_Ev = Ev.LevId,
						  L_ID_Ev = lec.LId,
						  L_Name_Ev = lec.LName
					  }).OrderBy(x => x.L_ID_Ev).ToList();

			return result.ToPagedList(page, pagesize);
		}
		#endregion

		#region SaveData P_Modify
		public BaseViewModel.errorMsg SaveDataPM(AssignBookViewModel.P_Modify_Model data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();

			CEvaluation cEvaluation = new CEvaluation();

			cEvaluation.LevId = Guid.NewGuid();
			cEvaluation.LevYear = DateTime.Now.Year + 1;
			cEvaluation.BUid = data.bookInfo.B_UID;
			cEvaluation.LUidEv = data.L_UID_Ev;
			cEvaluation.LevType = 1; // 0:課程 1:教材
			cEvaluation.Creuser = Guid.Parse(GetUserID(user));
			cEvaluation.Credate = DateTime.Now;

			db.CEvaluations.Add(cEvaluation);
			result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

			if (!result.CheckMsg)
			{
				result.ErrorMsg = "儲存失敗，請重新指定!";
			}

			return result;
		}
		#endregion

		#endregion



		#endregion

		#region Portal
		#endregion

	}
}


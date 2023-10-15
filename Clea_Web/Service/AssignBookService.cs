using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
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

		#region Portal
		#endregion

	}
}


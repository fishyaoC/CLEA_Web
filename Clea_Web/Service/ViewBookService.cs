using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Clea_Web.Service
{
	//評鑑成績檢視 0:課程 1:教材
	public class ViewBookService : BaseService
	{
		public ViewBookService(dbContext dbContext)
		{
			db = dbContext;
		}

		#region BackEnd

		#region Index
		public IPagedList<ViewBookViewModel.BookInfo> GetbookInfosPageList(ViewBookViewModel.SchBookItem data, Int32 page)
		{
			List<ViewBookViewModel.BookInfo> result = new List<ViewBookViewModel.BookInfo>();

			result = (from book in db.ViewBookEvaluationPs
					  join ev in db.CEvaluations on book.MId equals ev.BUid
					  where
					  (
					  (ev.LevType == 1) &&
					  (data.Year == null || ev.LevYear == data.Year) &&
					  (string.IsNullOrEmpty(data.B_ID) || book.MIndex.ToString().Contains(data.B_ID)) &&
					  (string.IsNullOrEmpty(data.B_Name) || book.MName.Contains(data.B_Name))
					  )
					  select new ViewBookViewModel.BookInfo()
					  {
						  CLv_UID = ev.LevId,
						  Year = ev.LevYear,
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
		public ViewBookViewModel.BookInfo GetModel(Guid B_UID)
		{
			ViewBookViewModel.BookInfo result = new ViewBookViewModel.BookInfo();

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
				//result.file = "";
				//result.FilePath = file is null ? null : file.FPath;
			}

			return result;
		}
		#endregion

		#region GetPTList
		public IPagedList<ViewBookViewModel.P_Teacher> p_TeachersPageList(Guid B_UID, Int32 Year, Int32 page)
		{
			List<ViewBookViewModel.P_Teacher> result = new List<ViewBookViewModel.P_Teacher>();

			result = (from ev in db.CEvaluations
					  where ev.LevType == 1 && ev.BUid == B_UID && ev.LevYear == Year
					  join lec in db.CLectors on ev.LUidEv equals lec.LUid
					  select new ViewBookViewModel.P_Teacher()
					  {
						  LEv_UID = ev.LevId,
						  L_ID_Ev = lec.LId,
						  L_Name_Ev = lec.LName,
						  IsEvaluation = ev.ScoreA == null ? false : true
					  }).ToList();

			return result.ToPagedList(page, pagesize);
		}
		#endregion

		#region 產生年度下拉選項
		public List<SelectListItem> GetYearItemList()
		{
			List<SelectListItem> result = new List<SelectListItem>();

			result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
			for (int x = (DateTime.Now.Year + 1); x > (DateTime.Now.Year -9); x--)
			{
				result.Add(new SelectListItem() { Text = x.ToString() +"年", Value = x.ToString() });
			}

			return result;
		}
		#endregion

		#endregion

		#region Portal
		#endregion

	}
}


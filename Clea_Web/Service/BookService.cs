using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing.Text;
using System.IO.Compression;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Novacode;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Org.BouncyCastle.Cms;
using X.PagedList;

namespace Clea_Web.Service
{
	//教材、訓練單位
	public class BookService : BaseService
	{
		public BookService(dbContext dbContext)
		{
			db = dbContext;
		}

		#region Book
		public IPagedList<BookViewModel.bookInfo> GetSchBookItemPageList(BookViewModel.SchBookItem data, Int32 page)
		{
			List<BookViewModel.bookInfo> result = new List<BookViewModel.bookInfo>();

			result = (from book in db.CBooks
					  where
					  (
					  (string.IsNullOrEmpty(data.M_Index) || book.MIndex.ToString().Contains(data.M_Index)) &&
					  (string.IsNullOrEmpty(data.M_Name) || book.MName.Contains(data.M_Name))
					  )
					  select new BookViewModel.bookInfo()
					  {
						  M_ID = book.MId,
						  M_Index = book.MIndex,
						  M_Name = book.MName
					  }).OrderByDescending(x => x.M_Index).ToList();

			return result.ToPagedList(page, pagesize);
		}

		public BaseViewModel.errorMsg SaveBkData(BookViewModel.bookInfo data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();

			try
			{
				CBook cBook = db.CBooks.Find(data.M_ID) ?? new CBook();
				cBook.MIndex = data.M_Index;
				cBook.MName = data.M_Name;

				if (cBook.MId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
				{
					cBook.MId = data.M_ID;
					cBook.Creuser = Guid.Parse(GetUserID(user));
					cBook.Credate = DateTime.Now;
					db.CBooks.Add(cBook);
				}
				else
				{
					cBook.Upduser = Guid.Parse(GetUserID(user));
					cBook.Upddate = DateTime.Now;
				}
				result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
			}
			catch (Exception ex)
			{
				result.CheckMsg = false;
				result.ErrorMsg = ex.Message;
			}

			return result;
		}

		public BaseViewModel.errorMsg SaveAddPub(BookViewModel.bookInfo data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			try
			{
				CBookDetail cBookDetail = new CBookDetail()
				{
					MdId = Guid.NewGuid(),
					MId = data.M_ID,
					MdPublish = data.BP_ID,
					RNumber = data.R_Number,
					RDate = Convert.ToDateTime(data.R_Date),
					Creuser = Guid.Parse(GetUserID(user)),
					Credate = DateTime.Now
				};
				db.CBookDetails.Add(cBookDetail);
				result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
			}
			catch (Exception ex)
			{
				result.CheckMsg = false;
				result.ErrorMsg = ex.Message;
			}

			return result;
		}

		public List<SelectListItem> GetPubItemList()
		{
			List<SelectListItem> result = new List<SelectListItem>();
			List<CBookPublish> lst_cBookPublishes = db.CBookPublishes.ToList();

			result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
			if (lst_cBookPublishes.Count > 0)
			{
				foreach (CBookPublish pub in lst_cBookPublishes)
				{
					result.Add(new SelectListItem() { Text = pub.BpName, Value = pub.BpId.ToString() });
				}
			}
			return result;
		}

		public List<BookViewModel.bdInfo> GetBookPubList(Guid M_ID)
		{
			List<BookViewModel.bdInfo> result = new List<BookViewModel.bdInfo>();
			List<CBookDetail> cBookDetails = db.CBookDetails.Where(x => x.MId == M_ID).ToList();

			result = (from bpd in db.CBookDetails
					  where bpd.MId == M_ID
					  select new BookViewModel.bdInfo()
					  {
						  MD_ID = bpd.MdId,
						  BP_ID = bpd.MdPublish,
						  BP_Name = (from pub in db.CBookPublishes where pub.BpId == bpd.MdPublish select pub).FirstOrDefault() == null ? "" : (from pub in db.CBookPublishes where pub.BpId == bpd.MdPublish select pub).FirstOrDefault().BpName,
						  R_Number = bpd.RNumber,
						  R_Date = bpd.RDate.ToShortDateString()
					  }).ToList();

			return result;
		}
		#endregion

		#region Publish
		public IPagedList<BookViewModel.PublishInfo> GetSchPubItemPageList(BookViewModel.SchBookItem data, Int32 page)
		{
			List<BookViewModel.PublishInfo> result = new List<BookViewModel.PublishInfo>();

			result = (from pub in db.CBookPublishes
					  where
					  (
					  (string.IsNullOrEmpty(data.BP_Name) || pub.BpName.Contains(data.BP_Name))
					  )
					  select new BookViewModel.PublishInfo()
					  {
						  BP_ID = pub.BpId,
						  BP_Name = pub.BpName
					  }).OrderByDescending(x => x.BP_Name).ToList();

			return result.ToPagedList(page, pagesize);
		}

		public BaseViewModel.errorMsg SavePubData(BookViewModel.PublishInfo data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();

			try
			{
				CBookPublish cBookPublish = db.CBookPublishes.Find(data.BP_ID) ?? new CBookPublish();
				cBookPublish.BpName = data.BP_Name;

				if (cBookPublish.BpId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
				{
					cBookPublish.BpId = data.BP_ID;
					cBookPublish.Creuser = Guid.Parse(GetUserID(user));
					cBookPublish.Credate = DateTime.Now;
					db.CBookPublishes.Add(cBookPublish);
				}
				else
				{
					cBookPublish.Upduser = Guid.Parse(GetUserID(user));
					cBookPublish.Upddate = DateTime.Now;
				}
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

	}
}
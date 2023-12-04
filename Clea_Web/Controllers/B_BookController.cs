using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Clea_Web.ViewModels;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using Org.BouncyCastle.Utilities;
using System.Security.Authentication.ExtendedProtection;
using MathNet.Numerics;
using NPOI.POIFS.Crypt.Dsig;


namespace Clea_Web.Controllers
{
	//後台-指定教材評鑑
	[Authorize]
	[UserPowerFilterAttribute]
	public class B_BookController : BaseController
	{
		private readonly ILogger<B_BookController> _logger;
		private BookService _bookService;
		private FileService _fileService;

		public B_BookController(ILogger<B_BookController> logger, dbContext dbCLEA, BookService bookService, FileService fileService)
		{
			_logger = logger;
			db = dbCLEA;
			_bookService = bookService;
			_fileService = fileService;
		}

		#region Index
		public IActionResult Index(String? data, Int32? page)
		{
			BookViewModel.SearchModel vmd = new BookViewModel.SearchModel();
			page = page ?? 1;

			if (!(page is null) && !string.IsNullOrEmpty(data))
			{
				vmd.schBookItem = JsonConvert.DeserializeObject<BookViewModel.SchBookItem>(value: data);
				ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schBookItem);
			}
			else
			{
				vmd.schBookItem = new BookViewModel.SchBookItem();
			}
			vmd.bookListsPageList = _bookService.GetSchBookItemPageList(data: vmd.schBookItem, page.Value);

			return View(vmd);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Index(BookViewModel.SearchModel vmd)
		{
			vmd.bookListsPageList = _bookService.GetSchBookItemPageList(data: vmd.schBookItem, 1);
			ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schBookItem);
			return View(vmd);
		}
		#endregion

		#region Add
		public IActionResult Add()
		{
			BookViewModel.bookInfo vmd = new BookViewModel.bookInfo();
			vmd.M_ID = Guid.NewGuid();
			return View(vmd);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Add(BookViewModel.bookInfo data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			_bookService.user = User;
			CBook? cBook = db.CBooks.Where(x => x.MIndex.Equals(data.M_Index) && x.MId != data.M_ID).FirstOrDefault() ?? null;

			if (!(cBook is null))
			{
				result.CheckMsg = false;
				result.ErrorMsg = "教材種類代碼重複!";
			}
			else
			{
				result = _bookService.SaveBkData(data);
			}

			if (result.CheckMsg)
			{
				TempData["TempMsgType"] = "success";
				TempData["TempMsgTitle"] = "儲存成功";
			}
			else
			{
				TempData["TempMsgType"] = "error";
				TempData["TempMsgTitle"] = "儲存失敗";
				TempData["TempMsg"] = result.ErrorMsg;
			}

			return RedirectToAction("Index");
		}
		#endregion

		#region Modify
		public IActionResult Modify(Guid U_ID)
		{
			BookViewModel.bookInfo vmd = new BookViewModel.bookInfo();
			CBook? cBook = db.CBooks.Find(U_ID) ?? null;

			if (!(cBook is null))
			{
				vmd.M_ID = cBook.MId;
				vmd.M_Index = cBook.MIndex;
				vmd.M_Name = cBook.MName;
				vmd.PublishItemList = _bookService.GetPubItemList();
				vmd.lst_pubList = _bookService.GetBookPubList(U_ID);
				return View(vmd);
			}
			else
			{
				TempData["TempMsgType"] = "error";
				TempData["TempMsgTitle"] = "查詢失敗";
				TempData["TempMsg"] = "查無此筆資料";
				return RedirectToAction("Index");
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Modify(BookViewModel.bookInfo data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			_bookService.user = User;
			CBook? cBook = db.CBooks.Where(x => x.MIndex.Equals(data.M_Index) && x.MId != data.M_ID).FirstOrDefault() ?? null;

			if (!(cBook is null))
			{
				result.CheckMsg = false;
				result.ErrorMsg = "教材代碼重複!";
			}
			else
			{
				result = _bookService.SaveBkData(data);
			}

			if (result.CheckMsg)
			{
				TempData["TempMsgType"] = "success";
				TempData["TempMsgTitle"] = "儲存成功";
			}
			else
			{
				TempData["TempMsgType"] = "error";
				TempData["TempMsgTitle"] = "儲存失敗";
				TempData["TempMsg"] = result.ErrorMsg;
			}

			return RedirectToAction("Index");
		}
		#endregion

		#region Delete
		public IActionResult Delete(Guid M_ID)
		{
			BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
			try
			{
				CBook? cBook = db.CBooks.Find(M_ID) ?? null;
				if (!(cBook is null))
				{
					List<CBookDetail> cBookDetails = db.CBookDetails.Where(x => x.MId == cBook.MId).ToList();
					if (cBookDetails.Count > 0)
					{
						foreach (CBookDetail cbd in cBookDetails)
						{
							SysFile? sysFile = db.SysFiles.Where(x => x.FMatchKey == cbd.MdId).FirstOrDefault() ?? null;
							if (sysFile != null)
							{
								error.CheckMsg = _fileService.DeleteFile(sysFile);
								if (error.CheckMsg)
								{
									db.SysFiles.Remove(sysFile);
								}
								else
								{
									throw new Exception("檔案刪除失敗!");
								}							
							}
							db.CBookDetails.Remove(cbd);
						}
					}
					db.CBooks.Remove(cBook);
					error.CheckMsg = Convert.ToBoolean(db.SaveChanges());
				}
				else
				{
					error.CheckMsg = false;
					error.ErrorMsg = "查無此筆資料!";
				}
			}
			catch (Exception ex)
			{
				error.CheckMsg = false;
				error.ErrorMsg = ex.Message;
			}

			return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
		}
		#endregion

		#region Publish
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult SavePublish_Modify(BookViewModel.bookInfo data)
		{
			BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
			_bookService.user = User;
			CBookDetail? cBookDetail = db.CBookDetails.Where(x => x.MId == data.M_ID && x.MdPublish == data.BP_ID).FirstOrDefault() ?? null;

			if (!(cBookDetail is null))
			{
				error.CheckMsg = false;
				error.ErrorMsg = "重複出版社!";
			}
			else
			{
				error = _bookService.SaveAddPub(data);
			}

			if (error.CheckMsg)
			{
				TempData["TempMsgType"] = "success";
				TempData["TempMsgTitle"] = "儲存成功";
			}
			else
			{
				TempData["TempMsgType"] = "error";
				TempData["TempMsgTitle"] = "儲存失敗";
				TempData["TempMsg"] = error.ErrorMsg;
			}

			return RedirectToAction("Modify", new { U_ID = data.M_ID });
		}

		public IActionResult DeletePub(Guid MD_ID)
		{
			BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
			try
			{
				CBookDetail? cBookDetail = db.CBookDetails.Find(MD_ID) ?? null;
				if (!(cBookDetail is null))
				{
					db.CBookDetails.Remove(cBookDetail);
					error.CheckMsg = Convert.ToBoolean(db.SaveChanges());
				}
				else
				{
					error.CheckMsg = false;
					error.ErrorMsg = "查無此筆資料!";
				}
			}
			catch (Exception ex)
			{
				error.CheckMsg = false;
				error.ErrorMsg = ex.Message;
			}

			return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
		}

		#endregion
	}
}
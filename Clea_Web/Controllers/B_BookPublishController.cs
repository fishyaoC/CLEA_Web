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
	public class B_BookPublishController : BaseController
	{
		private readonly ILogger<B_BookPublishController> _logger;
		private BookService _bookService;
		public B_BookPublishController(ILogger<B_BookPublishController> logger, dbContext dbCLEA, BookService bookService)
		{
			_logger = logger;
			db = dbCLEA;
			_bookService = bookService;
		}

		#region Index
		public IActionResult Index(String? data, Int32? page)
		{
			BookViewModel.SearchPModel vmd = new BookViewModel.SearchPModel();
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
			vmd.PublishInfoListsPageList = _bookService.GetSchPubItemPageList(data: vmd.schBookItem, page.Value);

			return View(vmd);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Index(BookViewModel.SearchPModel vmd)
		{
			vmd.PublishInfoListsPageList = _bookService.GetSchPubItemPageList(data: vmd.schBookItem, 1);
			ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schBookItem);
			return View(vmd);
		}
		#endregion

		#region Add
		public IActionResult Add()
		{
			BookViewModel.PublishInfo vmd = new BookViewModel.PublishInfo();
			vmd.BP_ID = Guid.NewGuid();
			return View(vmd);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Add(BookViewModel.PublishInfo data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			_bookService.user = User;
			CBookPublish? cBookPublish = db.CBookPublishes.Where(x => x.BpName.Equals(data.BP_Name) && x.BpId != data.BP_ID).FirstOrDefault() ?? null;

			if (!(cBookPublish is null))
			{
				result.CheckMsg = false;
				result.ErrorMsg = "訓練單位(出版社)!";
			}
			else
			{
				result = _bookService.SavePubData(data);
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
			BookViewModel.PublishInfo vmd = new BookViewModel.PublishInfo();
			CBookPublish? cBookPublish = db.CBookPublishes.Find(U_ID) ?? null;

			if (!(cBookPublish is null))
			{
				vmd.BP_ID = cBookPublish.BpId;
				vmd.BP_Name = cBookPublish.BpName;
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
		public IActionResult Modify(BookViewModel.PublishInfo data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			_bookService.user = User;
			CBookPublish? cBookPublish = db.CBookPublishes.Where(x => x.BpName.Equals(data.BP_Name) && x.BpId != data.BP_ID).FirstOrDefault() ?? null;

			if (!(cBookPublish is null))
			{
				result.CheckMsg = false;
				result.ErrorMsg = "出版社名稱重複!";
			}
			else
			{
				result = _bookService.SavePubData(data);
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
		public IActionResult Delete(Guid BP_ID)
		{
			BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();

			try
			{
				CBookPublish? cBookPublish = db.CBookPublishes.Find(BP_ID) ?? null;

				if (!(cBookPublish is null))
				{
					db.CBookPublishes.Remove(cBookPublish);
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
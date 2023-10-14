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


namespace Clea_Web.Controllers
{
	//後台-指定教材評鑑
	[Authorize]
	[UserPowerFilterAttribute]
	public class B_AssignBookController : BaseController
	{
		private readonly ILogger<B_AssignBookController> _logger;
		private AssignBookService _assignBookService;
		private FileService _fileService;
		public B_AssignBookController(ILogger<B_AssignBookController> logger, dbContext dbCLEA, AssignBookService Service, FileService fileService)
		{
			_logger = logger;
			db = dbCLEA;
			_assignBookService = Service;
			_fileService = fileService;
		}

		#region 教材列表
		public IActionResult Index(String? data, Int32? page)
		{
			AssignBookViewModel.schBookModel vmd = new AssignBookViewModel.schBookModel();
			page = page ?? 1;

			if (!(page is null) && !string.IsNullOrEmpty(data))
			{
				vmd.schBookItem = JsonConvert.DeserializeObject<AssignBookViewModel.SchBookItem>(value: data);
				ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schBookItem);
			}
			else
			{
				vmd.schBookItem = new AssignBookViewModel.SchBookItem();
			}

			vmd.bookInfosPageList = _assignBookService.GetbookInfosPageList(data: vmd.schBookItem, page.Value);
			return View(vmd);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Index(AssignBookViewModel.schBookModel vmd)
		{
			vmd.bookInfosPageList = _assignBookService.GetbookInfosPageList(data: vmd.schBookItem, 1);
			ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schBookItem);
			return View(vmd);
		}
		#endregion

		#region 成績編輯頁面(上傳檔案)
		public IActionResult Modify(Guid B_UID)
		{
			AssignBookViewModel.BookInfoModel vmd = new AssignBookViewModel.BookInfoModel();
			vmd = _assignBookService.GetModel(B_UID);
			return View(vmd);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Modify([FromForm] AssignBookViewModel.BookInfoModel data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			_assignBookService.user = User;
			_fileService.user = User;

			String chkExt = Path.GetExtension(data.file.FileName);

			if (chkExt.Contains(".ppt"))
			{
				result.CheckMsg = Convert.ToBoolean(_fileService.UploadFile(1, data.B_UID, data.file, true));
			}
			else
			{
				result.CheckMsg = false;
				result.ErrorMsg = "請上傳簡報檔案!";
			}

			if (result.CheckMsg)
			{
				TempData["TempMsgType"] = "success";
				TempData["TempMsgTitle"] = "檔案上傳成功";
			}
			else
			{
				TempData["TempMsgType"] = "error";
				TempData["TempMsgTitle"] = "檔案上傳失敗";
				TempData["TempMsg"] = result.ErrorMsg;
			}

			if (result.ErrorMsg.Contains("簡報"))
			{
				return RedirectToAction("Modify", new { B_UID = data.B_UID });
			}
			else
			{
				return RedirectToAction("Index");
			}
		}
		#endregion

		#region 刪除檔案
		public IActionResult Delete(Guid File_ID)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			Guid B_UID = Guid.Empty;
			SysFile? sysFile = db.SysFiles.Find(File_ID) ?? null;

			if (sysFile != null)
			{
				B_UID = sysFile.FMatchKey;
				db.SysFiles.Remove(sysFile);
				result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
			}
			else
			{
				result.CheckMsg = false;
				result.ErrorMsg = "查無此筆資料!";
			}

			if (result.CheckMsg)
			{
				TempData["TempMsgType"] = "success";
				TempData["TempMsgTitle"] = "刪除成功";
			}
			else
			{
				TempData["TempMsgType"] = "error";
				TempData["TempMsgTitle"] = "刪除失敗";
				TempData["TempMsg"] = result.ErrorMsg;
			}

			return RedirectToAction("Modify", new { B_UID = B_UID });
		}
		#endregion

		#region 指定教材評鑑教師
		public IActionResult P_Modify(Guid B_UID)
		{
			AssignBookViewModel.P_Modify_Model vmd = new AssignBookViewModel.P_Modify_Model();
			vmd.bookInfo = _assignBookService.GetModel(B_UID);
			vmd.DropDownList = _assignBookService.getTeacherItem();
			vmd.pTeacherPagedList = _assignBookService.Getp_Teachers(B_UID, 1);
			return View(vmd);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult P_Modify(AssignBookViewModel.P_Modify_Model data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			_assignBookService.user = User;

			CEvaluation? cEvaluation = db.CEvaluations.Where(x => x.LevType == 1 && x.BUid == data.bookInfo.B_UID && x.LUidEv == data.L_UID_Ev).FirstOrDefault();

			if (cEvaluation is null)
			{
				result = _assignBookService.SaveDataPM(data);
			}
			else
			{
				result.CheckMsg = false;
				result.ErrorMsg = "評鑑教師重複，請重新指定!";
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

			return RedirectToAction("P_Modify", new { B_UID = data.bookInfo.B_UID });
		}
		#endregion

		#region 刪除評鑑教師
		public IActionResult P_Delete(Guid LevId)
		{
			AssignClassViewModel.errorMsg error = new AssignClassViewModel.errorMsg();

			CEvaluation? cEvaluation = db.CEvaluations.Find(LevId) ?? null;
			if (cEvaluation is null)
			{
				error.CheckMsg = false;
				error.ErrorMsg = "查無此筆資料!";
			}
			else
			{
				db.CEvaluations.Remove(cEvaluation);
				error.CheckMsg = Convert.ToBoolean(db.SaveChanges());
			}

			return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
		}
		#endregion

	}
}
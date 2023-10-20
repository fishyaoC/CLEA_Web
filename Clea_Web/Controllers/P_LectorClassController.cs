using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.ViewModels;
using Newtonsoft.Json;

namespace Clea_Web.Controllers
{
	//後臺首頁-我的課程
	public class P_LectorClassController : BaseController
	{
		private readonly ILogger<P_LectorClassController> _logger;
		private LectorClassService _lectorClassService;
		private FileService _fileService;

		public P_LectorClassController(ILogger<P_LectorClassController> logger, dbContext dbCLEA, LectorClassService Service, FileService fileService)
		{
			_logger = logger;
			db = dbCLEA;
			_lectorClassService = Service;
			_fileService = fileService;

		}


		#region Index
		public IActionResult Index(String? data, Int32? page)
		{
			LectorClassViewModel vmd = new LectorClassViewModel();
			_lectorClassService.user = User;
			page = page ?? 1;
			vmd.classMenuPageList = _lectorClassService.GetClassMenuPageList(page.Value);
			return View(vmd);
		}
		#endregion

		#region Modify
		public IActionResult Modify(Guid ED_ID)
		{
			LectorClassViewModel.ModifyModel vmd = new LectorClassViewModel.ModifyModel();
			vmd.uploadLogs = _lectorClassService.GetUploadLogs(ED_ID);
			vmd.modify = _lectorClassService.GetModifyModel(ED_ID);
			return View(vmd);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Modify(LectorClassViewModel.ModifyModel data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			_lectorClassService.user = User;

			String chkExt = string.IsNullOrEmpty(data.modify.FileName) ? Path.GetExtension(data.modify.file.FileName) : data.modify.FileName;

			if (chkExt.Contains(".ppt"))
			{
				result = _lectorClassService.SaveModifyData(data.modify);
			}
			else
			{
				result.CheckMsg = false;
				result.ErrorMsg = "檔案格式有誤，請上傳簡報檔案!";
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
		public IActionResult Delete(Guid F_ID)
		{
			BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
			try
			{
				SysFile? sysFile = db.SysFiles.Find(F_ID) ?? null;

				if (sysFile != null)
				{
					error.CheckMsg = _fileService.DeleteFile(sysFile);
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
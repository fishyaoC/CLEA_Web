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

		#region NEW

		#region 教材列表
		public IActionResult Index(String? data, Int32? page)
		{
			AssignBookViewModel.SchBookModel vmd = new AssignBookViewModel.SchBookModel();


			page = page ?? 1;

			if (!(page is null) && !string.IsNullOrEmpty(data))
			{
				vmd.schBookItems = JsonConvert.DeserializeObject<AssignBookViewModel.SchBookItems>(value: data);
				ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schBookItems);
			}
			else
			{
				vmd.schBookItems = new AssignBookViewModel.SchBookItems();
			}
			vmd.selectListItems = _assignBookService.GetYearSelectItems();
			vmd.bookInforsPageList = _assignBookService.GetSchBookItemPageList(vmd.schBookItems, page.Value);

			return View(vmd);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Index(AssignBookViewModel.SchBookModel vmd)
		{
			vmd.selectListItems = _assignBookService.GetYearSelectItems();
			vmd.bookInforsPageList = _assignBookService.GetSchBookItemPageList(data: vmd.schBookItems, 1);
			ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schBookItems);
			return View(vmd);
		}
		#endregion

		#region 新增教材評鑑
		public IActionResult Add()
		{
			AssignBookViewModel.AddModel vmd = new AssignBookViewModel.AddModel();
			vmd.selectListItemsYear = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
			vmd.selectListItemsYear.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = (DateTime.Now.Year + 1).ToString(), Value = (DateTime.Now.Year + 1).ToString() });
			vmd.selectListItemsYear.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = (DateTime.Now.Year).ToString(), Value = (DateTime.Now.Year).ToString() });
			vmd.selectListItemsBook = _assignBookService.GetSelectListItemsBook();

			return View(vmd);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Add(AssignBookViewModel.AddModel vmd)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			_assignBookService.user = User;
			ViewBookEvaluate? viewBookEvaluate = db.ViewBookEvaluates.Where(x => x.EType == 1 && x.MId == vmd.addModify.B_UID && x.EYear == vmd.addModify.Year).FirstOrDefault();

			if (!(viewBookEvaluate is null))
			{
				result.CheckMsg = false;
				result.ErrorMsg = "當年度教材重複!";
			}
			else
			{
				result = _assignBookService.SaveAddData(vmd.addModify);
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

		#region 指定評鑑教師
		public IActionResult Modify(Guid E_ID)
		{
			AssignBookViewModel.ModifyModel vmd = new AssignBookViewModel.ModifyModel();

			EEvaluate? eEvaluate = db.EEvaluates.Find(E_ID) ?? null;

			vmd.bookInfor = _assignBookService.GetBookInfor(E_ID);
			vmd.modify = new AssignBookViewModel.Modify();
			vmd.modify.E_ID = E_ID;
			vmd.modify.B_UID = eEvaluate.MatchKey;
			vmd.lst_evTeacher = _assignBookService.getEvTeacherList(E_ID);
			vmd.selectListItems = _assignBookService.selectListItemsTeacher();
			return View(vmd);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Modify(AssignBookViewModel.ModifyModel vmd)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			_assignBookService.user = User;

			EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Where(x => x.EId == vmd.modify.E_ID && x.MatchKey2 == vmd.modify.B_UID && x.Evaluate == vmd.modify.L_UID_Ev).FirstOrDefault();

			if (eEvaluateDetail != null)
			{
				result.CheckMsg = false;
				result.ErrorMsg = "指定教師重複!";
			}
			else
			{
				result = _assignBookService.SaveModify(vmd.modify);
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

			return RedirectToAction("Modify", new { E_ID = vmd.modify.E_ID });
		}
		#endregion

		#region 刪除評鑑教師
		public IActionResult Delete(Guid E_ID, Guid L_UID_Ev)
		{
			AssignClassViewModel.errorMsg error = new AssignClassViewModel.errorMsg();

			List<EEvaluateDetail> lst_BD = new List<EEvaluateDetail>();
			lst_BD = db.EEvaluateDetails.Where(x => x.EId == E_ID && x.Evaluate == L_UID_Ev).ToList();

			if (lst_BD.Count > 0)
			{
				foreach (EEvaluateDetail bd in lst_BD)
				{
					db.EEvaluateDetails.Remove(bd);
				}
				error.CheckMsg = Convert.ToBoolean(db.SaveChanges());
			}
			else
			{
				error.CheckMsg = false;
				error.ErrorMsg = "查無此筆資料!";
			}

			return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
		}
		#endregion

		#region 匯出EXCEL統計表
		public IActionResult Export_ScoreExcel(Guid E_ID)
		{
			EEvaluate? eEvaluate = db.EEvaluates.Find(E_ID) ?? null;
			CBook? cBook = db.CBooks.Find(eEvaluate.MatchKey) ?? null;
			Byte[] file = _assignBookService.Export_Excel(E_ID);
			return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", eEvaluate.EYear.ToString() + "年-" + cBook.MName + "-教材審查統計表.xlsx");
		}
		#endregion

		#region 教材版本列表
		public IActionResult V_Index(Guid E_ID, Guid B_UID)
		{
			AssignBookViewModel vmd = new AssignBookViewModel();
			vmd.lst_EDInfo = _assignBookService.getEDInfoList(E_ID);
			return View(vmd);
		}
		#endregion

		#region 匯出審查表WORD
		public IActionResult Export_ScoreWord(Guid E_ID)
		{
			String dir = Guid.NewGuid().ToString();
			EEvaluate? eEvaluate = db.EEvaluates.Find(E_ID) ?? null;
			CBook? cBook = db.CBooks.Find(eEvaluate.MatchKey) ?? null;
			Byte[] file = _assignBookService.Export_ScoreZip(E_ID, dir);
			return File(file, "application/zip", eEvaluate.EYear.ToString() + "年-" + cBook.MName + "-教材審查表.zip");
		}
		#endregion

		#region 編輯審查表
		public IActionResult V_Modify(Guid ED_ID)
		{
			AssignBookViewModel.V_ModifyModel vmd = new AssignBookViewModel.V_ModifyModel();
			EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(ED_ID) ?? null;
			CBookDetail? cBookDetail = db.CBookDetails.Find(eEvaluateDetail.MatchKey2) ?? null;
			CBookPublish? cBookPublish = db.CBookPublishes.Find(cBookDetail.MdPublish) ?? null;
			CLector? cLector = db.CLectors.Find(eEvaluateDetail.Evaluate) ?? null;
			vmd.bookInfor = _assignBookService.GetBookInfor(eEvaluateDetail.EId);
			vmd.bookInfor.BP_ID = cBookPublish.BpNumber;
			vmd.bookInfor.B_Publish = cBookPublish.BpName;
			vmd.bookInfor.L_ID_Ev = cLector.LId;
			vmd.bookInfor.L_Name_Ev = cLector.LName;

			vmd.picPath = _fileService.GetImageBase64List_PNG(ED_ID);
			vmd.scoreModify = _assignBookService.GetScoreModel(ED_ID);

			return View(vmd);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult V_Modify(AssignBookViewModel.V_ModifyModel vmd)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			_assignBookService.user = User;
			result = _assignBookService.saveVModifyData(vmd.scoreModify);
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

			return RedirectToAction("V_Index", new { E_ID = vmd.scoreModify.E_ID });
		}
		#endregion

		#region 檔案上傳
		public IActionResult Import_File(Guid ED_ID)
		{
			AssignBookViewModel.ImportModel vmd = new AssignBookViewModel.ImportModel();
			EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(ED_ID) ?? null;
			EEvaluate? eEvaluate = db.EEvaluates.Find(eEvaluateDetail.EId) ?? null;
			CBookDetail? cBookDetail = db.CBookDetails.Find(eEvaluateDetail.MatchKey2) ?? null;
			CBook? cBook = db.CBooks.Find(eEvaluate.MatchKey) ?? null;
			CBookPublish? cBookPublish = db.CBookPublishes.Find(cBookDetail.MdPublish) ?? null;

			vmd.bookInfor = new AssignBookViewModel.BookInfor()
			{
				Year = eEvaluate.EYear,
				B_Name = cBook.MName,
				B_Publish = cBookPublish.BpName
			};

			SysFile? sysFile = db.SysFiles.Where(x => x.FMatchKey == ED_ID).FirstOrDefault();
			vmd.import = new AssignBookViewModel.Import() { ED_ID = ED_ID, E_ID = eEvaluate.EId, F_ID = sysFile == null ? null : sysFile.FileId, fileName = sysFile == null ? null : sysFile.FFullName };

			return View(vmd);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Import_File([FromForm] AssignBookViewModel.ImportModel data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			_assignBookService.user = User;
			_fileService.user = User;

			String chkExt = Path.GetExtension(data.import.file.FileName);

			if (chkExt.Contains(".pdf"))
			{
				result.CheckMsg = _fileService.UploadFile(false, 1, data.import.ED_ID, data.import.file, true);
			}
			else
			{
				result.CheckMsg = false;
				result.ErrorMsg = "檔案格式有誤，請上傳簡報檔案!";
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

			if (!result.CheckMsg && result.ErrorMsg.Contains("簡報"))
			{
				return RedirectToAction("Import_File", new { ED_ID = data.import.ED_ID });
			}
			else
			{
				return RedirectToAction("V_Index", new { E_ID = data.import.E_ID });
			}
		}
		#endregion

		#region 刪除檔案
		public IActionResult F_Delete(Guid F_ID)
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

		#endregion		

	}
}
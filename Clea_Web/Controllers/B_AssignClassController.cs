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
using Novacode;
using NPOI.OpenXmlFormats.Wordprocessing;

namespace Clea_Web.Controllers
{
	//後台講師專區-我的評鑑
	[Authorize]
	[UserPowerFilterAttribute]
	public class B_AssignClassController : BaseController
	{
		private readonly ILogger<B_AssignClassController> _logger;
		private AssignClassService _assignService;

		public B_AssignClassController(ILogger<B_AssignClassController> logger, dbContext dbCLEA, AssignClassService Service)
		{
			_logger = logger;
			db = dbCLEA;
			_assignService = Service;
		}

		#region NEW

		#region 課程列表
		public IActionResult Index(String? data, Int32? page)
		{
			AssignClassViewModel.SearchModel vmd = new AssignClassViewModel.SearchModel();
			page = page ?? 1;

			if (!(page is null) && !string.IsNullOrEmpty(data))
			{
				vmd.schClassItem = JsonConvert.DeserializeObject<AssignClassViewModel.schClassItem>(value: data);
				ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schClassItem);
			}
			else
			{
				vmd.schClassItem = new AssignClassViewModel.schClassItem();
			}
			vmd.selectListItems = _assignService.GetYearSelectItems();
			vmd.classInfoPageLists = _assignService.GetClassInfoPageList(vmd.schClassItem, page.Value);

			return View(vmd);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Index(AssignClassViewModel.SearchModel vmd)
		{
			vmd.selectListItems = _assignService.GetYearSelectItems();
			vmd.classInfoPageLists = _assignService.GetClassInfoPageList(vmd.schClassItem, 1);
			ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schClassItem);
			return View(vmd);
		}
		#endregion

		#region 新增教師評鑑
		public IActionResult Add()
		{
			AssignClassViewModel.AddModel vmd = new AssignClassViewModel.AddModel();
			vmd.Year_selectListItems = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
			vmd.Year_selectListItems.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = (DateTime.Now.Year + 1).ToString() + "年", Value = (DateTime.Now.Year + 1).ToString() });
			vmd.Year_selectListItems.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = (DateTime.Now.Year).ToString() + "年", Value = (DateTime.Now.Year).ToString() });

			vmd.Class_selectListItems = _assignService.GetClassSelectItems();

			return View(vmd);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Add(AssignClassViewModel.AddModel vmd)
		{
			_assignService.user = User;
			AssignClassViewModel.errorMsg result = new BaseViewModel.errorMsg();

			ViewClassEvaluate? viewClassEvaluate = db.ViewClassEvaluates.Where(x => x.CUid == vmd.addModify.C_UID && x.EYear == vmd.addModify.Year).FirstOrDefault();

			if (!(viewClassEvaluate is null))
			{
				result.CheckMsg = false;
				result.ErrorMsg = "當年度課程重複!";
			}
			else
			{
				result = _assignService.SaveAddData(vmd);
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

		#region 教師授課列表
		public IActionResult TC_Index(Guid U_ID, String? data, Int32? page)
		{
			AssignClassViewModel.TC_Model vmd = new AssignClassViewModel.TC_Model();
			page = page ?? 1;

			vmd.evaluationActonInfo = new BaseViewModel.EvaluationActonInfo() { Key = U_ID, ControllerName = "TC_Index" };
			vmd.E_ID = U_ID;

			if (!(page is null) && !string.IsNullOrEmpty(data))
			{
				vmd.cLschItem = JsonConvert.DeserializeObject<AssignClassViewModel.CLschItem>(value: data);
				ViewBag.schPageList = JsonConvert.SerializeObject(vmd.cLschItem);
			}
			else
			{
				vmd.cLschItem = new AssignClassViewModel.CLschItem();
			}

			ViewBag.EvaInfo = JsonConvert.SerializeObject(vmd.evaluationActonInfo);
			vmd.cLPageLists = _assignService.GetClPageList(U_ID, vmd.cLschItem, page.Value);
			return View(vmd);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult TC_Index(AssignClassViewModel.TC_Model vmd)
		{
			vmd.cLPageLists = _assignService.GetClPageList(vmd.evaluationActonInfo.Key, vmd.cLschItem, 1);
			ViewBag.schPageList = JsonConvert.SerializeObject(vmd.cLschItem);
			ViewBag.EvaInfo = JsonConvert.SerializeObject(vmd.evaluationActonInfo);
			return View(vmd);
		}
		#endregion

		#region 指定評鑑教師
		public IActionResult TC_Modify(Guid E_ID, Guid CL_UID)
		{
			AssignClassViewModel.TC_ModifyModel vmd = new AssignClassViewModel.TC_ModifyModel();
			vmd.cLInfo = _assignService.getCLinfo(E_ID, CL_UID);
			vmd.selectListItems = _assignService.getTeacherItem(vmd.cLInfo.L_UID);
			vmd.tCModify = new AssignClassViewModel.TCModify();
			vmd.tCModify.E_ID = E_ID;
			vmd.tCModify.CL_UID = CL_UID;
			vmd.lst_EvTeachers = _assignService.GetEvTeacherPageLists(E_ID, CL_UID);
			return View(vmd);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult TC_Modify(AssignClassViewModel.TC_ModifyModel vmd)
		{
			AssignClassViewModel.errorMsg result = new BaseViewModel.errorMsg();
			_assignService.user = User;
			EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Where(x => x.EId == vmd.tCModify.E_ID && x.MatchKey2 == vmd.tCModify.CL_UID && x.Evaluate == vmd.tCModify.L_UID_Ev).FirstOrDefault();

			if (eEvaluateDetail is null)
			{
				result = _assignService.SaveTcData(vmd.tCModify);
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

			return RedirectToAction("TC_Modify", new { E_ID = vmd.tCModify.E_ID, CL_UID = vmd.tCModify.CL_UID });
		}
		#endregion

		#region 刪除評鑑教師
		public IActionResult TC_Delete(Guid ED_ID)
		{
			AssignClassViewModel.errorMsg error = new AssignClassViewModel.errorMsg();

			EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(ED_ID) ?? null;
			if (eEvaluateDetail is null)
			{
				error.CheckMsg = false;
				error.ErrorMsg = "查無此筆資料!";
			}
			else
			{
				db.EEvaluateDetails.Remove(eEvaluateDetail);
				error.CheckMsg = Convert.ToBoolean(db.SaveChanges());
			}

			return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
		}
		#endregion

		#region 檢視審查表
		public IActionResult V_Modify(Guid ED_ID)
		{
			AssignClassViewModel.V_ModifyModel vmd = new AssignClassViewModel.V_ModifyModel();
			EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(ED_ID) ?? null;

			vmd.cLInfo = _assignService.getCLinfo(eEvaluateDetail.EId, eEvaluateDetail.MatchKey2);
			vmd.picPath = _assignService.GetPicList(ED_ID);
			vmd.v_ScoreModel = _assignService.GetVModel(ED_ID);
			return View(vmd);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult V_Modify(AssignClassViewModel.V_ModifyModel data)
		{
			_assignService.user = User;
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();

			EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(data.v_ScoreModel.ED_ID) ?? null;
			result = _assignService.SaveVData(data.v_ScoreModel);

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

			return RedirectToAction("TC_Index", new { U_ID = eEvaluateDetail.EId });
		}
		#endregion

		#region 匯出審查表
		public IActionResult Export_ScoreWord(Guid ED_ID)
		{
			EEvaluateDetail? cEvaluation = db.EEvaluateDetails.Find(ED_ID) ?? null;
			CClassLector? cClassLector = db.CClassLectors.Find(cEvaluation.MatchKey2) ?? null;
			CLector? cLector = db.CLectors.Find(cClassLector.LUid) ?? null;
			CClass? cClass = db.CClasses.Find(cClassLector.CUid) ?? null;
			CClassSubject? cClassSubject = db.CClassSubjects.Find(cClassLector.DUid) ?? null;

			String L_Name = cLector != null ? cLector.LName : string.Empty;
			String ScoreT = ((cEvaluation.EScoreA.Value) + (cEvaluation.EScoreB.Value ) + (cEvaluation.EScoreC.Value ) + (cEvaluation.EScoreD.Value) + (cEvaluation.EScoreE.Value)).ToString("#.##");

			String SourcePath = "./SampleFile/ClassEvaExample.docx";
			String SavePath = "./SampleFile/Output/" + L_Name + "-教學內容審查表.docx";

			using (DocX doc = DocX.Load(SourcePath))
			{
				doc.ReplaceText("[@Year$]", cEvaluation is null ? string.Empty : cEvaluation.Upddate is null ? string.Empty : cEvaluation.Upddate.Value.Year.ToString());    //年
				doc.ReplaceText("[@Month$]", cEvaluation is null ? string.Empty : cEvaluation.Upddate is null ? string.Empty : cEvaluation.Upddate.Value.Month.ToString());                            //月
				doc.ReplaceText("[@Day$]", cEvaluation is null ? string.Empty : cEvaluation.Upddate is null ? string.Empty : cEvaluation.Upddate.Value.Day.ToString());                                //日
				doc.ReplaceText("[@ClassName$]", cClass is null ? string.Empty : cClass.CName);
				doc.ReplaceText("[@SubName$]", cClassSubject is null ? string.Empty : cClassSubject.DName);
				doc.ReplaceText("[@LecName$]", L_Name);
				doc.ReplaceText("[@ScoreA$]", cEvaluation is null ? string.Empty : cEvaluation.EScoreA.ToString());
				doc.ReplaceText("[@ScoreB$]", cEvaluation is null ? string.Empty : cEvaluation.EScoreB.ToString());
				doc.ReplaceText("[@ScoreC$]", cEvaluation is null ? string.Empty : cEvaluation.EScoreC.ToString());
				doc.ReplaceText("[@ScoreD$]", cEvaluation is null ? string.Empty : cEvaluation.EScoreD.ToString());
				doc.ReplaceText("[@ScoreE$]", cEvaluation is null ? string.Empty : cEvaluation.EScoreE.ToString());
				doc.ReplaceText("[@ScoreT$]", ScoreT);
				doc.ReplaceText("[@Remark$]", cEvaluation is null ? string.Empty : string.IsNullOrEmpty(cEvaluation.ERemark) ? string.Empty : cEvaluation.ERemark);
				doc.ReplaceText("[@Pass$]", Convert.ToDouble(ScoreT) >= 85 ? "■" : "□");
				doc.ReplaceText("[@Fail$]", Convert.ToDouble(ScoreT) >= 85 ? "□" : "■");

				doc.SaveAs(SavePath);
			}

			FileInfo fi = new FileInfo(SavePath);
			FileStream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read);
			return File(fs, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", L_Name + "-教學內容審查表.docx");
		}
		#endregion

		#region 匯出未上傳檔案教師
		public IActionResult V_Export()
		{
			Byte[] file = _assignService.Export_Excel();
			return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", (DateTime.Now.Year + 1).ToString() + "年評鑑-未上傳課程資訊-" + DateTime.Now.ToLongDateString() + ".xlsx");
		}
		#endregion

		#endregion

	}
}
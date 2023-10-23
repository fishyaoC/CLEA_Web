using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.ViewModels;
using Newtonsoft.Json;

namespace Clea_Web.Controllers
{
	//前台講師專區-我的評鑑
	public class P_LectorEvaluationController : BaseController
	{
		private readonly ILogger<P_LectorEvaluationController> _logger;
		private LectorEvaluationService _lectorEvaluationService;
		private FileService _fileService;

		public P_LectorEvaluationController(ILogger<P_LectorEvaluationController> logger, dbContext dbCLEA, LectorEvaluationService Service, FileService fileService)
		{
			_logger = logger;
			db = dbCLEA;
			_lectorEvaluationService = Service;
			_fileService = fileService;
		}


		#region Index
		public IActionResult Index(String? data, Int32? page)
		{
			LectorEvaluationViewModel vmd = new LectorEvaluationViewModel();
			page = page ?? 1;
			_lectorEvaluationService.user = User;
			vmd.schEvInfosPageLists = _lectorEvaluationService.GetSchEvInfosPageList(page.Value);

			return View(vmd);
		}
		#endregion

		#region Modify
		public IActionResult Modify(Guid ED_ID)
		{
			LectorEvaluationViewModel.ScoreModel vmd = new LectorEvaluationViewModel.ScoreModel();

			EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(ED_ID);
			EEvaluate? eEvaluate = db.EEvaluates.Find(eEvaluateDetail.EId) ?? null;
			String L_name = string.Empty;
			String C_B_name = string.Empty;
			String S_P_name = string.Empty;

			if (eEvaluate.EType == 0)
			{
				CLector? cLector = db.CLectors.Find(eEvaluateDetail.Reception) ?? null;
				CClass? cClass = db.CClasses.Find(eEvaluate.MatchKey) ?? null;
				CClassLector? cClassLector = db.CClassLectors.Find(eEvaluateDetail.MatchKey2) ?? null;
				CClassSubject? cClassSubject = db.CClassSubjects.Where(x => x.DUid == cClassLector.DUid).FirstOrDefault();
				L_name = cLector.LName;
				C_B_name = cClass.CName;
				S_P_name = cClassSubject.DName;
			}
			else
			{
				CBook? cBook = db.CBooks.Find(eEvaluate.MatchKey) ?? null;
				CBookDetail? cBookDetail = db.CBookDetails.Where(x => x.MId == cBook.MId).FirstOrDefault();
				CBookPublish? cBookPublish = db.CBookPublishes.Find(cBookDetail.MdPublish) ?? null;
				C_B_name = cBook.MName;
				S_P_name = cBookPublish.BpName;
			}

			vmd.evInfo = new LectorEvaluationViewModel.EvInfo()
			{
				Year = eEvaluate.EYear,
				L_Name = L_name,
				C_B_Name = C_B_name,
				S_P_Name = S_P_name
			};
			vmd.scoreModify = new LectorEvaluationViewModel.scoreModify()
			{
				ED_ID = ED_ID,
				mType = eEvaluate.EType,
				lst_pic = _fileService.GetImageBase64List_PNG(ED_ID),
				ScoreA = eEvaluateDetail.EScoreA == null ? 0 : eEvaluateDetail.EScoreA.Value,
				ScoreB = eEvaluate.EType == 0 ? eEvaluateDetail.EScoreB == null ? 0 : eEvaluateDetail.EScoreB.Value : 0,
				ScoreBB = eEvaluate.EType == 0 ? 0 : eEvaluateDetail.EScoreB == null ? 0 : eEvaluateDetail.EScoreB.Value,
				ScoreC = eEvaluate.EType == 0 ? eEvaluateDetail.EScoreC == null ? 0 : eEvaluateDetail.EScoreC.Value : 0,
				ScoreCB = eEvaluate.EType == 0 ? 0 : eEvaluateDetail.EScoreC == null ? 0 : eEvaluateDetail.EScoreC.Value,
				ScoreD = eEvaluateDetail.EScoreD == null ? 0 : eEvaluateDetail.EScoreD.Value,
				ScoreE = eEvaluateDetail.EScoreE == null ? 0 : eEvaluateDetail.EScoreE.Value,
			};

			return View(vmd);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Modify(LectorEvaluationViewModel.ScoreModel data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			result = _lectorEvaluationService.SaveScoreData(data.scoreModify);

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

	}
}
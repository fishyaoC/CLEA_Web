using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.ViewModels;
using Newtonsoft.Json;
using MathNet.Numerics;

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
			ViewPLectorEvaluate? viewPLectorEvaluate = db.ViewPLectorEvaluates.Where(x => x.EdId == ED_ID).FirstOrDefault();
			if (viewPLectorEvaluate != null)
			{
				EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(ED_ID);
				if (viewPLectorEvaluate.EType == 0)
				{
					EEvaluationSche? eEvaluationSche = db.EEvaluationSches.Find(eEvaluateDetail.EsId) ?? null;
					if (eEvaluationSche != null)
					{
						vmd.subInfo = new LectorEvaluationViewModel.SubInfo()
						{
							Syllabus = eEvaluationSche.ETeachSyllabus,
							Object = eEvaluationSche.ETeachObject,
							Abstract = eEvaluationSche.ETeachAbstract
						};
					}
				}

				vmd.evInfo = new LectorEvaluationViewModel.EvInfo()
				{
					mType = viewPLectorEvaluate.EType == 0 ? "課程" : "教材",
					L_Name = viewPLectorEvaluate.EType == 0 ? viewPLectorEvaluate.LName : "訓練單位",
					C_B_Name = viewPLectorEvaluate.EType == 0 ? viewPLectorEvaluate.CName : viewPLectorEvaluate.MName,
					S_P_Name = viewPLectorEvaluate.EType == 0 ? viewPLectorEvaluate.DName : viewPLectorEvaluate.BpName
				};
				vmd.scoreModify = new LectorEvaluationViewModel.scoreModify()
				{
					ED_ID = ED_ID,
					mType = viewPLectorEvaluate.EType,
					lst_pic = _fileService.GetImageBase64List_PNG(eEvaluateDetail.EsId),
					ScoreA = eEvaluateDetail.EScoreA,
					ScoreB = eEvaluateDetail.EScoreB,
					ScoreBB = eEvaluateDetail.EScoreB,
					ScoreC = eEvaluateDetail.EScoreC,
					ScoreCB = eEvaluateDetail.EScoreC ,
					ScoreD = eEvaluateDetail.EScoreD,
					ScoreE = eEvaluateDetail.EScoreE,
					Remark = eEvaluateDetail.ERemark,
					IsClose = eEvaluateDetail.IsClose,
					Status =eEvaluateDetail.Status
				};
			}

			return View(vmd);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Modify(LectorEvaluationViewModel.ScoreModel data)
		{
			_lectorEvaluationService.user = User;
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
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
using MathNet.Numerics;
using Org.BouncyCastle.Ocsp;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using NPOI.POIFS.Crypt.Dsig.Facets;
using Microsoft.VisualBasic.ApplicationServices;

namespace Clea_Web.Controllers
{
	//後台講師專區-我的評鑑
	[Authorize]
	[UserPowerFilterAttribute]
	public class B_AssignClassController : BaseController
	{
		private readonly ILogger<B_AssignClassController> _logger;
		private AssignClassService _assignService;
		private FileService _fileService;
		private SMTPService _smtpService;
		private IConfiguration configuration;
		public B_AssignClassController(ILogger<B_AssignClassController> logger, dbContext dbCLEA, AssignClassService Service, FileService fileService, SMTPService smtpService, IConfiguration configuration)
		{
			_logger = logger;
			db = dbCLEA;
			_assignService = Service;
			_fileService = fileService;
			_smtpService = smtpService;
			this.configuration = configuration;
		}

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

		#region 資料轉入

		public IActionResult AddNewEv()
		{
			AssignClassViewModel.errorMsg result = new BaseViewModel.errorMsg();
			List<String> lst_mail = new List<string>();
			Int32 Count = 0;
			try
			{
				List<CClassLector> cClassLectors = db.CClassLectors.Where(x => x.IsEvaluate == false).OrderBy(x => x.CUid).ToList();


				if (cClassLectors != null && cClassLectors.Count > 0)
				{
					foreach (CClassLector ccl in cClassLectors)
					{
						EEvaluate? eEvaluateOri = db.EEvaluates.Where(x => x.MatchKey == ccl.CUid).FirstOrDefault() ?? null;
						if (eEvaluateOri != null)
						{
							EEvaluationSche eEvaluationSche = new EEvaluationSche()
							{
								EsId = Guid.NewGuid(),
								EId = eEvaluateOri.EId,
								MatchKey = ccl.ClUid,
								Reception = ccl.LUid,
								ChkNum = 0,
								Status = 0,
								IsMail = false,
								ScheNum = 0,
								IsSche = true,
								IsClose = false,
								Creuser = ccl.Creuser.Value,
								Credate = ccl.Credate.Value
							};
							db.EEvaluationSches.Add(eEvaluationSche);
							db.SaveChanges();
						}
						else
						{
							EEvaluate eEvaluate = new EEvaluate()
							{
								EId = Guid.NewGuid(),
								EType = 0,
								EYear = DateTime.Now.Year,
								MatchKey = ccl.CUid.Value,
								Creuser = ccl.Creuser.Value,
								Credate = ccl.Credate.Value
							};
							db.EEvaluates.Add(eEvaluate);

							EEvaluationSche eEvaluationSche = new EEvaluationSche()
							{
								EsId = Guid.NewGuid(),
								EId = eEvaluate.EId,
								MatchKey = ccl.ClUid,
								Reception = ccl.LUid,
								ChkNum = 0,
								Status = 0,
								IsMail = false,
								ScheNum = 0,
								IsSche = true,
								IsClose = false,
								Creuser = ccl.Creuser.Value,
								Credate = ccl.Credate.Value
							};
							db.EEvaluationSches.Add(eEvaluationSche);
							db.SaveChanges();
						}
						Count++;
						CLector? cLector = db.CLectors.Find(ccl.LUid);
						if (cLector != null)
						{
							if (string.IsNullOrEmpty(cLector.LEmail))
							{
								//lst_mail.Add(cLector.LName);
								continue;
							}
							else
							{
								lst_mail.Add(cLector.LEmail);
							}
						}
						ccl.IsEvaluate = true;
						db.SaveChanges();
					}
					result.CheckMsg = true;
				}
				else
				{
					result.CheckMsg = false;
					result.ErrorMsg = "目前無新資料";
				}

				//_smtpService.SendMail(lst_mail, "[通知]-CLEA授課資訊填寫", "老師您好，請至本會網站進行課程授課內容填寫，謝謝您。");
			}
			catch (Exception e)
			{

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

		#region 新增教師評鑑(由資料轉入取代)
		//public IActionResult Add()
		//{
		//	AssignClassViewModel.AddModel vmd = new AssignClassViewModel.AddModel();
		//	vmd.Year_selectListItems = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
		//	vmd.Year_selectListItems.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = (DateTime.Now.Year + 1).ToString() + "年", Value = (DateTime.Now.Year + 1).ToString() });
		//	vmd.Year_selectListItems.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = (DateTime.Now.Year).ToString() + "年", Value = (DateTime.Now.Year).ToString() });

		//	vmd.Class_selectListItems = _assignService.GetClassSelectItems();

		//	return View(vmd);
		//}
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public IActionResult Add(AssignClassViewModel.AddModel vmd)
		//{
		//	_assignService.user = User;
		//	AssignClassViewModel.errorMsg result = new BaseViewModel.errorMsg();

		//	ViewClassEvaluate? viewClassEvaluate = db.ViewClassEvaluates.Where(x => x.CUid == vmd.addModify.C_UID && x.EYear == vmd.addModify.Year).FirstOrDefault();

		//	if (!(viewClassEvaluate is null))
		//	{
		//		result.CheckMsg = false;
		//		result.ErrorMsg = "當年度課程重複!";
		//	}
		//	else
		//	{
		//		result = _assignService.SaveAddData(vmd);
		//	}

		//	if (result.CheckMsg)
		//	{
		//		TempData["TempMsgType"] = "success";
		//		TempData["TempMsgTitle"] = "儲存成功";
		//	}
		//	else
		//	{
		//		TempData["TempMsgType"] = "error";
		//		TempData["TempMsgTitle"] = "儲存失敗";
		//		TempData["TempMsg"] = result.ErrorMsg;
		//	}

		//	return RedirectToAction("Index");
		//}
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
		public IActionResult TC_Modify(Guid ES_ID)
		{
			AssignClassViewModel.TC_ModifyModel vmd = new AssignClassViewModel.TC_ModifyModel();
			ViewBAssignClassLector? viewBAssignClassLector = db.ViewBAssignClassLectors.Where(x => x.EsId == ES_ID).FirstOrDefault() ?? null;
			if (viewBAssignClassLector != null)
			{
				vmd.cLInfo = new AssignClassViewModel.CLInfo()
				{
					ES_ID = viewBAssignClassLector.EsId,
					Year = viewBAssignClassLector.EYear,
					C_ID = viewBAssignClassLector.CId,
					C_Name = viewBAssignClassLector.CName,
					S_ID = viewBAssignClassLector.DId,
					S_Name = viewBAssignClassLector.DName,
					L_ID = viewBAssignClassLector.LId,
					L_Name = viewBAssignClassLector.LName
				};
				vmd.tCModify = new AssignClassViewModel.TCModify()
				{
					E_ID = viewBAssignClassLector.EId,
					ES_ID = ES_ID
				};
			}
			vmd.selectListItems = _assignService.getTeacherItem(vmd.cLInfo.L_UID);
			vmd.lst_EvTeachers = _assignService.GetEvTeacherPageLists(ES_ID);
			return View(vmd);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult TC_Modify(AssignClassViewModel.TC_ModifyModel vmd)
		{
			AssignClassViewModel.errorMsg result = new BaseViewModel.errorMsg();
			_assignService.user = User;
			EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Where(x => x.EsId == vmd.tCModify.ES_ID && x.Evaluate == vmd.tCModify.L_UID_Ev).FirstOrDefault();

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

			return RedirectToAction("TC_Modify", new { ES_ID = vmd.tCModify.ES_ID });
		}
		#endregion

		#region 刪除評鑑教師
		public IActionResult TC_Delete(Guid ED_ID)
		{
			AssignClassViewModel.errorMsg error = new AssignClassViewModel.errorMsg();
			Guid ES_ID = Guid.Empty;
			EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(ED_ID) ?? null;
			if (eEvaluateDetail is null)
			{
				error.CheckMsg = false;
				error.ErrorMsg = "查無此筆資料!";
			}
			else
			{
				ES_ID = eEvaluateDetail.EsId;
				db.EEvaluateDetails.Remove(eEvaluateDetail);
				error.CheckMsg = Convert.ToBoolean(db.SaveChanges());
				if (error.CheckMsg)
				{
					Int32 ETcount = db.EEvaluateDetails.Where(x => x.EsId == ES_ID).ToList().Count;
					if (ETcount == 0)
					{
						EEvaluationSche? eEvaluationSche = db.EEvaluationSches.Find(ES_ID) ?? null;
						eEvaluationSche.Status = 1;
						error.CheckMsg = Convert.ToBoolean(db.SaveChanges());
					}
				}
			}

			return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
		}
		#endregion

		#region 檢視科目評鑑教師
		public IActionResult V_Index(Guid ES_ID)
		{
			AssignClassViewModel.vIndexModel vmd = new AssignClassViewModel.vIndexModel();
			EEvaluationSche? eEvaluationSche = db.EEvaluationSches.Find(ES_ID) ?? null;
			vmd.E_ID = eEvaluationSche.EId;
			vmd.ES_ID = ES_ID;
			vmd.vIndexLists = (from bcs in db.ViewBAssignClassScores
							   where bcs.EsId == ES_ID
							   select new AssignClassViewModel.vIndexList()
							   {
								   ED_ID = bcs.EdId,
								   Lv_Teacher = bcs.LName,
								   Status = bcs.Status
							   }).ToList();
			return View(vmd);
		}
		#endregion

		#region 檢視審查表
		public IActionResult V_Modify(Guid ED_ID)
		{
			AssignClassViewModel.V_ModifyModel vmd = new AssignClassViewModel.V_ModifyModel();
			EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(ED_ID) ?? null;
			ViewBAssignClassLector? viewBAssignClassLector = db.ViewBAssignClassLectors.Where(x => x.EsId == eEvaluateDetail.EsId).FirstOrDefault() ?? null;
			if (viewBAssignClassLector != null)
			{
				vmd.cLInfo = new AssignClassViewModel.CLInfo()
				{
					ES_ID = viewBAssignClassLector.EsId,
					Year = viewBAssignClassLector.EYear,
					C_ID = viewBAssignClassLector.CId,
					C_Name = viewBAssignClassLector.CName,
					S_ID = viewBAssignClassLector.DId,
					S_Name = viewBAssignClassLector.DName,
					L_ID = viewBAssignClassLector.LId,
					L_Name = viewBAssignClassLector.LName
				};
				vmd.picPath = _fileService.GetImageBase64List_PNG(viewBAssignClassLector.EsId);
				vmd.v_ScoreModel = _assignService.GetVModel(ED_ID);
			}

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

			return RedirectToAction("V_Index", new { ES_ID = eEvaluateDetail.EsId });
		}
		#endregion

		#region 匯出審查表
		public IActionResult Export_ScoreWord(Guid ES_ID)
		{
			ViewBAssignClassScore? viewBAssignClassScore = db.ViewBAssignClassScores.Where(x => x.EsId == ES_ID).FirstOrDefault() ?? null;
			ViewBAssignClassLector? viewBAssignClassLector = db.ViewBAssignClassLectors.Where(x => x.EsId == viewBAssignClassScore.EsId).FirstOrDefault() ?? null;

			String L_Name = viewBAssignClassScore != null ? viewBAssignClassScore.LName : string.Empty;
			String ScoreT = ((viewBAssignClassScore.EScoreA.Value) + (viewBAssignClassScore.EScoreB.Value) + (viewBAssignClassScore.EScoreC.Value) + (viewBAssignClassScore.EScoreD.Value) + (viewBAssignClassScore.EScoreE.Value)).ToString("#.##");

			String SourcePath = "./SampleFile/ClassEvaExample.docx";
			String SavePath = "./SampleFile/Output/" + L_Name + "-教學內容審查表.docx";

			using (DocX doc = DocX.Load(SourcePath))
			{
				doc.ReplaceText("[@Year$]", viewBAssignClassLector is null ? string.Empty : viewBAssignClassLector.Credate.Year.ToString());    //年
				doc.ReplaceText("[@Month$]", viewBAssignClassLector is null ? string.Empty : viewBAssignClassLector.Credate.Month.ToString());                            //月
				doc.ReplaceText("[@Day$]", viewBAssignClassLector is null ? string.Empty : viewBAssignClassLector.Credate.Day.ToString());                                //日
				doc.ReplaceText("[@ClassName$]", viewBAssignClassLector is null ? string.Empty : viewBAssignClassLector.CName);
				doc.ReplaceText("[@SubName$]", viewBAssignClassLector is null ? string.Empty : viewBAssignClassLector.DName);
				doc.ReplaceText("[@LecName$]", L_Name);
				doc.ReplaceText("[@ScoreA$]", viewBAssignClassScore is null ? string.Empty : viewBAssignClassScore.EScoreA.ToString());
				doc.ReplaceText("[@ScoreB$]", viewBAssignClassScore is null ? string.Empty : viewBAssignClassScore.EScoreB.ToString());
				doc.ReplaceText("[@ScoreC$]", viewBAssignClassScore is null ? string.Empty : viewBAssignClassScore.EScoreC.ToString());
				doc.ReplaceText("[@ScoreD$]", viewBAssignClassScore is null ? string.Empty : viewBAssignClassScore.EScoreD.ToString());
				doc.ReplaceText("[@ScoreE$]", viewBAssignClassScore is null ? string.Empty : viewBAssignClassScore.EScoreE.ToString());
				doc.ReplaceText("[@ScoreT$]", ScoreT);
				doc.ReplaceText("[@Remark$]", viewBAssignClassScore is null ? string.Empty : string.IsNullOrEmpty(viewBAssignClassScore.ERemark) ? string.Empty : viewBAssignClassScore.ERemark);
				doc.ReplaceText("[@Pass$]", Convert.ToDouble(ScoreT) >= 85 ? "■" : "□");
				doc.ReplaceText("[@Fail$]", Convert.ToDouble(ScoreT) >= 85 ? "□" : "■");

				doc.SaveAs(SavePath);
			}

			Byte[] result = System.IO.File.ReadAllBytes(SavePath);
			System.IO.File.Delete(SavePath);

			return File(result, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", L_Name + "-教學內容審查表.docx");
		}
		#endregion

		#region 匯出未上傳檔案教師
		public IActionResult V_Export()
		{
			Byte[] file = _assignService.Export_Excel();
			return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "未上傳課程檔案清單-" + DateTime.Now.ToLongDateString() + ".xlsx");
		}
		#endregion

		#region 檢視科目資訊
		public IActionResult ScoreIndex(Guid ES_ID)
		{
			AssignClassViewModel.S_Model vmd = new AssignClassViewModel.S_Model();
			EEvaluationSche? eEvaluationSche = db.EEvaluationSches.Find(ES_ID) ?? null;
			EEvaluate? eEvaluate = db.EEvaluates.Find(eEvaluationSche.EId) ?? null;
			if (eEvaluationSche != null)
			{
				vmd.E_ID = eEvaluationSche.EId;
				vmd.ES_ID = ES_ID;
				CClassLector? cClassLector = db.CClassLectors.Find(eEvaluationSche.MatchKey) ?? null;
				CClass? cClass = db.CClasses.Where(x => x.CUid == cClassLector.CUid).FirstOrDefault() ?? null;
				CBook? c_Book = null;
				if (cClass != null && !string.IsNullOrEmpty(cClass.CBookNum))
				{
					c_Book = db.CBooks.Where(x => x.MIndex == cClass.CBookNum).FirstOrDefault() ?? null;
				}
				vmd.Syllabus = eEvaluationSche.ETeachSyllabus;
				vmd.Objectives = eEvaluationSche.ETeachObject;
				vmd.Abstract = eEvaluationSche.ETeachAbstract;
				vmd.ClassName = db.CClasses.Find(eEvaluate.MatchKey).CName;
				vmd.ClassSub = db.CClassSubjects.Find(cClassLector.DUid).DName;
				vmd.SubClassTime = db.CClassSubjects.Find(cClassLector.DUid).DHour.ToString();
				vmd.ClassTeacher = "授課講師:" + db.CLectors.Find(cClassLector.LUid).LName + "\n 符合職業安全衛生教育訓練規則:" + cClassLector.ClQualify;
				vmd.IsPass = eEvaluationSche.IsPass;
				vmd.BookNamePublish = c_Book == null ? string.Empty : string.IsNullOrEmpty(c_Book.MName) ? string.Empty : c_Book.MName;
			}
			return View(vmd);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult ScoreIndex(AssignClassViewModel.S_Model vmd)
		{
			BaseService baseService = new BaseService();
			EEvaluationSche? eEvaluationSche = db.EEvaluationSches.Find(vmd.ES_ID) ?? null;
			if (eEvaluationSche != null && vmd.IsPass != null)
			{
				eEvaluationSche.IsPass = vmd.IsPass;
				eEvaluationSche.Upduser = Guid.Parse(baseService.GetUserID(User)); ;
				eEvaluationSche.Upddate = DateTime.Now;
				db.SaveChanges();
			}
			return RedirectToAction("ScoreIndex", new { ES_ID = vmd.ES_ID });
		}
		#endregion

		#region 匯出科目資訊
		public IActionResult ScoreExport(Guid ES_ID)
		{
			AssignClassViewModel.S_Model vmd = new AssignClassViewModel.S_Model();
			EEvaluationSche? eEvaluationSche = db.EEvaluationSches.Find(ES_ID) ?? null;
			EEvaluate? eEvaluate = db.EEvaluates.Find(eEvaluationSche.EId) ?? null;
			CClassLector? cClassLector = db.CClassLectors.Find(eEvaluationSche.MatchKey) ?? null;
			CClass? cClass = db.CClasses.Find(cClassLector.CUid) ?? null;
			CBook? c_Book = null;
			SysFile? sysFile1 = db.SysFiles.Where(x => x.FMatchKey == cClassLector.LUid).FirstOrDefault() ?? null;
			SysFile? sysFile2 = db.SysFiles.Where(x => x.FMatchKey == eEvaluationSche.Upduser).FirstOrDefault() ?? null;
			if (cClass != null && !string.IsNullOrEmpty(cClass.CBookNum))
			{
				c_Book = db.CBooks.Where(x => x.MIndex == cClass.CBookNum).FirstOrDefault() ?? null;
			}

			if (eEvaluationSche != null)
			{
				vmd.ES_ID = ES_ID;
				vmd.Syllabus = eEvaluationSche.ETeachSyllabus;
				vmd.Objectives = eEvaluationSche.ETeachObject;
				vmd.Abstract = eEvaluationSche.ETeachAbstract;
				vmd.ClassName = db.CClasses.Find(eEvaluate.MatchKey).CName;
				vmd.ClassSub = db.CClassSubjects.Find(cClassLector.DUid).DName;
				vmd.SubClassTime = db.CClassSubjects.Find(cClassLector.DUid).DHour.ToString();
				vmd.ClassTeacher = db.CLectors.Find(cClassLector.LUid).LName;
			}

			String SourcePath = "./SampleFile/教學單元大綱Sample.docx";
			String SavePath = "./SampleFile/Output/" + vmd.ClassSub + "(" + vmd.ClassName + ")-教學單元大綱.docx";

			using (DocX doc = DocX.Load(SourcePath))
			{
				doc.ReplaceText("[@Year$]", DateTime.Now.Year.ToString());
				doc.ReplaceText("[@ClassName$]", string.IsNullOrEmpty(vmd.ClassName) ? string.Empty : vmd.ClassName);
				doc.ReplaceText("[@ClassSub$]", string.IsNullOrEmpty(vmd.ClassSub) ? string.Empty : vmd.ClassSub);
				doc.ReplaceText("[@ClassSubTime$]", string.IsNullOrEmpty(vmd.SubClassTime) ? string.Empty : vmd.SubClassTime);
				doc.ReplaceText("[@Syllabus$]", string.IsNullOrEmpty(vmd.Syllabus) ? string.Empty : vmd.Syllabus);
				doc.ReplaceText("[@Objectives$]", string.IsNullOrEmpty(vmd.Objectives) ? string.Empty : vmd.Objectives);
				doc.ReplaceText("[@LName$]", string.IsNullOrEmpty(vmd.ClassTeacher) ? string.Empty : vmd.ClassTeacher);
				doc.ReplaceText("[@ClQualify$]", cClassLector.ClQualify);
				doc.ReplaceText("[@Abstract$]", string.IsNullOrEmpty(vmd.Abstract) ? string.Empty : vmd.Abstract);
				doc.ReplaceText("[@BookNamePublish$]", c_Book == null ? string.Empty : string.IsNullOrEmpty(c_Book.MName) ? string.Empty : c_Book.MName);
				doc.ReplaceText("[@BookNumber$]", string.IsNullOrEmpty(vmd.BookNumber) ? string.Empty : vmd.BookNumber);
				String PicWrite = sysFile1 == null ? string.Empty : Path.Combine(configuration.GetValue<String>("FileRootPath"), sysFile1.FPath + "\\" + sysFile1.FNameDl + "." + sysFile1.FExt);
				if (!string.IsNullOrEmpty(PicWrite))
				{
					var docxImage1 = doc.AddImage(PicWrite);
					var paragraphs = doc.Paragraphs.Where(x => x.Text.Equals("[@Write$]"));
					foreach (var paragraph in paragraphs)
					{
						paragraph.InsertPicture(docxImage1.CreatePicture(50, 150), 0);
						paragraph.ReplaceText("[@Write$]", "");
					}
				}
				else
				{
					doc.ReplaceText("[@Write$]", "");
				}

				String PicEvName = sysFile2 == null ? string.Empty : Path.Combine(configuration.GetValue<String>("FileRootPath"), sysFile2.FPath + "\\" + sysFile2.FNameDl + "." + sysFile2.FExt);
				if (!string.IsNullOrEmpty(PicWrite))
				{
					var docxImage2 = doc.AddImage(PicEvName);
					var paragraphs2 = doc.Paragraphs.Where(x => x.Text.Equals("[@EvName$]"));
					foreach (var paragraph2 in paragraphs2)
					{
						paragraph2.InsertPicture(docxImage2.CreatePicture(50, 150), 0);
						paragraph2.ReplaceText("[@EvName$]", "");
					}
				}
				else
				{
					doc.ReplaceText("[@EvName$]", "");
				}

				doc.ReplaceText("[@Pass$]", eEvaluationSche.IsPass == true ? "■" : "□");
				doc.ReplaceText("[@Fail$]", eEvaluationSche.IsPass != true ? "■" : "□");

				doc.SaveAs(SavePath);
			}

			Byte[] result = System.IO.File.ReadAllBytes(SavePath);
			System.IO.File.Delete(SavePath);

			return File(result, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", vmd.ClassSub + "(" + vmd.ClassName + ")-教學單元大綱.docx");
		}
		#endregion
	}
}
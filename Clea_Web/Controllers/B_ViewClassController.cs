using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Clea_Web.ViewModels;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using System;
using Novacode;

namespace Clea_Web.Controllers
{
    //後台講師專區-評鑑成績
    [Authorize]
    [UserPowerFilterAttribute]
    public class B_ViewClassController : BaseController
    {
        private readonly ILogger<B_ViewClassController> _logger;
        private ViewClassService _viewClassService;

        public B_ViewClassController(ILogger<B_ViewClassController> logger, dbContext dbCLEA, ViewClassService Service)
        {
            _logger = logger;
            db = dbCLEA;
            _viewClassService = Service;
        }

        #region 開課列表
        public IActionResult Index(String? data, Int32? page)
        {
            ViewClassViewModel.SchClassModel_V vmd = new ViewClassViewModel.SchClassModel_V();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schClassItem_V = JsonConvert.DeserializeObject<ViewClassViewModel.SchClassItem_V>(value: data);
                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schClassItem_V);
            }
            else
            {
                vmd.schClassItem_V = new ViewClassViewModel.SchClassItem_V();
            }

            vmd.DropDownList_Year = _viewClassService.GetSelectListItems_Year();
            vmd.getClassList_Vs = _viewClassService.getClassPageList_Vs(vmd.schClassItem_V, page.Value);

            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ViewClassViewModel.SchClassModel_V vmd)
        {
            vmd.DropDownList_Year = _viewClassService.GetSelectListItems_Year();
            vmd.getClassList_Vs = _viewClassService.getClassPageList_Vs(vmd.schClassItem_V, 1);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schClassItem_V);
            return View(vmd);
        }
        #endregion

        #region 科目列表
        public IActionResult L_Index(Guid U_ID, Int32 Year, String? data, Int32? page)
        {
            ViewClassViewModel.SchSubLecModel_V? vmd = new ViewClassViewModel.SchSubLecModel_V();
            page = page ?? 1;

            vmd.EvaluationActonInfo = new BaseViewModel.EvaluationActonInfo() { Key = U_ID, ControllerName = "L_Index", Year = Year };

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schSubLecItem_V = JsonConvert.DeserializeObject<ViewClassViewModel.SchSubLecItem_V>(value: data);
                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schSubLecItem_V);
            }
            else
            {
                vmd.schSubLecItem_V = new ViewClassViewModel.SchSubLecItem_V();
            }

            vmd.getSubLecPageLists_V = _viewClassService.GetSubLecPageLists_V(U_ID, Year, vmd.schSubLecItem_V, page.Value);
            ViewBag.EvaInfo = JsonConvert.SerializeObject(vmd.EvaluationActonInfo);
            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult L_Index(ViewClassViewModel.SchSubLecModel_V vmd)
        {
            vmd.getSubLecPageLists_V = _viewClassService.GetSubLecPageLists_V(vmd.EvaluationActonInfo.Key, vmd.EvaluationActonInfo.Year.Value, vmd.schSubLecItem_V, 1);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schSubLecItem_V);
            ViewBag.EvaInfo = JsonConvert.SerializeObject(vmd.EvaluationActonInfo);
            return View(vmd);
        }
        #endregion

        #region 科目授課教師

        public IActionResult P_Index(Guid C_ID, Guid Sub_UID, Int32 Year, String? data, Int32? page)
        {
            ViewClassViewModel.CL_Model_V? vmd = new ViewClassViewModel.CL_Model_V();
            page = page ?? 1;

            vmd.ClassInfo_V = _viewClassService.GetClassInfo_V(Year, C_ID, Sub_UID);
            vmd.getClassLectorPageLists_V = _viewClassService.GetClassLectorPageLists_V(C_ID, Sub_UID, Year, 0, page.Value);

            return View(vmd);
        }
        #endregion


        #region 檢視受評教師列表
        public IActionResult G_Index(Guid CLUid, Int32 Year)
        {
            CClassLector? cClassLector = db.CClassLectors.Find(CLUid) ?? null;
            List<CEvaluation>? lst_cEvaluation = cClassLector != null ? db.CEvaluations.Where(x => x.CUid == cClassLector.CUid && x.DUid == cClassLector.DUid && x.LevYear == Year).ToList() : null;

            ViewClassViewModel.Modify_Model_V vmd = new ViewClassViewModel.Modify_Model_V();

            vmd.cSTinfo_V = _viewClassService.GetCSTInfo_V(Year, CLUid, cClassLector.CUid.Value, cClassLector.DUid.Value, cClassLector.LUid.Value);
            vmd.m_EvTeacherPageLists_V = _viewClassService.m_EvTeacher_Vs(Year, cClassLector.CUid.Value, cClassLector.DUid.Value, 0, 1);
            return View(vmd);
        }
        #endregion


        #region 檢視個別教師成績 Modify
        public IActionResult L_Modify(Guid CEvUid)
        {
            CEvaluation? cEvaluation = db.CEvaluations.Find(CEvUid) ?? null;
            CClassLector? cClassLector = db.CClassLectors.Find(cEvaluation.ClUid) ?? null;

            ViewClassViewModel.Modify_Score vmd = new ViewClassViewModel.Modify_Score();
            vmd.cSTinfo_V = _viewClassService.GetCSTInfo_V(cEvaluation.LevYear, cClassLector.ClUid, cClassLector.CUid.Value, cClassLector.DUid.Value, cClassLector.LUid.Value);
            vmd.picPath = _viewClassService.GetClassSubPic(cClassLector.ClUid, 0);
            vmd.scoreModify = _viewClassService.Get_EvaData(CEvUid);

            return View(vmd);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult L_Modify(ViewClassViewModel.Modify_Score data)
        {
            ViewClassViewModel.errorMsg errorMsg = new ViewClassViewModel.errorMsg();
            _viewClassService.user = User;
            errorMsg = _viewClassService.Savedata_V(data.scoreModify);

            if (errorMsg.CheckMsg)
            {
                TempData["TempMsgType"] = "success";
                TempData["TempMsgTitle"] = "儲存成功";
            }
            else
            {
                TempData["TempMsgType"] = "error";
                TempData["TempMsgTitle"] = "儲存失敗";
                TempData["TempMsg"] = errorMsg.ErrorMsg;
            }

            return RedirectToAction("G_Index", new { CLUid = data.cSTinfo_V.CL_UID, Year = data.cSTinfo_V.Year });
        }
        #endregion

        #region 匯出科目教師評分表Word
        public IActionResult Export_ScoreWord(Guid CEvUid)
        {
            CEvaluation? cEvaluation = db.CEvaluations.Find(CEvUid) ?? null;
            CLector? cLector = db.CLectors.Find(cEvaluation.LUid) ?? null;
            CClass? cClass = db.CClasses.Find(cEvaluation.CUid) ?? null;
            CClassSubject? cClassSubject = db.CClassSubjects.Find(cEvaluation.DUid) ?? null;

            String L_Name = cLector != null ? cLector.LName : string.Empty;
            String ScoreT = ((cEvaluation.ScoreA.Value * 0.6) + (cEvaluation.ScoreB.Value * 0.1) + (cEvaluation.ScoreC.Value * 0.1) + (cEvaluation.ScoreD.Value * 0.1) + (cEvaluation.ScoreE.Value * 0.1)).ToString("#.##");

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
                doc.ReplaceText("[@ScoreA$]", cEvaluation is null ? string.Empty : cEvaluation.ScoreA.ToString());
                doc.ReplaceText("[@ScoreB$]", cEvaluation is null ? string.Empty : cEvaluation.ScoreB.ToString());
                doc.ReplaceText("[@ScoreC$]", cEvaluation is null ? string.Empty : cEvaluation.ScoreC.ToString());
                doc.ReplaceText("[@ScoreD$]", cEvaluation is null ? string.Empty : cEvaluation.ScoreD.ToString());
                doc.ReplaceText("[@ScoreE$]", cEvaluation is null ? string.Empty : cEvaluation.ScoreE.ToString());
                doc.ReplaceText("[@ScoreT$]", ScoreT);
                doc.ReplaceText("[@Remark$]", cEvaluation is null ? string.Empty : string.IsNullOrEmpty(cEvaluation.Remark) ? string.Empty : cEvaluation.Remark);
                doc.ReplaceText("[@Pass$]", Convert.ToDouble(ScoreT) >= 85 ? "■" : "□");
                doc.ReplaceText("[@Fail$]", Convert.ToDouble(ScoreT) >= 85 ? "□" : "■");

                doc.SaveAs(SavePath);
            }

            FileInfo fi = new FileInfo(SavePath);
            FileStream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read);
            return File(fs, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", L_Name + "-教學內容審查表.docx");
        }
        #endregion
    }
}
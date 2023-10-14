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

        #region 開課列表
        public IActionResult Index(String? data, Int32? page)
        {
            AssignClassViewModel.SchClassModel vmd = new AssignClassViewModel.SchClassModel();
            page = page ?? 1;


            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schClassItem = JsonConvert.DeserializeObject<AssignClassViewModel.SchClassItem>(value: data);
                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schClassItem);
            }
            else
            {
                vmd.schClassItem = new AssignClassViewModel.SchClassItem();
            }

            vmd.getClassPageList = _assignService.GetClassPageList(vmd.schClassItem, page.Value);

            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(AssignClassViewModel.SchClassModel vmd)
        {
            vmd.getClassPageList = _assignService.GetClassPageList(vmd.schClassItem, 1);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schClassItem);
            return View(vmd);
        }
        #endregion

        #region 科目列表
        /// <summary>
        /// 教師課程表
        /// </summary>
        /// <param name="U_ID">Class_UID</param>
        /// <param name="data"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IActionResult L_Index(Guid U_ID, String? data, Int32? page)
        {

            AssignClassViewModel.SchSubLecModel? vmd = new AssignClassViewModel.SchSubLecModel();
            page = page ?? 1;

            vmd.EvaluationActonInfo = new BaseViewModel.EvaluationActonInfo() { Key = U_ID, ControllerName = "L_Index" };

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schSubLecItem = JsonConvert.DeserializeObject<AssignClassViewModel.SchSubLecItem>(value: data);
                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schSubLecItem);
            }
            else
            {
                vmd.schSubLecItem = new AssignClassViewModel.SchSubLecItem();
            }

            vmd.getSubLecPageLists = _assignService.GetSubLecPageLists(U_ID, 0, vmd.schSubLecItem, page.Value);
            ViewBag.EvaInfo = JsonConvert.SerializeObject(vmd.EvaluationActonInfo);


            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult L_Index(AssignClassViewModel.SchSubLecModel vmd)
        {
            vmd.getSubLecPageLists = _assignService.GetSubLecPageLists(vmd.EvaluationActonInfo.Key, 0, vmd.schSubLecItem, 1);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schSubLecItem);
            ViewBag.EvaInfo = JsonConvert.SerializeObject(vmd.EvaluationActonInfo);
            return View(vmd);
        }
        #endregion

        #region 科目授課教師
        /// <summary>
        /// 科目授課教師
        /// </summary>
        /// <param name="C_ID">課程PK</param>
        /// <param name="Sub_UID">科目PK</param>
        /// <param name="data"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IActionResult P_Index(Guid C_ID, Guid Sub_UID, String? data, Int32? page)
        {
            AssignClassViewModel.CL_Model? vmd = new AssignClassViewModel.CL_Model();
            page = page ?? 1;

            vmd.ClassInfo = _assignService.GetClassInfo(C_ID, Sub_UID);
            vmd.getClassLectorPageLists = _assignService.GetClassLectorPageLists(C_ID, Sub_UID, 0, page.Value);

            return View(vmd);
        }
        #endregion

        #region 指定評鑑教師 Modify
        public IActionResult L_Modify(Guid CL_UID)
        {
            CClassLector? cClassLector = db.CClassLectors.Find(CL_UID) ?? null;

            AssignClassViewModel.Modify_Model vmd = new AssignClassViewModel.Modify_Model();
            if (cClassLector != null)
            {
                vmd.cSTinfo = _assignService.GetCSTInfo(CL_UID, cClassLector.CUid.Value, cClassLector.DUid.Value, cClassLector.LUid.Value);
                vmd.DropDownItem = _assignService.getTeacherItem(cClassLector.LUid.Value);
                vmd.lModify = new AssignClassViewModel.LModify();
                vmd.m_EvTeacherPageLists = _assignService.GetEvTeacherPageLists(cClassLector.CUid.Value, cClassLector.DUid.Value, 0, 1);
            }
            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult L_Modify(AssignClassViewModel.Modify_Model data)
        {
            AssignClassViewModel.errorMsg result = new BaseViewModel.errorMsg();
            _assignService.user = User;
            CEvaluation? cEvaluation = db.CEvaluations.Where(x => x.LevYear == (DateTime.Now.Year + 1) && x.LevYear == (DateTime.Now.Year + 1) && x.LevType == 0 && x.ClUid == data.cSTinfo.CL_UID && x.LUidEv == data.lModify.L_UID_Ev).FirstOrDefault();

            if (cEvaluation is null)
            {
                result = _assignService.SaveData(data, 0);
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

            return RedirectToAction("L_Modify", new { CL_UID = data.cSTinfo.CL_UID });
        }
        #endregion

        #region 刪除評鑑教師
        public IActionResult L_Delete(Guid LevId)
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

        #region 
        public IActionResult V_Index()
        {
            return View();
        }
        #endregion

        #region 匯出未上傳檔案教師
        public IActionResult V_Export()
        {
            Byte[] file = _assignService.Export_Excel();
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", (DateTime.Now.Year + 1).ToString() + "年評鑑-未上傳課程資訊-" + DateTime.Now.ToLongDateString() + ".xlsx");
        }
        #endregion
    }
}
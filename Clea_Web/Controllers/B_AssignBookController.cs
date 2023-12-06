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
using Org.BouncyCastle.Ocsp;


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
        private SMTPService _smtpService;
        public B_AssignBookController(ILogger<B_AssignBookController> logger, dbContext dbCLEA, AssignBookService Service, FileService fileService, SMTPService smtpService)
        {
            _logger = logger;
            db = dbCLEA;
            _assignBookService = Service;
            _fileService = fileService;
            _smtpService = smtpService;
        }

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
            vmd.selectListItemsBook = _assignBookService.GetSelectListItemsBook();
            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(AssignBookViewModel.AddModel vmd)
        {
            BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
            _assignBookService.user = User;
            result = _assignBookService.SaveAddData(vmd.addModify);

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

        #region 檔案上傳
        public IActionResult ImportPub(Guid E_ID)
        {
            AssignBookViewModel.uploadModel vmd = new AssignBookViewModel.uploadModel();

            EEvaluate? eEvaluate = db.EEvaluates.Find(E_ID) ?? null;
            if (eEvaluate != null)
            {
                CBook? cBook = db.CBooks.Find(eEvaluate.MatchKey) ?? null;
                if (cBook != null)
                {
                    vmd.bookInfor = new AssignBookViewModel.BookInfor();
                    vmd.bookInfor.E_ID = E_ID;
                    vmd.bookInfor.M_Index = cBook.MIndex;
                    vmd.bookInfor.M_Name = cBook.MName;
                }

                List<EEvaluationSche> eEvaluationSches = db.EEvaluationSches.Where(x => x.EId == E_ID).ToList();
                if (eEvaluationSches.Count > 0)
                {
                    vmd.uploadFiles = new List<AssignBookViewModel.uploadFile>();
                    foreach (EEvaluationSche EES in eEvaluationSches)
                    {
                        CBookDetail? cBookDetail = db.CBookDetails.Find(EES.MatchKey) ?? null;
                        vmd.uploadFiles.Add(new AssignBookViewModel.uploadFile()
                        {
                            ES_ID = EES.EsId,
                            PubName = (from CDP in db.CBookPublishes where CDP.BpId == cBookDetail.MdPublish select CDP).FirstOrDefault() == null ? null : (from CDP in db.CBookPublishes where CDP.BpId == cBookDetail.MdPublish select CDP).FirstOrDefault().BpName,
                            fileName = (from file in db.SysFiles where file.FMatchKey == EES.EsId select file).FirstOrDefault() == null ? null : (from file in db.SysFiles where file.FMatchKey == EES.EsId select file).FirstOrDefault().FFullName,
                            F_ID = (from file in db.SysFiles where file.FMatchKey == EES.EsId select file).FirstOrDefault() == null ? null : (from file in db.SysFiles where file.FMatchKey == EES.EsId select file).FirstOrDefault().FileId
                        });
                    }
                }
            }

            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportPub(AssignBookViewModel.uploadModel data)
        {
            BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
            _assignBookService.user = User;
            _fileService.user = User;

            if (data.uploadFiles.Count > 0)
            {
                foreach (var item in data.uploadFiles)
                {
                    if (string.IsNullOrEmpty(item.fileName))
                    {
                        result.CheckMsg = Path.GetExtension(item.file.FileName).Contains(".pdf") ? true : false;
                        if (!result.CheckMsg)
                        {
                            result.ErrorMsg = item.fileName + "檔案格式有誤，請上傳PDF檔案!";
                            break;
                        }
                        else
                        {
                            result.CheckMsg = _fileService.UploadFile(true, 1, item.ES_ID, item.file, true);
                        }
                    }
                    else
                    {
                        result.CheckMsg = true;
                        continue;
                    }
                }
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

            if (!result.CheckMsg && result.ErrorMsg.Contains("PDF"))
            {
                return RedirectToAction("ImportPub", new { E_ID = data.bookInfor.E_ID });
            }
            else
            {
                return RedirectToAction("Index");
            }
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
            result.CheckMsg = true;
            List<EEvaluateDetail> eEvaluateDetails = new List<EEvaluateDetail>();
            List<EEvaluationSche> eEvaluationSches = db.EEvaluationSches.Where(x => x.EId == vmd.modify.E_ID).ToList();
            if (eEvaluationSches.Count > 0)
            {
                foreach (EEvaluationSche thr in eEvaluationSches)
                {
                    List<EEvaluateDetail> eEvaluateDetail2 = db.EEvaluateDetails.Where(x => x.EsId == thr.EsId).ToList();

                    eEvaluateDetails.AddRange(eEvaluateDetail2);
                }

                if (eEvaluateDetails.Count > 0)
                {
                    foreach (EEvaluateDetail chk in eEvaluateDetails)
                    {
                        if (chk.Evaluate != null && chk.Evaluate.Value == vmd.modify.L_UID_Ev)
                        {
                            result.CheckMsg = false;
                            result.ErrorMsg = "指定教師重複!";
                        }
                    }
                }
            }

            if (result.CheckMsg)
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
            ViewBAssignBookScore? viewBAssignBookScore = db.ViewBAssignBookScores.Where(x => x.EId == E_ID).FirstOrDefault() ?? null;
            String M_Name = viewBAssignBookScore is null ? "查無教材名稱" : viewBAssignBookScore.MName;
            Byte[] file = _assignBookService.Export_Excel(E_ID);
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", M_Name + "-教材審查統計表.xlsx");
        }
        #endregion

        #region 教材版本列表
        public IActionResult V_Index(Guid E_ID)
        {
            AssignBookViewModel vmd = new AssignBookViewModel();
            vmd.lst_EDInfo = _assignBookService.getEDInfoList(E_ID);
            vmd.E_ID = E_ID;
            EEvaluationSche? evaluationSche = db.EEvaluationSches.Where(x => x.EId == E_ID).FirstOrDefault() ?? null;
            if (evaluationSche != null)
            {
                vmd.IsClose = evaluationSche.IsClose;
            }
            return View(vmd);
        }
        #endregion

        #region 匯出審查表WORD
        public IActionResult Export_ScoreWord(Guid E_ID)
        {
            String dir = Guid.NewGuid().ToString();
            ViewBAssignBookScore? viewBAssignBookScore = db.ViewBAssignBookScores.Where(x => x.EId == E_ID).FirstOrDefault() ?? null;
            String M_Name = viewBAssignBookScore is null ? "查無教材名稱" : viewBAssignBookScore.MName;
            Byte[] file = _assignBookService.Export_ScoreZip(E_ID, dir);
            return File(file, "application/zip", M_Name + "-教材審查表.zip");
        }
        #endregion

        #region 編輯審查表
        public IActionResult V_Modify(Guid ED_ID)
        {
            AssignBookViewModel.V_ModifyModel vmd = new AssignBookViewModel.V_ModifyModel();
            ViewBAssignBookEvaluateTeacher? viewBAssignBookEvaluateTeacher = db.ViewBAssignBookEvaluateTeachers.Where(x => x.EdId == ED_ID).FirstOrDefault() ?? null;
            EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(ED_ID) ?? null;

            if (viewBAssignBookEvaluateTeacher != null && eEvaluateDetail != null)
            {
                vmd.bookInfor = new AssignBookViewModel.BookInfor();
                vmd.bookInfor.E_ID = viewBAssignBookEvaluateTeacher.EId;
                vmd.bookInfor.B_Name = viewBAssignBookEvaluateTeacher.MName;
                vmd.bookInfor.B_ID = viewBAssignBookEvaluateTeacher.MIndex;
                vmd.bookInfor.B_Publish = viewBAssignBookEvaluateTeacher.BpName;
                vmd.bookInfor.L_ID_Ev = viewBAssignBookEvaluateTeacher.LId;
                vmd.bookInfor.L_Name_Ev = viewBAssignBookEvaluateTeacher.LName;
                vmd.picPath = _fileService.GetImageBase64List_PNG(viewBAssignBookEvaluateTeacher.EsId);
                vmd.scoreModify = _assignBookService.GetScoreModel(ED_ID);
            }

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

        #region 刪除檔案
        public IActionResult F_Delete(Guid F_ID)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            try
            {
                SysFile? sysFile = db.SysFiles.Find(F_ID) ?? null;

                if (sysFile != null)
                {
                    error.CheckMsg = _fileService.DeleteFile(sysFile, true);
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

        #region 結案
        public IActionResult Modify_Status(Guid E_ID, Boolean IsType)
        {
            BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
            AssignClassService assignClassService = new AssignClassService(db);
            BaseService baseService = new BaseService();
            assignClassService.user = User;
            List<EEvaluateDetail> eEvaluateDetails = db.EEvaluateDetails.Where(x => x.EId == E_ID && x.Status < 4).ToList();
            if (IsType && eEvaluateDetails != null && eEvaluateDetails.Count > 0)
            {
                result.CheckMsg = false;
                result.ErrorMsg = "尚有項目未完成(檔案未上傳、尚未評分、尚未審核)";
            }
            else
            {
                List<EEvaluationSche> eEvaluationSches = db.EEvaluationSches.Where(x => x.EId == E_ID).ToList();
                if (eEvaluationSches != null && eEvaluationSches.Count > 0)
                {
                    foreach (EEvaluationSche item in eEvaluationSches)
                    {
                        item.IsClose = IsType;
                        item.Status = IsType ? 4 : item.Status;
                        item.Upduser = Guid.Parse(baseService.GetUserID(User));
                        item.Upddate = DateTime.Now;
                        result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
                        assignClassService.SaveClose(item.EsId, IsType);
                    }
                }
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
    }
}
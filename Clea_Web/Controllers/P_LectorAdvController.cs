using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.ViewModels;
using Newtonsoft.Json;

namespace Clea_Web.Controllers
{
    //前台講師專區-我的進修資料
    public class P_LectorAdvController : BaseController
    {
        private readonly ILogger<P_LectorAdvController> _logger;
        private P_LectorAdvService _P_LectorAdvService;

        public P_LectorAdvController(ILogger<P_LectorAdvController> logger, dbContext dbCLEA, P_LectorAdvService Service)
        {
            _logger = logger;
            db = dbCLEA;
            _P_LectorAdvService = Service;
        }


        #region Index
        public IActionResult Index(String? data, Int32? page)
        {
            _P_LectorAdvService.user = User;

            B_LectorAdvViewModel.SchModel vmd = new B_LectorAdvViewModel.SchModel();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<B_LectorAdvViewModel.SchItem>(value: data);
                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                vmd.schItem = new B_LectorAdvViewModel.SchItem();
            }
            vmd.schPageList2 = _P_LectorAdvService.schPages(vmd.schItem, page.Value, 15);

            return View(vmd);
        }
        #endregion

        #region D_Index
        public IActionResult D_Index(String LUid, int YearNow, String? data, Int32? page)
        {
            B_LectorAdvViewModel.D_Model vmd = new B_LectorAdvViewModel.D_Model();
            page = page ?? 1;


            vmd.D_PageList = _P_LectorAdvService.D_schPages(LUid, YearNow, page.Value, 15);
            vmd.YearNow = YearNow;

            CLector cl = db.CLectors.Where(x => x.LUid.ToString() == LUid).FirstOrDefault();
            vmd.LName = cl.LName;

            return View(vmd);
        }
        #endregion

        #region V_Modify
        public IActionResult V_Modify(string LaUid)
        {
            B_LectorAdvViewModel.Modify? vm = null;

            if (!string.IsNullOrEmpty(LaUid))
            {
                vm = _P_LectorAdvService.GetEditData(LaUid);
            }
            return View(vm);
        }
        #endregion

        #region Modify
        public IActionResult Modify(string LaUid)
        {
            B_LectorAdvViewModel.Modify? vm = null;
            _P_LectorAdvService.user = User;

            if (!string.IsNullOrEmpty(LaUid))
            {
                //編輯
                vm = _P_LectorAdvService.GetEditData(LaUid);
            }
            else
            {
                //新增
                vm = new B_LectorAdvViewModel.Modify();
                vm.LaYear = DateTime.Now.Year - 1911;
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modify(B_LectorAdvViewModel.Modify vm)
        {
            _P_LectorAdvService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _P_LectorAdvService.SaveData(vm);

            //SWAL儲存成功
            if (error.CheckMsg)
            {
                TempData["TempMsgType"] = "success";
                TempData["TempMsgTitle"] = "儲存成功";
            }
            else
            {
                TempData["TempMsgType"] = "error";
                TempData["TempMsgTitle"] = "儲存失敗";
                TempData["TempMsg"] = error.ErrorMsg;
            }


            if (error.CheckMsg)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Modify");

            }

            return RedirectToAction("Index");


            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion

        #region DownloadFile
        public ActionResult DownloadFile(String FilePath, String FileName)
        {
            try
            {
                FileStream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                return File(stream, "application/octet-stream", FileName); //MME 格式 可上網查 此為通用設定
            }
            catch (System.Exception)
            {
                return Content("<script>alert('查無此檔案');window.close()</script>");
            }
        }
        #endregion

        #region Del

        [HttpPost]
        public IActionResult Delete(Guid LaUid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _P_LectorAdvService.DelData(LaUid);

            return Json(new { chk = true, msg = "" });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion

    }
}
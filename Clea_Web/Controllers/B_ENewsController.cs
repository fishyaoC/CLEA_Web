using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Clea_Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clea_Web.Controllers
{
    //勞工教育電子報
    [Authorize]
    [UserPowerFilterAttribute]
    public class B_ENewsController : BaseController
    {
        private readonly ILogger<B_ENewsController> _logger;
        private ENewsService _ENewsService;
        private FileService _fileservice;
        private IConfiguration configuration;

        public B_ENewsController(ILogger<B_ENewsController> logger, dbContext dbCLEA, ENewsService Service, FileService fileservice, IConfiguration configuration)
        {
            _logger = logger;
            db = dbCLEA;
            _ENewsService = Service;
            _fileservice = fileservice;
            this.configuration = configuration;
        }

        #region 新增、編輯
        public IActionResult Modify(Guid Uid)
        {
            ENewsViewModel.Modify? vm = new ENewsViewModel.Modify();

            if (Uid != null)
            {
                //編輯
                vm = _ENewsService.GetEditData(Uid);
            }
            else
            {
                //新增
                vm = new ENewsViewModel.Modify();
            }


            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modify(ENewsViewModel.Modify vm)
        {
            _ENewsService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _ENewsService.SaveData(vm);

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

            return RedirectToAction("Index");
        }
        #endregion

        #region 查詢
        public IActionResult Index(String? data, Int32? page)
        {
            ENewsViewModel.SchModel vmd = new ENewsViewModel.SchModel();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<ENewsViewModel.SchItem>(value: data);

                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                vmd.schItem = new ENewsViewModel.SchItem();
            }

            vmd.schPageList2 = _ENewsService.schPages(vmd.schItem, page.Value, 15);           
            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ENewsViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _ENewsService.schPages(vmd.schItem, 1, 15);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            return View(vmd);
        }
        #endregion

        #region 刪除

        [HttpPost]
        public IActionResult Delete(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _ENewsService.DelData(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion

        #region 刪除檔案

        [HttpPost]
        public IActionResult DeleteFile(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _ENewsService.DelFile(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
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
    }
}
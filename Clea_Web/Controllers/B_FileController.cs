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
    //檔案下載管理
    [Authorize]
    [UserPowerFilterAttribute]
    public class B_FileController  : BaseController
    {
        private readonly ILogger<B_FileController > _logger;
        private FileDownloadService _FileDownloadService;
        private FileService _fileservice;
        private IConfiguration configuration;

        public B_FileController (ILogger<B_FileController > logger, dbContext dbCLEA, FileDownloadService Service, FileService fileservice, IConfiguration configuration)
        {
            _logger = logger;
            db = dbCLEA;
            _FileDownloadService = Service;
            _fileservice = fileservice;
            this.configuration = configuration;

        }

        #region 新增、編輯
        public IActionResult Modify(Guid Uid)
        {
            FileDownloadViewModel.Modify? vm = new FileDownloadViewModel.Modify();

            if (Uid != null)
            {
                //編輯
                vm = _FileDownloadService.GetEditData(Uid);
            }
            else
            {
                //新增
                vm = new FileDownloadViewModel.Modify();
            }
            vm.DropDownLevel = _FileDownloadService.getLevelItem();
            vm.DropDownClass = _FileDownloadService.getClassItem();



            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modify(FileDownloadViewModel.Modify vm)
        {
            _FileDownloadService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _FileDownloadService.SaveData(vm);

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
            FileDownloadViewModel.SchModel vmd = new FileDownloadViewModel.SchModel();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<FileDownloadViewModel.SchItem>(value: data);

                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                vmd.schItem = new FileDownloadViewModel.SchItem();
            }

            vmd.schPageList2 = _FileDownloadService.schPages(vmd.schItem, page.Value, 15);
            vmd.DropDownLevel = _FileDownloadService.getLevelItem();
            vmd.DropDownClass = _FileDownloadService.getClassItem();
            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(FileDownloadViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _FileDownloadService.schPages(vmd.schItem, 1, 15);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            vmd.DropDownLevel = _FileDownloadService.getLevelItem();
            vmd.DropDownClass = _FileDownloadService.getClassItem();
            return View(vmd);
        }
        #endregion

        #region 刪除

        [HttpPost]
        public IActionResult Delete(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _FileDownloadService.DelData(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion

        #region 刪除檔案

        [HttpPost]
        public IActionResult DeleteFile(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _FileDownloadService.DelFile(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion
    }
}
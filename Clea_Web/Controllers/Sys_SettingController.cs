using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.HPSF;
using System.IO;
using Microsoft.VisualBasic.ApplicationServices;

namespace Clea_Web.Controllers
{
    //後台講師專區-講師進修資料管理
    public class Sys_SettingController : BaseController
    {
        private readonly ILogger<Sys_SettingController> _logger;
        private AccountService _accountService;
        private AccountSettingService AccountSettingService;
        private FileService _fileService;

        public Sys_SettingController(ILogger<Sys_SettingController> logger, dbContext dbCLEA, AccountSettingService Service, FileService fileService)
        {
            _logger = logger;
            db = dbCLEA;
            AccountSettingService = Service;
            _fileService = fileService;
        }

        #region Index
        public IActionResult Index()
        {
            AccountSettingViewModel.Modify? vm = null;
            AccountSettingService.user = User;
            vm = AccountSettingService.GetEditData();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(AccountSettingViewModel.Modify vm)
        {
            AccountSettingService.user = User;
            //_fileService.user = User;    
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = AccountSettingService.SaveDataSign(vm);

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

            var claims = User.Identities.FirstOrDefault().Claims.ToArray();
            String Type = claims[4].Value.Equals("True") ? "B" : "P";


            if (error.CheckMsg)
            {
                if (Type == "B")
                {
                    return RedirectToAction("Index", "B_Home");
                }
                else
                {
                    return RedirectToAction("Index", "P_Home");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }


            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion

        #region Modify
        public IActionResult Modify()
        {
            AccountSettingViewModel.Modify? vm = null;
            AccountSettingService.user = User;

            vm = AccountSettingService.GetEditData();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modify(AccountSettingViewModel.Modify vm)
        {
            AccountSettingService.user = User;
            //_fileService.user = User;    
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = AccountSettingService.SaveData(vm);

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

            var claims = User.Identities.FirstOrDefault().Claims.ToArray();
            String Type = claims[4].Value.Equals("True") ? "B" : "P";


            if (error.CheckMsg)
            {
                if (Type == "B")
                {
                    return RedirectToAction("Index", "B_Home");
                }
                else
                {
                    return RedirectToAction("Index", "P_Home");
                }
            }
            else
            {
                return RedirectToAction("Modify");
            }


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
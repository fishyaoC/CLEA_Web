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

namespace Clea_Web.Controllers
{
    //後臺角色權限管理
    [Authorize]
    [UserPowerFilterAttribute]
    public class Sys_CodeController : BaseController
    {
        private readonly ILogger<Sys_CodeController> _logger;
        private CodeService _codeService;

        public Sys_CodeController(ILogger<Sys_CodeController> logger, dbContext dbCLEA, CodeService Service)
        {
            _logger = logger;
            db = dbCLEA;
            _codeService = Service;
        }


        #region 新增、編輯
        //public IActionResult Modify(String Type, String? R_ID)
        public IActionResult Modify(Guid Uid)
        {
            SysCodeViewModel.Modify? vm = new SysCodeViewModel.Modify();


            if (!string.IsNullOrEmpty(Uid.ToString()))
            {
                //編輯
                vm = _codeService.GetEditData(Uid);
                //vm.IsEdit = true;
                
            }
            else
            {
                //新增
                vm = new SysCodeViewModel.Modify();
            }


            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modify([FromForm] SysCodeViewModel.Modify vm)
        {
            _codeService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _codeService.SaveData(vm);

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
            SysCodeViewModel.SchModel vmd = new SysCodeViewModel.SchModel();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<SysCodeViewModel.SchItem>(value: data);
                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                vmd.schItem = new SysCodeViewModel.SchItem();
            }

            vmd.schPageList2 = _codeService.schPages(vmd.schItem, page.Value, 15);

            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(SysCodeViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _codeService.schPages(vmd.schItem, 1, 15);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            return View(vmd);
        }
        #endregion

        #region 刪除

        [HttpPost]
        public IActionResult Delete(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _codeService.DelData(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion

        #region 刪除

        [HttpPost]
        public IActionResult DeletePList(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _codeService.DelDataPList(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion
    }
}
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
    //廠商徵才管理
    [Authorize]
    [UserPowerFilterAttribute]
    public class B_CVController : BaseController
    {
        private readonly ILogger<B_CVController> _logger;
        private CVService _CVService;

        public B_CVController(ILogger<B_CVController> logger, dbContext dbCLEA, CVService Service)
        {
            _logger = logger;
            db = dbCLEA;
            _CVService = Service;
        }

        #region 新增、編輯
        public IActionResult Modify(Guid Uid)
        {
            CVViewModel.Modify? vm = new CVViewModel.Modify();

            if (Uid != null)
            {
                //編輯
                vm = _CVService.GetEditData(Uid);
            }
            else
            {
                //新增
                vm = new CVViewModel.Modify();
            }
            vm.DropDownApprove = _CVService.getApprovedItem();


            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modify(CVViewModel.Modify vm)
        {
            _CVService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _CVService.SaveData(vm);

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
            CVViewModel.SchModel vmd = new CVViewModel.SchModel();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<CVViewModel.SchItem>(value: data);

                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                vmd.schItem = new CVViewModel.SchItem();
            }

            vmd.schPageList2 = _CVService.schPages(vmd.schItem, page.Value, 15);
            vmd.DropDownApprove = _CVService.getApprovedItem();
            vmd.DropDownCompany = _CVService.getCompanyItem();
            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CVViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _CVService.schPages(vmd.schItem, 1, 15);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            vmd.DropDownApprove = _CVService.getApprovedItem();
            vmd.DropDownCompany = _CVService.getCompanyItem();
            return View(vmd);
        }
        #endregion

        #region 刪除

        [HttpPost]
        public IActionResult Delete(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _CVService.DelData(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion
    }
}
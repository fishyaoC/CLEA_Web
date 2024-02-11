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
    //相關連結管理
    [Authorize]
    [UserPowerFilterAttribute]
    public class B_LinkController : BaseController
    {
        private readonly ILogger<B_LinkController> _logger;
        private LinkService _LinkService;

        public B_LinkController(ILogger<B_LinkController> logger, dbContext dbCLEA, LinkService Service)
        {
            _logger = logger;
            db = dbCLEA;
            _LinkService = Service;
        }

        #region 新增、編輯
        public IActionResult Modify(Guid Uid)
        {
            LinkViewModel.Modify? vm = new LinkViewModel.Modify();


            if (Uid != null)
            {
                //編輯
                vm = _LinkService.GetEditData(Uid);
            }
            else
            {
                //新增
                vm = new LinkViewModel.Modify();
            }


            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modify(LinkViewModel.Modify vm)
        {
            _LinkService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _LinkService.SaveData(vm);

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
            LinkViewModel.SchModel vmd = new LinkViewModel.SchModel();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<LinkViewModel.SchItem>(value: data);
                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                vmd.schItem = new LinkViewModel.SchItem();
            }

            vmd.schPageList2 = _LinkService.schPages(vmd.schItem, page.Value, 15);
            vmd.DropDownItem = _LinkService.getTypeItem();
            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LinkViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _LinkService.schPages(vmd.schItem, 1, 15);
            vmd.DropDownItem = _LinkService.getTypeItem();
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            return View(vmd);
        }
        #endregion

        #region 刪除

        [HttpPost]
        public IActionResult Delete(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _LinkService.DelData(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion
    }
}
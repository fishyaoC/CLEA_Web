using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Clea_Web.Controllers
{
    //後臺帳號管理
    public class B_AccountController : BaseController
    {
        private readonly ILogger<B_AccountController> _logger;
        private AccountService _accountService;

        public B_AccountController(ILogger<B_AccountController> logger, dbContext dbCLEA, AccountService Service)
        {
            _logger = logger;
            db = dbCLEA;
            _accountService = Service;
        }


        #region 新增、編輯
        //public IActionResult Modify(String Type, String? R_ID)
        public IActionResult Modify(Guid U_ID)
        {
            AccountViewModel.Modify? vm = null;

            if (U_ID != null)
            {
                //編輯
                vm = _accountService.GetEditData(U_ID);
            }
            else
            {
                //新增
                vm = new AccountViewModel.Modify();
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modify(AccountViewModel.Modify vm)
        {
            _accountService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _accountService.SaveData(vm);

            return RedirectToAction("Index", new { msg = error });
        }
        #endregion

        #region 查詢
        public IActionResult Index(String? data, Int32? page)
        {
            AccountViewModel.SchModel vmd = new AccountViewModel.SchModel();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<AccountViewModel.SchItem>(value: data);
                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                vmd.schItem = new AccountViewModel.SchItem();
            }
            vmd.DropDownItem = _accountService.getSysRoleItem();
            vmd.schPageList2 = _accountService.schPages(vmd.schItem, page.Value, 15);

            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(AccountViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _accountService.schPages(vmd.schItem, 1, 15);
            vmd.DropDownItem = _accountService.getSysRoleItem();
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            return View(vmd);
        }
        #endregion

        #region 刪除

        [HttpPost]
        public IActionResult Delete(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _accountService.DelData(Uid);

            TempData["TempMsgType"] = "success";
            TempData["TempMsgTitle"] = "訊息";
            TempData["TempMsg"] = error.ErrorMsg;


            return Json(new { chk = true, msg = "" });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion


    }
}
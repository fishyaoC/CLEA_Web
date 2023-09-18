using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.ViewModels;

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
        public IActionResult Modify(Guid R_UID)
        {
            AccountViewModel.Modify? vm = null;

            if (R_UID != null)
            {
                //編輯
                vm = _accountService.GetEditData(R_UID);
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
        public IActionResult Index(BaseViewModel.errorMsg msg)
        {
            AccountViewModel.SchItem vm = new AccountViewModel.SchItem();
            AccountViewModel.SchModel vmd = new AccountViewModel.SchModel();

            //撈資料
            vmd.schPageList = _accountService.GetPageLists(vm);

            return View(vmd);
        }

        [HttpPost]
        public IActionResult Index(AccountViewModel.SchModel vmd)
        {
            //AccountViewModel.SchModel vmd = new AccountViewModel.SchModel();

            //撈資料
            vmd.schPageList = _accountService.GetPageLists(vmd.schItem);

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
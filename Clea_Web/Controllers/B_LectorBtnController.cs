using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.ViewModels;

namespace Clea_Web.Controllers
{
    //後台講師專區-公佈欄
    public class B_LectorBtnController : BaseController
    {
        private readonly ILogger<B_LectorBtnController> _logger;
        private AccountService _accountService;
        private RoleService _roleService;

        public B_LectorBtnController(ILogger<B_LectorBtnController> logger, dbContext dbCLEA, AccountService Service)
        {
            _logger = logger;
            db = dbCLEA;
            _accountService = Service;
        }


        #region 首頁
        public IActionResult Index()
        {
            String tmp = HttpContext.Session.GetString("role");
            //List<SysMenu> menuList = new List<SysMenu>();
            //menuList = db.SysMenus.ToList();
            //ViewBag.MenuList = menuList;
            return View();
        }
        #endregion
        #region 新增、編輯
        //public IActionResult Modify(String Type, String? R_ID)
        public IActionResult Modify(Guid NewsID)
        {
            UserRoleViewModel.Modify? vm = null;

            if (NewsID != null)
            {
                //編輯
                vm = _roleService.GetEditData(NewsID);
            }
            else
            {
                //新增
                vm = new UserRoleViewModel.Modify();
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modify(UserRoleViewModel.Modify vm)
        {
            _roleService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _roleService.SaveData(vm);

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


    }
}
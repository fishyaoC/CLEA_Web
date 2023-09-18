using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.VisualBasic;

namespace Clea_Web.Controllers
{
    //後臺角色權限管理
    public class Sys_RoleController : BaseController
    {
        private readonly ILogger<Sys_RoleController> _logger;
        private RoleService _roleService;

        public Sys_RoleController(ILogger<Sys_RoleController> logger, dbContext dbCLEA, RoleService Service)
        {
            _logger = logger;
            db = dbCLEA;
            _roleService = Service;
        }


        #region 新增、編輯
        //public IActionResult Modify(String Type, String? R_ID)
        public IActionResult Modify(Guid R_UID)
        {
            UserRoleViewModel.Modify? vm = null;

            if (R_UID != null)
            {
                //編輯
                vm = _roleService.GetEditData(R_UID);
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

        #region 查詢
        public IActionResult Index()
        {
            UserRoleViewModel.SchItem vm = new UserRoleViewModel.SchItem();
            UserRoleViewModel.SchModel vmd = new UserRoleViewModel.SchModel();

            //撈資料
            //vmd.schPageList = _roleService.GetPageLists(vm);
            vmd.schPageList2 = _roleService.schPages(vm,1,3);

            return View(vmd);
        }

        [HttpPost]
        public IActionResult Index(UserRoleViewModel.SchModel vmd,Int32 page,Int32? pagesize = null)
        {
            //UserRoleViewModel.SchModel vmd = new UserRoleViewModel.SchModel();
            pagesize = pagesize.HasValue ? pagesize : 3;
            //撈資料
            //vmd.schPageList = _roleService.GetPageLists(vmd.schItem);
            vmd.schPageList2 = _roleService.schPages(vmd.schItem, page, pagesize.Value);

            return View(vmd);
        }
        #endregion

        #region 刪除

        [HttpPost]
        public IActionResult Delete(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _roleService.DelData(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion
    }
}
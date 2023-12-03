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

            if (R_UID != null && R_UID.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                //編輯
                vm = _roleService.GetEditData(R_UID);
                //vm = _roleService.GetUserPower(R_UID);
                vm.IsEdit = true;

            }
            else
            {
                //新增
                //vm = new UserRoleViewModel.Modify();
                vm = _roleService.GetEditData(R_UID);
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

        #region 新增、編輯
        //public IActionResult Modify(String Type, String? R_ID)
        public IActionResult C_Index()
        {
            UserRoleViewModel.Modify? vm = null;


            //新增
            vm = new UserRoleViewModel.Modify();
            vm.IsEdit = false;
            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult C_Index(UserRoleViewModel.Modify vm)
        {
            if (vm.RBackEnd == true)
            {
                return RedirectToAction("C_Modify", new { RBackEnd = true });
            }
            else
            {
                return RedirectToAction("C_Modify", new { RBackEnd = false });
            }

            //_roleService.user = User;
            //BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            //error = _roleService.SaveData(vm);

            ////SWAL儲存成功
            //if (error.CheckMsg)
            //{
            //    TempData["TempMsgType"] = "success";
            //    TempData["TempMsgTitle"] = "儲存成功";
            //}
            //else
            //{
            //    TempData["TempMsgType"] = "error";
            //    TempData["TempMsgTitle"] = "儲存失敗";
            //    TempData["TempMsg"] = error.ErrorMsg;
            //}

            //return RedirectToAction("Index");

        }

        public IActionResult C_Modify(Boolean RBackEnd)
        {
            UserRoleViewModel.Modify? vm = null;


            //新增
            vm = new UserRoleViewModel.Modify();
            vm = _roleService.GetData(RBackEnd);
            vm.IsEdit = false;
            vm.RBackEnd = RBackEnd;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult C_Modify(UserRoleViewModel.Modify vm)
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
        public IActionResult Index(String? data, Int32? page)
        {
            UserRoleViewModel.SchModel vmd = new UserRoleViewModel.SchModel();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<UserRoleViewModel.SchItem>(value: data);
                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                vmd.schItem = new UserRoleViewModel.SchItem();
            }

            vmd.schPageList2 = _roleService.schPages(vmd.schItem, page.Value, 15);

            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(UserRoleViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _roleService.schPages(vmd.schItem, 1, 15);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
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
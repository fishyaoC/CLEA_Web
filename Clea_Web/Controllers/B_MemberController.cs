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
    //一般會員管理
    [Authorize]
    [UserPowerFilterAttribute]
    public class B_MemberController : BaseController
    {
        private readonly ILogger<B_MemberController> _logger;
        private MemberService _MemberService;

        public B_MemberController(ILogger<B_MemberController> logger, dbContext dbCLEA, MemberService Service)
        {
            _logger = logger;
            db = dbCLEA;
            _MemberService = Service;
        }

        #region 新增、編輯
        public IActionResult Modify(Guid Uid)
        {
            MemberViewModel.Modify? vm = new MemberViewModel.Modify();

            if (Uid != null)
            {
                //編輯
                vm = _MemberService.GetEditData(Uid);
            }
            else
            {
                //新增
                vm = new MemberViewModel.Modify();
            }
            vm.DropDownLevel = _MemberService.getLevelItem();


            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modify(MemberViewModel.Modify vm)
        {
            _MemberService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _MemberService.SaveData(vm);

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
            MemberViewModel.SchModel vmd = new MemberViewModel.SchModel();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<MemberViewModel.SchItem>(value: data);

                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                vmd.schItem = new MemberViewModel.SchItem();
            }

            vmd.schPageList2 = _MemberService.schPages(vmd.schItem, page.Value, 15);
            vmd.DropDownLevel = _MemberService.getLevelItem();
            vmd.DropDownMember = _MemberService.getMemberItem();
            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(MemberViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _MemberService.schPages(vmd.schItem, 1, 15);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            vmd.DropDownLevel = _MemberService.getLevelItem();
            vmd.DropDownMember = _MemberService.getMemberItem();
            return View(vmd);
        }
        #endregion

        #region 匯出EXCEL表
        public IActionResult ExportExcel()
        {
            Byte[] file = _MemberService.Export_Execl();
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "中華民國勞工教育協進會-網路會員資料.xlsx");
        }
        #endregion

        #region 刪除

        [HttpPost]
        public IActionResult Delete(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _MemberService.DelData(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion
    }
}
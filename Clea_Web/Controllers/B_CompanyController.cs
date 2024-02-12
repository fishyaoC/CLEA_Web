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
    //廠商會員管理
    [Authorize]
    [UserPowerFilterAttribute]
    public class B_CompanyController : BaseController
    {
        private readonly ILogger<B_CompanyController> _logger;
        private CompanyService _CompanyService;

        public B_CompanyController(ILogger<B_CompanyController> logger, dbContext dbCLEA, CompanyService Service)
        {
            _logger = logger;
            db = dbCLEA;
            _CompanyService = Service;
        }

        #region 新增、編輯
        public IActionResult Modify(Guid Uid)
        {
            CompanyViewModel.Modify? vm = new CompanyViewModel.Modify();

            if (Uid != null)
            {
                //編輯
                vm = _CompanyService.GetEditData(Uid);
            }
            else
            {
                //新增
                vm = new CompanyViewModel.Modify();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modify(CompanyViewModel.Modify vm)
        {
            _CompanyService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _CompanyService.SaveData(vm);

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
            CompanyViewModel.SchModel vmd = new CompanyViewModel.SchModel();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<CompanyViewModel.SchItem>(value: data);

                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                vmd.schItem = new CompanyViewModel.SchItem();
            }

            vmd.schPageList2 = _CompanyService.schPages(vmd.schItem, page.Value, 15);
            vmd.DropDownID = _CompanyService.getIDItem();
            vmd.DropDownCompany = _CompanyService.getCompanyItem();
            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CompanyViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _CompanyService.schPages(vmd.schItem, 1, 15);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            vmd.DropDownID = _CompanyService.getIDItem();
            vmd.DropDownCompany = _CompanyService.getCompanyItem();
            return View(vmd);
        }
        #endregion

        #region 匯出EXCEL表
        public IActionResult ExportExcel()
        {
            Byte[] file = _CompanyService.Export_Execl();
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "中華民國勞工教育協進會-網路廠商會員資料.xlsx");
        }
        #endregion

        #region 刪除

        [HttpPost]
        public IActionResult Delete(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _CompanyService.DelData(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion
    }
}
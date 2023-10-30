﻿using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NPOI.POIFS.Crypt.Dsig;

namespace Clea_Web.Controllers
{
    //後台講師專區-公佈欄
    public class P_LectorBtnController : BaseController
    {
        private readonly ILogger<P_LectorBtnController> _logger;
        private AccountService _accountService;
        private P_LectorBtnService _P_LectorBtnService;
        private FileService _fileService;

        public P_LectorBtnController(ILogger<P_LectorBtnController> logger, dbContext dbCLEA, P_LectorBtnService Service)
        {
            _logger = logger;
            db = dbCLEA;
            _P_LectorBtnService = Service;
        }


        #region 首頁
        public IActionResult Index(String? data, Int32? page)
        {
            P_LectorBtnViewModel.SchModel vmd = new P_LectorBtnViewModel.SchModel();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<P_LectorBtnViewModel.SchItem>(value: data);
                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                vmd.schItem = new P_LectorBtnViewModel.SchItem();
            }

            vmd.schPageList2 = _P_LectorBtnService.schPages(vmd.schItem, page.Value, 15);

            return View(vmd);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(P_LectorBtnViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _P_LectorBtnService.schPages(vmd.schItem, 1, 15);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            return View(vmd);
        }
        #endregion
        #region 新增、編輯
        //public IActionResult Modify(String Type, String? R_ID)
        public IActionResult Modify(string NewsID)
        {
            P_LectorBtnViewModel.Modify? vm = null;
            
            if (!string.IsNullOrEmpty(NewsID))
            {
                _P_LectorBtnService.user = User;
                //編輯
                vm = _P_LectorBtnService.GetEditData(NewsID);
                vm.IsEdit = true;
            }
            else
            {
                //新增
                vm = new P_LectorBtnViewModel.Modify();
            }

           
            vm.N_StartDate = DateTime.Now;
            vm.N_EndDate = DateTime.Now;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modify(P_LectorBtnViewModel.Modify vm)
        {
            _P_LectorBtnService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _P_LectorBtnService.SaveData(vm);

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
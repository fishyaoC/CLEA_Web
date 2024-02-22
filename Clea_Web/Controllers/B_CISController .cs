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
using Microsoft.VisualBasic.ApplicationServices;
using X.PagedList;

namespace Clea_Web.Controllers
{
    //即測即評管理
    //41 = 最新消息
    //42 = 核定項目 
    //43 = 報名地點= false
    //44 = 聯絡資訊
    //45 = 收費標準
    //46 = 辦理梯次
    //47 = 報檢資格
    //48 = 表單下載
    //49 = 友善連結
    //72 = 簡章上傳
    [Authorize]
    [UserPowerFilterAttribute]
    public class B_CISController : BaseController
    {
        private readonly ILogger<B_CISController> _logger;
        private TestInfoService _TestInfoService;
        private FileService _fileservice;
        private IConfiguration configuration;

        public B_CISController(ILogger<B_CISController> logger, dbContext dbCLEA, TestInfoService Service, FileService fileservice, IConfiguration configuration)
        {
            _logger = logger;
            db = dbCLEA;
            _TestInfoService = Service;
            _fileservice = fileservice;
            this.configuration = configuration;
        }


        #region 最新消息

        #region Index
        public IActionResult NewsIndex(String? data, Int32? page)
        {
            BtnViewModel.SchModel vmd = new BtnViewModel.SchModel();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<BtnViewModel.SchItem>(value: data);
                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                //vmd.schItem = null;
                vmd.schItem = new BtnViewModel.SchItem();
            }
            vmd.DropDownListType = getTypeItem();
            vmd.schPageList2 = _TestInfoService.schPagesNews(vmd.schItem, page.Value, 15,41);

            return View(vmd);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewsIndex(BtnViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _TestInfoService.schPagesNews(vmd.schItem, 1, 15,41);
            vmd.DropDownListType = getTypeItem();
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            return View(vmd);
        }
        #endregion
        #region 新增、編輯
        //public IActionResult Modify(String Type, String? R_ID)
        public IActionResult NewsModify(string NewsID)
        {
            BtnViewModel.Modify? vm = new BtnViewModel.Modify();


            if (!string.IsNullOrEmpty(NewsID))
            {
                //編輯
                vm = _TestInfoService.GetEditDataNews(NewsID);
                vm.IsEdit = true;
                vm.DropDownListLevel = getLevelItem();
                vm.DropDownListType = getTypeItem();

            }
            else
            {
                //新增
                vm = new BtnViewModel.Modify();
                vm.DropDownListLevel = getLevelItem();
                vm.DropDownListType = getTypeItem();
                vm.DropDownListType = getTypeItem();
                vm.NStartDate = DateTime.Now;
                vm.NEndDate = DateTime.Now;
            }
            vm.NType = 41;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewsModify(BtnViewModel.Modify vm)
        {
            _TestInfoService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _TestInfoService.SaveDataNews(vm);

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

            return RedirectToAction("NewsIndex");
        }
        #endregion
        #region 下拉選單
        public List<SelectListItem> getLevelItem()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
            List<SysCode> lst_cLectors = db.SysCodes.Where(x => x.CParentCode == "MemberLevel").ToList();
            if (lst_cLectors != null && lst_cLectors.Count() > 0)
            {
                foreach (SysCode L in lst_cLectors)
                {
                    result.Add(new SelectListItem() { Text = L.CItemName, Value = L.CItemCode.ToString() });
                }
            }
            return result;
        }
        public List<SelectListItem> getTypeItem()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
            List<SysCode> lst_cLectors = db.SysCodes.Where(x => x.CParentCode == "btn").ToList();
            if (lst_cLectors != null && lst_cLectors.Count() > 0)
            {
                foreach (SysCode L in lst_cLectors)
                {
                    result.Add(new SelectListItem() { Text = L.CItemName, Value = L.CItemCode.ToString() });
                }
            }
            return result;
        }
        #endregion
        #region DownloadFile
        public ActionResult DownloadFile(String FilePath, String FileName)
        {
            try
            {
                FileStream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                return File(stream, "application/octet-stream", FileName); //MME 格式 可上網查 此為通用設定
            }
            catch (System.Exception)
            {
                return Content("<script>alert('查無此檔案');window.close()</script>");
            }
        }
        #endregion

        #endregion

        #region 表單下載(含下載及範本)

        #region 查詢
        public IActionResult FormIndex(String? data, Int32? page)
        {
            FileDownloadViewModel.SchModel vmd = new FileDownloadViewModel.SchModel();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<FileDownloadViewModel.SchItem>(value: data);

                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                vmd.schItem = new FileDownloadViewModel.SchItem();
            }

            vmd.schPageList2 = _TestInfoService.schPagesFile(vmd.schItem, page.Value, 15,48);
            vmd.DropDownLevel = _TestInfoService.getLevelItem();
            vmd.DropDownClass = _TestInfoService.getClassItem();
            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FormIndex(FileDownloadViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _TestInfoService.schPagesFile(vmd.schItem, 1, 15,48);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            vmd.DropDownLevel = _TestInfoService.getLevelItem();
            vmd.DropDownClass = _TestInfoService.getClassItem();
            return View(vmd);
        }
        #endregion

        #region 新增、編輯
        public IActionResult FormModify(Guid Uid)
        {
            FileDownloadViewModel.Modify? vm = new FileDownloadViewModel.Modify();

            if (Uid != null)
            {
                //編輯
                vm = _TestInfoService.GetEditDataFile(Uid);
            }
            else
            {
                //新增
                vm = new FileDownloadViewModel.Modify();
            }
            vm.DropDownLevel = _TestInfoService.getLevelItem();
            vm.DropDownClass = _TestInfoService.getClassItem();
            vm.Type = 48;



            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FormModify(FileDownloadViewModel.Modify vm)
        {
            _TestInfoService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _TestInfoService.SaveDataFile(vm);

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

            return RedirectToAction("FormIndex");
        }
        #endregion

        #endregion

        #region 全國技能檢定_友善連結
        #region 查詢
        public IActionResult LinkIndex(String? data, Int32? page)
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

            vmd.schPageList2 = _TestInfoService.schPages(vmd.schItem, page.Value, 15,49);
            vmd.DropDownItem = _TestInfoService.getTypeItem();
            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LinkIndex(LinkViewModel.SchModel vmd)
        {
            //39模組代碼
            vmd.schPageList2 = _TestInfoService.schPages(vmd.schItem, 1, 15,49);
            vmd.DropDownItem = _TestInfoService.getTypeItem();
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            return View(vmd);
        }
        #endregion

        #region 新增、編輯
        public IActionResult LinkModify(Guid Uid)
        {
            LinkViewModel.Modify? vmLink = new LinkViewModel.Modify();


            if (Uid != null)
            {
                //編輯
                vmLink = _TestInfoService.GetEditData(Uid);
            }
            else
            {
                //新增
                vmLink = new LinkViewModel.Modify();
            }
            vmLink.LType = 49;


            return View(vmLink);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LinkModify(LinkViewModel.Modify vmLink)
        {
            _TestInfoService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _TestInfoService.SaveData(vmLink);

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

            return RedirectToAction("LinkIndex");
        }
        #endregion
        #endregion

        #region 簡章上傳
        public IActionResult DMIndex()
        {
            IntroViewModel.Rate? vm = new IntroViewModel.Rate();
            vm = _TestInfoService.GetEditDataDM(72);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DMIndex(IntroViewModel.Rate vm)
        {
            _TestInfoService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _TestInfoService.SaveDataDM(vm,72);

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

            return RedirectToAction("DMIndex");
        }
        #endregion

        #region 核定項目
        public IActionResult ApprovedIndex()
        {
            IntroViewModel.Rate? vm = new IntroViewModel.Rate();
            vm = _TestInfoService.GetEditDataIMG(42);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApprovedIndex(IntroViewModel.Rate vm)
        {
            _TestInfoService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _TestInfoService.SaveDataIMG(vm, 42);

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

            return RedirectToAction("ApprovedIndex");
        }
        #endregion

        #region 聯絡資訊
        public IActionResult ContactInfoIndex()
        {
            IntroViewModel.Rate? vm = new IntroViewModel.Rate();
            vm = _TestInfoService.GetEditDataIMG(44);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ContactInfoIndex(IntroViewModel.Rate vm)
        {
            _TestInfoService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _TestInfoService.SaveDataIMG(vm, 44);

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

            return RedirectToAction("ContactInfoIndex");
        }
        #endregion

        #region 收費標準
        public IActionResult RateIndex()
        {
            IntroViewModel.Rate? vm = new IntroViewModel.Rate();
            vm = _TestInfoService.GetEditDataIMG(45);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RateIndex(IntroViewModel.Rate vm)
        {
            _TestInfoService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _TestInfoService.SaveDataIMG(vm, 45);

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

            return RedirectToAction("RateIndex");
        }
        #endregion

        #region 辦理梯次
        public IActionResult LadderIndex()
        {
            IntroViewModel.Rate? vm = new IntroViewModel.Rate();
            vm = _TestInfoService.GetEditDataIMG(46);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LadderIndex(IntroViewModel.Rate vm)
        {
            _TestInfoService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _TestInfoService.SaveDataIMG(vm, 46);

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

            return RedirectToAction("LadderIndex");
        }
        #endregion

        #region 報檢資格
        public IActionResult QualifyIndex()
        {
            IntroViewModel.Rate? vm = new IntroViewModel.Rate();
            vm = _TestInfoService.GetEditDataIMG(47);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult QualifyIndex(IntroViewModel.Rate vm)
        {
            _TestInfoService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _TestInfoService.SaveDataIMG(vm, 47);

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

            return RedirectToAction("QualifyIndex");
        }
        #endregion

    }
}
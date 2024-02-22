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
    //51 = 最新消息
    //52 = 辦理職類 
    //53 = 報名費用
    //54 = 報名地點= false
    //55 = 測驗地點= false
    //56 = 友善連結

    [Authorize]
    [UserPowerFilterAttribute]
    public class B_ETestController : BaseController
    {
        private readonly ILogger<B_ETestController> _logger;
        private TestInfoService _TestInfoService;
        private FileService _fileservice;
        private IConfiguration configuration;

        public B_ETestController(ILogger<B_ETestController> logger, dbContext dbCLEA, TestInfoService Service, FileService fileservice, IConfiguration configuration)
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
            vmd.schPageList2 = _TestInfoService.schPagesNews(vmd.schItem, page.Value, 15,51);

            return View(vmd);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewsIndex(BtnViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _TestInfoService.schPagesNews(vmd.schItem, 1, 15,51);
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
            vm.NType = 51;

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

        #region 友善連結
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
            vmLink.LType = 56;


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


        #region 辦理職類
        public IActionResult TypeIndex()
        {
            IntroViewModel.Rate? vm = new IntroViewModel.Rate();
            vm = _TestInfoService.GetEditDataIMG(52);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TypeIndex(IntroViewModel.Rate vm)
        {
            _TestInfoService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _TestInfoService.SaveDataIMG(vm, 52);

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

            return RedirectToAction("TypeIndex");
        }
        #endregion

        #region 報名費用
        public IActionResult RateIndex()
        {
            IntroViewModel.Rate? vm = new IntroViewModel.Rate();
            vm = _TestInfoService.GetEditDataIMG(53);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RateIndex(IntroViewModel.Rate vm)
        {
            _TestInfoService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _TestInfoService.SaveDataIMG(vm, 53);

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

    }
}
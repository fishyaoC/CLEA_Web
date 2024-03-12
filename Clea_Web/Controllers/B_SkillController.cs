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
    //全國技能檢定管理
    [Authorize]
    [UserPowerFilterAttribute]
    public class B_SkillController : BaseController
    {
        private readonly ILogger<B_SkillController> _logger;
        private TestInfoService _TestInfoService;
        private FileService _fileservice;
        private IConfiguration configuration;

        public B_SkillController(ILogger<B_SkillController> logger, dbContext dbCLEA, TestInfoService Service, FileService fileservice, IConfiguration configuration)
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
            vmd.schPageList2 = _TestInfoService.schPagesNews(vmd.schItem, page.Value, 15,33);

            return View(vmd);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewsIndex(BtnViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _TestInfoService.schPagesNews(vmd.schItem, 1, 15,33);
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
            vm.NType = 33;

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

        #region 報檢資格

        #region 查詢
        public IActionResult QualifyIndex(String? data, Int32? page)
        {
            SysCodeViewModel.SchModel vmd = new SysCodeViewModel.SchModel();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<SysCodeViewModel.SchItem>(value: data);
                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                vmd.schItem = new SysCodeViewModel.SchItem();
            }

            vmd.schPageList2 = _TestInfoService.schPages(vmd.schItem, page.Value, 15);

            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(SysCodeViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _TestInfoService.schPages(vmd.schItem, 1, 15);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            return View(vmd);
        }
        #endregion

        #region 新增、編輯
        public IActionResult QualifyModify(Guid Uid)
        {
            TestInfoViewModel.PListModify? vm = new TestInfoViewModel.PListModify();


            if (!string.IsNullOrEmpty(Uid.ToString()))
            {
                //編輯
                vm = _TestInfoService.GetEditDataList(Uid);
                //vm.IsEdit = true;

            }
            else
            {
                //新增
                vm = new TestInfoViewModel.PListModify();
            }


            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult QualifyModify([FromForm] TestInfoViewModel.PListModify vm)
        {
            _TestInfoService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _TestInfoService.SaveDataList(vm);

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

        #region 刪除

        [HttpPost]
        public IActionResult Delete(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _TestInfoService.DelData(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
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

            vmd.schPageList2 = _TestInfoService.schPagesFile(vmd.schItem, page.Value, 15,38);
            vmd.DropDownLevel = _TestInfoService.getLevelItem();
            vmd.DropDownClass = _TestInfoService.getClassItem();
            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FormIndex(FileDownloadViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _TestInfoService.schPagesFile(vmd.schItem, 1, 15,38);
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
            vm.Type = 38;



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

            vmd.schPageList2 = _TestInfoService.schPages(vmd.schItem, page.Value, 15,39);
            vmd.DropDownItem = _TestInfoService.getTypeItem();
            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LinkIndex(LinkViewModel.SchModel vmd)
        {
            //39模組代碼
            vmd.schPageList2 = _TestInfoService.schPages(vmd.schItem, 1, 15,39);
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
            vmLink.LType = 39;


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
            vm = _TestInfoService.GetEditDataDM(71);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DMIndex(IntroViewModel.Rate vm)
        {
            _TestInfoService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _TestInfoService.SaveDataDM(vm,71);

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

    }
}
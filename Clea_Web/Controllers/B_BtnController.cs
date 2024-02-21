using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NPOI.POIFS.Crypt.Dsig;
using Clea_Web.Filters;
using Microsoft.AspNetCore.Authorization;

namespace Clea_Web.Controllers
{
    //後台講師專區-公佈欄
    [Authorize]
    [UserPowerFilterAttribute]
    public class B_BtnController : BaseController
    {
        private readonly ILogger<B_BtnController> _logger;
        private BtnService _BtnService;
        private FileService _fileService;
        private IConfiguration configuration;

        public B_BtnController(ILogger<B_BtnController> logger, dbContext dbCLEA, BtnService Service, FileService fileService, IConfiguration configuration)
        {
            _logger = logger;
            db = dbCLEA;
            _BtnService = Service;
            _fileService = fileService;
            this.configuration = configuration;
        }


        #region 首頁
        public IActionResult Index(String? data, Int32? page)
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
            vmd.schPageList2 = _BtnService.schPages(vmd.schItem, page.Value, 15);

            return View(vmd);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(BtnViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _BtnService.schPages(vmd.schItem, 1, 15);
            vmd.DropDownListType = getTypeItem();
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            return View(vmd);
        }
        #endregion
        #region 新增、編輯
        //public IActionResult Modify(String Type, String? R_ID)
        public IActionResult Modify(string NewsID)
        {
            BtnViewModel.Modify? vm = new BtnViewModel.Modify();


            if (!string.IsNullOrEmpty(NewsID))
            {
                //編輯
                vm = _BtnService.GetEditData(NewsID);
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


            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modify(BtnViewModel.Modify vm)
        {
            _BtnService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _BtnService.SaveData(vm);

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
        #region 刪除

        [HttpPost]
        public IActionResult Delete(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _BtnService.DelData(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion
        #region 刪除檔案

        [HttpPost]
        public IActionResult DeleteFile(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _BtnService.DelFile(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
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

    }
}
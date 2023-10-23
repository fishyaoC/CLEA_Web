using Clea_Web.Models;
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
    public class B_LectorBtnController : BaseController
    {
        private readonly ILogger<B_LectorBtnController> _logger;
        private AccountService _accountService;
        private B_LectorBtnService _B_LectorBtnService;
        private FileService _fileService;

        public B_LectorBtnController(ILogger<B_LectorBtnController> logger, dbContext dbCLEA, B_LectorBtnService Service)
        {
            _logger = logger;
            db = dbCLEA;
            _B_LectorBtnService = Service;
        }


        #region 查詢
        public IActionResult Index(String? data, Int32? page)
        {
            B_LectorBtnViewModel.SchModel vmd = new B_LectorBtnViewModel.SchModel();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<B_LectorBtnViewModel.SchItem>(value: data);
                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                vmd.schItem = new B_LectorBtnViewModel.SchItem();
            }

            vmd.schPageList2 = _B_LectorBtnService.schPages(vmd.schItem, page.Value, 15);

            return View(vmd);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(B_LectorBtnViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _B_LectorBtnService.schPages(vmd.schItem, 1, 15);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            return View(vmd);
        }
        #endregion
        #region 新增、編輯
        //public IActionResult Modify(String Type, String? R_ID)
        public IActionResult Modify(string NewsID)
        {
            B_LectorBtnViewModel.Modify? vm = null;
            
            if (!string.IsNullOrEmpty(NewsID))
            {
                //編輯
                vm = _B_LectorBtnService.GetEditData(NewsID);
                vm.IsEdit = true;
            }
            else
            {
                //新增
                vm = new B_LectorBtnViewModel.Modify();
            }

            vm.DropDownList = getTeacherItem();
            vm.DropDownListUser = getsysuserItem();
            vm.DropDownListType = getTypeItem();
            vm.N_StartDate = DateTime.Now;
            vm.N_EndDate = DateTime.Now;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modify(B_LectorBtnViewModel.Modify vm)
        {
            _B_LectorBtnService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _B_LectorBtnService.SaveData(vm);

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
        #region 教師下拉選單
        public List<SelectListItem> getTeacherItem()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
            List<SysCode> lst_cLectors = db.SysCodes.Where(x=>x.CParentCode== "L_Tpye").ToList();
            if (lst_cLectors != null && lst_cLectors.Count() > 0)
            {
                foreach (SysCode L in lst_cLectors)
                {
                    result.Add(new SelectListItem() { Text = L.CItemName, Value = L.Uid.ToString() });
                }
            }
            return result;
        }
        public List<SelectListItem> getsysuserItem()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
            List<SysUser> lst_sysuser = db.SysUsers.ToList();
            if (lst_sysuser != null && lst_sysuser.Count() > 0)
            {
                foreach (SysUser L in lst_sysuser)
                {
                    result.Add(new SelectListItem() { Text = L.UName, Value = L.UId.ToString() });
                }
            }
            return result;
        }
        public List<SelectListItem> getTypeItem()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
            List<SysCode> lst_cLectors = db.SysCodes.Where(x => x.CParentCode == "btnType").ToList();
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
        //#region 上傳檔案

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Modify([FromForm] B_LectorBtnViewModel.Modify data)
        //{
        //    BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
        //    _B_LectorBtnService.user = User;
        //    _fileService.user = User;

        //    String chkExt = Path.GetExtension(data.file.FileName);

        //    if (!string.IsNullOrEmpty(chkExt))
        //    {
        //        result.CheckMsg = Convert.ToBoolean(_fileService.UploadFile(1, data.R_ID, data.file, true));
        //    }
        //    else
        //    {
        //        result.CheckMsg = false;
        //        result.ErrorMsg = "請選擇檔案!";
        //    }

        //    if (result.CheckMsg)
        //    {
        //        TempData["TempMsgType"] = "success";
        //        TempData["TempMsgTitle"] = "檔案上傳成功";
        //    }
        //    else
        //    {
        //        TempData["TempMsgType"] = "error";
        //        TempData["TempMsgTitle"] = "檔案上傳失敗";
        //        TempData["TempMsg"] = result.ErrorMsg;
        //    }

        //    if (!string.IsNullOrEmpty(result.ErrorMsg))
        //    {
        //        return RedirectToAction("Modify", new { News_ID = data.News_ID });
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index");
        //    }
        //}
        //#endregion
        //#region 刪除檔案
        //public IActionResult Delete(Guid File_ID)
        //{
        //    BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
        //    Guid B_UID = Guid.Empty;
        //    SysFile? sysFile = db.SysFiles.Find(File_ID) ?? null;

        //    if (sysFile != null)
        //    {
        //        B_UID = sysFile.FMatchKey;
        //        db.SysFiles.Remove(sysFile);
        //        result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
        //    }
        //    else
        //    {
        //        result.CheckMsg = false;
        //        result.ErrorMsg = "查無此筆資料!";
        //    }

        //    if (result.CheckMsg)
        //    {
        //        TempData["TempMsgType"] = "success";
        //        TempData["TempMsgTitle"] = "刪除成功";
        //    }
        //    else
        //    {
        //        TempData["TempMsgType"] = "error";
        //        TempData["TempMsgTitle"] = "刪除失敗";
        //        TempData["TempMsg"] = result.ErrorMsg;
        //    }

        //    return RedirectToAction("Modify", new { B_UID = B_UID });
        //}
        //#endregion
        #region 刪除

        [HttpPost]
        public IActionResult Delete(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _B_LectorBtnService.DelData(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion
    }
}
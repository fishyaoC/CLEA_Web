using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clea_Web.Controllers
{
    //後台講師專區-公佈欄
    public class B_LectorBtnController : BaseController
    {
        private readonly ILogger<B_LectorBtnController> _logger;
        private AccountService _accountService;
        private B_LectorBtnService _B_LectorBtnService;
        private FileService _fileService;

        public B_LectorBtnController(ILogger<B_LectorBtnController> logger, dbContext dbCLEA, AccountService Service)
        {
            _logger = logger;
            db = dbCLEA;
            _accountService = Service;
        }


        #region 首頁
        public IActionResult Index()
        {
            String tmp = HttpContext.Session.GetString("role");
            //List<SysMenu> menuList = new List<SysMenu>();
            //menuList = db.SysMenus.ToList();
            //ViewBag.MenuList = menuList;
            return View();
        }
        #endregion
        #region 新增、編輯
        //public IActionResult Modify(String Type, String? R_ID)
        public IActionResult Modify(Guid NewsID)
        {
            B_LectorBtnViewModel.Modify? vm = null;

            if (string.IsNullOrEmpty(NewsID.ToString()))
            {
                //編輯
                vm = _B_LectorBtnService.GetEditData(NewsID);
            }
            else
            {
                //新增
                vm = new B_LectorBtnViewModel.Modify();
            }
            vm.DropDownList = getTeacherItem();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modify(UserRoleViewModel.Modify vm)
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
            List<CLector> lst_cLectors = db.CLectors.ToList();
            if (lst_cLectors != null && lst_cLectors.Count() > 0)
            {
                foreach (CLector L in lst_cLectors)
                {
                    result.Add(new SelectListItem() { Text = L.LName, Value = L.LUid.ToString() });
                }
            }
            return result;
        }
        #endregion
        #region 上傳檔案

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modify([FromForm] B_LectorBtnViewModel.Modify data)
        {
            BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
            _B_LectorBtnService.user = User;
            _fileService.user = User;

            String chkExt = Path.GetExtension(data.file.FileName);

            if (!string.IsNullOrEmpty(chkExt))
            {
                result.CheckMsg = Convert.ToBoolean(_fileService.UploadFile(1, data.R_ID, data.file, true));
            }
            else
            {
                result.CheckMsg = false;
                result.ErrorMsg = "請選擇檔案!";
            }

            if (result.CheckMsg)
            {
                TempData["TempMsgType"] = "success";
                TempData["TempMsgTitle"] = "檔案上傳成功";
            }
            else
            {
                TempData["TempMsgType"] = "error";
                TempData["TempMsgTitle"] = "檔案上傳失敗";
                TempData["TempMsg"] = result.ErrorMsg;
            }

            if (!string.IsNullOrEmpty(result.ErrorMsg))
            {
                return RedirectToAction("Modify", new { News_ID = data.News_ID });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        #endregion
        #region 刪除檔案
        public IActionResult Delete(Guid File_ID)
        {
            BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
            Guid B_UID = Guid.Empty;
            SysFile? sysFile = db.SysFiles.Find(File_ID) ?? null;

            if (sysFile != null)
            {
                B_UID = sysFile.FMatchKey;
                db.SysFiles.Remove(sysFile);
                result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
            }
            else
            {
                result.CheckMsg = false;
                result.ErrorMsg = "查無此筆資料!";
            }

            if (result.CheckMsg)
            {
                TempData["TempMsgType"] = "success";
                TempData["TempMsgTitle"] = "刪除成功";
            }
            else
            {
                TempData["TempMsgType"] = "error";
                TempData["TempMsgTitle"] = "刪除失敗";
                TempData["TempMsg"] = result.ErrorMsg;
            }

            return RedirectToAction("Modify", new { B_UID = B_UID });
        }
        #endregion
    }
}
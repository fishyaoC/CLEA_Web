using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.HPSF;
using System.IO;

namespace Clea_Web.Controllers
{
    //後台講師專區-講師進修資料管理
    public class B_LectorAdvController : BaseController
    {
        private readonly ILogger<B_LectorAdvController> _logger;
        private AccountService _accountService;
        private B_LectorAdvService _B_LectorAdvService;
        private FileService _fileService;

        public B_LectorAdvController(ILogger<B_LectorAdvController> logger, dbContext dbCLEA, B_LectorAdvService Service)
        {
            _logger = logger;
            db = dbCLEA;
            _B_LectorAdvService = Service;
        }


        #region Index
        public IActionResult Index(String? data, Int32? page)
        {
            B_LectorAdvViewModel.SchModel vmd = new B_LectorAdvViewModel.SchModel();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<B_LectorAdvViewModel.SchItem>(value: data);
                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                vmd.schItem = new B_LectorAdvViewModel.SchItem();
            }

            vmd.schPageList2 = _B_LectorAdvService.schPages(vmd.schItem, page.Value, 15);

            return View(vmd);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(B_LectorAdvViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _B_LectorAdvService.schPages(vmd.schItem, 1, 15);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            return View(vmd);
        }
        #endregion

        #region D_Index
        public IActionResult D_Index(String LUid, int YearNow, String? data, Int32? page)
        {
            B_LectorAdvViewModel.D_Model vmd = new B_LectorAdvViewModel.D_Model();
            page = page ?? 1;


            vmd.D_PageList = _B_LectorAdvService.D_schPages(LUid, YearNow, page.Value, 15);
            vmd.YearNow = YearNow;

            CLector cl = db.CLectors.Where(x => x.LUid.ToString() == LUid).FirstOrDefault();
            vmd.LName = cl.LName;

            return View(vmd);
        }
        #endregion

        #region Modify
        public IActionResult Modify(string LaUid)
        {
            B_LectorAdvViewModel.Modify? vm = null;
            _B_LectorAdvService.user = User;

            if (!string.IsNullOrEmpty(LaUid))
            {
                //編輯
                vm = _B_LectorAdvService.GetEditData(LaUid);
                vm.IsEdit = true;

            }
            else
            {
                //新增
                vm = new B_LectorAdvViewModel.Modify();
                vm.LaYear = DateTime.Now.Year - 1911;
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modify(B_LectorAdvViewModel.Modify vm)
        {
            _B_LectorAdvService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _B_LectorAdvService.SaveData(vm);

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


            if (error.CheckMsg)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Modify");

            }

            return RedirectToAction("Index");


            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion

        #region V_Modify
        public IActionResult V_Modify(string LaUid)
        {
            B_LectorAdvViewModel.Modify? vm = null;

            if (!string.IsNullOrEmpty(LaUid))
            {
                vm = _B_LectorAdvService.GetEditData(LaUid);
            }
            return View(vm);
        }
        #endregion

        #region C_Modify
        public IActionResult C_Modify(String LaUid)
        {
            B_LectorAdvViewModel.Modify? vm = null;
            _B_LectorAdvService.user = User;

            //if (!string.IsNullOrEmpty(LaUid))
            //{
            //    //編輯
            //    vm = _B_LectorAdvService.GetEditData(LaUid);
            //}
            //else
            //{
            //新增
            vm = new B_LectorAdvViewModel.Modify();
            vm.DropDownList = getsysuserItem();
            vm.YearList = getYearItem();
            //vm.LaYear = DateTime.Now.Year - 1911;
            //    vm.LName = LName;
            //    vm.LaYear = LaYear;
            //}
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult C_Modify(B_LectorAdvViewModel.Modify vm)
        {
            _B_LectorAdvService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _B_LectorAdvService.SaveData(vm);

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

        #region 匯出EXCEL表
        public IActionResult ExportExcel(String LUid, int YearNow)
        {

            SysUser? su = db.SysUsers.Where(x => x.UId == Guid.Parse(LUid)).FirstOrDefault() ?? null;
            Byte[] file = _B_LectorAdvService.Export_LectorAnnaulZip(LUid, YearNow);
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", YearNow.ToString() + "年-" + su.UName + "-進修資料.zip");
        }
        #endregion

        #region 教師下拉選單
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
        #endregion

        #region 年度下拉選單
        public List<SelectListItem> getYearItem()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });

            int YearNow = DateTime.Now.Year - 1911;

            for (int i = 0; YearNow > 107; i++)
            {
                result.Add(new SelectListItem() { Text = YearNow.ToString(), Value = YearNow.ToString() });

                YearNow--;
            }

            return result;
        }
        #endregion
    }
}
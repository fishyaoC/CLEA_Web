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

            if (!string.IsNullOrEmpty(LaUid))
            {
                vm = _B_LectorAdvService.GetEditData(LaUid);
            }
            return View(vm);
        }
        #endregion

        #region DownloadFile
        public ActionResult DownloadFile(String FilePath,String FileName) {
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
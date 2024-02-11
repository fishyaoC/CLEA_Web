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
    //輪播圖管理
    [Authorize]
    [UserPowerFilterAttribute]
    public class B_BannerController : BaseController
    {
        private readonly ILogger<B_BannerController> _logger;
        private BannerService _BannerService;
        private FileService _fileservice;
        private IConfiguration configuration;

        public B_BannerController(ILogger<B_BannerController> logger, dbContext dbCLEA, BannerService Service, FileService fileservice, IConfiguration configuration)
        {
            _logger = logger;
            db = dbCLEA;
            _BannerService = Service;
            _fileservice = fileservice;
            this.configuration = configuration;
        }

        #region 新增、編輯
        public IActionResult Modify(Guid Uid)
        {
            BannerViewModel.Modify? vm = new BannerViewModel.Modify();

            if (Uid != null)
            {
                //編輯
                vm = _BannerService.GetEditData(Uid);
            }
            else
            {
                //新增
                vm = new BannerViewModel.Modify();
            }


            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modify(BannerViewModel.Modify vm)
        {
            _BannerService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _BannerService.SaveData(vm);

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
            BannerViewModel.SchModel vmd = new BannerViewModel.SchModel();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<BannerViewModel.SchItem>(value: data);

                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                vmd.schItem = new BannerViewModel.SchItem();
            }

            vmd.schPageList2 = _BannerService.schPages(vmd.schItem, page.Value, 15);
            foreach (var item in vmd.schPageList2)
            {
                SysFile sf = db.SysFiles.Where(x=>x.FMatchKey.Equals(Guid.Parse(item.Uid))).FirstOrDefault();
                if (sf != null)
                {
                    string fileNameDL = sf.FNameDl + "." + sf.FExt;
                    string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                    item.BannerIMG = Convert.ToBase64String(imageBytes);
                }
            }

            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(BannerViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _BannerService.schPages(vmd.schItem, 1, 15);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            foreach (var item in vmd.schPageList2)
            {
                SysFile sf = db.SysFiles.Where(x => x.FMatchKey.Equals(Guid.Parse(item.Uid))).FirstOrDefault();
                if (sf != null)
                {
                    string fileNameDL = sf.FNameDl + "." + sf.FExt;
                    string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                    item.BannerIMG = Convert.ToBase64String(imageBytes);
                }
            }
            return View(vmd);
        }
        #endregion

        #region 刪除

        [HttpPost]
        public IActionResult Delete(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _BannerService.DelData(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion
    }
}
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
    //收退費標準管理
    [Authorize]
    [UserPowerFilterAttribute]
    public class B_IntroController : BaseController
    {
        private readonly ILogger<B_IntroController> _logger;
        private IntroService _IntroService;
        private FileService _fileservice;
        private IConfiguration configuration;

        public B_IntroController(ILogger<B_IntroController> logger, dbContext dbCLEA, IntroService Service, FileService fileservice, IConfiguration configuration)
        {
            _logger = logger;
            db = dbCLEA;
            _IntroService = Service;
            _fileservice = fileservice;
            this.configuration = configuration;
        }

        #region 收退費標準編輯Rate
        public IActionResult Rate()
        {
            IntroViewModel.Rate? vm = new IntroViewModel.Rate();


            vm = _IntroService.GetEditData();



            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Rate(IntroViewModel.Rate vm)
        {
            _IntroService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _IntroService.SaveData(vm);

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

            return RedirectToAction("Rate");
        }
        #endregion

        #region 合格場地GreatPlace

        #region 查詢
        public IActionResult GreatPlace(String? data, Int32? page)
        {
            IntroViewModel.SchModel vmd = new IntroViewModel.SchModel();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<IntroViewModel.SchItem>(value: data);

                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                vmd.schItem = new IntroViewModel.SchItem();
            }

            vmd.schPageList2 = _IntroService.schPages(vmd.schItem, page.Value, 15);
            foreach (var item in vmd.schPageList2)
            {
                SysFile sf = db.SysFiles.Where(x => x.FMatchKey.Equals(Guid.Parse(item.Uid))).FirstOrDefault();
                if (sf != null)
                {
                    string fileNameDL = sf.FNameDl + "." + sf.FExt;
                    string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                    item.IMG = Convert.ToBase64String(imageBytes);
                }
            }

            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GreatPlace(IntroViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _IntroService.schPages(vmd.schItem, 1, 15);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            foreach (var item in vmd.schPageList2)
            {
                SysFile sf = db.SysFiles.Where(x => x.FMatchKey.Equals(Guid.Parse(item.Uid))).FirstOrDefault();
                if (sf != null)
                {
                    string fileNameDL = sf.FNameDl + "." + sf.FExt;
                    string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                    item.IMG = Convert.ToBase64String(imageBytes);
                }
            }
            return View(vmd);
        }
        #endregion

        #region 新增/編輯
        public IActionResult GreatPlaceModify(Guid Uid)
        {
            IntroViewModel.Rate? vm = new IntroViewModel.Rate();

            if (Uid != null)
            {
                //編輯
                vm = _IntroService.GetEditDataGP(Uid);
            }
            else
            {
                //新增
                vm = new IntroViewModel.Rate();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GreatPlaceModify(IntroViewModel.Rate vm)
        {
            _IntroService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _IntroService.SaveDataGP(vm);

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

            return RedirectToAction("GreatPlace");
        }
        #endregion

        #region 刪除

        [HttpPost]
        public IActionResult DeleteGreatPlace(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _IntroService.DelData(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion

        #endregion

        #region 本會位置 Nav
        #region 查詢
        public IActionResult NavIndex(String? data, Int32? page)
        {
            IntroViewModel.SchModel vmd = new IntroViewModel.SchModel();
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<IntroViewModel.SchItem>(value: data);

                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                vmd.schItem = new IntroViewModel.SchItem();
            }

            vmd.schPageList2 = _IntroService.schPagesNav(vmd.schItem, page.Value, 15);
            //foreach (var item in vmd.schPageList2)
            //{
            //    SysFile sf = db.SysFiles.Where(x => x.FMatchKey.Equals(Guid.Parse(item.Uid))).FirstOrDefault();
            //    if (sf != null)
            //    {
            //        string fileNameDL = sf.FNameDl + "." + sf.FExt;
            //        string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
            //        byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
            //        item.IMG = Convert.ToBase64String(imageBytes);
            //    }
            //}

            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NavIndex(IntroViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _IntroService.schPagesNav(vmd.schItem, 1, 15);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            //foreach (var item in vmd.schPageList2)
            //{
            //    SysFile sf = db.SysFiles.Where(x => x.FMatchKey.Equals(Guid.Parse(item.Uid))).FirstOrDefault();
            //    if (sf != null)
            //    {
            //        string fileNameDL = sf.FNameDl + "." + sf.FExt;
            //        string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
            //        byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
            //        item.IMG = Convert.ToBase64String(imageBytes);
            //    }
            //}
            return View(vmd);
        }
        #endregion

        #region 新增/編輯
        public IActionResult NavModify(Guid Uid)
        {
            IntroViewModel.Nav? vm = new IntroViewModel.Nav();

            if (Uid != null)
            {
                //編輯
                vm = _IntroService.GetEditDataNav(Uid);
            }
            else
            {
                //新增
                vm = new IntroViewModel.Nav();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NavModify(IntroViewModel.Nav vm)
        {
            _IntroService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _IntroService.SaveDataNav(vm);

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

            return RedirectToAction("Nav");
        }


        #region 刪除

        [HttpPost]
        public IActionResult DeleteNav(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _IntroService.DelDataNav(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion
        #endregion

        #region 刪除

        [HttpPost]
        public IActionResult Delete(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _IntroService.DelData(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion
        #endregion
    }
}
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
        private TestInfoService _TestInfoService;
        private FileService _fileservice;
        private IConfiguration configuration;

        public B_IntroController(ILogger<B_IntroController> logger, dbContext dbCLEA, IntroService Service, TestInfoService ServiceT, FileService fileservice, IConfiguration configuration)
        {
            _logger = logger;
            db = dbCLEA;
            _IntroService = Service;
            _TestInfoService = ServiceT;
            _fileservice = fileservice;
            this.configuration = configuration;
        }

        #region 收費標準編輯Fare
        public IActionResult FareIndex()
        {
            IntroViewModel.Rate? vm = new IntroViewModel.Rate();


            vm = _IntroService.GetEditDataFare();



            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FareIndex(IntroViewModel.Rate vm)
        {
            _IntroService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _IntroService.SaveFareData(vm);

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

            return RedirectToAction("FareIndex");
        }
        #endregion

        #region 退費標準編輯Rate
        public IActionResult RateIndex()
        {
            IntroViewModel.Rate? vm = new IntroViewModel.Rate();


            vm = _IntroService.GetEditData();



            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RateIndex(IntroViewModel.Rate vm)
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

            return RedirectToAction("RateIndex");
        }
        #endregion

        #region 合格場地GreatPlace

        #region 查詢
        public IActionResult GreatPlaceIndex(String? data, Int32? page)
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
        public IActionResult GreatPlaceIndex(IntroViewModel.SchModel vmd)
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

            return RedirectToAction("GreatPlaceIndex");
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

        #region 本會願景與使命Idea

        #region 查詢
        public IActionResult IdeaIndex(String? data, Int32? page)
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
            //type=61
            vmd.schPageList2 = _TestInfoService.schPages(vmd.schItem, page.Value, 15,61);

            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IdeaIndex(SysCodeViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _TestInfoService.schPages(vmd.schItem, 1, 15,61);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            return View(vmd);
        }
        #endregion

        #region 新增、編輯
        public IActionResult IdeaModify(Guid Uid)
        {
            TestInfoViewModel.PListModify? vm = new TestInfoViewModel.PListModify();


            if (!string.IsNullOrEmpty(Uid.ToString()))
            {
                //編輯
                vm = _TestInfoService.GetEditDataList(Uid,61);
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
        public IActionResult IdeaModify([FromForm] TestInfoViewModel.PListModify vm)
        {
            _TestInfoService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _TestInfoService.SaveDataList(vm,61);

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

            return RedirectToAction("IdeaIndex");
        }
        #endregion

        #region 刪除

        [HttpPost]
        public IActionResult DeletePList(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _TestInfoService.DelDataPList(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion

        #endregion

        #region 本會訓練目標與發展政策Goal

        #region 查詢
        public IActionResult GoalIndex(String? data, Int32? page)
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
            //type=61
            vmd.schPageList2 = _TestInfoService.schPages(vmd.schItem, page.Value, 15, 62);

            return View(vmd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GoalIndex(SysCodeViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _TestInfoService.schPages(vmd.schItem, 1, 15, 62);
            ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            return View(vmd);
        }
        #endregion

        #region 新增、編輯
        public IActionResult GoalModify(Guid Uid)
        {
            TestInfoViewModel.PListModify? vm = new TestInfoViewModel.PListModify();


            if (!string.IsNullOrEmpty(Uid.ToString()))
            {
                //編輯
                vm = _TestInfoService.GetEditDataList(Uid, 62);
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
        public IActionResult GoalModify([FromForm] TestInfoViewModel.PListModify vm)
        {
            _TestInfoService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _TestInfoService.SaveDataList(vm, 62);

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

            return RedirectToAction("GoalIndex");
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

            return RedirectToAction("NavIndex");
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

        #endregion

        #region 環境介紹Env

        #region 查詢
        public IActionResult EnvIndex(String? data, Int32? page)
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

            vmd.schPageList2 = _IntroService.schPagesEnv(vmd.schItem, page.Value, 15);
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
        public IActionResult EnvIndex(IntroViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _IntroService.schPagesEnv(vmd.schItem, 1, 15);
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
        public IActionResult EnvModify(Guid Uid)
        {
            IntroViewModel.Env? vm = new IntroViewModel.Env();

            if (Uid != null)
            {
                //編輯
                vm = _IntroService.GetEditDataEnv(Uid);
            }
            else
            {
                //新增
                vm = new IntroViewModel.Env();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EnvModify(IntroViewModel.Env vm)
        {
            _IntroService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _IntroService.SaveDataEnv(vm);

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

            return RedirectToAction("ClassInfoIndex");
        }


        #endregion

        #region 刪除

        [HttpPost]
        public IActionResult DeleteEnv(Guid Uid)
        {
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _IntroService.DelDataEnv(Uid);

            return Json(new { chk = error.CheckMsg, msg = error.ErrorMsg });
            //return RedirectToAction("Index", new { msg = error });
        }
        #endregion

        #endregion

        #region 課程及承辦資訊 ClassInfo
        #region 查詢
        public IActionResult ClassInfoIndex(String? data, Int32? page)
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

            vmd.schPageList2 = _IntroService.schPagesClassInfo(vmd.schItem, page.Value, 15);
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
        public IActionResult ClassInfoIndex(IntroViewModel.SchModel vmd)
        {
            vmd.schPageList2 = _IntroService.schPagesClassInfo(vmd.schItem, 1, 15);
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
        public IActionResult ClassInfoModify(Guid Uid)
        {
            IntroViewModel.ClassInfo? vm = new IntroViewModel.ClassInfo();

            if (Uid != null)
            {
                //編輯
                vm = _IntroService.GetEditDataClassInfo(Uid);
            }
            else
            {
                //新增
                vm = new IntroViewModel.ClassInfo();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClassInfoModify(IntroViewModel.ClassInfo vm)
        {
            _IntroService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _IntroService.SaveDataClassInfo(vm);

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

            return RedirectToAction("ClassInfoIndex");
        }


        #endregion

        #region 刪除

        [HttpPost]
        public IActionResult DeleteClassInfo(Guid Uid)
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
    }
}
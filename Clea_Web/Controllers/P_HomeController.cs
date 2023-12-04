using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.ViewModels;
using Newtonsoft.Json;

namespace Clea_Web.Controllers
{
    //後臺首頁
    public class P_HomeController : BaseController
    {
        private readonly ILogger<P_HomeController> _logger;
        private AccountService _accountService;
        private P_LectorBtnService _P_LectorBtnService;


        public P_HomeController(ILogger<P_HomeController> logger, dbContext dbCLEA, AccountService Service, P_LectorBtnService ServiceA)
        {
            _logger = logger;
            db = dbCLEA;
            _accountService = Service;
            _P_LectorBtnService = ServiceA;
        }


        #region 首頁
        //public IActionResult Index()
        //{
        //    return View();
        //}
        public IActionResult Index(String? data, Int32? page)
        {
            P_LectorBtnViewModel.SchModel vmd = new P_LectorBtnViewModel.SchModel();
            _P_LectorBtnService.user = User;
            page = page ?? 1;

            if (!(page is null) && !string.IsNullOrEmpty(data))
            {
                vmd.schItem = JsonConvert.DeserializeObject<P_LectorBtnViewModel.SchItem>(value: data);
                ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schItem);
            }
            else
            {
                vmd.schItem = new P_LectorBtnViewModel.SchItem();
            }

            vmd.schPageList2 = _P_LectorBtnService.schPages(vmd.schItem, page.Value, 15);

            return View(vmd);
        }
        #endregion

    }
}
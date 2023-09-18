using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;

namespace Clea_Web.Controllers
{
    //後臺首頁-我的課程
    public class P_LectorClassController : BaseController
    {
        private readonly ILogger<P_LectorClassController> _logger;
        private AccountService _accountService;

        public P_LectorClassController(ILogger<P_LectorClassController> logger, dbContext dbCLEA, AccountService Service)
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

    }
}
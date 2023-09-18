using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;

namespace Clea_Web.Controllers
{
    //前台講師專區-首頁
    public class P_LectorRoleController : BaseController
    {
        private readonly ILogger<P_LectorRoleController> _logger;
        private AccountService _accountService;

        public P_LectorRoleController(ILogger<P_LectorRoleController> logger, dbContext dbCLEA, AccountService Service)
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
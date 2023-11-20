﻿using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;

namespace Clea_Web.Controllers
{
    //後台講師專區-我的評鑑
    public class B_LectorEvaluationController : BaseController
    {
        private readonly ILogger<B_LectorEvaluationController> _logger;
        private AccountService _accountService;

        public B_LectorEvaluationController(ILogger<B_LectorEvaluationController> logger, dbContext dbCLEA, AccountService Service)
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
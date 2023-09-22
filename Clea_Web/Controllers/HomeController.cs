using Clea_Web.Controllers;
using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Clea_Web.Service;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private BaseService _service;

        public HomeController(ILogger<HomeController> logger,BaseService baseService,dbContext a)
        {
            _logger = logger;
            _service = baseService;
            db = a;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            //return View();
            //導向登入頁面
            return RedirectToAction("Index", "Sys_Login");

        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
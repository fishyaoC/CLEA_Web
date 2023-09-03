using Clea_Web.Controllers;
using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Clea_Web.Service;
using System.Diagnostics;

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

        public IActionResult Index()
        {
            return View();
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
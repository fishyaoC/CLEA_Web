using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;

namespace Clea_Web.Controllers
{
    //後臺首頁
    public class P_HomeController : BaseController
    {
        private readonly ILogger<P_HomeController> _logger;
        private AccountService _accountService;

        public P_HomeController(ILogger<P_HomeController> logger, dbContext dbCLEA, AccountService Service)
        {
            _logger = logger;
            db = dbCLEA;
            _accountService = Service;
        }


        #region 首頁
        public IActionResult Index()
        {
            return View();
        }
        #endregion

    }
}
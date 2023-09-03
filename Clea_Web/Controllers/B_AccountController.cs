using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;

namespace Clea_Web.Controllers
{
    //後臺帳號管理
    public class B_AccountController : BaseController
    {
        private readonly ILogger<B_AccountController> _logger;
        private AccountService _accountService;

        public B_AccountController(ILogger<B_AccountController> logger, dbContext dbCLEA, AccountService Service)
        {
            _logger = logger;
            db = dbCLEA;
            _accountService = Service;
        }


        #region 新增、編輯
        public IActionResult Modify()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Modify(int a)
        {
            return View();
        }
        #endregion

        #region 查詢
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(int a)
        {
            return View();
        }
        #endregion

        #region 刪除

        [HttpPost]
        public IActionResult Delete(int a)
        {
            return View();
        }
        #endregion
    }
}
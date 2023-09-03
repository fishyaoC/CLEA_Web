using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.ViewModels;
using Clea_Web.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Clea_Web.Controllers
{
    //後臺登入頁面
    public class Sys_LoginController : BaseController
    {
        private readonly ILogger<Sys_LoginController> _logger;
        private LoginService _loginService = new LoginService();
        private BaseService _baseService;

        public Sys_LoginController(ILogger<Sys_LoginController> logger, dbContext dbCLEA,BaseService baseService)
        {
            _logger = logger;
            db = dbCLEA;
             _baseService = baseService;
        }

        //註冊頁面、登入頁面、變更密碼

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
            LoginViewModel vm = new LoginViewModel();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginViewModel vm)
        {
            UserRoleViewModel.UserRole model = new UserRoleViewModel.UserRole();
            model.lst_sysMenu = db.SysMenus.ToList();
            HttpContext.Session.SetString("role","admin");

            BaseViewModel.errorMsg errorMsg = new BaseViewModel.errorMsg();        

            List<Claim> claims = _loginService.Login(vm);

            if (claims.Count > 0)
            {
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProps = new AuthenticationProperties { IsPersistent = true };

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProps);

                _baseService.user = User;

                _baseService.GetUserID(User);

                return RedirectToAction("Index", "B_Home");
            }
            else
            {
                errorMsg.ErrorMsg = "帳號密碼有誤!請檢查!";
                return View(errorMsg);
            }
        }
        #endregion



        #region Logout
       
        public IActionResult Logout(int a)
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index");
        }
        #endregion
    }
}
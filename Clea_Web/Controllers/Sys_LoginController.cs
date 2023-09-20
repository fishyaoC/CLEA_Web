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
            BaseViewModel.errorMsg errorMsg = new BaseViewModel.errorMsg();
            if (!ModelState.IsValid)
            {
                errorMsg.ErrorMsg = "帳號密碼有誤!請檢查!";
                return View(errorMsg);
            }
            else
            {
                UserRoleViewModel.UserRole model = new UserRoleViewModel.UserRole();
                model.lst_sysMenu = db.SysMenus.ToList();




                List<Claim> claims = _loginService.Login(vm);

                if (claims.Count > 0)
                {
                    HttpContext.Session.SetString("role", claims[2].Value);
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProps = new AuthenticationProperties { IsPersistent = true };

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authProps);

                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    Thread.CurrentPrincipal = claimsPrincipal;
                    _baseService.user = claimsPrincipal;

                    return RedirectToAction("Index", "B_Home");
                }
                else
                {

                    TempData["TempMsgType"] = "error";
                    TempData["TempMsgTitle"] = "登入失敗";
                    TempData["TempMsg"] = "帳號密碼有誤!請檢查!";
                    return RedirectToAction("Index");
                }
            }
            
        }
        #endregion



        #region Logout
       
        public IActionResult Logout(int a)
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Thread.CurrentPrincipal = null;
            return RedirectToAction("Index");
        }
        #endregion
    }
}
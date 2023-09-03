using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.ViewModels;

namespace Clea_Web.Controllers
{
    //後臺帳號管理
    public class Sys_RoleController : BaseController
    {
        private readonly ILogger<Sys_RoleController> _logger;
        private RoleService _roleService;        

        public Sys_RoleController(ILogger<Sys_RoleController> logger, dbContext dbCLEA, RoleService Service)
        {
            _logger = logger;
            db = dbCLEA;
            _roleService = Service;
        }

        
        #region 新增、編輯
        //public IActionResult Modify(String Type, String? R_ID)
        public IActionResult Modify(String? R_ID)
        {
            UserRoleViewModel.Modify? vm = null;
            if (!string.IsNullOrEmpty(R_ID))
            {
                //編輯
                
            }
            else
            {
                //新增
                vm = new UserRoleViewModel.Modify();
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Modify(UserRoleViewModel.Modify vm)
        {
            _roleService.user = User;
            BaseViewModel.errorMsg error = new BaseViewModel.errorMsg();
            error = _roleService.SaveData(vm);
            
            return View(error);
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
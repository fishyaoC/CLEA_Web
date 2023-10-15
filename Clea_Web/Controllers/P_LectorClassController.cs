using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.ViewModels;
using Newtonsoft.Json;

namespace Clea_Web.Controllers
{
    //後臺首頁-我的課程
    public class P_LectorClassController : BaseController
    {
        private readonly ILogger<P_LectorClassController> _logger;
        private LectorClassService _lectorClassService;

        public P_LectorClassController(ILogger<P_LectorClassController> logger, dbContext dbCLEA, LectorClassService Service)
        {
            _logger = logger;
            db = dbCLEA;
			_lectorClassService = Service;
        }


        #region Index
        public IActionResult Index(String? data, Int32? page)
        {
			LectorClassViewModel vmd = new LectorClassViewModel();
			_lectorClassService.user = User;
			page = page ?? 1;
			vmd.classMenuPageList = _lectorClassService.GetClassMenuPageList(page.Value);
			return View(vmd);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult Index(String? data)
		{

			return View();
		}
		#endregion

		#region Modify
		public IActionResult Modify(Guid CL_UID)
        {
            return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Modify(String Data)
		{
			return View();
		}
		#endregion

	}
}
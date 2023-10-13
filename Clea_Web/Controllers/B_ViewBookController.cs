using Clea_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clea_Web.Service;
using Clea_Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Clea_Web.ViewModels;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using System;
using Novacode;

namespace Clea_Web.Controllers
{
	//後台-教材評鑑成績
	[Authorize]
	[UserPowerFilterAttribute]
	public class B_ViewBookController : BaseController
	{
		private readonly ILogger<B_ViewBookController> _logger;
		private ViewBookService _viewBookService;

		public B_ViewBookController(ILogger<B_ViewBookController> logger, dbContext dbCLEA, ViewBookService Service)
		{
			_logger = logger;
			db = dbCLEA;
			_viewBookService = Service;
		}

		#region 教材列表
		public IActionResult Index(String? data, Int32? page)
		{
			ViewBookViewModel.schBookModel vmd = new ViewBookViewModel.schBookModel();
			page = page ?? 1;

			if (!(page is null) && !string.IsNullOrEmpty(data))
			{
				vmd.schBookItem = JsonConvert.DeserializeObject<ViewBookViewModel.SchBookItem>(value: data);
				ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schBookItem);
			}
			else
			{
				vmd.schBookItem = new ViewBookViewModel.SchBookItem();
			}
			vmd.selectList = _viewBookService.GetYearItemList();
			vmd.bookInfosPageList = _viewBookService.GetbookInfosPageList(data: vmd.schBookItem, page.Value);
			return View(vmd);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Index(ViewBookViewModel.schBookModel vmd)
		{
			vmd.selectList = _viewBookService.GetYearItemList();
			vmd.bookInfosPageList = _viewBookService.GetbookInfosPageList(data: vmd.schBookItem, 1);
			ViewBag.schPageList = JsonConvert.SerializeObject(vmd.schBookItem);
			return View(vmd);
		}
		#endregion

		#region 評鑑教師列表
		public IActionResult P_Index(Guid B_UID, Int32 Year)
		{
			ViewBookViewModel.P_TeacherInfo vmd = new ViewBookViewModel.P_TeacherInfo();

			vmd.bookInfo = _viewBookService.GetModel(B_UID);
			vmd.p_TeachersPageList = _viewBookService.p_TeachersPageList(B_UID, Year, 1);

			return View(vmd);
		}
		#endregion

		#region 教材成績審查表
		public IActionResult Modify(Guid CLvUid)
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Modify()
		{
			return RedirectToAction("P_Index", new { B_UID = "", Year = 1 });
		}
		#endregion

		#region 匯出教材審查表
		public IActionResult Export_ScoreWord(Guid ClvUid)
		{
			return View();
		}
		#endregion
	}
}
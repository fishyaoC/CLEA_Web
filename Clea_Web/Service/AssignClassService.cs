using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using X.PagedList;

namespace Clea_Web.Service
{
	//評鑑 mType => 0:課程 1:教材
	public class AssignClassService : BaseService
	{
		public AssignClassService(dbContext dbContext)
		{
			db = dbContext;
		}

		#region BackEnd

		#region ClassIndex
		public IPagedList<AssignClassViewModel.ClassInfor> GetClassInfoPageList(AssignClassViewModel.schClassItem data, Int32 page)
		{
			List<AssignClassViewModel.ClassInfor> result = new List<AssignClassViewModel.ClassInfor>();
			result = (from CE in db.ViewClassEvaluates
					  where
					  (
					  (CE.EType == 0) &&
					  (data.Year == null || CE.EYear == data.Year) &&
					  (string.IsNullOrEmpty(data.C_Number) || CE.CId.Contains(data.C_Number)) &&
					  (string.IsNullOrEmpty(data.C_Name) || CE.CName.Contains(data.C_Name))
					  )
					  select new AssignClassViewModel.ClassInfor()
					  {
						  E_ID = CE.EId,
						  Year = CE.EYear,
						  C_ID = CE.CId,
						  C_Name = CE.CName
					  }).OrderBy(x => x.C_Name).ToList();

			return result.ToPagedList(page, pagesize);
		}
		#endregion

		#region 取得10年度
		public List<SelectListItem> GetYearSelectItems()
		{
			List<SelectListItem> result = new List<SelectListItem>();

			Int32 StartYear = (DateTime.Now.Year + 1) - 10;
			Int32 EndYear = DateTime.Now.Year + 1;

			result.Add(new SelectListItem() { Text = "請選擇", Value = null });


			for (Int32 i = EndYear; i > StartYear; i--)
			{
				result.Add(new SelectListItem() { Text = i.ToString() + "年", Value = i.ToString() });
			}

			return result;
		}
		#endregion

		#region 取得課程教師列表
		public List<SelectListItem> GetClassSelectItems()
		{
			List<SelectListItem> result = new List<SelectListItem>();
			result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });

			List<CClass> lst_Class = new List<CClass>();
			lst_Class = db.CClasses.ToList();

			if (lst_Class.Count() > 0)
			{
				foreach (CClass Class in lst_Class)
				{
					result.Add(new SelectListItem() { Text = Class.CName, Value = Class.CUid.ToString() });
				}
			}

			return result;
		}
		#endregion

		#region SaveAddData
		public BaseViewModel.errorMsg SaveAddData(AssignClassViewModel.AddModel data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			try
			{
				EEvaluate eEvaluate = new EEvaluate();
				eEvaluate.EId = Guid.NewGuid();
				eEvaluate.EType = 0;
				eEvaluate.EYear = data.addModify.Year;
				eEvaluate.MatchKey = data.addModify.C_UID;
				eEvaluate.Creuser = Guid.Parse(GetUserID(user));
				eEvaluate.Credate = DateTime.Now;
				db.EEvaluates.Add(eEvaluate);
				result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
			}
			catch (Exception e)
			{
				result.CheckMsg = false;
				result.ErrorMsg = e.Message;
			}
			return result;
		}
		#endregion

		#region TC_Index
		public IPagedList<AssignClassViewModel.CL> GetClPageList(Guid E_ID, AssignClassViewModel.CLschItem data, Int32 page)
		{
			List<AssignClassViewModel.CL> result = new List<AssignClassViewModel.CL>();

			EEvaluate? eEvaluate = db.EEvaluates.Find(E_ID) ?? null;




			List<EEvaluateDetail> lst_ED = db.EEvaluateDetails.Where(x => x.EId == E_ID).ToList();

			result = (from CL in db.ViewClassLectors
					  where
					  (
					  (CL.CUid == eEvaluate.MatchKey) &&
					  (string.IsNullOrEmpty(data.S_Name) || CL.CName.Contains(data.S_Name)) &&
					  (string.IsNullOrEmpty(data.L_Name) || CL.LName.Contains(data.L_Name))
					  )
					  //join ed in db.EEvaluateDetails on CL.ClUid equals ed.MatchKey2
					  select new AssignClassViewModel.CL()
					  {
						  ED_ID = E_ID,
						  CL_UID = CL.ClUid,
						  C_Name = CL.CName,
						  S_Name = CL.DName,
						  L_Name = CL.LName,
						  IsEvaluate = (from ed in db.EEvaluateDetails where ed.MatchKey2 == CL.ClUid select ed).FirstOrDefault() == null ? false : (from ed in db.EEvaluateDetails where ed.MatchKey2 == CL.ClUid select ed).FirstOrDefault().EScoreA == null ? false : true
					  }).OrderBy(x => x.S_Name).ThenBy(x => x.L_Name).ToList();




			return result.ToPagedList(page, pagesize);
		}
		#endregion

		#region TC_Moidify
		public AssignClassViewModel.CLInfo getCLinfo(Guid E_ID, Guid CL_UID)
		{
			AssignClassViewModel.CLInfo result = new AssignClassViewModel.CLInfo();
			CClassLector? cClassLector = db.CClassLectors.Find(CL_UID) ?? null;
			EEvaluate? eEvaluate = db.EEvaluates.Find(E_ID) ?? null;
			if (cClassLector != null)
			{
				CClass? cClass = db.CClasses.Find(cClassLector.CUid) ?? null;
				CClassSubject? cClassSubject = db.CClassSubjects.Find(cClassLector.DUid) ?? null;
				CLector? cLector = db.CLectors.Find(cClassLector.LUid) ?? null;

				result.Year = eEvaluate.EYear;
				result.C_ID = cClass.CId;
				result.C_Name = cClass.CName;
				result.S_ID = cClassSubject.DId;
				result.S_Name = cClassSubject.DName;
				result.L_UID = cLector.LUid;
				result.L_ID = cLector.LId;
				result.L_Name = cLector.LName;
			}


			return result;
		}
		#endregion

		#region 取得教師下拉選單
		public List<SelectListItem> getTeacherItem(Guid L_UID)
		{
			List<SelectListItem> result = new List<SelectListItem>();
			result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
			List<CLector> lst_cLectors = db.CLectors.Where(x => x.LUid != L_UID).ToList();
			if (lst_cLectors != null && lst_cLectors.Count() > 0)
			{
				foreach (CLector L in lst_cLectors)
				{
					result.Add(new SelectListItem() { Text = L.LName, Value = L.LUid.ToString() });
				}
			}
			return result;
		}
		#endregion

		#region 取得評鑑教師列表
		public List<AssignClassViewModel.EvTeacher> GetEvTeacherPageLists(Guid E_ID, Guid CL_UID)
		{
			Int32 LocalYear = DateTime.Now.Year + 1;
			List<AssignClassViewModel.EvTeacher> result = new List<AssignClassViewModel.EvTeacher>();
			result = (from ED in db.EEvaluateDetails
					  where ED.EId == E_ID && ED.MatchKey2 == CL_UID
					  join lec in db.CLectors on ED.Evaluate equals lec.LUid
					  select new AssignClassViewModel.EvTeacher()
					  {
						  ED_ID = ED.EdId,
						  L_Ev_ID = lec.LId,
						  L_Ev_Name = lec.LName
					  }).ToList();
			return result;
		}
		#endregion

		#region SaveTCData
		public BaseViewModel.errorMsg SaveTcData(AssignClassViewModel.TCModify data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();

			try
			{
				CClassLector? cClassLector = db.CClassLectors.Find(data.CL_UID) ?? null;
				EEvaluateDetail eEvaluateDetail = new EEvaluateDetail()
				{
					EdId = Guid.NewGuid(),
					EId = data.E_ID,
					MatchKey2 = data.CL_UID,
					Reception = cClassLector.LUid.Value,
					Evaluate = data.L_UID_Ev,
					Creuser = Guid.Parse(GetUserID(user)),
					Credate = DateTime.Now
				};

				db.EEvaluateDetails.Add(eEvaluateDetail);
				result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
			}
			catch (Exception ex)
			{
				result.CheckMsg = false;
				result.ErrorMsg = ex.Message;
			}

			return result;
		}
		#endregion

		#region 匯出Excel資料

		public Byte[] Export_Excel()
		{
			List<ViewClassLectorUnFileLoad> data = db.ViewClassLectorUnFileLoads.ToList();

			#region ExportExcel
			String[] lst_Header = new string[] { "項次", "教師名稱", "課程名稱", "科目名稱" };
			using (var exportData = new MemoryStream())
			{
				IWorkbook wb = new XSSFWorkbook();  //字型定義
				ISheet sheet = wb.CreateSheet((DateTime.Now.Year + 1).ToString() + "年評鑑-未上傳課程明細");
				XSSFCellStyle TitleStyle = (XSSFCellStyle)wb.CreateCellStyle(); //標題字型
				TitleStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
				TitleStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
				TitleStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
				TitleStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
				TitleStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
				TitleStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
				XSSFFont font = (XSSFFont)wb.CreateFont();
				font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
				TitleStyle.SetFont(font);

				XSSFCellStyle ContentStyle = (XSSFCellStyle)wb.CreateCellStyle();//內容造型
				ContentStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
				ContentStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
				ContentStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
				ContentStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
				ContentStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
				ContentStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

				for (int i = 0; i < data.Count + 1; i++) //row
				{
					sheet.CreateRow(i);
					for (int j = 0; j < lst_Header.Length; j++)
					{
						sheet.GetRow(i).CreateCell(j).CellStyle = ContentStyle; //產生 cell
						sheet.SetColumnWidth(j, 24 * 256);//寬度
					}
				}

				for (int x = 0; x < lst_Header.Length; x++)
				{
					sheet.GetRow(0).GetCell(x).SetCellValue(lst_Header[x]);
				}

				if (data.Count() > 0)
				{
					Int32 RowCount = 1;
					foreach (ViewClassLectorUnFileLoad item in data)
					{
						sheet.GetRow(RowCount).GetCell(0).SetCellValue(RowCount.ToString());
						sheet.GetRow(RowCount).GetCell(1).SetCellValue(item.LName);
						sheet.GetRow(RowCount).GetCell(2).SetCellValue(item.CName);
						sheet.GetRow(RowCount).GetCell(3).SetCellValue(item.DName);
						RowCount++;
					}
				}

				wb.Write(exportData, true);

				Byte[] result = exportData.ToArray();
				return result;
			}
			#endregion

		}

		#endregion

		#region 取得審查表
		public AssignClassViewModel.V_ScoreModel GetVModel(Guid ED_ID)
		{
			AssignClassViewModel.V_ScoreModel result = new AssignClassViewModel.V_ScoreModel();
			EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(ED_ID) ?? null;

			if (eEvaluateDetail != null)
			{
				result.ED_ID = eEvaluateDetail.EdId;
				result.Score_A = eEvaluateDetail.EScoreA;
				result.Score_B = eEvaluateDetail.EScoreB;
				result.Score_C = eEvaluateDetail.EScoreC;
				result.Score_D = eEvaluateDetail.EScoreD;
				result.Score_E = eEvaluateDetail.EScoreE;
				result.Remark = eEvaluateDetail.ERemark;
				result.Syllabus = eEvaluateDetail.ETeachSyllabus;
				result.Object = eEvaluateDetail.ETeachObject;
				result.Abstract = eEvaluateDetail.ETeachAbstract;
			}

			return result;
		}
		#endregion

		#region 取得教案圖片
		public List<String> GetPicList(Guid ED_ID)
		{
			List<String> result = new List<string>();



			return result;
		}
		#endregion

		#region SaveVData
		public BaseViewModel.errorMsg SaveVData(AssignClassViewModel.V_ScoreModel data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();

			try
			{
				EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(data.ED_ID) ?? null;
				if (eEvaluateDetail != null)
				{
					//eEvaluateDetail.ETeachSyllabus = data.Syllabus;
					//eEvaluateDetail.ETeachObject = data.Object;
					//eEvaluateDetail.ETeachAbstract = data.Abstract;
					eEvaluateDetail.EScoreA = data.Score_A;
					eEvaluateDetail.EScoreB = data.Score_B;
					eEvaluateDetail.EScoreC = data.Score_C;
					eEvaluateDetail.EScoreD = data.Score_D;
					eEvaluateDetail.EScoreE = data.Score_E;
					eEvaluateDetail.ERemark = data.Remark;
					eEvaluateDetail.Upduser = Guid.Parse(GetUserID(user));
					eEvaluateDetail.Upddate = DateTime.Now;
					result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
				}
			}
			catch (Exception ex)
			{
				result.CheckMsg = false;
				result.ErrorMsg = ex.Message;
			}

			return result;
		}
		#endregion

		#endregion

		#region Portal
		#endregion

	}
}


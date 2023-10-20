using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing.Text;
using System.IO.Compression;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Novacode;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Org.BouncyCastle.Cms;
using X.PagedList;

namespace Clea_Web.Service
{
	//評鑑 mType => 0:課程 1:教材
	public class AssignBookService : BaseService
	{
		public AssignBookService(dbContext dbContext)
		{
			db = dbContext;
		}

		#region BackEnd

		#region NEW

		#region IndexPage
		public IPagedList<AssignBookViewModel.BookInfor> GetSchBookItemPageList(AssignBookViewModel.SchBookItems data, Int32 page)
		{
			List<AssignBookViewModel.BookInfor> result = new List<AssignBookViewModel.BookInfor>();

			result = (from book in db.ViewBookEvaluates
					  where
					  (
					  (book.EType == 1) &&
					  (data.Year == null || book.EYear == data.Year) &&
					  (string.IsNullOrEmpty(data.B_ID) || book.MIndex.ToString().Contains(data.B_ID)) &&
					  (string.IsNullOrEmpty(data.B_Name) || book.MName.Contains(data.B_Name))
					  )
					  select new AssignBookViewModel.BookInfor()
					  {
						  Year = book.EYear,
						  B_UID = book.MId,
						  B_ID = book.MIndex.ToString(),
						  B_Name = book.MName,
						  E_ID = book.EId
					  }).OrderByDescending(x => x.Year).ToList();

			return result.ToPagedList(page, pagesize);
		}
		#endregion

		#region 取得評鑑年度
		public List<SelectListItem> GetYearSelectItems()
		{
			List<SelectListItem> result = new List<SelectListItem>();
			Int32 Start = (DateTime.Now.Year + 1) - 10;
			Int32 End = (DateTime.Now.Year + 1);

			result.Add(new SelectListItem() { Text = "請選擇", Value = null });
			for (int x = End; x > Start; x--)
			{
				result.Add(new SelectListItem() { Text = x.ToString() + "年", Value = x.ToString() });
			}

			return result;
		}
		#endregion

		#region 取得課程列表
		public List<SelectListItem> GetSelectListItemsBook()
		{
			List<SelectListItem> result = new List<SelectListItem>();

			List<CBook> lst_Book = new List<CBook>();
			lst_Book = db.CBooks.ToList();

			if (lst_Book.Count > 0)
			{
				foreach (CBook bk in lst_Book.OrderBy(x => x.MOrder))
				{
					result.Add(new SelectListItem() { Text = bk.MName, Value = bk.MId.ToString() });
				}
			}

			return result;
		}
		#endregion

		#region SaveAddData
		public BaseViewModel.errorMsg SaveAddData(AssignBookViewModel.AddModify data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();

			List<CBookDetail> lst_BD = new List<CBookDetail>();
			lst_BD = db.CBookDetails.Where(x => x.MId == data.B_UID).ToList();

			try
			{
				EEvaluate eEvaluate = new EEvaluate()
				{
					EId = Guid.NewGuid(),
					EType = 1,
					EYear = data.Year,
					MatchKey = data.B_UID,
					Creuser = Guid.Parse(GetUserID(user)),
					Credate = DateTime.Now
				};
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

		#region 取得教材基本資訊
		public AssignBookViewModel.BookInfor GetBookInfor(Guid E_ID)
		{
			AssignBookViewModel.BookInfor result = new AssignBookViewModel.BookInfor();

			EEvaluate? eEvaluate = db.EEvaluates.Find(E_ID) ?? null;
			CBook? cBook = db.CBooks.Find(eEvaluate.MatchKey) ?? null;

			if (eEvaluate != null)
			{
				result.E_ID = E_ID;
				result.Year = eEvaluate.EYear;
				result.B_ID = cBook.MIndex.ToString();
				result.B_Name = cBook.MName;
			}

			return result;
		}
		#endregion

		#region 取得評鑑教師列表
		public List<AssignClassViewModel.EvTeacher> getEvTeacherList(Guid E_ID)
		{
			List<AssignClassViewModel.EvTeacher> result = new List<AssignClassViewModel.EvTeacher>();

			result = (from ed in db.ViewBookEvaluateTeachers
					  where (ed.EId == E_ID)
					  select new AssignClassViewModel.EvTeacher()
					  {
						  E_ID = ed.EId.Value,
						  L_UID_Ev = ed.LUid,
						  L_Ev_ID = ed.LId,
						  L_Ev_Name = ed.LName
					  }).ToList();

			return result;
		}
		#endregion

		#region 取得評鑑教師item
		public List<SelectListItem> selectListItemsTeacher()
		{
			List<SelectListItem> result = new List<SelectListItem>();
			List<CLector> lst_lec = db.CLectors.ToList();
			result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });

			if (lst_lec != null && lst_lec.Count > 0)
			{
				foreach (CLector lec in lst_lec)
				{
					result.Add(new SelectListItem() { Text = lec.LName, Value = lec.LUid.ToString() });
				}
			}

			return result;
		}
		#endregion

		#region SaveModify
		public BaseViewModel.errorMsg SaveModify(AssignBookViewModel.Modify data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			try
			{
				List<CBookDetail> lst_BD = db.CBookDetails.Where(x => x.MId == data.B_UID).ToList();
				if (lst_BD != null && lst_BD.Count > 0)
				{
					foreach (CBookDetail BD in lst_BD)
					{
						EEvaluateDetail eEvaluateDetail = new EEvaluateDetail()
						{
							EdId = Guid.NewGuid(),
							EId = data.E_ID,
							MatchKey2 = BD.MdId,
							Evaluate = data.L_UID_Ev,
							Creuser = Guid.Parse(GetUserID(user)),
							Credate = DateTime.Now
						};
						db.EEvaluateDetails.Add(eEvaluateDetail);
					}
					result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
				}
				else
				{
					result.CheckMsg = false;
					result.ErrorMsg = "尚無教材版本資料!";
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

		#region V_Index
		public List<AssignBookViewModel.EDInfo> getEDInfoList(Guid E_ID)
		{
			List<AssignBookViewModel.EDInfo> result = new List<AssignBookViewModel.EDInfo>();
			EEvaluate? oriEv = db.EEvaluates.Find(E_ID) ?? null;
			String bookName = string.Empty;
			if (oriEv != null)
			{
				bookName = db.CBooks.Find(oriEv.MatchKey).MName;
			}

			result = (from ed in db.EEvaluateDetails
					  where (ed.EId == E_ID)
					  select new AssignBookViewModel.EDInfo()
					  {
						  ED_ID = ed.EdId,
						  B_Name = bookName,
						  BD_Publish = (from cp in db.CBookPublishes where cp.BpId == ((from bd in db.CBookDetails where bd.MdId == ed.MatchKey2 select bd).FirstOrDefault().MdPublish) select cp).FirstOrDefault().BpName,
						  L_UID_Ev = (from le in db.CLectors where le.LUid == ed.Evaluate select le).FirstOrDefault().LName,
						  IsEvaluate = ed.EScoreA == null ? false : true,
						  IsUpload = db.SysFiles.Where(x => x.FMatchKey == ed.EdId).Count() == 0 ? false : true
					  }).ToList();

			return result;
		}
		#endregion

		#region V_Modify
		public AssignBookViewModel.ScoreModify GetScoreModel(Guid ED_ID)
		{
			AssignBookViewModel.ScoreModify result = new AssignBookViewModel.ScoreModify();
			EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(ED_ID) ?? null;
			if (eEvaluateDetail != null)
			{
				result.E_ID = eEvaluateDetail.EId;
				result.ED_ID = ED_ID;
				result.Score_A = eEvaluateDetail.EScoreA;
				result.Score_B = eEvaluateDetail.EScoreB;
				result.Score_C = eEvaluateDetail.EScoreC;
				result.Remark = eEvaluateDetail.ERemark;
			}

			return result;
		}
		#endregion

		#region SaveVModofiyData
		public BaseViewModel.errorMsg saveVModifyData(AssignBookViewModel.ScoreModify data)
		{
			BaseViewModel.errorMsg result = new BaseViewModel.errorMsg();
			try
			{
				EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(data.ED_ID) ?? null;

				eEvaluateDetail.EScoreA = data.Score_A;
				eEvaluateDetail.EScoreB = data.Score_B;
				eEvaluateDetail.EScoreC = data.Score_C;
				eEvaluateDetail.ERemark = data.Remark;
				eEvaluateDetail.Upduser = Guid.Parse(GetUserID(user));
				eEvaluateDetail.Upddate = DateTime.Now;

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

		#region 匯出Excel
		public Byte[] Export_Excel(Guid E_ID)
		{
			EEvaluate? eEvaluate = db.EEvaluates.Find(E_ID) ?? null;
			List<EEvaluateDetail> lstED = db.EEvaluateDetails.Where(x => x.EId == E_ID).ToList();

			String B_Name = db.CBooks.Find(eEvaluate.MatchKey).MName;
			List<AssignBookViewModel.ScoreTable> scoreTable = new List<AssignBookViewModel.ScoreTable>();

			Int32 PublishCount = lstED.GroupBy(i => i.MatchKey2).Count();
			var lstPublish = lstED.GroupBy(x => x.MatchKey2).ToList();  //評鑑版本
			Int32 EvTeacherCount = lstED.GroupBy(x => x.Evaluate).Count();
			var lstEvTeacher = lstED.GroupBy(x => x.Evaluate).ToList(); //評鑑教師

			#region ExportExcel
			String[] lst_Header = new string[] { "教材名稱", "教材版本", "平均總分", "排序" };
			using (var exportData = new MemoryStream())
			{
				IWorkbook wb = new XSSFWorkbook();  //字型定義
				ISheet sheet = wb.CreateSheet(B_Name);
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

				for (int i = 0; i < 4; i++) //row
				{
					sheet.CreateRow(i);
					for (int j = 0; j < lst_Header.Length; j++)
					{
						sheet.GetRow(i).CreateCell(j).CellStyle = ContentStyle; //產生 cell
						sheet.SetColumnWidth(j, 24 * 256);//寬度
					}
				}

				CellRangeAddress range0 = new CellRangeAddress(0, 0, 1, PublishCount);
				sheet.AddMergedRegion(range0);

				for (int x = 0; x < lst_Header.Length; x++)
				{
					sheet.GetRow(x).GetCell(0).SetCellValue(lst_Header[x]);
				}

				sheet.GetRow(0).GetCell(1).SetCellValue(B_Name);

				if (lstPublish != null && lstPublish.Count > 0) //處理教材版本與成績統計
				{
					foreach (var p in lstPublish)
					{
						Int32 Score = 0;
						CBookDetail? cBookDetail = db.CBookDetails.Find(p.FirstOrDefault().MatchKey2) ?? null;
						CBookPublish? cBookPublish = db.CBookPublishes.Find(cBookDetail.MdPublish) ?? null;

						List<EEvaluateDetail> list_score = db.EEvaluateDetails.Where(x => x.EId == E_ID && x.MatchKey2 == p.FirstOrDefault().MatchKey2 && x.EScoreA != null).ToList();
						if (list_score != null && list_score.Count > 0)
						{
							foreach (EEvaluateDetail ed in list_score)
							{
								Score += ed.EScoreA.Value + ed.EScoreB.Value + ed.EScoreC.Value;
							}
						}
						scoreTable.Add(new AssignBookViewModel.ScoreTable() { P_Name = cBookPublish.BpName, P_Score = (Score / EvTeacherCount).ToString("#0.00") });
					}
				}

				if (scoreTable.Count > 0)
				{
					Int32 ColCount = 1;
					foreach (var score in scoreTable.OrderByDescending(x => x.P_Score).ToList())
					{
						sheet.GetRow(1).GetCell(ColCount).SetCellValue(score.P_Name);
						sheet.GetRow(2).GetCell(ColCount).SetCellValue(score.P_Score);
						sheet.GetRow(3).GetCell(ColCount).SetCellValue(ColCount.ToString());
						ColCount++;
					}
				}

				wb.Write(exportData, true);

				Byte[] result = exportData.ToArray();
				return result;
			}
			#endregion

		}
		#endregion

		#region 匯出ZIP
		public Byte[] Export_ScoreZip(Guid E_ID, String dir)
		{
			EEvaluate? eEvaluate = db.EEvaluates.Find(E_ID) ?? null;
			List<EEvaluateDetail> lstED = db.EEvaluateDetails.Where(x => x.EId == E_ID).ToList();

			String B_Name = db.CBooks.Find(eEvaluate.MatchKey).MName;


			Int32 PublishCount = lstED.GroupBy(i => i.MatchKey2).Count();
			var lstPublish = lstED.GroupBy(x => x.MatchKey2).ToList();  //評鑑版本
			Int32 EvTeacherCount = lstED.GroupBy(x => x.Evaluate).Count();
			var lstEvTeacher = lstED.GroupBy(x => x.Evaluate).ToList(); //評鑑教師

			String SourcePath = "./SampleFile/Sample教材評核表V" + PublishCount.ToString() + ".docx";
			String SavePath = "./SampleFile/Output/" + eEvaluate.EYear + B_Name;
			String SavePathzip = "./SampleFile/Output/" + eEvaluate.EYear + B_Name + "審查表.zip";

			Directory.CreateDirectory(SavePath);
			if (lstEvTeacher != null && lstEvTeacher.Count > 0)
			{
				foreach (var t in lstEvTeacher)
				{
					List<AssignBookViewModel.ScoreTable> scoreTable = new List<AssignBookViewModel.ScoreTable>();

					CLector? cLector = db.CLectors.Find(t.Key) ?? null;
					String L_Name_Ev = cLector.LName;

					//List<EEvaluateDetail> list_score = db.EEvaluateDetails.Where(x => x.EId == E_ID && x.Evaluate == t.FirstOrDefault().MatchKey2 && x.EScoreA != null).ToList();
					List<EEvaluateDetail> list_score = db.EEvaluateDetails.Where(x => x.EId == E_ID && x.Evaluate == t.Key).ToList();

					if (list_score != null && list_score.Count > 0)
					{
						foreach (EEvaluateDetail ed in list_score)
						{
							CBookDetail? cBookDetail = db.CBookDetails.Find(ed.MatchKey2) ?? null;
							CBookPublish? cBookPublish = db.CBookPublishes.Find(cBookDetail.MdPublish) ?? null;
							String P_Name = cBookPublish.BpName;

							scoreTable.Add(new AssignBookViewModel.ScoreTable()
							{
								P_Name = P_Name,
								P_ScoreA = ed.EScoreA == null ? "0" : ed.EScoreA.ToString(),
								P_ScoreB = ed.EScoreB == null ? "0" : ed.EScoreB.ToString(),
								P_ScoreC = ed.EScoreC == null ? "0" : ed.EScoreC.ToString(),
								P_ScoreRemark = ed.ERemark,
								P_Score = (ed.EScoreA == null ? 0 : ed.EScoreA + ed.EScoreB == null ? 0 : ed.EScoreB + ed.EScoreC == null ? 0 : ed.EScoreC).ToString()
							});
						}

						//export
						if (scoreTable.Count > 0)
						{
							using (DocX doc = DocX.Load(SourcePath))
							{
								doc.ReplaceText("[@Year$]", DateTime.Now.Year.ToString());    //年
								doc.ReplaceText("[@Month$]", DateTime.Now.Year.ToString().PadLeft(2, '0'));                            //月
								doc.ReplaceText("[@Day$]", DateTime.Now.Year.ToString().PadLeft(2, '0'));                                //日
								doc.ReplaceText("[@BName$]", B_Name);
								doc.ReplaceText("[@L_Name_Ev$]", L_Name_Ev);

								int c = 0;
								String m = "a";
								String RemarkTotle = string.Empty;
								foreach (var sc in scoreTable)
								{
									switch (c)
									{
										case 1:
											m = "b";
											break;
										case 2:
											m = "c";
											break;
										case 3:
											m = "d";
											break;
										case 4:
											m = "e";
											break;
										default:
											m = "a";
											break;
									}
									doc.ReplaceText("[@P" + m.ToUpper() + "$]", sc.P_Name);
									doc.ReplaceText("[@S" + m + "1$]", sc.P_ScoreA);
									doc.ReplaceText("[@S" + m + "2$]", sc.P_ScoreB);
									doc.ReplaceText("[@S" + m + "3$]", sc.P_ScoreC);
									doc.ReplaceText("[@St" + m + "$]", string.IsNullOrEmpty(sc.P_ScoreRemark) ? string.Empty : sc.P_ScoreRemark);
									RemarkTotle += string.IsNullOrEmpty(sc.P_ScoreRemark) ? string.Empty : sc.P_ScoreRemark;
									c++;
								}
								doc.ReplaceText("[@Remark$]", RemarkTotle);
								doc.SaveAs(SavePath + "/" + L_Name_Ev + "-" + B_Name + "教材審查表.docx");
							}
						}
					}
				}
			}

			ZipFile.CreateFromDirectory(SavePath, SavePathzip);
			Byte[] result = System.IO.File.ReadAllBytes(SavePathzip);
			Directory.Delete(SavePath, true);
			File.Delete(SavePathzip);
			return result;
		}
		#endregion

		#endregion

		#endregion

		#region Portal
		#endregion

	}
}


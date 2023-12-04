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

			result = (from book in db.ViewBAssignBooks
					  where
					  (
					  (string.IsNullOrEmpty(data.B_ID) || book.MIndex.ToString().Contains(data.B_ID.Trim())) &&
					  (string.IsNullOrEmpty(data.B_Name) || book.MName.Contains(data.B_Name.Trim()))
					  )
					  select new AssignBookViewModel.BookInfor()
					  {
						  E_ID = book.EId.Value,
						  M_Index = book.MIndex,
						  M_Name = book.MName,
						  IsClose = book.IsClose.Value,
						  T_Count = (from eed in db.EEvaluateDetails where eed.EId == book.EId select eed).Count(),
						  M_Status = (from ees in db.EEvaluationSches where ees.EId == book.EId select ees).FirstOrDefault() == null ? 0 : (from ees in db.EEvaluationSches where ees.EId == book.EId select ees).FirstOrDefault().Status
					  }).ToList();

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

		#region 取得教材列表
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
					EYear = DateTime.Now.Year,
					MatchKey = data.B_UID, //M_ID
					Creuser = Guid.Parse(GetUserID(user)),
					Credate = DateTime.Now
				};
				db.EEvaluates.Add(eEvaluate);

				if (lst_BD.Count > 0)
				{
					foreach (CBookDetail CBD in lst_BD)
					{
						EEvaluationSche eEvaluationSche = new EEvaluationSche()
						{
							EsId = Guid.NewGuid(),
							EId = eEvaluate.EId,
							MatchKey = CBD.MdId,
							Status = 0,
							ScheNum = 0,
							IsSche = true,
							IsClose = false,
							Creuser = eEvaluate.Creuser,
							Credate = eEvaluate.Credate
						};
						db.EEvaluationSches.Add(eEvaluationSche);
					}
					result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
				}
				else
				{
					result.CheckMsg = false;
					result.ErrorMsg = "教材種類尚未指定訓練單位(出版社)!";
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

			result = (from ed in db.ViewBBookEvaluateTeachers
					  where (ed.EId == E_ID)
					  select new AssignClassViewModel.EvTeacher()
					  {
						  E_ID = ed.EId,
						  L_Ev_ID = ed.LId,
						  L_UID_Ev = ed.Evaluate,
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
				List<EEvaluationSche> eEvaluationSches = db.EEvaluationSches.Where(x => x.EId == data.E_ID).ToList();
				if (eEvaluationSches.Count > 0)
				{
					foreach (EEvaluationSche EES in eEvaluationSches)
					{
						EEvaluateDetail eEvaluateDetail = new EEvaluateDetail()
						{
							EdId = Guid.NewGuid(),
							EId = data.E_ID,
							EsId = EES.EsId,
							Evaluate = data.L_UID_Ev,
							Status = 2,
							IsClose = false,
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

			result = (from item in db.ViewBAssignBookEvaluateTeachers
					  where item.EId == E_ID
					  select new AssignBookViewModel.EDInfo()
					  {
						  ED_ID = item.EdId,
						  M_Book = item.MName,
						  M_Publish = item.BpName,
						  M_lv_Teacher = item.LName,
						  M_Status = item.Status,
						  IsClose = item.IsClose
					  }).OrderBy(x => x.M_lv_Teacher).ToList();

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
				result.IsClose = eEvaluateDetail.IsClose;
				result.Status = eEvaluateDetail.Status;
				if (result.Score_A != null)
				{
					result.TotalScore = result.Score_A.Value + result.Score_C.Value + result.Score_B.Value;
				}
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

				if (data.Status == 4 && data.IsClose)
				{
					eEvaluateDetail.Status = 6;
				}
				else if (data.Status == 4 && !data.IsClose)
				{
					eEvaluateDetail.Status = 4;
				}
				else if (data.Status == 5 && data.IsClose)
				{
					eEvaluateDetail.Status = 7;
				}
				else if (data.Status == 5 && !data.IsClose)
				{
					eEvaluateDetail.Status = 5;
				}
				eEvaluateDetail.EScoreA = data.Score_A;
				eEvaluateDetail.EScoreB = data.Score_B;
				eEvaluateDetail.EScoreC = data.Score_C;
				eEvaluateDetail.ERemark = data.Remark;
				eEvaluateDetail.IsClose = data.IsClose;
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
			List<AssignBookViewModel.ScoreTable> scoreTable = new List<AssignBookViewModel.ScoreTable>();
			List<ViewBAssignBookScore> viewBAssignBookScore = db.ViewBAssignBookScores.Where(x => x.EId == E_ID).OrderBy(x => x.BpName).ToList();
			String B_Name = viewBAssignBookScore.Count > 0 ? viewBAssignBookScore.FirstOrDefault().MName : string.Empty;
			Int32 PublishCount = viewBAssignBookScore.GroupBy(i => i.BpName).Count();
			var lstPublish = viewBAssignBookScore.GroupBy(x => x.BpName).ToList();  //評鑑版本
			Int32 EvTeacherCount = viewBAssignBookScore.GroupBy(x => x.Evaluate).Count();
			var lstEvTeacher = viewBAssignBookScore.GroupBy(x => x.Evaluate).ToList(); //評鑑教師

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
						sheet.SetColumnWidth(j, 24 * 512);//寬度
					}
				}

				CellRangeAddress range0 = new CellRangeAddress(0, 0, 1, PublishCount < 3 ? 3 : PublishCount);
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

						List<ViewBAssignBookScore> viewBAssignBookScores = new List<ViewBAssignBookScore>();
						viewBAssignBookScores = viewBAssignBookScore.Where(x => x.BpName == p.Key.ToString()).ToList();
						if (viewBAssignBookScores != null && viewBAssignBookScores.Count > 0)
						{
							foreach (ViewBAssignBookScore sc in viewBAssignBookScores)
							{
								Score += sc.EScoreA is null ? 0 : sc.EScoreA.Value + sc.EScoreB is null ? 0 : sc.EScoreB.Value + sc.EScoreC is null ? 0 : sc.EScoreC.Value;
							}
						}
						scoreTable.Add(new AssignBookViewModel.ScoreTable() { P_Name = p.Key, P_Score = (Score / EvTeacherCount).ToString("#0.00") });
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
			List<ViewBAssignBookScore> viewBAssignBookScore = db.ViewBAssignBookScores.Where(x => x.EId == E_ID).OrderBy(x => x.BpName).ToList();
			String B_Name = viewBAssignBookScore.Count > 0 ? viewBAssignBookScore.FirstOrDefault().MName.Trim() : string.Empty;
			Int32 PublishCount = viewBAssignBookScore.GroupBy(i => i.BpName).Count();
			var lstPublish = viewBAssignBookScore.GroupBy(x => x.BpName).ToList();  //評鑑版本
			Int32 EvTeacherCount = viewBAssignBookScore.GroupBy(x => x.Evaluate).Count();
			var lstEvTeacher = viewBAssignBookScore.GroupBy(x => x.LName).ToList(); //評鑑教師

			String SourcePath = "./SampleFile/Sample教材評核表V" + (PublishCount < 3 ? "3" : PublishCount.ToString()) + ".docx";
			String SavePath = "./SampleFile/Output/" + B_Name;
			String SavePathzip = "./SampleFile/Output/" + B_Name + "審查表.zip";

			Directory.CreateDirectory(SavePath);
			if (lstEvTeacher != null && lstEvTeacher.Count > 0)
			{
				foreach (var t in lstEvTeacher)
				{
					List<AssignBookViewModel.ScoreTable> scoreTable = new List<AssignBookViewModel.ScoreTable>();

					List<ViewBAssignBookScore> viewBAssignBookScores = new List<ViewBAssignBookScore>();
					viewBAssignBookScores = viewBAssignBookScore.Where(x => x.LName == t.Key.ToString()).ToList();

					if (viewBAssignBookScores != null && viewBAssignBookScores.Count > 0)
					{
						foreach (ViewBAssignBookScore sc in viewBAssignBookScores)
						{
							String P_Name = sc.BpName;

							scoreTable.Add(new AssignBookViewModel.ScoreTable()
							{
								P_Name = P_Name,
								P_ScoreA = sc.EScoreA == null ? "0" : sc.EScoreA.ToString(),
								P_ScoreB = sc.EScoreB == null ? "0" : sc.EScoreB.ToString(),
								P_ScoreC = sc.EScoreC == null ? "0" : sc.EScoreC.ToString(),
								P_ScoreRemark = sc.ERemark,
								P_Score = (sc.EScoreA == null ? 0 : sc.EScoreA + sc.EScoreB == null ? 0 : sc.EScoreB + sc.EScoreC == null ? 0 : sc.EScoreC).ToString()
							});
						}

						//export
						if (scoreTable.Count > 0)
						{
							if (scoreTable.Count < 3)
							{
								Int32 lostCount = 3 - scoreTable.Count;
								for (int i = 0; i < lostCount; i++)
								{
									scoreTable.Add(new AssignBookViewModel.ScoreTable()
									{
										P_Name = string.Empty,
										P_ScoreA = string.Empty,
										P_ScoreB = string.Empty,
										P_ScoreC = string.Empty,
										P_ScoreRemark = string.Empty,
										P_Score = string.Empty
									});
								}
							}

							using (DocX doc = DocX.Load(SourcePath))
							{
								doc.ReplaceText("[@Year$]", DateTime.Now.Year.ToString());    //年
								doc.ReplaceText("[@Month$]", DateTime.Now.Year.ToString().PadLeft(2, '0'));                            //月
								doc.ReplaceText("[@Day$]", DateTime.Now.Year.ToString().PadLeft(2, '0'));                                //日
								doc.ReplaceText("[@BName$]", B_Name);
								doc.ReplaceText("[@L_Name_Ev$]", t.Key.ToString());

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
									String ScoreTotal = string.IsNullOrEmpty(sc.P_ScoreA) ? string.Empty : (Convert.ToInt32(sc.P_ScoreA) + Convert.ToInt32(sc.P_ScoreB) + Convert.ToInt32(sc.P_ScoreC)).ToString();
									doc.ReplaceText("[@St" + m + "$]", ScoreTotal.ToString());
									RemarkTotle += string.IsNullOrEmpty(sc.P_ScoreRemark) ? string.Empty : sc.P_ScoreRemark;
									c++;
								}
								doc.ReplaceText("[@Remark$]", RemarkTotle);
								doc.SaveAs(SavePath + "/" + t.Key.ToString() + "-" + B_Name + "教材審查表.docx");
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


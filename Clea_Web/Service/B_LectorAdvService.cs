using System.Diagnostics.Contracts;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using MathNet.Numerics;
using Novacode;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using X.PagedList;

namespace Clea_Web.Service
{
    //後台講師專區-講師進修資料管理
    public class B_LectorAdvService : BaseService
    {
        private B_LectorAdvViewModel.Modify vm = new B_LectorAdvViewModel.Modify();
        private IConfiguration configuration;


        public B_LectorAdvService(dbContext dbContext, IConfiguration configuration)
        {
            db = dbContext;
            this.configuration = configuration;
        }

        #region Index
        public IPagedList<B_LectorAdvViewModel.schPageList> schPages(B_LectorAdvViewModel.SchItem data, Int32 page, Int32 pagesize)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageLists(data).ToPagedList(page, pagesize);

        }

        public List<B_LectorAdvViewModel.schPageList> GetPageLists(B_LectorAdvViewModel.SchItem data)
        {
            List<B_LectorAdvViewModel.schPageList> result = new List<B_LectorAdvViewModel.schPageList>();


            result = (from la in db.CLectorAdvInfos
                      join l in db.CLectors on la.LUid equals l.LUid
                      group la by new { l.LUid, l.LName, la.LaYear } into grp
                      where
                      (
                      (string.IsNullOrEmpty(data.LaYear) || grp.Key.LaYear.ToString().Contains(data.LaYear)) &&
                      (string.IsNullOrEmpty(data.LName) || grp.Key.LName.Contains(data.LName))
                      )
                      select new B_LectorAdvViewModel.schPageList
                      {
                          //LUid = (from lector in db.CLectors where grp.Key.LName.Equals(lector.LUid) select lector).FirstOrDefault().LName,
                          LUid = grp.Key.LUid.ToString(),
                          LName = grp.Key.LName,
                          LaYear = grp.Key.LaYear,
                          YearNow = DateTime.Now.Year - 1911,
                      }).OrderByDescending(x => x.LaYear).ToList();

            return result;
        }
        #endregion

        #region D_Index
        public IPagedList<B_LectorAdvViewModel.D_PageList> D_schPages(String LUid, int YearNow, Int32 page, Int32 pagesize)
        {
            return D_GetPageLists(LUid, YearNow).ToPagedList(page, pagesize);
        }

        public List<B_LectorAdvViewModel.D_PageList> D_GetPageLists(String LUid, int YearNow)
        {
            List<B_LectorAdvViewModel.D_PageList> result = new List<B_LectorAdvViewModel.D_PageList>();


            result = (from la in db.CLectorAdvInfos
                      join sf in db.SysFiles on la.LaUid equals sf.FMatchKey
                      where (la.LaYear.Equals(YearNow) && la.LUid.ToString() == LUid)
                      select new B_LectorAdvViewModel.D_PageList
                      {
                          LaUid = la.LaUid.ToString(),
                          LUid = la.LUid.ToString(),
                          LaTitle = la.LaTitle,
                          FileName = sf.FFullName,
                      }).ToList();

            return result;
        }
        #endregion

        #region Modify
        public B_LectorAdvViewModel.Modify GetEditData(string LaUid)
        {
            //撈資料
            CLectorAdvInfo la = db.CLectorAdvInfos.Where(x => x.LaUid.ToString() == LaUid).FirstOrDefault();
            SysFile sf = db.SysFiles.Where(x => x.FMatchKey.ToString() == la.LaUid.ToString()).FirstOrDefault();
            CLector l = db.CLectors.Where(x => x.LUid == la.LUid).FirstOrDefault();

            string fileNameDL = sf.FNameDl + "." + sf.FExt;
            string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);

            vm = new B_LectorAdvViewModel.Modify();
            if (la != null && sf != null && l != null)
            {
                vm.LUid = la.LUid;
                vm.LaUid = la.LaUid;
                vm.LaYear = la.LaYear;
                vm.LaTitle = la.LaTitle;
                vm.LName = l.LName;
                vm.FileID = sf.FileId;
                //vm.FNameReal = sf.FNameReal;
                vm.FileName = sf.FFullName;
                vm.FilePath = filePath;
                //vm.FExt = sf.FExt;
                vm.UptDate = la.Upddate == null ? la.Credate.ToShortDateString() : la.Upddate.Value.ToShortDateString();
            }
            return vm;
        }
        #endregion

        #region 匯出Excel
        public Byte[] Export_Excel(String LUid, int YearNow)
        {

            List<CLectorAdvInfo> l = db.CLectorAdvInfos.Where(x => x.LUid == Guid.Parse(LUid) && x.LaYear.Equals(YearNow)).ToList();
            SysUser? su = db.SysUsers.Where(x => x.UId == Guid.Parse(LUid)).FirstOrDefault();


            #region ExportExcel
            using (var exportData = new MemoryStream())
            {
                IWorkbook wb = new XSSFWorkbook();  //字型定義
                ISheet sheet = wb.CreateSheet(YearNow.ToString());
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


                var rowYear = sheet.CreateRow(0);
                rowYear.CreateCell(0).SetCellValue("年度");
                rowYear.CreateCell(1).SetCellValue(YearNow);

                var rowTeacher = sheet.CreateRow(1);
                rowTeacher.CreateCell(0).SetCellValue("講師");
                rowTeacher.CreateCell(1).SetCellValue(su.UName);



                sheet.CreateRow(2).CreateCell(0).SetCellValue("序號");
                //sheet.GetRow(2).GetCell(0).CellStyle = TitleStyle;

                int count = 1;
                int index = 3;



                foreach (var item in l)
                {
                    var row = sheet.CreateRow(index);

                    //序號
                    row.CreateCell(0).SetCellValue(count);
                    //標題
                    row.CreateCell(1).SetCellValue(item.LaTitle);


                    index++;
                    count++;

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
                                    Int32 ScoreTotal = Convert.ToInt32(sc.P_ScoreA) + Convert.ToInt32(sc.P_ScoreB) + Convert.ToInt32(sc.P_ScoreC);
                                    doc.ReplaceText("[@St" + m + "$]", ScoreTotal.ToString());
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

    }
}


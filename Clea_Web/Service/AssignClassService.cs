using System.Collections.Generic;
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

        #region 查詢列表

        #region 課程列表

        /// <summary>
        /// 課程列表
        /// </summary>
        /// <param name="data">查詢條件</param>
        /// <param name="Type">0:課程 1:教材</param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public IPagedList<AssignClassViewModel.GetClassList> GetClassPageList(AssignClassViewModel.SchClassItem data, Int32 page)
        {
            return GetClassLists(data).ToPagedList(page, pagesize);
        }

        public List<AssignClassViewModel.GetClassList> GetClassLists(AssignClassViewModel.SchClassItem data)
        {
            List<AssignClassViewModel.GetClassList> result = new List<AssignClassViewModel.GetClassList>();

            result = (from Class in db.CClasses
                      where
                      (
                      (string.IsNullOrEmpty(data.ClassID) || Class.CId.Contains(data.ClassID)) &&
                      (string.IsNullOrEmpty(data.ClassName) || Class.CName.Contains(data.ClassName)) &&
                      (string.IsNullOrEmpty(data.ClassType) || Class.CType.Contains(data.ClassType)) &&
                      (string.IsNullOrEmpty(data.BookNumber) || Class.CBookNum.Contains(data.BookNumber))
                      )
                      select new AssignClassViewModel.GetClassList
                      {
                          ClassUid = Class.CUid,
                          ClassID = Class.CId,
                          ClassName = Class.CName,
                          ClassType = Class.CType,
                          BookNumber = Class.CBookNum
                      }).OrderBy(x => x.ClassID).ToList();

            return result;
        }
        #endregion

        #region 科目教師列表
        public IPagedList<AssignClassViewModel.GetSubLecList> GetSubLecPageLists(Guid Class_Uid, Int32 Type, AssignClassViewModel.SchSubLecItem data, Int32 page)
        {
            return getSubLecLists(Class_Uid, Type, data).ToPagedList(page, pagesize);
        }

        public List<AssignClassViewModel.GetSubLecList> getSubLecLists(Guid Class_Uid, Int32 Type, AssignClassViewModel.SchSubLecItem data)
        {
            List<AssignClassViewModel.GetSubLecList> result = new List<AssignClassViewModel.GetSubLecList>();

            result = (from CL in db.ViewClassSubLectorPs
                      where
                      (
                      (CL.CKey == Class_Uid) &&
                      (string.IsNullOrEmpty(data.SubID) || CL.SubId.Contains(data.SubID)) &&
                      (string.IsNullOrEmpty(data.SubName) || CL.SubName.Contains(data.SubName)) &&
                      //(string.IsNullOrEmpty(data.SubLectorNumber) || CL.) &&
                      (string.IsNullOrEmpty(data.SubLector) || CL.LName.Contains(data.SubLector))
                      )
                      select new AssignClassViewModel.GetSubLecList
                      {
                          C_UID = CL.CKey.Value,
                          SUB_UID = CL.SubKey,
                          SubID = CL.SubId,
                          SubName = CL.SubName,
                          SubLector = CL.LName
                      }).OrderBy(x => x.SubName).ToList();

            return result;
        }
        #endregion

        #region 授課教師列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="C_UID"></param>
        /// <param name="Sub_UID"></param>
        /// <param name="Type">0:課程 1:教材</param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IPagedList<AssignClassViewModel.GetClassLector> GetClassLectorPageLists(Guid C_UID, Guid Sub_UID, Int32? Type, Int32 page)
        {
            List<AssignClassViewModel.GetClassLector> result = new List<AssignClassViewModel.GetClassLector>();

            result = (from CL in db.CClassLectors
                      join lec in db.CLectors on CL.LUid equals lec.LUid
                      where (CL.CUid == C_UID && CL.DUid == Sub_UID)
                      select new AssignClassViewModel.GetClassLector
                      {
                          CL_UID = CL.ClUid,
                          L_ID = lec.LId,
                          L_Name = lec.LName,
                          IsUpload = (from sysFile in db.SysFiles where sysFile.FMatchKey == CL.ClUid select sysFile).FirstOrDefault() != null ? true : false
                      }).ToList();

            return result.ToPagedList(page, pagesize);
        }
        #endregion

        #region 取得評鑑教師列表
        public IPagedList<AssignClassViewModel.M_EvTeacher> GetEvTeacherPageLists(Guid C_UID, Guid Sub_UID, Int32 mType, Int32 page)
        {
            Int32 LocalYear = DateTime.Now.Year + 1;
            List<AssignClassViewModel.M_EvTeacher> result = new List<AssignClassViewModel.M_EvTeacher>();
            result = (from Ev in db.CEvaluations
                      join lec in db.CLectors on Ev.LUid equals lec.LUid
                      where Ev.LevType == 0 && Ev.LevYear == LocalYear && Ev.LevType == mType && Ev.CUid == C_UID && Ev.DUid == Sub_UID
                      select new AssignClassViewModel.M_EvTeacher()
                      {
                          CEv_UID = Ev.LevId,
                          L_ID_Ev = lec.LId,
                          L_Name_Ev = lec.LName
                      }).ToList();

            return result.ToPagedList(page, pagesize);
        }
        #endregion

        #endregion

        #region 授課資訊
        public AssignClassViewModel.CSTinfo GetCSTInfo(Guid CL_UID, Guid C_UID, Guid Sub_UID, Guid L_UID)
        {
            AssignClassViewModel.CSTinfo result = new AssignClassViewModel.CSTinfo();
            CLector? c_Lector = db.CLectors.Find(L_UID) ?? null;
            AssignClassViewModel.ClassInfo Cinfo = GetClassInfo(C_UID, Sub_UID);

            result.Ev_Year = DateTime.Now.Year + 1;
            result.CL_UID = CL_UID;
            result.L_UID = L_UID;
            result.L_ID = c_Lector != null ? c_Lector.LId : null;
            result.L_Name = c_Lector != null ? c_Lector.LName : null;
            result.C_UID = C_UID;
            result.ClassID = Cinfo != null ? Cinfo.ClassID : null;
            result.ClassName = Cinfo != null ? Cinfo.ClassName : null;
            result.Sub_UID = Sub_UID;
            result.SubID = Cinfo != null ? Cinfo.SubID : null;
            result.SubName = Cinfo != null ? Cinfo.SubName : null;
            return result;
        }
        public AssignClassViewModel.ClassInfo GetClassInfo(Guid C_UID, Guid Sub_UID)
        {
            AssignClassViewModel.ClassInfo result = new AssignClassViewModel.ClassInfo();
            CClass? cClass = db.CClasses.Find(C_UID) ?? null;
            CClassSubject? cClassSubject = db.CClassSubjects.Find(Sub_UID) ?? null;

            result.C_UID = cClass != null ? cClass.CUid : null;
            result.ClassID = cClass != null ? cClass.CId : null;
            result.ClassName = cClass != null ? cClass.CName : null;
            result.SubID = cClassSubject != null ? cClassSubject.DId : null;
            result.SubName = cClassSubject != null ? cClassSubject.DName : null;
            return result;
        }
        #endregion

        #region Modify

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

        #region 指定評鑑存檔
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mType">0:課程 1:教材</param>
        /// <returns></returns>
        public AssignClassViewModel.errorMsg SaveData(AssignClassViewModel.Modify_Model data, Int32 mType)
        {
            AssignClassViewModel.errorMsg result = new BaseViewModel.errorMsg();

            CClassLector? cClassLector = db.CClassLectors.Find(data.cSTinfo.CL_UID) ?? null;

            if (cClassLector != null)
            {
                CEvaluation cEvaluation = new CEvaluation();
                cEvaluation.LevId = Guid.NewGuid();
                cEvaluation.LevYear = DateTime.Now.Year + 1;                
                cEvaluation.LUid = cClassLector.LUid.Value;
                cEvaluation.LUidEv = data.lModify.L_UID_Ev;
                cEvaluation.LevType = mType; // 0:課程 1:教材
                cEvaluation.CUid = cClassLector.CUid.Value;
                cEvaluation.DUid = cClassLector.DUid.Value;
                cEvaluation.ClUid = cClassLector.ClUid;
                cEvaluation.Creuser = Guid.Parse(GetUserID(user));
                cEvaluation.Credate = DateTime.Now;

                db.CEvaluations.Add(cEvaluation);
                result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
            }
            else
            {
                result.CheckMsg = false;
                result.ErrorMsg = "查無授課資訊!";
            }

            return result;
        }
        #endregion

        #endregion

        #region 匯出資料

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

        #endregion

        #region Portal
        #endregion

    }
}


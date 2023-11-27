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
        private FileService _fileservice;


        public B_LectorAdvService(dbContext dbContext, IConfiguration configuration, FileService fileservice)
        {
            db = dbContext;
            this.configuration = configuration;
            _fileservice = fileservice;
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

        #region V_Modify
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

        #region Save
        public BaseViewModel.errorMsg SaveData(B_LectorAdvViewModel.Modify vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                CLectorAdvInfo? cl = db.CLectorAdvInfos.Find(vm.LaUid);

                if (cl is null)
                {
                    cl = new CLectorAdvInfo();
                }
                if (vm.LaTitle != null)
                {
                    cl.LaTitle = vm.LaTitle;
                }
                else
                {
                    result.CheckMsg = false;
                    result.ErrorMsg = "標題為必填";
                    return result;
                }
                cl.LaYear = vm.LaYear;

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    cl.Upduser = Guid.Parse(GetUserID(user));
                    cl.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    cl.LaUid = Guid.NewGuid();
                    if (vm.LUid != null)
                    {
                        cl.LUid = vm.LUid;
                    }
                    else
                    {
                        result.CheckMsg = false;
                        result.ErrorMsg = "講師為必填";
                        return result;
                    }
                    if (vm.LaYear != null)
                    {
                        cl.LaYear = vm.LaYear;

                    }
                    else
                    {
                        result.CheckMsg = false;
                        result.ErrorMsg = "年度為必填";
                        return result;
                    }
                    //cl.LUid = vm.LUid;
                    //cl.LaYear = vm.LaYear;
                    cl.Creuser = Guid.Parse(GetUserID(user));
                    cl.Credate = DateTime.Now;
                    db.CLectorAdvInfos.Add(cl);
                }

                if (vm.IsEdit == false && vm.file == null)
                {
                    result.CheckMsg = false;
                    result.ErrorMsg = "請上傳檔案";
                    return result;
                }
                result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

                if (vm.file == null)
                {
                    result.CheckMsg = true;
                }
                else if (vm.file != null)
                {
                    _fileservice.user = user;
                    result.CheckMsg = _fileservice.UploadAdvFile(cl.LaUid, vm.file);
                    if (result.CheckMsg)
                    {

                    }
                    else
                    {
                        result.CheckMsg = false;
                        result.ErrorMsg = "檔案上傳失敗";
                    }
                }
            }
            catch (Exception e)
            {
                result.ErrorMsg = e.Message;
                //return false;
            }
            return result;

        }
        #endregion

        //#region 匯出Excel
        //public Byte[] Export_Excel(String LUid, int YearNow)
        //{

        //    List<CLectorAdvInfo> l = db.CLectorAdvInfos.Where(x => x.LUid == Guid.Parse(LUid) && x.LaYear.Equals(YearNow)).ToList();
        //    SysUser? su = db.SysUsers.Where(x => x.UId == Guid.Parse(LUid)).FirstOrDefault();


        //    #region ExportExcel
        //    using (var exportData = new MemoryStream())
        //    {
        //        IWorkbook wb = new XSSFWorkbook();  //字型定義
        //        ISheet sheet = wb.CreateSheet(YearNow.ToString());
        //        XSSFCellStyle TitleStyle = (XSSFCellStyle)wb.CreateCellStyle(); //標題字型
        //        TitleStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        //        TitleStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        //        TitleStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        //        TitleStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        //        TitleStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
        //        TitleStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
        //        XSSFFont font = (XSSFFont)wb.CreateFont();
        //        font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
        //        TitleStyle.SetFont(font);

        //        XSSFCellStyle ContentStyle = (XSSFCellStyle)wb.CreateCellStyle();//內容造型
        //        ContentStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        //        ContentStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        //        ContentStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        //        ContentStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        //        ContentStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
        //        ContentStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;


        //        var rowYear = sheet.CreateRow(0);
        //        rowYear.CreateCell(0).SetCellValue("年度");
        //        rowYear.CreateCell(1).SetCellValue(YearNow);

        //        var rowTeacher = sheet.CreateRow(1);
        //        rowTeacher.CreateCell(0).SetCellValue("講師");
        //        rowTeacher.CreateCell(1).SetCellValue(su.UName);



        //        sheet.CreateRow(2).CreateCell(0).SetCellValue("序號");
        //        //sheet.GetRow(2).GetCell(0).CellStyle = TitleStyle;

        //        int count = 1;
        //        int index = 3;



        //        foreach (var item in l)
        //        {
        //            var row = sheet.CreateRow(index);

        //            //序號
        //            row.CreateCell(0).SetCellValue(count);
        //            //標題
        //            row.CreateCell(1).SetCellValue(item.LaTitle);


        //            index++;
        //            count++;

        //        }


        //        wb.Write(exportData, true);

        //        Byte[] result = exportData.ToArray();
        //        return result;
        //    }
        //    #endregion

        //}
        //#endregion

        #region 匯出ZIP
        public Byte[] Export_LectorAnnaulZip(String LUid, int YearNow)
        {
            List<CLectorAdvInfo> l = db.CLectorAdvInfos.Where(x => x.LUid == Guid.Parse(LUid) && x.LaYear.Equals(YearNow)).ToList();
            SysUser? su = db.SysUsers.Where(x => x.UId == Guid.Parse(LUid)).FirstOrDefault();
            List<SysFile> sfList = new List<SysFile>();

            String SavePath = "./SampleFile/Output/" + YearNow.ToString() + "年-" + su.UName + "-進修資料";  //temp資料夾名稱
            String SavePathzip = "./SampleFile/Output/" + YearNow.ToString() + "年-" + su.UName + "-進修資料.zip"; //ZIP名稱
            Directory.CreateDirectory(SavePath);  //建立資料夾

            foreach (var item in l)
            {
                //先抓出所有檔案
                SysFile sf = db.SysFiles.Where(x => x.FMatchKey == item.LaUid).FirstOrDefault();
                if (sf != null)
                {
                    sfList.Add(sf);
                }
            }

            foreach (var item in sfList)
            {
                //將檔案複製至臨時資料夾
                string fileNameDL = item.FNameDl + "." + item.FExt;
                string filePathDL = Path.Combine(configuration.GetValue<String>("FileRootPath"), item.FPath, fileNameDL);
                string destinationFilePath = Path.Combine(SavePath, item.FFullName);

                File.Copy(filePathDL, destinationFilePath, true); //複製檔案到temp資料夾
            }



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


                var rowTitle = sheet.CreateRow(2);
                rowTitle.CreateCell(0).SetCellValue("序號");
                rowTitle.CreateCell(1).SetCellValue("標題");
                rowTitle.CreateCell(2).SetCellValue("檔案名稱");

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
                    //檔案名稱
                    SysFile sf = db.SysFiles.Where(x => x.FMatchKey == item.LaUid).FirstOrDefault();
                    if (sf != null)
                    {
                        row.CreateCell(2).SetCellValue(sf.FFullName);
                    }



                    index++;
                    count++;

                }


                wb.Write(exportData, true);


                #region excel寫入

                string fileName = YearNow.ToString() + "年-" + su.UName + "-進修資料.xlsx"; //匯出EXCEL檔案名稱 
                string filePath = Path.Combine(SavePath, fileName);

                // Write the content of the MemoryStream to a file
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    exportData.WriteTo(fileStream);
                    fileStream.Close();
                }

                #endregion

                //Byte[] result = exportData.ToArray();
                //return result;
            }
            #endregion          


            ZipFile.CreateFromDirectory(SavePath, SavePathzip);
            Byte[] result = System.IO.File.ReadAllBytes(SavePathzip);
            Directory.Delete(SavePath, true);
            File.Delete(SavePathzip);
            return result;
        }
        #endregion

        #region Del
        public BaseViewModel.errorMsg DelData(Guid U_ID)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            CLectorAdvInfo cl = db.CLectorAdvInfos.Find(U_ID);
            vm = new B_LectorAdvViewModel.Modify();

            try
            {
                db.CLectorAdvInfos.Remove(cl);
            }
            catch (Exception e)
            {
                result.ErrorMsg = e.Message;
            }
            result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

            return result;
        }

        #endregion

    }
}


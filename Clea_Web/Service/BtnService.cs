using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using MathNet.Numerics;
using Microsoft.VisualBasic.ApplicationServices;
using NPOI.OpenXmlFormats;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using X.PagedList;
using static Clea_Web.ViewModels.AssignClassViewModel;

namespace Clea_Web.Service
{
    //後臺角色權限管理
    public class BtnService : BaseService
    {
        private BtnViewModel.Modify vm = new BtnViewModel.Modify();
        private FileService _fileservice;
        private IConfiguration configuration;

        public BtnService(dbContext dbContext, FileService fileservice, IConfiguration configuration)
        {
            db = dbContext;
            _fileservice = fileservice;
            this.configuration = configuration;
        }

        #region 查詢
        public IPagedList<BtnViewModel.schPageList> schPages(BtnViewModel.SchItem data, Int32 page, Int32 pagesize)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageLists(data).ToPagedList(page, pagesize);

        }

        public List<BtnViewModel.schPageList> GetPageLists(BtnViewModel.SchItem data)
        {
            BtnViewModel.schPageList model;
            List<BtnViewModel.schPageList> result = new List<BtnViewModel.schPageList>();

            result = (from pn in db.PNews
                          //join user in db.SysUsers on r.Creuser equals user.UName
                      where
                      (
                      ////公告類型、公告標題、開始日期、結束日期
                      (string.IsNullOrEmpty(data.NTitle) || pn.NTitle.Contains(data.NTitle)) &&
                      ((string.IsNullOrEmpty(data.NStartDate.ToString()) || pn.NStartDate > data.NStartDate) &&
                      (string.IsNullOrEmpty(data.NEndDate.ToString()) || pn.NEndDate < data.NEndDate)) &&
                      (string.IsNullOrEmpty(data.NClass) || pn.NType.Equals(data.NClass)) &&
                      pn.NType == "29"
                      )
                      select new BtnViewModel.schPageList
                      {
                          NewsId = pn.NewsId,
                          //NType = (from code in db.SysCodes where code.CParentCode.Equals("btnType") && pn.NType.Equals(code.CItemCode) select code).FirstOrDefault().CItemName,
                          NTitle = pn.NTitle,
                          NClass = pn.NClass,
                          NStartDate = pn.NStartDate,
                          NStartDateStr = pn.NStartDate.ToShortDateString(),
                          NEndDate = pn.NEndDate,
                          NIsTop = pn.NIsTop,
                          NIsShow = pn.NIsShow,
                          NStatus = pn.NStatus,
                          NContent = pn.NContent,
                          NRole = pn.NRole,
                          RId = pn.RId,
                          ViewCount = (from Log in db.PNewsReadLogs where Log.NewsId.Equals(pn.NewsId) select Log).FirstOrDefault().NewsViews,
                          NLevel = (from code in db.SysCodes where code.CParentCode.Equals("MemberLevel") && pn.NLevel.Equals(code.CItemCode) select code).FirstOrDefault().CItemName,
                          NClassName = (from code in db.SysCodes where code.CParentCode.Equals("btn") && pn.NClass.ToString().Equals(code.CItemCode) select code).FirstOrDefault().CItemName,
                          Date = pn.Upddate == null ? pn.Credate : pn.Upddate.Value
                          //creDate = r.Credate.ToShortDateString(),
                          //creUser = r.Creuser,
                          //Upddate = pn.Upddate == null ? pn.Curdate.ToShortDateString() : pn.Upddate.Value.ToShortDateString(),s
                          //Upduser = string.IsNullOrEmpty(pn.Upduser.ToString()) ? pn.Creuser : pn.Upduser
                      }).OrderByDescending(x => x.Date).ToList();

            return result;
        }
        #endregion

        #region 編輯
        public BtnViewModel.Modify GetEditData(string NewsID)
        {
            //撈資料
            BtnViewModel.Modify model = new BtnViewModel.Modify();
            //List<BtnViewModel.Modify> result = new List<BtnViewModel.Modify>();
            var _PNews = db.PNews.Where(x => x.NewsId.ToString() == NewsID).FirstOrDefault();
            List<SysFile> sfList = db.SysFiles.Where(x => x.FMatchKey == Guid.Parse(NewsID)).ToList();

            model = new BtnViewModel.Modify();
            model.NTitle = _PNews.NTitle;
            model.Upddate = _PNews.Upddate;
            model.Upduser = _PNews.Upduser;
            model.Creuser = _PNews.Creuser;
            model.Credate = _PNews.Credate;
            model.NStatus = _PNews.NStatus;
            model.RId = _PNews.RId;
            model.NContent = _PNews.NContent;
            model.NEndDate = _PNews.NEndDate;
            model.NStartDate = _PNews.NStartDate;
            model.NEndDate = _PNews.NEndDate;
            model.NStartDate = _PNews.NStartDate;
            model.NewsId = _PNews.NewsId;
            model.NIsShow = _PNews.NIsShow;
            model.NIsTop = _PNews.NIsTop;
            model.NIsShow = _PNews.NIsShow;
            model.NIsTop = _PNews.NIsTop;
            model.NClass = _PNews.NClass;
            //model.NType = _PNews.NType;
            model.Level = _PNews.NLevel;
            if (sfList != null)
            {
                foreach (var sf in sfList)
                {
                    BtnViewModel.FileModel file = new BtnViewModel.FileModel();
                    file.FileID = sf.FileId;
                    file.FileName = sf.FFullName;
                    string fileNameDL = sf.FNameDl + "." + sf.FExt;
                    string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
                    file.FilePath = filePath;

                    model.fileModels.Add(file);
                }
            }

            return model;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveData(BtnViewModel.Modify vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                PNews? PNews = db.PNews.Find(vm.NewsId);                

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    PNews.NTitle = vm.NTitle;
                    PNews.NIsShow = vm.NIsShow;
                    PNews.NIsTop = vm.NIsTop;
                    PNews.NClass = vm.NClass; //分類
                    PNews.NLevel = vm.Level;
                    PNews.NStartDate = vm.NStartDate.Date;
                    if (vm.NEndDate != null)
                    {
                        PNews.NEndDate = Convert.ToDateTime(vm.NEndDate).Date;
                    }
                    else
                    {
                        PNews.NEndDate = null;
                    }
                    PNews.NStatus = vm.NStatus;
                    PNews.NContent = vm.NContent;
                    PNews.Upduser = Guid.Parse(GetUserID(user));
                    PNews.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    PNews = new PNews();
                    PNews.NewsId = Guid.NewGuid();
                    PNews.NTitle = vm.NTitle;
                    PNews.NIsShow = vm.NIsShow;
                    PNews.NIsTop = vm.NIsTop;
                    PNews.NType = "29"; //模組
                    PNews.NLevel = vm.Level;
                    PNews.NClass = vm.NClass; //分類
                    PNews.NStartDate = vm.NStartDate.Date;
                    if (vm.NEndDate != null)
                    {
                        PNews.NEndDate = Convert.ToDateTime(vm.NEndDate).Date;
                    }
                    else
                    {
                        PNews.NEndDate = null;
                    }
                    PNews.NStatus = vm.NStatus;
                    PNews.NContent = vm.NContent;
                    PNews.Creuser = Guid.Parse(GetUserID(user));
                    PNews.Credate = DateTime.Now;
                    db.PNews.Add(PNews);

                    PNewsReadLog pNewsReadLog = new PNewsReadLog();
                    pNewsReadLog.NewsId = PNews.NewsId;
                    pNewsReadLog.NewsViews = 0;
                    pNewsReadLog.Creuser = Guid.Parse(GetUserID(user));
                    pNewsReadLog.Credate = DateTime.Now;
                    db.PNewsReadLogs.Add(pNewsReadLog);
                }
                result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

                if (vm.file == null)
                {
                    result.CheckMsg = true;
                }
                else if (vm.file != null)
                {
                        _fileservice.user = user;
                        result.CheckMsg = _fileservice.UploadMultFile(PNews.NewsId, vm.file,29);
                        if (result.CheckMsg)
                        {

                        }
                        else
                        {
                            result.CheckMsg = false;
                            result.ErrorMsg = "檔案上傳失敗";
                        }
                        _fileservice.user = user;
                }
            }
            catch (Exception e)
            {
                result.CheckMsg = false;
                result.ErrorMsg = e.Message;
                //return false;
            }
            return result;

        }
        #endregion

        #region 刪除
        public BaseViewModel.errorMsg DelData(Guid NewsId)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            PNews _PNews = db.PNews.Where(x => x.NewsId == NewsId).FirstOrDefault();
            PNewsReadLog _PNewsReadLog = db.PNewsReadLogs.Where(x => x.NewsId.Equals(NewsId)).FirstOrDefault();
            vm = new BtnViewModel.Modify();

            try
            {
                db.PNewsReadLogs.Remove(_PNewsReadLog);
                db.PNews.Remove(_PNews);
            }
            catch (Exception e)
            {
                result.ErrorMsg = e.Message;
            }
            result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

            return result;
        }

        #endregion

        #region 刪除檔案
        public BaseViewModel.errorMsg DelFile(Guid Uid)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            SysFile sf = db.SysFiles.Where(x => x.FileId == Uid).FirstOrDefault();
            //PNews _PNews = db.PNews.Where(x => x.NewsId == NewsId).FirstOrDefault();
            //vm = new BtnViewModel.Modify();

            try
            {
                //db.PNews.Remove(_PNews);
                db.SysFiles.Remove(sf);
            }
            catch (Exception e)
            {
                result.ErrorMsg = e.Message;
            }
            result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

            return result;
        }

        #endregion

        #region 匯出Excel
        public Byte[] ExportExcel(Guid NewsID, String Title, Boolean? Role, String RId)
        {

            PNews pn = db.PNews.Where(x => x.NewsId == NewsID).FirstOrDefault();

            #region ExportExcel
            using (var exportData = new MemoryStream())
            {
                IWorkbook wb = new XSSFWorkbook();  //字型定義
                ISheet sheet = wb.CreateSheet("已讀名單");
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


                var rowTitle = sheet.CreateRow(0);
                rowTitle.CreateCell(0).SetCellValue("序號");
                rowTitle.CreateCell(1).SetCellValue("講師名單");
                rowTitle.CreateCell(2).SetCellValue("是否已讀");
                int count = 1;

                //true=群發，false=個人
                if (Role == true)
                {

                    List<CLector> clList = new List<CLector>();
                    if (pn.RId.ToLower() == "ABD874FC-6C65-4CC1-84A1-92869D599E77".ToLower())
                    {
                        //全部講師
                        clList = db.CLectors.Where(x => x.LActive == true).ToList();
                    }
                    else
                    {
                        //講師(依分類)
                        SysCode sc = db.SysCodes.Where(x => x.Uid == Guid.Parse(pn.RId)).FirstOrDefault();
                        clList = db.CLectors.Where(x => x.LType == sc.CItemCode && x.LActive == true).ToList();
                    }


                    foreach (var item in clList)
                    {

                        var row = sheet.CreateRow(count);
                        //序號
                        row.CreateCell(0).SetCellValue(count);
                        //講師名單
                        row.CreateCell(1).SetCellValue(item.LName);
                        //是否已讀
                        SysUser user = db.SysUsers.Where(x => x.UAccount.Equals(item.LId)).FirstOrDefault();
                        PNewsReadLog pnLog = db.PNewsReadLogs.Where(x => x.NewsId == NewsID && x.Creuser == user.UId).FirstOrDefault();


                        if (pnLog == null)
                        {
                            row.CreateCell(2).SetCellValue("未讀");
                        }
                        else
                        {
                            row.CreateCell(2).SetCellValue("已讀");
                        }

                        count++;

                    }
                }
                else
                {
                    var row = sheet.CreateRow(1);
                    SysUser user = db.SysUsers.Where(x => x.UId == Guid.Parse(RId)).FirstOrDefault();
                    //CLector cl = db.CLectors.Where(x=>x.LId.Equals(user.UAccount)).FirstOrDefault();
                    //序號
                    row.CreateCell(0).SetCellValue(1);
                    //講師名單
                    row.CreateCell(1).SetCellValue(user.UName);
                    //是否已讀
                    PNewsReadLog pnLog = db.PNewsReadLogs.Where(x => x.NewsId == NewsID && x.Creuser == user.UId).FirstOrDefault();

                    if (pnLog == null)
                    {
                        row.CreateCell(2).SetCellValue("未讀");
                    }
                    else
                    {
                        row.CreateCell(2).SetCellValue("已讀");
                    }
                }


                wb.Write(exportData, true);

                Byte[] result = exportData.ToArray();
                return result;
            }
            #endregion

        }
        #endregion
    }

}


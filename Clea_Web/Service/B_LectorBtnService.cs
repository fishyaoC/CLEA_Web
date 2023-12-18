using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using MathNet.Numerics;
using NPOI.OpenXmlFormats;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using X.PagedList;
using static Clea_Web.ViewModels.AssignClassViewModel;

namespace Clea_Web.Service
{
    //後臺角色權限管理
    public class B_LectorBtnService : BaseService
    {
        private B_LectorBtnViewModel.Modify vm = new B_LectorBtnViewModel.Modify();
        private FileService _fileservice;
        private IConfiguration configuration;

        public B_LectorBtnService(dbContext dbContext, FileService fileservice, IConfiguration configuration)
        {
            db = dbContext;
            _fileservice = fileservice;
            this.configuration = configuration;
        }

        #region 查詢
        public IPagedList<B_LectorBtnViewModel.schPageList> schPages(B_LectorBtnViewModel.SchItem data, Int32 page, Int32 pagesize)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageLists(data).ToPagedList(page, pagesize);

        }

        public List<B_LectorBtnViewModel.schPageList> GetPageLists(B_LectorBtnViewModel.SchItem data)
        {
            B_LectorBtnViewModel.schPageList model;
            List<B_LectorBtnViewModel.schPageList> result = new List<B_LectorBtnViewModel.schPageList>();

            result = (from pn in db.PNews
                          //join user in db.SysUsers on r.Creuser equals user.UName
                      where
                      (
                      ////公告類型、公告標題、開始日期、結束日期
                      (string.IsNullOrEmpty(data.NTitle) || pn.NTitle.Contains(data.NTitle)) &&
                      ((string.IsNullOrEmpty(data.NStartDate.ToString()) || pn.NStartDate > data.NStartDate) &&
                      (string.IsNullOrEmpty(data.NEndDate.ToString()) || pn.NEndDate < data.NEndDate)) &&
                      (string.IsNullOrEmpty(data.NType) || pn.NType.Equals(data.NType))
                      )
                      select new B_LectorBtnViewModel.schPageList
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
                          NTypeName = (from code in db.SysCodes where code.CParentCode.Equals("btnType") && pn.NType.Equals(code.CItemCode) select code).FirstOrDefault().CItemName,
                          Date = pn.Upddate == null ? pn.Credate : pn.Upddate.Value
                          //creDate = r.Credate.ToShortDateString(),
                          //creUser = r.Creuser,
                          //Upddate = pn.Upddate == null ? pn.Curdate.ToShortDateString() : pn.Upddate.Value.ToShortDateString(),s
                          //Upduser = string.IsNullOrEmpty(pn.Upduser.ToString()) ? pn.Creuser : pn.Upduser
                      }).OrderByDescending(x => x.Date).ToList();

            return result;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveData(B_LectorBtnViewModel.Modify vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                PNews? PNews = db.PNews.Find(vm.NewsId);

                if (PNews is null)
                {
                    PNews = new PNews();
                }

                PNews.NType = vm.NType;
                PNews.NTitle = vm.NTitle;
                PNews.NClass = vm.NClass;
                PNews.NStartDate = vm.NStartDate.Date;
                if (vm.NEndDate != null)
                {
                    PNews.NEndDate = Convert.ToDateTime(vm.NEndDate).Date;
                }
                else {
                    PNews.NEndDate = null;
                }
                PNews.NIsTop = vm.NIsTop;
                PNews.NIsShow = vm.NIsShow;
                PNews.NStatus = vm.NStatus;
                PNews.NContent = vm.NContent;
                PNews.NRole = vm.NRole;
                if (vm.NRole == true)
                {
                    PNews.RId = vm.GroupID.ToString();
                }
                else
                {
                    PNews.RId = vm.PersonID.ToString();

                }

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    PNews.Upduser = Guid.Parse(GetUserID(user));
                    PNews.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    PNews.NewsId = Guid.NewGuid();
                    PNews.Creuser = Guid.Parse(GetUserID(user));
                    PNews.Credate = DateTime.Now;
                    db.PNews.Add(PNews);
                }
                result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

                if (vm.file == null)
                {
                    result.CheckMsg = true;
                }
                else if (vm.file != null)
                {
                    _fileservice.user = user;
                    result.CheckMsg = _fileservice.UploadNewFile(PNews.NewsId, vm.file);
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

        #region 編輯
        public B_LectorBtnViewModel.Modify GetEditData(string NewsID)
        {
            //撈資料
            B_LectorBtnViewModel.Modify model = new B_LectorBtnViewModel.Modify();
            //List<B_LectorBtnViewModel.Modify> result = new List<B_LectorBtnViewModel.Modify>();
            var _PNews = db.PNews.Where(x => x.NewsId.ToString() == NewsID).FirstOrDefault();
            SysFile sf = db.SysFiles.Where(x => x.FMatchKey == Guid.Parse(NewsID)).FirstOrDefault();

            model = new B_LectorBtnViewModel.Modify();
            model.NTitle = _PNews.NTitle;
            model.Upddate = _PNews.Upddate;
            model.Upduser = _PNews.Upduser;
            model.Creuser = _PNews.Creuser;
            model.Credate = _PNews.Credate;
            model.NStatus = _PNews.NStatus;
            model.RId = _PNews.RId;
            model.NContent = _PNews.NContent;
            model.NClass = _PNews.NClass;
            model.NEndDate = _PNews.NEndDate;
            model.NStartDate = _PNews.NStartDate;
            model.NEndDate = _PNews.NEndDate;
            model.NStartDate = _PNews.NStartDate;
            model.NewsId = _PNews.NewsId;
            model.NIsShow = _PNews.NIsShow;
            model.NIsTop = _PNews.NIsTop;
            model.NIsShow = _PNews.NIsShow;
            model.NIsTop = _PNews.NIsTop;
            model.NType = _PNews.NType;
            model.NRole = _PNews.NRole;
            if (_PNews.NRole == true)
            {
                model.GroupID = db.SysCodes.Where(x => x.CParentCode.Equals("L_Type") && x.Uid == Guid.Parse(_PNews.RId)).Select(x => x.Uid).FirstOrDefault();
            }
            else
            {
                model.PersonID = db.SysUsers.Where(x => x.UId == Guid.Parse(_PNews.RId)).Select(x => x.UId).FirstOrDefault();
            }
            if (sf != null)
            {
                model.FileID = sf.FileId;
                model.FileName = sf.FFullName;
                string fileNameDL = sf.FNameDl + "." + sf.FExt;
                string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
                model.FilePath = filePath;
            }

            return model;
        }
        #endregion

        #region 刪除
        public BaseViewModel.errorMsg DelData(Guid NewsId)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            PNews _PNews = db.PNews.Where(x => x.NewsId == NewsId).FirstOrDefault();
            vm = new B_LectorBtnViewModel.Modify();

            try
            {
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
        public BaseViewModel.errorMsg DelFile(Guid NewsId)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            SysFile sf = db.SysFiles.Where(x => x.FMatchKey == NewsId).FirstOrDefault();
            //PNews _PNews = db.PNews.Where(x => x.NewsId == NewsId).FirstOrDefault();
            //vm = new B_LectorBtnViewModel.Modify();

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


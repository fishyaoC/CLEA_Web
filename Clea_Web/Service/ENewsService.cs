using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.Formula.Functions;
using X.PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Clea_Web.Service
{
    //勞工教育電子報
    public class ENewsService : BaseService
    {
        private ENewsViewModel.Modify vm = new ENewsViewModel.Modify();
        private FileService _fileservice;
        private IConfiguration configuration;

        public ENewsService(dbContext dbContext, FileService fileservice, IConfiguration configuration)
        {
            db = dbContext;
            _fileservice = fileservice;
            this.configuration = configuration;
        }

        #region 查詢
        public IPagedList<ENewsViewModel.schPageList> schPages(ENewsViewModel.SchItem data, Int32 page, Int32 pagesize)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageLists(data).ToPagedList(page, pagesize);

        }

        public List<ENewsViewModel.schPageList> GetPageLists(ENewsViewModel.SchItem data)
        {
            List<ENewsViewModel.schPageList> result = new List<ENewsViewModel.schPageList>();

            result = (from ENews in db.PNews
                      join Logs in db.PNewsReadLogs on ENews.NewsId equals Logs.NewsId
                      where
                      (
                      (ENews.NType == "28")
                      )
                      select new ENewsViewModel.schPageList
                      {
                          Uid = ENews.NewsId.ToString(),
                          Title = ENews.NTitle,
                          StartDate = ENews.NStartDate.ToShortDateString(),
                          Status = ENews.NStatus == true ? "是" : "否",
                          NewsViews = Logs.NewsViews,
                          updDate = ENews.Upddate == null ? ENews.Credate.ToShortDateString() : ENews.Upddate.Value.ToShortDateString(),
                          updUser = (from user in db.SysUsers where (ENews.Upduser == null ? ENews.Creuser : ENews.Upduser).Equals(user.UId) select user).FirstOrDefault().UName,
                          ENewsStartD = ENews.NStartDate
                      }).OrderByDescending(x => x.ENewsStartD).ToList();

            return result;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveData(ENewsViewModel.Modify vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                PNews? pNews = db.PNews.Find(vm.Uid);

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    pNews.NTitle = vm.Title;
                    pNews.NStartDate = vm.StartDate;
                    pNews.NStatus = vm.Status;
                    pNews.NClick = vm.Click;
                    pNews.Upduser = Guid.Parse(GetUserID(user));
                    pNews.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    pNews = new PNews();
                    pNews.NewsId = Guid.NewGuid();
                    pNews.NTitle = vm.Title;
                    pNews.NStartDate = vm.StartDate;
                    pNews.NStatus = vm.Status;
                    pNews.NType = "28";
                    pNews.NIsShow = true;
                    pNews.NIsTop = true;
                    pNews.NClick = vm.Click;
                    pNews.Creuser = Guid.Parse(GetUserID(user));
                    pNews.Credate = DateTime.Now;
                    db.PNews.Add(pNews);

                    PNewsReadLog pNewsReadLog = new PNewsReadLog();
                    pNewsReadLog.NewsId = pNews.NewsId;
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
                    result.CheckMsg = _fileservice.UploadENews(pNews.NewsId, vm.file);
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

        #region 新增/編輯
        public ENewsViewModel.Modify GetEditData(Guid Uid)
        {
            //撈資料
            PNews? pNews = db.PNews.Where(x => x.NewsId.Equals(Uid)).FirstOrDefault();
            vm = new ENewsViewModel.Modify();

            if (pNews != null)
            {
                vm.Uid = pNews.NewsId;
                vm.Title = pNews.NTitle;
                vm.StartDate = pNews.NStartDate;
                vm.Status = pNews.NStatus;
                vm.Click = pNews.NClick;

                SysFile sf = db.SysFiles.Where(x => x.FMatchKey.Equals(pNews.NewsId)).FirstOrDefault();
                if (sf != null)
                {
                    vm.FileID = sf.FileId;
                    vm.FileName = sf.FFullName;
                    string fileNameDL = sf.FNameDl + "." + sf.FExt;
                    string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
                    vm.FilePath = filePath;
                }
                vm.IsEdit = true;
            }
            else
            {
                //新增
                vm.IsEdit = false;
                vm.StartDate = DateTime.Now;
            }
            return vm;
        }
        #endregion

        #region 刪除
        public BaseViewModel.errorMsg DelData(Guid Uid)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            PNews pNews = db.PNews.Find(Uid);
            PNewsReadLog pNewsReadLog = db.PNewsReadLogs.Where(x=>x.NewsId.Equals(Uid)).FirstOrDefault();
            vm = new ENewsViewModel.Modify();

            try
            {
                db.PNewsReadLogs.Remove(pNewsReadLog);
                db.SaveChanges();
                db.PNews.Remove(pNews);
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
    }
}


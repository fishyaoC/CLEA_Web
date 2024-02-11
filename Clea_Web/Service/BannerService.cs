using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.Formula.Functions;
using X.PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Clea_Web.Service
{
    //輪播圖管理
    public class BannerService : BaseService
    {
        private BannerViewModel.Modify vm = new BannerViewModel.Modify();
        private FileService _fileservice;
        private IConfiguration configuration;

        public BannerService(dbContext dbContext, FileService fileservice, IConfiguration configuration)
        {
            db = dbContext;
            _fileservice = fileservice;
            this.configuration = configuration;
        }

        #region 查詢
        public IPagedList<BannerViewModel.schPageList> schPages(BannerViewModel.SchItem data, Int32 page, Int32 pagesize)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageLists(data).ToPagedList(page, pagesize);

        }

        public List<BannerViewModel.schPageList> GetPageLists(BannerViewModel.SchItem data)
        {
            List<BannerViewModel.schPageList> result = new List<BannerViewModel.schPageList>();

            result = (from Banner in db.PBanners
                      where
                      (
                      (string.IsNullOrEmpty(data.BannerName) || Banner.BannerName.Contains(data.BannerName)) &&
                      (Banner.BannerType == 0)
                      )
                      select new BannerViewModel.schPageList
                      {
                          Uid = Banner.BannerId.ToString(),
                          BannerName = Banner.BannerName,
                          BannerURL = Banner.BannerUrl,
                          BannerStart = Banner.BannerStart.HasValue ? Banner.BannerStart.Value.ToShortDateString() : null,
                          BannerEnd = Banner.BannerEnd.HasValue ? Banner.BannerEnd.Value.ToShortDateString() : null,
                          BannerOrder = Banner.BannerOrder,
                          BannerStatus = Banner.BannerStatus == true ? "是" : "否",
                          updDate = Banner.Upddate == null ? Banner.Credate.ToShortDateString() : Banner.Upddate.Value.ToShortDateString(),
                          updUser = (from user in db.SysUsers where (Banner.Upduser == null ? Banner.Creuser : Banner.Upduser).Equals(user.UId) select user).FirstOrDefault().UName,
                          BannerStartD = Banner.BannerStart,
                          //BannerIMG = (from file in db.SysFiles where (Banner.BannerId.Equals(file.FMatchKey)) select file).FirstOrDefault().UName,
                      }).OrderByDescending(x => x.BannerStartD).ToList();

            return result;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveData(BannerViewModel.Modify vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                PBanner? pBanner = db.PBanners.Find(vm.Uid);

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    pBanner.BannerName = vm.Name;
                    pBanner.BannerUrl = vm.Url;
                    pBanner.BannerStart = vm.StartDate;
                    pBanner.BannerEnd = vm.EndDate;
                    pBanner.BannerOrder = vm.Order;
                    pBanner.BannerStatus = vm.Status;
                    pBanner.Upduser = Guid.Parse(GetUserID(user));
                    pBanner.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    pBanner = new PBanner();
                    pBanner.BannerId = Guid.NewGuid();
                    pBanner.BannerName = vm.Name;
                    pBanner.BannerUrl = vm.Url;
                    pBanner.BannerStart = vm.StartDate;
                    pBanner.BannerEnd = vm.EndDate;
                    pBanner.BannerOrder = vm.Order;
                    pBanner.BannerStatus = vm.Status;
                    pBanner.Creuser = Guid.Parse(GetUserID(user));
                    pBanner.Credate = DateTime.Now;
                    db.PBanners.Add(pBanner);
                }
                result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

                if (vm.file == null)
                {
                    result.CheckMsg = true;
                }
                else if (vm.file != null)
                {
                    _fileservice.user = user;
                    result.CheckMsg = _fileservice.UploadBanner(pBanner.BannerId, vm.file);
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
        public BannerViewModel.Modify GetEditData(Guid Uid)
        {
            //撈資料
            PBanner? pBanner = db.PBanners.Where(x => x.BannerId.Equals(Uid)).FirstOrDefault();
            vm = new BannerViewModel.Modify();

            if (pBanner != null)
            {
                vm.Uid = pBanner.BannerId;
                vm.Name = pBanner.BannerName;
                vm.Url = pBanner.BannerUrl;
                vm.StartDate = pBanner.BannerStart;
                vm.EndDate = pBanner.BannerEnd;
                vm.Order = pBanner.BannerOrder;
                vm.Status = pBanner.BannerStatus;
                SysFile sf = db.SysFiles.Where(x => x.FMatchKey.Equals(pBanner.BannerId)).FirstOrDefault();
                if (sf != null)
                {
                    string fileNameDL = sf.FNameDl + "." + sf.FExt;
                    string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                    vm.BannerIMG = Convert.ToBase64String(imageBytes);
                }
                vm.IsEdit = true;
            }
            else
            {
                //新增
                vm.IsEdit = false;
                String filePath = "./SampleFile/1920x680.gif";
                byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                vm.BannerIMG = Convert.ToBase64String(imageBytes);
            }
            return vm;
        }
        #endregion

        #region 刪除
        public BaseViewModel.errorMsg DelData(Guid Uid)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            PBanner pBanner = db.PBanners.Find(Uid);
            vm = new BannerViewModel.Modify();

            try
            {
                db.PBanners.Remove(pBanner);
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


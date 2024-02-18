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
    //收退費標準管理
    public class IntroService : BaseService
    {
        private IntroViewModel.Rate vm = new IntroViewModel.Rate();
        private FileService _fileservice;
        private IConfiguration configuration;

        public IntroService(dbContext dbContext, FileService fileservice, IConfiguration configuration)
        {
            db = dbContext;
            _fileservice = fileservice;
            this.configuration = configuration;
        }

        #region 查詢
        public IPagedList<IntroViewModel.schPageList> schPages(IntroViewModel.SchItem data, Int32 page, Int32 pagesize)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageLists(data).ToPagedList(page, pagesize);

        }

        public List<IntroViewModel.schPageList> GetPageLists(IntroViewModel.SchItem data)
        {
            List<IntroViewModel.schPageList> result = new List<IntroViewModel.schPageList>();

            result = (from pFile in db.PFiles
                      where
                      (
                      (string.IsNullOrEmpty(data.Title) || pFile.FTitle.Contains(data.Title)) &&
                      (pFile.FType == 59)
                      )
                      select new IntroViewModel.schPageList
                      {
                          Uid = pFile.FileId.ToString(),
                          Title = pFile.FTitle,
                          Order = pFile.FOrder,
                          Status = pFile.FStatus == true ? "是" : "否",
                          updDate = pFile.Upddate == null ? pFile.Credate.ToShortDateString() : pFile.Upddate.Value.ToShortDateString(),
                          updUser = (from user in db.SysUsers where (pFile.Upduser == null ? pFile.Creuser : pFile.Upduser).Equals(user.UId) select user).FirstOrDefault().UName,
                          //BannerIMG = (from file in db.SysFiles where (Banner.BannerId.Equals(file.FMatchKey)) select file).FirstOrDefault().UName,
                      }).OrderByDescending(x => x.Order).ToList();

            return result;
        }
        #endregion


        #region 收退費標準 rate

        #region 儲存
        public BaseViewModel.errorMsg SaveData(IntroViewModel.Rate vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                PFile? pFile = db.PFiles.Find(vm.Uid);

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    pFile.FTitle = vm.Title;
                    pFile.FMemo = vm.Memo;
                    pFile.FStatus = vm.Status;
                    pFile.Upduser = Guid.Parse(GetUserID(user));
                    pFile.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    pFile = new PFile();
                    pFile.FileId = Guid.NewGuid();
                    pFile.FTitle = vm.Title;
                    pFile.FType = 58;
                    pFile.FIsTop = false;
                    pFile.FOrder = 0;
                    pFile.FMemo = vm.Memo;
                    pFile.FStatus = vm.Status;
                    pFile.FStatus = vm.Status;
                    pFile.Creuser = Guid.Parse(GetUserID(user));
                    pFile.Credate = DateTime.Now;
                    db.PFiles.Add(pFile);
                }
                result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

                if (vm.file == null)
                {
                    result.CheckMsg = true;
                }
                else if (vm.file != null)
                {
                    _fileservice.user = user;
                    result.CheckMsg = _fileservice.UploadIntro(pFile.FileId, vm.file,58);
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
        public IntroViewModel.Rate GetEditData()
        {
            //撈資料
            PFile? pFile = db.PFiles.Where(x => x.FType.Equals(58)).FirstOrDefault();
            vm = new IntroViewModel.Rate();

            if (pFile != null)
            {
                vm.Uid = pFile.FileId;
                vm.Title = pFile.FTitle;
                vm.Memo = pFile.FMemo;
                vm.Status = pFile.FStatus;
                SysFile sf = db.SysFiles.Where(x => x.FMatchKey.Equals(pFile.FileId)).FirstOrDefault();
                if (sf != null)
                {
                    string fileNameDL = sf.FNameDl + "." + sf.FExt;
                    string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                    vm.IMG = Convert.ToBase64String(imageBytes);
                }
                vm.IsEdit = true;
            }
            else
            {
                //新增
                vm.IsEdit = false;
                String filePath = "./SampleFile/1920x680.gif";
                byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                vm.IMG = Convert.ToBase64String(imageBytes);
            }
            return vm;
        }
        #endregion

        #endregion

        #region 合格場地 greatPlace

        #region 儲存
        public BaseViewModel.errorMsg SaveDataGP(IntroViewModel.Rate vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                PFile? pFile = db.PFiles.Find(vm.Uid);

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    pFile.FTitle = vm.Title;
                    pFile.FMemo = vm.Memo;
                    pFile.FStatus = vm.Status;
                    pFile.FOrder = vm.Order;
                    pFile.Upduser = Guid.Parse(GetUserID(user));
                    pFile.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    pFile = new PFile();
                    pFile.FileId = Guid.NewGuid();
                    pFile.FTitle = vm.Title;
                    pFile.FType = 59;
                    pFile.FIsTop = false;
                    pFile.FOrder = vm.Order;
                    pFile.FMemo = vm.Memo;
                    pFile.FStatus = vm.Status;
                    pFile.FStatus = vm.Status;
                    pFile.Creuser = Guid.Parse(GetUserID(user));
                    pFile.Credate = DateTime.Now;
                    db.PFiles.Add(pFile);
                }
                result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

                if (vm.file == null)
                {
                    result.CheckMsg = true;
                }
                else if (vm.file != null)
                {
                    _fileservice.user = user;
                    result.CheckMsg = _fileservice.UploadIntro(pFile.FileId, vm.file,59);
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
        public IntroViewModel.Rate GetEditDataGP(Guid Uid)
        {
            //撈資料
            PFile? pFile = db.PFiles.Where(x => x.FType.Equals(59) && x.FileId.Equals(Uid)).FirstOrDefault();
            vm = new IntroViewModel.Rate();

            if (pFile != null)
            {
                vm.Uid = pFile.FileId;
                vm.Title = pFile.FTitle;
                vm.Order = pFile.FOrder;
                vm.Memo = pFile.FMemo;
                vm.Status = pFile.FStatus;
                SysFile sf = db.SysFiles.Where(x => x.FMatchKey.Equals(pFile.FileId)).FirstOrDefault();
                if (sf != null)
                {
                    string fileNameDL = sf.FNameDl + "." + sf.FExt;
                    string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                    vm.IMG = Convert.ToBase64String(imageBytes);
                }
                vm.IsEdit = true;
            }
            else
            {
                //新增
                vm.IsEdit = false;
                String filePath = "./SampleFile/1920x680.gif";
                byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                vm.IMG = Convert.ToBase64String(imageBytes);
            }
            return vm;
        }
        #endregion

        #endregion


        #region 刪除
        public BaseViewModel.errorMsg DelData(Guid Uid)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            PFile pFile = db.PFiles.Find(Uid);
            vm = new IntroViewModel.Rate();

            try
            {
                db.PFiles.Remove(pFile);
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


using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.Formula.Functions;
using X.PagedList;
using static Clea_Web.ViewModels.IntroViewModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Clea_Web.Service
{
    //收退費標準管理
    public class IntroService : BaseService
    {

        private IntroViewModel.Rate vm = new IntroViewModel.Rate();
        private List<IntroViewModel.Rate> vmFare = new List<IntroViewModel.Rate>();

        private IntroViewModel.Nav vmNav = new IntroViewModel.Nav();
        private IntroViewModel.ClassInfo vmClassInfo = new IntroViewModel.ClassInfo();
        private IntroViewModel.Env vmEnv = new IntroViewModel.Env();
        private IntroViewModel.PAlbumList pAlbum = new IntroViewModel.PAlbumList();



        private FileService _fileservice;
        private IConfiguration configuration;

        public IntroService(dbContext dbContext, FileService fileservice, IConfiguration configuration)
        {
            db = dbContext;
            _fileservice = fileservice;
            this.configuration = configuration;
        }


        #region 收費標準 fare

        #region 儲存
        public BaseViewModel.errorMsg SaveFareData(IntroViewModel.Rate vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                PFile? pFile = db.PFiles.Find(vm.Uid);
                PFile? pFile2 = db.PFiles.Find(vm.Uid2);


                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    //pFile.FTitle = vm.Title;
                    //pFile.FMemo = vm.Memo;
                    pFile.FStatus = vm.Status;
                    pFile.Upduser = Guid.Parse(GetUserID(user));
                    pFile.Upddate = DateTime.Now;

                    pFile2.FStatus = vm.Status2;
                    pFile2.Upduser = Guid.Parse(GetUserID(user));
                    pFile2.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    //pFile = new PFile();
                    //pFile.FileId = Guid.NewGuid();
                    //pFile.FTitle = vm.Title;
                    //pFile.FType = 59;
                    //pFile.FIsTop = false;
                    //pFile.FOrder = 0;
                    //pFile.FMemo = vm.Memo;
                    //pFile.FStatus = vm.Status;
                    //pFile.FStatus = vm.Status;
                    //pFile.Creuser = Guid.Parse(GetUserID(user));
                    //pFile.Credate = DateTime.Now;
                    //db.PFiles.Add(pFile);
                }
                result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

                if (vm.file == null)
                {
                    result.CheckMsg = true;
                }
                else if (vm.file != null)
                {
                    _fileservice.user = user;
                    result.CheckMsg = _fileservice.UploadIntro(pFile.FileId, vm.file, 58);
                    result.CheckMsg = _fileservice.UploadIntro(pFile2.FileId, vm.file2, 58);

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
        public IntroViewModel.Rate GetEditDataFare()
        {
            //撈資料
            List<PFile?> pFile = db.PFiles.Where(x => x.FType.Equals(58)).ToList();
            vm = new IntroViewModel.Rate();


            if (pFile != null)
            {
                foreach (var item in pFile)
                {
                    if (item.FTitle == "台中")
                    {
                        vm.Uid = item.FileId;
                        vm.Title = item.FTitle;
                        //vm.Memo = item.FMemo;
                        vm.Status = item.FStatus;
                        SysFile sf = db.SysFiles.Where(x => x.FMatchKey.Equals(item.FileId)).FirstOrDefault();
                        if (sf != null)
                        {
                            string fileNameDL = sf.FNameDl + "." + sf.FExt;
                            string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
                            byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                            vm.IMG = Convert.ToBase64String(imageBytes);
                        }
                    }
                    else if (item.FTitle == "彰化") {
                        vm.Uid2 = item.FileId;
                        vm.Title2 = item.FTitle;
                        //vm.Memo = item.FMemo;
                        vm.Status2 = item.FStatus;
                        SysFile sf = db.SysFiles.Where(x => x.FMatchKey.Equals(item.FileId)).FirstOrDefault();
                        if (sf != null)
                        {
                            string fileNameDL = sf.FNameDl + "." + sf.FExt;
                            string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
                            byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                            vm.IMG2 = Convert.ToBase64String(imageBytes);
                        }
                    }

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

        #region 退費標準 rate

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
                    pFile.FType = 59;
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
                    result.CheckMsg = _fileservice.UploadIntro(pFile.FileId, vm.file, 59);
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
            PFile? pFile = db.PFiles.Where(x => x.FType.Equals(59)).FirstOrDefault();
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
                      (pFile.FType == 60)
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
                    pFile.FType = 60;
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
                    result.CheckMsg = _fileservice.UploadIntro(pFile.FileId, vm.file, 60);
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
            PFile? pFile = db.PFiles.Where(x => x.FType.Equals(60) && x.FileId.Equals(Uid)).FirstOrDefault();
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

        #region 本會位置 Nav

        #region 查詢
        public IPagedList<IntroViewModel.schPageList> schPagesNav(IntroViewModel.SchItem data, Int32 page, Int32 pagesize)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageListsNav(data).ToPagedList(page, pagesize);

        }

        public List<IntroViewModel.schPageList> GetPageListsNav(IntroViewModel.SchItem data)
        {
            List<IntroViewModel.schPageList> result = new List<IntroViewModel.schPageList>();

            result = (from pNav in db.PNavs
                      where
                      (
                      (string.IsNullOrEmpty(data.Title) || pNav.NTitle.Contains(data.Title))
                      )
                      select new IntroViewModel.schPageList
                      {
                          Uid = pNav.Uid.ToString(),
                          Title = pNav.NTitle,
                          Order = pNav.NOrder,
                          Status = pNav.NStatus == true ? "是" : "否",
                          updDate = pNav.Upddate == null ? pNav.Credate.ToShortDateString() : pNav.Upddate.Value.ToShortDateString(),
                          updUser = (from user in db.SysUsers where (pNav.Upduser == null ? pNav.Creuser : pNav.Upduser).Equals(user.UId) select user).FirstOrDefault().UName,
                          //BannerIMG = (from file in db.SysFiles where (Banner.BannerId.Equals(file.FMatchKey)) select file).FirstOrDefault().UName,
                      }).OrderByDescending(x => x.Order).ToList();

            return result;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveDataNav(IntroViewModel.Nav vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                PNav? pNav = db.PNavs.Find(vm.Uid);

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    pNav.NTitle = vm.Title;
                    pNav.NAddress = vm.Address;
                    pNav.NPhone = vm.Phone;
                    pNav.NFax = vm.Fax;
                    pNav.NEmbed = vm.Embed;
                    pNav.NMemo = vm.Memo;
                    pNav.NOrder = vm.Order;
                    pNav.NStatus = vm.Status;
                    pNav.Upduser = Guid.Parse(GetUserID(user));
                    pNav.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    pNav = new PNav();
                    pNav.Uid = Guid.NewGuid();
                    pNav.NTitle = vm.Title;
                    pNav.NAddress = vm.Address;
                    pNav.NPhone = vm.Phone;
                    pNav.NFax = vm.Fax;
                    pNav.NEmbed = vm.Embed;
                    pNav.NMemo = vm.Memo;
                    pNav.NOrder = vm.Order;
                    pNav.NStatus = vm.Status;
                    pNav.Creuser = Guid.Parse(GetUserID(user));
                    pNav.Credate = DateTime.Now;
                    db.PNavs.Add(pNav);
                }
                result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

                if (vm.file == null)
                {
                    result.CheckMsg = true;
                }
                else if (vm.file != null)
                {
                    _fileservice.user = user;
                    result.CheckMsg = _fileservice.UploadIntro(pNav.Uid, vm.file, 63);
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
        public IntroViewModel.Nav GetEditDataNav(Guid Uid)
        {
            //撈資料
            PNav? pNav = db.PNavs.Where(x => x.Uid.Equals(Uid)).FirstOrDefault();
            vmNav = new IntroViewModel.Nav();

            if (pNav != null)
            {
                vmNav.Uid = pNav.Uid;
                vmNav.Title = pNav.NTitle;
                vmNav.Order = pNav.NOrder;
                vmNav.Memo = pNav.NMemo;
                vmNav.Embed = pNav.NEmbed;
                vmNav.Status = pNav.NStatus;
                vmNav.Phone = pNav.NPhone;
                vmNav.Address = pNav.NAddress;
                vmNav.Fax = pNav.NFax;
                SysFile sf = db.SysFiles.Where(x => x.FMatchKey.Equals(pNav.Uid)).FirstOrDefault();
                if (sf != null)
                {
                    string fileNameDL = sf.FNameDl + "." + sf.FExt;
                    string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                    vmNav.IMG = Convert.ToBase64String(imageBytes);
                }
                vmNav.IsEdit = true;
            }
            else
            {
                //新增
                vmNav.IsEdit = false;
                String filePath = "./SampleFile/1920x680.gif";
                byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                vmNav.IMG = Convert.ToBase64String(imageBytes);
            }
            return vmNav;
        }
        #endregion

        #region 刪除
        public BaseViewModel.errorMsg DelDataNav(Guid Uid)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            PNav pNav = db.PNavs.Find(Uid);
            vm = new IntroViewModel.Rate();

            try
            {
                db.PNavs.Remove(pNav);
            }
            catch (Exception e)
            {
                result.ErrorMsg = e.Message;
            }
            result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

            return result;
        }

        #endregion

        #endregion

        #region 環境介紹 Env

        #region 查詢
        public IPagedList<IntroViewModel.schPageList> schPagesEnv(IntroViewModel.SchItem data, Int32 page, Int32 pagesize)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageListsEnv(data).ToPagedList(page, pagesize);

        }

        public List<IntroViewModel.schPageList> GetPageListsEnv(IntroViewModel.SchItem data)
        {
            List<IntroViewModel.schPageList> result = new List<IntroViewModel.schPageList>();

            result = (from pList in db.PLists
                      where
                      (
                      (string.IsNullOrEmpty(data.Title) || pList.LTitle.Contains(data.Title)) && pList.LType == 65
                      )
                      select new IntroViewModel.schPageList
                      {
                          Uid = pList.Uid.ToString(),
                          Title = pList.LTitle,
                          Order = pList.LOrder,
                          Status = pList.LStatus == true ? "是" : "否",
                          //IMG = pList.LStatus == true ? "是" : "否",
                          updDate = pList.Upddate == null ? pList.Credate.ToShortDateString() : pList.Upddate.Value.ToShortDateString(),
                          updUser = (from user in db.SysUsers where (pList.Upduser == null ? pList.Creuser : pList.Upduser).Equals(user.UId) select user).FirstOrDefault().UName,
                          //BannerIMG = (from file in db.SysFiles where (Banner.BannerId.Equals(file.FMatchKey)) select file).FirstOrDefault().UName,
                      }).OrderByDescending(x => x.Order).ToList();

            return result;
        }
        #endregion

        #region 新增/編輯
        public IntroViewModel.Env GetEditDataEnv(Guid Uid)
        {
            //撈資料
            PList? pList = db.PLists.Where(x => x.Uid.Equals(Uid)).FirstOrDefault();
            vmEnv = new IntroViewModel.Env();

            if (pList != null)
            {
                vmEnv.Uid = pList.Uid;
                vmEnv.Title = pList.LTitle;
                vmEnv.Memo = pList.LMemo;
                vmEnv.Order = pList.LOrder;
                vmEnv.Status = pList.LStatus;
                List<SysFile> sfList = db.SysFiles.Where(x => x.FMatchKey.Equals(pList.Uid)).ToList();
                if (sfList != null)
                {
                    vmEnv.IMGList = new List<PAlbumList>();
                    foreach (SysFile sf in sfList)
                    {
                        string fileNameDL = sf.FNameDl + "." + sf.FExt;
                        string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
                        byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                        //vmNav.IMG = Convert.ToBase64String(imageBytes);

                        pAlbum = new PAlbumList();
                        pAlbum.Uid = sf.FileId;
                        if (sf.FDescription == "1")
                        {
                            pAlbum.thum = true; //首圖
                        }
                        else
                        {
                            pAlbum.thum = false; //首圖                           
                        }
                        pAlbum.Memo = sf.FRemark; //圖片說明
                        pAlbum.IMG = Convert.ToBase64String(imageBytes);

                        vmEnv.IMGList.Add(pAlbum);
                    }
                }
                vmEnv.IsEdit = true;
            }
            else
            {
                //新增
                vmEnv.IsEdit = false;
                String filePath = "./SampleFile/1920x680.gif";
                byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                //vmEnv.IMGList.Add(Convert.ToBase64String(imageBytes));
            }
            //vmClassInfo.DropDownRegionItem = getRegionItem();
            return vmEnv;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveDataEnv(IntroViewModel.Env vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                PClassInfo? pClassInfo = db.PClassInfos.Find(vm.Uid);

                //if (vm != null && vm.IsEdit == true)
                //{
                //    //編輯
                //    pClassInfo.CName = vm.Name;
                //    pClassInfo.CWork = vm.Work;
                //    pClassInfo.CWorkPlace = vm.WorkPlace;
                //    pClassInfo.CLinelink = vm.LineLink;
                //    pClassInfo.CPhone = vm.Phone;
                //    pClassInfo.CStatus = vm.Status;
                //    pClassInfo.COrder = vm.Order;
                //    pClassInfo.Upduser = Guid.Parse(GetUserID(user));
                //    pClassInfo.Upddate = DateTime.Now;
                //}
                //else if (vm != null && vm.IsEdit == false)
                //{
                //    //新增
                //    pClassInfo = new PClassInfo();
                //    pClassInfo.Uid = Guid.NewGuid();
                //    pClassInfo.CName = vm.Name;
                //    pClassInfo.CWork = vm.Work;
                //    pClassInfo.CWorkPlace = vm.WorkPlace;
                //    pClassInfo.CLinelink = vm.LineLink;
                //    pClassInfo.CPhone = vm.Phone;
                //    pClassInfo.CStatus = vm.Status;
                //    pClassInfo.COrder = vm.Order;
                //    pClassInfo.Creuser = Guid.Parse(GetUserID(user));
                //    pClassInfo.Credate = DateTime.Now;
                //    db.PClassInfos.Add(pClassInfo);
                //}
                result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

                if (vm.file == null)
                {
                    result.CheckMsg = true;
                }
                else if (vm.file != null)
                {
                    _fileservice.user = user;
                    //result.CheckMsg = _fileservice.UploadIntro(pClassInfo.Uid, vm.file, 63);
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

        #region 刪除
        public BaseViewModel.errorMsg DelDataEnv(Guid Uid)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            PClassInfo pClassInfo = db.PClassInfos.Find(Uid);
            vm = new IntroViewModel.Rate();

            try
            {
                db.PClassInfos.Remove(pClassInfo);
            }
            catch (Exception e)
            {
                result.ErrorMsg = e.Message;
            }
            result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

            return result;
        }

        #endregion


        #endregion

        #region 課程及承辦資訊 ClassInfo

        #region 查詢
        public IPagedList<IntroViewModel.schPageList> schPagesClassInfo(IntroViewModel.SchItem data, Int32 page, Int32 pagesize)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageListsClassInfo(data).ToPagedList(page, pagesize);

        }

        public List<IntroViewModel.schPageList> GetPageListsClassInfo(IntroViewModel.SchItem data)
        {
            List<IntroViewModel.schPageList> result = new List<IntroViewModel.schPageList>();

            result = (from pClassInfo in db.PClassInfos
                      where
                      (
                      (string.IsNullOrEmpty(data.Title) || pClassInfo.CName.Contains(data.Title))
                      )
                      select new IntroViewModel.schPageList
                      {
                          Uid = pClassInfo.Uid.ToString(),
                          Title = pClassInfo.CName,
                          Order = pClassInfo.COrder,
                          Status = pClassInfo.CStatus == true ? "是" : "否",
                          updDate = pClassInfo.Upddate == null ? pClassInfo.Credate.ToShortDateString() : pClassInfo.Upddate.Value.ToShortDateString(),
                          updUser = (from user in db.SysUsers where (pClassInfo.Upduser == null ? pClassInfo.Creuser : pClassInfo.Upduser).Equals(user.UId) select user).FirstOrDefault().UName,
                          //BannerIMG = (from file in db.SysFiles where (Banner.BannerId.Equals(file.FMatchKey)) select file).FirstOrDefault().UName,
                      }).OrderByDescending(x => x.Order).ToList();

            return result;
        }
        #endregion

        #region 新增/編輯
        public IntroViewModel.ClassInfo GetEditDataClassInfo(Guid Uid)
        {
            //撈資料
            PClassInfo? pClassInfo = db.PClassInfos.Where(x => x.Uid.Equals(Uid)).FirstOrDefault();
            vmClassInfo = new IntroViewModel.ClassInfo();

            if (pClassInfo != null)
            {
                vmClassInfo.Uid = pClassInfo.Uid;
                vmClassInfo.Name = pClassInfo.CName;
                vmClassInfo.Work = pClassInfo.CWork;
                //vmClassInfo.WorkPlace = db.SysCodes.Where(x=>x.CParentCode.Equals("region") && x.CItemCode.Equals(pClassInfo.CWorkPlace)).FirstOrDefault().CItemName;
                vmClassInfo.WorkPlace = pClassInfo.CWorkPlace;
                //vmClassInfo.Phone = pClassInfo.CPhone;
                vmClassInfo.LineLink = pClassInfo.CLinelink;
                vmClassInfo.Order = pClassInfo.COrder;
                vmClassInfo.Status = pClassInfo.CStatus;
                SysFile sf = db.SysFiles.Where(x => x.FMatchKey.Equals(pClassInfo.Uid)).FirstOrDefault();
                if (sf != null)
                {
                    string fileNameDL = sf.FNameDl + "." + sf.FExt;
                    string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
                    byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                    vmClassInfo.IMG = Convert.ToBase64String(imageBytes);
                }
                vmClassInfo.IsEdit = true;
            }
            else
            {
                //新增
                vmClassInfo.IsEdit = false;
                String filePath = "./SampleFile/1920x680.gif";
                byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                vmClassInfo.IMG = Convert.ToBase64String(imageBytes);
            }
            vmClassInfo.DropDownRegionItem = getRegionItem();
            return vmClassInfo;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveDataClassInfo(IntroViewModel.ClassInfo vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                PClassInfo? pClassInfo = db.PClassInfos.Find(vm.Uid);

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    pClassInfo.CName = vm.Name;
                    pClassInfo.CWork = vm.Work;
                    pClassInfo.CWorkPlace = vm.WorkPlace;
                    pClassInfo.CLinelink = vm.LineLink;
                    //pClassInfo.CPhone = vm.Phone;
                    pClassInfo.CStatus = vm.Status;
                    pClassInfo.COrder = vm.Order;
                    pClassInfo.Upduser = Guid.Parse(GetUserID(user));
                    pClassInfo.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    pClassInfo = new PClassInfo();
                    pClassInfo.Uid = Guid.NewGuid();
                    pClassInfo.CName = vm.Name;
                    pClassInfo.CWork = vm.Work;
                    pClassInfo.CWorkPlace = vm.WorkPlace;
                    pClassInfo.CLinelink = vm.LineLink;
                    //pClassInfo.CPhone = vm.Phone;
                    pClassInfo.CStatus = vm.Status;
                    pClassInfo.COrder = vm.Order;
                    pClassInfo.Creuser = Guid.Parse(GetUserID(user));
                    pClassInfo.Credate = DateTime.Now;
                    db.PClassInfos.Add(pClassInfo);
                }
                result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

                if (vm.file == null)
                {
                    result.CheckMsg = true;
                }
                else if (vm.file != null)
                {
                    _fileservice.user = user;
                    result.CheckMsg = _fileservice.UploadIntro(pClassInfo.Uid, vm.file, 65);
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

        #region 刪除
        public BaseViewModel.errorMsg DelDataClassInfo(Guid Uid)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            PClassInfo pClassInfo = db.PClassInfos.Find(Uid);
            vm = new IntroViewModel.Rate();

            try
            {
                db.PClassInfos.Remove(pClassInfo);
            }
            catch (Exception e)
            {
                result.ErrorMsg = e.Message;
            }
            result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

            return result;
        }

        #endregion

        #endregion


        #region 刪除 pFile
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

        #region 負責地區分類_選單
        public List<SelectListItem> getRegionItem()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
            List<SysCode> lst_cLectors = db.SysCodes.Where(x => x.CParentCode == "region").ToList();
            if (lst_cLectors != null && lst_cLectors.Count() > 0)
            {
                foreach (SysCode L in lst_cLectors)
                {
                    result.Add(new SelectListItem() { Text = L.CItemName, Value = L.CItemCode.ToString() });
                }
            }
            return result;
        }
        #endregion

    }
}


﻿using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.Formula.Functions;
using X.PagedList;
using static Clea_Web.ViewModels.TestInfoViewModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Clea_Web.Service
{
    //考試資訊
    public class TestInfoService : BaseService
    {

        private LinkViewModel.Modify vmLink = new LinkViewModel.Modify();
        private BtnViewModel.Modify vmBtn = new BtnViewModel.Modify();
        private FileDownloadViewModel.Modify vmFile = new FileDownloadViewModel.Modify();
        private IntroViewModel.Rate vmForm = new IntroViewModel.Rate();



        private FileService _fileservice;
        private IConfiguration configuration;

        public TestInfoService(dbContext dbContext, FileService fileservice, IConfiguration configuration)
        {
            db = dbContext;
            _fileservice = fileservice;
            this.configuration = configuration;
        }

        #region 公佈欄

        #region 查詢
        public IPagedList<BtnViewModel.schPageList> schPagesNews(BtnViewModel.SchItem data, Int32 page, Int32 pagesize,int Type)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageLists(data,Type).ToPagedList(page, pagesize);

        }

        public List<BtnViewModel.schPageList> GetPageLists(BtnViewModel.SchItem data,int Type)
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
                      pn.NType == Type.ToString()
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
        public BtnViewModel.Modify GetEditDataNews(string NewsID)
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
            model.NType = Convert.ToInt32(_PNews.NType);
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
        public BaseViewModel.errorMsg SaveDataNews(BtnViewModel.Modify vm)
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
                    PNews.NType = vm.NType.ToString(); //模組
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
                    result.CheckMsg = _fileservice.UploadMultFile(PNews.NewsId, vm.file, Convert.ToInt32(vm.NType));
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

        #endregion

        #region List 標題配條列內容
        #endregion

        #region 表單下載(含下載及範本)

        #region 查詢
        public IPagedList<FileDownloadViewModel.schPageList> schPagesFile(FileDownloadViewModel.SchItem data, Int32 page, Int32 pagesize,int Type)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageListsFile(data,Type).ToPagedList(page, pagesize);

        }

        public List<FileDownloadViewModel.schPageList> GetPageListsFile(FileDownloadViewModel.SchItem data,int Type)
        {
            List<FileDownloadViewModel.schPageList> result = new List<FileDownloadViewModel.schPageList>();

            result = (from pFile in db.PFiles
                          //join Member in db.PMembers on CV.CvCompanyName equals Member.Uid
                      where
                      (
                      (string.IsNullOrEmpty(data.Class) || pFile.FType.Equals(Guid.Parse(data.Class))) &&
                      (string.IsNullOrEmpty(data.Level) || pFile.FLevel.ToString() == data.Level) &&
                      (pFile.FType == Type)
                      )
                      select new FileDownloadViewModel.schPageList
                      {
                          Uid = pFile.FileId.ToString(),
                          Title = pFile.FTitle,
                          Class = (from code in db.SysCodes where code.CParentCode.Equals("fileDownload") && pFile.FClass.Equals(code.CItemCode) select code).FirstOrDefault().CItemName,
                          Level = (from code in db.SysCodes where code.CParentCode.Equals("MemberLevel") && pFile.FLevel.ToString().Equals(code.CItemCode) select code).FirstOrDefault().CItemName,
                          Status = pFile.FStatus == true ? "是" : "否",
                          updDate = pFile.Upddate == null ? pFile.Credate.ToShortDateString() : pFile.Upddate.Value.ToShortDateString(),
                          updUser = (from user in db.SysUsers where (pFile.Upduser == null ? pFile.Creuser : pFile.Upduser).Equals(user.UId) select user).FirstOrDefault().UName,
                          StartD = pFile.Credate,
                      }).OrderByDescending(x => x.StartD).ToList();

            return result;
        }
        #endregion

        #region 新增/編輯
        public FileDownloadViewModel.Modify GetEditDataFile(Guid Uid)
        {
            //撈資料
            PFile? pFile = db.PFiles.Where(x => x.FileId.Equals(Uid)).FirstOrDefault();
            vmFile = new FileDownloadViewModel.Modify();
            List<SysFile> sfList = db.SysFiles.Where(x => x.FMatchKey == Uid).ToList();

            if (pFile != null)
            {
                vmFile.Uid = pFile.FileId;
                vmFile.Title = pFile.FTitle;
                vmFile.Class = pFile.FClass;
                vmFile.ClassID = pFile.FClassId;
                vmFile.Status = pFile.FStatus;
                vmFile.Level = pFile.FLevel;
                vmFile.isTop = pFile.FIsTop;
                vmFile.Memo = pFile.FMemo;
                vmFile.IsEdit = true;
            }
            else
            {
                //新增
                vmFile.IsEdit = false;
            }
            vmFile.DropDownClass = getClassItem();
            vmFile.DropDownClassID = getClassIDItem();
            vmFile.DropDownLevel = getLevelItem();

            if (sfList != null)
            {
                foreach (var sf in sfList)
                {
                    FileDownloadViewModel.FileModel file = new FileDownloadViewModel.FileModel();
                    file.FileID = sf.FileId;
                    file.FileName = sf.FFullName;
                    file.FileMemo = sf.FRemark;
                    string fileNameDL = sf.FNameDl + "." + sf.FExt;
                    string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
                    file.FilePath = filePath;

                    vmFile.fileModels.Add(file);
                }
            }

            return vmFile;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveDataFile(FileDownloadViewModel.Modify vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                PFile? pFile = db.PFiles.Find(vm.Uid);

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    pFile.FTitle = vm.Title;
                    pFile.FClass = vm.Class;
                    pFile.FStatus = vm.Status;
                    pFile.FIsTop = vm.isTop;
                    pFile.FLevel = vm.Level;
                    pFile.FClassId = vm.ClassID;
                    pFile.FMemo = vm.Memo;
                    pFile.Upduser = Guid.Parse(GetUserID(user));
                    pFile.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    pFile = new PFile();
                    pFile.FileId = Guid.NewGuid();
                    pFile.FTitle = vm.Title;
                    pFile.FType = vm.Type;
                    pFile.FClass = vm.Class;
                    pFile.FStatus = vm.Status;
                    pFile.FIsTop = vm.isTop;
                    pFile.FLevel = vm.Level;
                    pFile.FClassId = vm.ClassID;
                    pFile.FMemo = vm.Memo;
                    pFile.Creuser = Guid.Parse(GetUserID(user));
                    pFile.Credate = DateTime.Now;
                    db.PFiles.Add(pFile);
                }
                result.CheckMsg = Convert.ToBoolean(db.SaveChanges());


                //檔案
                if (vm.file1 == null)
                {
                    result.CheckMsg = true;
                }
                else if (vm.file1 != null)
                {
                    _fileservice.user = user;
                    result.CheckMsg = _fileservice.UploadFormFile(pFile.FileId, vm.file1, 38,"1");
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

                //範本
                if (vm.file2 == null)
                {
                    result.CheckMsg = true;
                }
                else if (vm.file2 != null)
                {
                    _fileservice.user = user;
                    result.CheckMsg = _fileservice.UploadFormFile(pFile.FileId, vm.file2, 38, "2");
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
                result.ErrorMsg = e.Message;
                //return false;
            }
            return result;

        }
        #endregion

        #region 會員等級_選單
        public List<SelectListItem> getLevelItem()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
            List<SysCode> lst_cLectors = db.SysCodes.Where(x => x.CParentCode == "MemberLevel").OrderBy(x => x.CItemOrder).ToList();
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

        #region 分類_選單
        public List<SelectListItem> getClassItem()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
            List<SysCode> lst_cLectors = db.SysCodes.Where(x => x.CParentCode == "fileDownload").OrderBy(x => x.CItemOrder).ToList();
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

        #region 課程_選單
        public List<SelectListItem> getClassIDItem()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
            List<PClassMain> lst = db.PClassMains.Where(x => x.CmStatus == true).ToList();
            if (lst != null && lst.Count() > 0)
            {
                foreach (PClassMain L in lst)
                {
                    result.Add(new SelectListItem() { Text = L.CmClassName, Value = L.ClassMainId.ToString() });
                }
            }
            return result;
        }
        #endregion

        #endregion

        #region 相關連結

        #region 查詢
        public IPagedList<LinkViewModel.schPageList> schPages(LinkViewModel.SchItem data, Int32 page, Int32 pagesize, int LType)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageLists(data, LType).ToPagedList(page, pagesize);

        }

        public List<LinkViewModel.schPageList> GetPageLists(LinkViewModel.SchItem data, int LType)
        {
            List<LinkViewModel.schPageList> result = new List<LinkViewModel.schPageList>();

            result = (from pLink in db.PLinks
                      where
                      (
                      (string.IsNullOrEmpty(data.Title) || pLink.LTitle.Contains(data.Title)) &&
                      (string.IsNullOrEmpty(data.Class) || pLink.LClass == data.Class) &&
                      pLink.LType == LType
                      )
                      select new LinkViewModel.schPageList
                      {
                          Uid = pLink.LinkId.ToString(),
                          Class = (from code in db.SysCodes where code.CParentCode.Equals("P_Link") && pLink.LClass.Equals(code.CItemCode) select code).FirstOrDefault().CItemName,
                          Title = pLink.LTitle,
                          Url = pLink.LUrl,
                          Order = pLink.LOrder,
                          Status = pLink.LStatus == true ? "是" : "否",
                          updDate = pLink.Upddate == null ? pLink.Credate.ToShortDateString() : pLink.Upddate.Value.ToShortDateString(),
                          updUser = (from user in db.SysUsers where (pLink.Upduser == null ? pLink.Creuser : pLink.Upduser).Equals(user.UId) select user).FirstOrDefault().UName,
                      }).OrderBy(x => x.Class).ThenBy(x => x.Order).ToList();

            return result;
        }
        #endregion

        #region 新增/編輯
        public LinkViewModel.Modify GetEditData(Guid Uid)
        {
            //撈資料
            PLink? pLink = db.PLinks.Where(x => x.LinkId.Equals(Uid)).FirstOrDefault();
            vmLink = new LinkViewModel.Modify();
            vmLink.getTypeItem = getTypeItem();
            if (pLink != null)
            {
                vmLink.Uid = pLink.LinkId;
                vmLink.Class = pLink.LClass;
                vmLink.Title = pLink.LTitle;
                vmLink.Url = pLink.LUrl;
                vmLink.Order = pLink.LOrder;
                vmLink.Status = pLink.LStatus;
                vmLink.IsEdit = true;
            }
            else
            {
                //新增
                vmLink.IsEdit = false;
            }
            return vmLink;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveData(LinkViewModel.Modify vmLink)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                PLink? pLink = db.PLinks.Find(vmLink.Uid);

                if (pLink is null)
                {
                    pLink = new PLink();
                }

                if (vmLink != null && vmLink.IsEdit == true)
                {
                    //編輯
                    pLink.LClass = vmLink.Class;
                    pLink.LTitle = vmLink.Title;
                    pLink.LOrder = vmLink.Order;
                    pLink.LUrl = vmLink.Url;
                    pLink.LStatus = vmLink.Status;
                    pLink.Upduser = Guid.Parse(GetUserID(user));
                    pLink.Upddate = DateTime.Now;


                }
                else if (vmLink != null && vmLink.IsEdit == false)
                {
                    //新增
                    pLink = new PLink();
                    pLink.LinkId = Guid.NewGuid();
                    pLink.LType = vmLink.LType;
                    pLink.LClass = vmLink.Class;
                    pLink.LTitle = vmLink.Title;
                    pLink.LOrder = vmLink.Order;
                    pLink.LUrl = vmLink.Url;
                    pLink.LStatus = vmLink.Status;
                    pLink.Creuser = Guid.Parse(GetUserID(user));
                    pLink.Credate = DateTime.Now;
                    db.PLinks.Add(pLink);

                }

                result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
            }
            catch (Exception e)
            {
                result.ErrorMsg = e.Message;
                //return false;
            }
            return result;

        }
        #endregion

        #region 相關連結分類_選單
        public List<SelectListItem> getTypeItem()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
            List<SysCode> lst_cLectors = db.SysCodes.Where(x => x.CParentCode == "P_Link").ToList();
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

        #endregion

        #region 簡章上傳
        #region 編輯
        public IntroViewModel.Rate GetEditDataDM(int Type)
        {
            //撈資料
            PFile? pFile = db.PFiles.Where(x => x.FType.Equals(Type)).FirstOrDefault();
            vmForm = new IntroViewModel.Rate();

            if (pFile != null)
            {
                vmForm.Uid = pFile.FileId;
                vmForm.Title = pFile.FTitle;
                //vmForm.Memo = pFile.FMemo;
                //vmForm.Status = pFile.FStatus;
                SysFile sf = db.SysFiles.Where(x => x.FMatchKey.Equals(pFile.FileId)).FirstOrDefault();
                if (sf != null)
                {
                    string fileNameDL = sf.FNameDl + "." + sf.FExt;
                    string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
                    //byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                    //vm.IMG = Convert.ToBase64String(imageBytes);
                    vmForm.FilePath = filePath;
                    vmForm.FileName = fileNameDL;
                    vmForm.FileID = sf.FileId;
                }
                vmForm.IsEdit = true;
            }
            else
            {
                //新增
                vmForm.IsEdit = false;
                //String filePath = "./SampleFile/1920x680.gif";
                //byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                //vm.IMG = Convert.ToBase64String(imageBytes);
            }
            return vmForm;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveDataDM(IntroViewModel.Rate vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                PFile? pFile = db.PFiles.Find(vm.Uid);

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    pFile.FTitle = vm.Title;
                    //pFile.FMemo = vm.Memo;
                    //pFile.FStatus = vm.Status;
                    pFile.Upduser = Guid.Parse(GetUserID(user));
                    pFile.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    pFile = new PFile();
                    pFile.FileId = Guid.NewGuid();
                    pFile.FTitle = vm.Title;
                    pFile.FType = 71;
                    pFile.FIsTop = false;
                    pFile.FOrder = 0;
                    //pFile.FMemo = vm.Memo;
                    pFile.FStatus = true;
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
                    result.CheckMsg = _fileservice.UploadIntro(pFile.FileId, vm.file, 71);
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
        #endregion

        #region 圖片上傳
        #endregion

    }
}


using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using X.PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Clea_Web.Service
{
    //一般會員管理
    public class FileDownloadService : BaseService
    {
        private FileDownloadViewModel.Modify vm = new FileDownloadViewModel.Modify();
        private FileService _fileservice;
        private IConfiguration configuration;

        public FileDownloadService(dbContext dbContext, FileService fileservice, IConfiguration configuration)
        {
            db = dbContext;
            _fileservice = fileservice;
            this.configuration = configuration;
        }

        #region 查詢
        public IPagedList<FileDownloadViewModel.schPageList> schPages(FileDownloadViewModel.SchItem data, Int32 page, Int32 pagesize)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageLists(data).ToPagedList(page, pagesize);

        }

        public List<FileDownloadViewModel.schPageList> GetPageLists(FileDownloadViewModel.SchItem data)
        {
            List<FileDownloadViewModel.schPageList> result = new List<FileDownloadViewModel.schPageList>();

            result = (from pFile in db.PFiles
                          //join Member in db.PMembers on CV.CvCompanyName equals Member.Uid
                      where
                      (
                      (string.IsNullOrEmpty(data.Class) || pFile.FType.Equals(Guid.Parse(data.Class))) &&
                      (string.IsNullOrEmpty(data.Level) || pFile.FLevel.ToString() == data.Level) &&
                      (pFile.FType == 27)
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

        #region 儲存
        public BaseViewModel.errorMsg SaveData(FileDownloadViewModel.Modify vm)
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
                    pFile.Upduser = Guid.Parse(GetUserID(user));
                    pFile.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    pFile = new PFile();
                    pFile.FileId = Guid.NewGuid();
                    pFile.FTitle = vm.Title;
                    pFile.FType = 27;
                    pFile.FClass = vm.Class;
                    pFile.FStatus = vm.Status;
                    pFile.FIsTop = vm.isTop;
                    pFile.FLevel = vm.Level;
                    pFile.FClassId = vm.ClassID;
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
                    result.CheckMsg = _fileservice.UploadMultFile(pFile.FileId, vm.file, 27);
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

        #region 新增/編輯
        public FileDownloadViewModel.Modify GetEditData(Guid Uid)
        {
            //撈資料
            PFile? pFile = db.PFiles.Where(x => x.FileId.Equals(Uid)).FirstOrDefault();
            vm = new FileDownloadViewModel.Modify();
            List<SysFile> sfList = db.SysFiles.Where(x => x.FMatchKey == Uid).ToList();

            if (pFile != null)
            {
                vm.Uid = pFile.FileId;
                vm.Title = pFile.FTitle;
                vm.Class = pFile.FClass;
                vm.ClassID = pFile.FClassId;
                vm.Status = pFile.FStatus;
                vm.Level = pFile.FLevel;
                vm.isTop = pFile.FIsTop;

                vm.IsEdit = true;
            }
            else
            {
                //新增
                vm.IsEdit = false;
            }
            vm.DropDownClass = getClassItem();
            vm.DropDownClassID = getClassIDItem();
            vm.DropDownLevel = getLevelItem();

            if (sfList != null)
            {
                foreach (var sf in sfList)
                {
                    FileDownloadViewModel.FileModel file = new FileDownloadViewModel.FileModel();
                    file.FileID = sf.FileId;
                    file.FileName = sf.FFullName;
                    string fileNameDL = sf.FNameDl + "." + sf.FExt;
                    string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
                    file.FilePath = filePath;

                    vm.fileModels.Add(file);
                }
            }

            return vm;
        }
        #endregion

        #region 刪除
        public BaseViewModel.errorMsg DelData(Guid Uid)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            PFile pFile = db.PFiles.Find(Uid);
            vm = new FileDownloadViewModel.Modify();

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
    }
}


using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using MathNet.Numerics;
using NPOI.HPSF;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.Formula.Functions;
using X.PagedList;

namespace Clea_Web.Service
{
    //前台講師專區-我的進修資料
    public class P_LectorAdvService : BaseService
    {
        private B_LectorAdvViewModel.Modify vm = new B_LectorAdvViewModel.Modify();
        private FileService _fileservice;
        private IConfiguration configuration;

        public P_LectorAdvService(dbContext dbContext, FileService fileservice, IConfiguration configuration)
        {
            db = dbContext;
            _fileservice = fileservice;
            this.configuration = configuration;
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
            Guid userId = Guid.Parse(GetUserID(user));

            result = (from la in db.CLectorAdvInfos
                      join l in db.CLectors on la.LUid equals l.LUid
                      group la by new { l.LUid, l.LName, la.LaYear } into grp
                      where (grp.Key.LUid == userId)
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
                          LaYear = la.LaYear,
                          FileName = sf.FFullName,
                          YearNow = DateTime.Now.Year - 1911,
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
                vm.FilePath = filePath;
                vm.FileName = sf.FFullName;
                //vm.FExt = sf.FExt;
                vm.IsEdit = true;
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
                else {
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
                    cl.LUid = Guid.Parse(GetUserID(user));
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
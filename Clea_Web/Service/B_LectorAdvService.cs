using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using MathNet.Numerics;
using NPOI.HPSF;
using X.PagedList;

namespace Clea_Web.Service
{
    //後台講師專區-講師進修資料管理
    public class B_LectorAdvService : BaseService
    {
        private B_LectorAdvViewModel.Modify vm = new B_LectorAdvViewModel.Modify();

        public B_LectorAdvService(dbContext dbContext)
        {
            db = dbContext;
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
                          LUid = la.LUid.ToString(),
                          LaTitle = la.LaTitle,
                          FileName = sf.FNameReal + sf.FExt,
                      }).ToList();

            return result;
        }
        #endregion

        #region Modify
        public B_LectorAdvViewModel.Modify GetEditData(string LUid)
        {
            //撈資料
            CLectorAdvInfo la = db.CLectorAdvInfos.Where(x => x.LUid.ToString() == LUid).FirstOrDefault();
            SysFile sf = db.SysFiles.Where(x => x.FMatchKey.ToString() == la.LaUid.ToString()).FirstOrDefault();
            CLector l = db.CLectors.Where(x => x.LUid == la.LUid).FirstOrDefault();

            vm = new B_LectorAdvViewModel.Modify();
            if (la != null && sf != null && l != null)
            {
                vm.LUid = la.LUid;
                vm.LaUid = la.LaUid;
                vm.LaYear = la.LaYear;
                vm.LaTitle = la.LaTitle;
                vm.LName = l.LName;
                vm.FileID = sf.FileId;
                vm.FNameReal = sf.FNameReal;
                vm.FilePath = sf.FPath;
                vm.FExt = sf.FExt;
                vm.UptDate = la.Upddate == null ? la.Credate.ToShortDateString() : la.Upddate.Value.ToShortDateString();
            }
            return vm;
        }
        #endregion

    }
}


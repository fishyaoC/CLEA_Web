using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using MathNet.Numerics;
using NPOI.SS.Formula.Functions;
using X.PagedList;

namespace Clea_Web.Service
{
    //後臺角色權限管理
    public class P_LectorBtnService : BaseService
    {
        private P_LectorBtnViewModel.Modify vm = new P_LectorBtnViewModel.Modify();
        private IConfiguration configuration;

        public P_LectorBtnService(dbContext dbContext, IConfiguration configuration)
        {
            db = dbContext;
            this.configuration = configuration;
        }

        #region 查詢
        public IPagedList<P_LectorBtnViewModel.schPageList> schPages(P_LectorBtnViewModel.SchItem data, Int32 page, Int32 pagesize)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageLists(data).ToPagedList(page, pagesize);

        }

        public List<P_LectorBtnViewModel.schPageList> GetPageLists(P_LectorBtnViewModel.SchItem data)
        {
            P_LectorBtnViewModel.schPageList model;
            List<P_LectorBtnViewModel.schPageList> result = new List<P_LectorBtnViewModel.schPageList>();
            //db.PNews.Where(x=>x.NIsShow==true && x.NIsTop==true).ToList().ForEach(x =>
            //{
            //    model = new P_LectorBtnViewModel.schPageList();
            //    model.NTitle = x.NTitle;
            //    model.Upddate = x.Upddate;
            //    model.Upduser = x.Upduser;
            //    model.Creuser = x.Creuser;
            //    model.Credate = x.Credate;
            //    model.NStatus = x.NStatus;
            //    model.RId = x.RId;
            //    model.NContent = x.NContent;
            //    model.NClass = x.NClass;
            //    model.NEndDate = x.NEndDate;
            //    model.NStartDate = x.NStartDate;
            //    model.NewsId = x.NewsId;
            //    model.NIsShow = x.NIsShow;
            //    model.NIsTop = x.NIsTop;
            //    model.NType = x.NType;
            //    model.NTypeName = db.SysCodes.Where(y => y.CParentCode == "btnType" && y.CItemCode == x.NType.ToString()).Select(z => z.CItemName).FirstOrDefault();
            //    model.N_CreateDate = x.Credate.ToShortDateString();
            //    result.Add(model);
            //});

            //現在時間
            DateTime dateTime = DateTime.Now;
            //講師Uid
            Guid userUid = Guid.Parse(GetUserID(user));
            //登入者Uid
            CLector cl = db.CLectors.Where(x => x.LUid == userUid).FirstOrDefault();
            SysCode sc = db.SysCodes.Where(x => x.CParentCode.Equals("L_Type") && x.CItemCode.Equals(cl.LType)).FirstOrDefault();

            result = (from pn in db.PNews
                          //join user in db.SysUsers on r.Creuser equals user.UName
                      where
                      (
                      ////公告類型、公告標題、開始日期、結束日期
                      (pn.NStatus == true) &&
                      (pn.NIsShow == true) &&
                      (pn.NStartDate <= dateTime && pn.NEndDate >= dateTime) &&
                      (pn.RId.ToLower() == userUid.ToString().ToLower() || pn.RId.ToLower() == sc.Uid.ToString().ToLower() || pn.RId.ToLower() == "ABD874FC-6C65-4CC1-84A1-92869D599E77".ToLower()) //ABD874FC-6C65-4CC1-84A1-92869D599E77==全部講師
                      )
                      select new P_LectorBtnViewModel.schPageList
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
                      }).OrderByDescending(x => x.NIsTop).ThenByDescending(x => x.Date).ToList();

            return result;
        }
        #endregion


        #region 編輯
        public P_LectorBtnViewModel.Modify GetEditData(string NewsID)
        {

            //撈資料
            P_LectorBtnViewModel.Modify model = new P_LectorBtnViewModel.Modify();
            var _PNews = db.PNews.Where(x => x.NewsId.ToString() == NewsID).FirstOrDefault();
            SysFile sf = db.SysFiles.Where(x => x.FMatchKey == Guid.Parse(NewsID)).FirstOrDefault();

            //存LOG
            PNewsReadLog P_Log = db.PNewsReadLogs.Where(x => x.NewsId.ToString() == NewsID && x.Creuser.ToString() == GetUserID(user)).FirstOrDefault();
            if (P_Log == null)
            {
                //create
                P_Log = new PNewsReadLog();
                P_Log.NewsId = Guid.Parse(NewsID);
                P_Log.Creuser = Guid.Parse(GetUserID(user));
                P_Log.Credate = DateTime.Now;
                db.PNewsReadLogs.Add(P_Log);
            }
            else
            {
                //update
                P_Log.NewsId = Guid.Parse(NewsID);
                P_Log.Upddate = DateTime.Now;
                P_Log.Upduser = Guid.Parse(GetUserID(user));
            }
            Convert.ToBoolean(db.SaveChanges());
            model = new P_LectorBtnViewModel.Modify();
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
            model.NewsId = _PNews.NewsId;
            model.NIsShow = _PNews.NIsShow;
            model.NIsTop = _PNews.NIsTop;
            model.NType = _PNews.NType;
            model.NTypeName = db.SysCodes.Where(y => y.CParentCode == "btnType" && y.CItemCode == _PNews.NType.ToString()).Select(z => z.CItemName).FirstOrDefault();
            model.N_CreateDate = _PNews.Credate.ToShortDateString();
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
    }
}


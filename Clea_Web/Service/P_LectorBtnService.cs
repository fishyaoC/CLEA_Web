using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using MathNet.Numerics;
using X.PagedList;

namespace Clea_Web.Service
{
    //後臺角色權限管理
    public class P_LectorBtnService : BaseService
    {
        private P_LectorBtnViewModel.Modify vm = new P_LectorBtnViewModel.Modify();


        public P_LectorBtnService(dbContext dbContext)
        {
            db = dbContext;
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
            db.PNews.Where(x=>x.NIsShow==true && x.NIsTop==true).ToList().ForEach(x =>
            {
                model = new P_LectorBtnViewModel.schPageList();
                model.NTitle = x.NTitle;
                model.Upddate = x.Upddate;
                model.Upduser = x.Upduser;
                model.Creuser = x.Creuser;
                model.Curdate = x.Curdate;
                model.NStatus = x.NStatus;
                model.RId = x.RId;
                model.NContent = x.NContent;
                model.NClass = x.NClass;
                model.NEndDate = x.NEndDate;
                model.NStartDate = x.NStartDate;
                model.NewsId = x.NewsId;
                model.NIsShow = x.NIsShow;
                model.NIsTop = x.NIsTop;
                model.NType = x.NType;
                model.NTypeName = db.SysCodes.Where(y => y.CParentCode == "btnType" && y.CItemCode == x.NType.ToString()).Select(z => z.CItemName).FirstOrDefault();
                model.N_CreateDate = x.Curdate.ToShortDateString();
                result.Add(model);
            });

            //result = (from r in db.PNews
            //              //join user in db.SysUsers on r.Creuser equals user.UName
            //          where
            //          (
            //          //
            //          (string.IsNullOrEmpty(data.rId) || r.RId.Contains(data.rId)) &&
            //          (string.IsNullOrEmpty(data.s_Title) || r.NTitle.Contains(data.s_Title)) &&
            //          (string.IsNullOrEmpty(data.s_StartDate.ToString()) || r.NStartDate == data.s_StartDate) &&
            //          (string.IsNullOrEmpty(data.s_type.ToString()) || r.NTitle.Contains(data.s_type.ToString()))
            //          )
            //          select new P_LectorBtnViewModel.schPageList
            //          {
            //              News_ID = r.NewsId.ToString(),
            //              rId = r.RId,
            //              s_Title = r.NTitle,
            //              s_StartDate = r.NStartDate,
            //              s_EndDate = r.NEndDate,
            //              s_type = r.NType,
            //              //creDate = r.Credate.ToShortDateString(),
            //              //creUser = r.Creuser,
            //              updDate = r.Upddate == null ? r.Curdate.ToShortDateString() : r.Upddate.Value.ToShortDateString(),
            //              updUser = string.IsNullOrEmpty(r.Upduser.ToString()) ? r.Creuser : r.Upduser
            //          }).OrderBy(x => x.updDate).ToList();

            return result;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveData(P_LectorBtnViewModel.Modify vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                PNews? PNews = db.PNews.Find(vm.R_ID);

                if (PNews is null)
                {
                    PNews = new PNews();
                }

                PNews.NType = vm.NType;
                PNews.NTitle = vm.NTitle;
                PNews.NClass = vm.N_Class;
                PNews.NStartDate = vm.N_StartDate.Date;
                PNews.NEndDate = vm.N_EndDate.Date;
                PNews.NIsShow = vm.N_IsShow;
                PNews.NStatus = vm.N_Status;
                PNews.NContent = vm.NContent;

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
                    PNews.RId = vm.R_ID.ToString();
                    PNews.Creuser = Guid.Parse(GetUserID(user));
                    PNews.Curdate = DateTime.Now;
                    db.PNews.Add(PNews);
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

        #region 編輯
        public P_LectorBtnViewModel.Modify GetEditData(string NewsID)
        {
           
            //撈資料
            P_LectorBtnViewModel.Modify model;
            //List<P_LectorBtnViewModel.Modify> result = new List<P_LectorBtnViewModel.Modify>();
            var _PNews = db.PNews.Where(x => x.NewsId.ToString() == NewsID).FirstOrDefault();
            var lstisread = db.PNewsReadLogs.Where(x => x.NewsId.ToString() == NewsID).ToList();
            PNewsReadLog P_Log = new PNewsReadLog();
            if (lstisread.Count == 0)
            {
                //P_Log.Sn = 1;
                P_Log.NewsId = _PNews.NewsId;
                P_Log.Creuser = Guid.Parse(GetUserID(user));
                P_Log.Credate = DateTime.Now;
                db.PNewsReadLogs.Add(P_Log);
            }
            else
            {
                P_Log =  db.PNewsReadLogs.Where(x=>x.NewsId== _PNews.NewsId).FirstOrDefault();
                P_Log.Upddate = DateTime.Now;
                P_Log.Upduser = Guid.Parse(GetUserID(user));
            }
            Convert.ToBoolean(db.SaveChanges());
            model = new P_LectorBtnViewModel.Modify();
            model.NTitle = _PNews.NTitle;
            model.Upddate = _PNews.Upddate;
            model.Upduser = _PNews.Upduser;
            model.Creuser = _PNews.Creuser;
            model.Curdate = _PNews.Curdate;
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
            model.N_CreateDate = _PNews.Curdate.ToShortDateString();
            return model;
        }
        #endregion

        #region 刪除
        public BaseViewModel.errorMsg DelData(Guid NewsId)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            PNews _PNews = db.PNews.Find(NewsId);
            vm = new P_LectorBtnViewModel.Modify();

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

    }
    #endregion
}


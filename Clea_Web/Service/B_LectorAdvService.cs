using System.Diagnostics.Contracts;
using System.Threading;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using MathNet.Numerics;
using X.PagedList;

namespace Clea_Web.Service
{
    //後臺角色權限管理
    public class B_LectorAdvService : BaseService
    {
        private B_LectorAdvViewModel.Modify vm = new B_LectorAdvViewModel.Modify();


        public B_LectorAdvService(dbContext dbContext)
        {
            db = dbContext;
        }

        #region 查詢
        public IPagedList<B_LectorAdvViewModel.schPageList> schPages(B_LectorAdvViewModel.SchItem data, Int32 page, Int32 pagesize)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageLists(data).ToPagedList(page, pagesize);

        }

        public List<B_LectorAdvViewModel.schPageList> GetPageLists(B_LectorAdvViewModel.SchItem data)
        {
            B_LectorAdvViewModel.schPageList model;
            List<B_LectorAdvViewModel.schPageList> result = new List<B_LectorAdvViewModel.schPageList>();
            db.CLectorAdvInfos.ToList().ForEach(x =>
            {
                model = new B_LectorAdvViewModel.schPageList();
                model.LaUid = x.LaUid;
                model.LUid = x.LUid;
                model.LaYear = x.LaYear;
                model.LaTitle = x.LaTitle;
                model.Upddate = x.Upddate;
                model.Upduser = x.Upduser;
                model.Creuser = x.Creuser;
                model.Credate = x.Credate;
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
            //          select new B_LectorAdvViewModel.schPageList
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
        public BaseViewModel.errorMsg SaveData(B_LectorAdvViewModel.Modify vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                CLectorAdvInfo? CLectorAdvInfo = db.CLectorAdvInfos.Find(vm.LUid);

                if (CLectorAdvInfo is null)
                {
                    CLectorAdvInfo = new CLectorAdvInfo();
                }

                CLectorAdvInfo.LaUid = vm.LaUid;
                
                CLectorAdvInfo.LaYear = vm.LaYear;
                CLectorAdvInfo.LaTitle = vm.LaTitle;
                CLectorAdvInfo.Credate = vm.Credate;
                CLectorAdvInfo.Creuser = vm.Creuser;
                CLectorAdvInfo.Upddate = vm.Upddate;
                CLectorAdvInfo.Upduser = vm.Upduser;

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    CLectorAdvInfo.Upduser = Guid.Parse(GetUserID(user));
                    CLectorAdvInfo.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    CLectorAdvInfo.LUid = Guid.NewGuid();
                    CLectorAdvInfo.LaUid = Guid.NewGuid();
                    CLectorAdvInfo.Creuser = Guid.Parse(GetUserID(user));
                    CLectorAdvInfo.Credate = DateTime.Now;
                    db.CLectorAdvInfos.Add(CLectorAdvInfo);
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
        public B_LectorAdvViewModel.Modify GetEditData(string Uid)
        {
            //撈資料
            B_LectorAdvViewModel.Modify model;
            //List<B_LectorAdvViewModel.Modify> result = new List<B_LectorAdvViewModel.Modify>();
          var _B_LectorAdv = db.CLectorAdvInfos.Where(x => x.LUid.ToString() == Uid).FirstOrDefault();
            
                model = new B_LectorAdvViewModel.Modify();
            model.LaUid = _B_LectorAdv.LaUid;
            model.LUid = _B_LectorAdv.LUid;
            model.LaYear = _B_LectorAdv.LaYear;
            model.LaTitle = _B_LectorAdv.LaTitle;
            model.Upddate = _B_LectorAdv.Upddate;
            model.Upduser = _B_LectorAdv.Upduser;
            model.Creuser = _B_LectorAdv.Creuser;
            model.Credate = _B_LectorAdv.Credate;
            return model;
        }
        #endregion

        #region 刪除
        public BaseViewModel.errorMsg DelData(Guid UID)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            CLectorAdvInfo _CLectorAdvInfo = db.CLectorAdvInfos.Where(x=>x.LUid== UID).FirstOrDefault();
            vm = new B_LectorAdvViewModel.Modify();

            try
            {
                db.CLectorAdvInfos.Remove(_CLectorAdvInfo);
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


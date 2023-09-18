using Clea_Web.Models;
using Clea_Web.ViewModels;
using System.Diagnostics.Contracts;

namespace Clea_Web.Service
{
    //後臺帳號管理
    public class AccountService : BaseService
    {
        private AccountViewModel.Modify vm = new AccountViewModel.Modify();


        public AccountService(dbContext dbContext)
        {
            db = dbContext;
        }

        #region 查詢
        public List<AccountViewModel.schPageList> GetPageLists(AccountViewModel.SchItem data)
        {
            List<AccountViewModel.schPageList> result = new List<AccountViewModel.schPageList>();


            //result = (from r in db.ViewRoles
            //              //join user in db.SysUsers on r.Creuser equals user.UName
            //          where
            //          (
            //          //
            //          (string.IsNullOrEmpty(data.rId) || r.RId.Contains(data.rId)) &&
            //          (string.IsNullOrEmpty(data.rName) || r.RName.Contains(data.rName)) &&
            //          (string.IsNullOrEmpty(data.rOrder.ToString()) || r.ROrder == data.rOrder) &&
            //          (data.rStatus == null || r.RStatus == data.rStatus)

            //          )
            //          select new AccountViewModel.schPageList
            //          {
            //              rUid = r.RUid.ToString(),
            //              rId = r.RId,
            //              rName = r.RName,
            //              rOrder = r.ROrder,
            //              rStatus = r.RStatus == true ? "是" : "否",
            //              //creDate = r.Credate.ToShortDateString(),
            //              //creUser = r.Creuser,
            //              updDate = r.Upddate == null ? r.Credate.ToShortDateString() : r.Upddate.Value.ToShortDateString(),
            //              updUser = string.IsNullOrEmpty(r.Upduser) ? r.Creuser : r.Upduser
            //          }).OrderBy(x => x.rOrder).ToList();

            return result;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveData(AccountViewModel.Modify vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            //try
            //{
            //    SysRole? userRole = db.SysRoles.Find(vm.RUId);

            //    if (userRole is null)
            //    {
            //        userRole = new SysRole();
            //    }

            //    userRole.RId = vm.RId;
            //    userRole.RName = vm.RName;
            //    userRole.ROrder = Convert.ToByte(vm.ROrder);
            //    userRole.RStatus = vm.RStatus;

            //    if (vm != null && vm.IsEdit == true)
            //    {
            //        //編輯
            //        userRole.Upduser = Guid.Parse(GetUserID(user));
            //        userRole.Upddate = DateTime.Now;
            //    }
            //    else if (vm != null && vm.IsEdit == false)
            //    {
            //        //新增
            //        userRole.RUid = Guid.NewGuid();
            //        userRole.Creuser = Guid.Parse(GetUserID(user));
            //        userRole.Credate = DateTime.Now;
            //        db.SysRoles.Add(userRole);
            //    }

                //result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
            //}
            //catch (Exception e)
            //{
            //    result.ErrorMsg = e.Message;
            //    //return false;
            //}
            return result;

        }
        #endregion

        #region 編輯
        public AccountViewModel.Modify GetEditData(Guid R_UID)
        {
            //撈資料
            SysRole sysRole = db.SysRoles.Where(x => x.RUid.Equals(R_UID)).FirstOrDefault();
            vm = new AccountViewModel.Modify();
            if (sysRole != null)
            {
                //vm.RUId = sysRole.RUid;
                //vm.RId = sysRole.RId;
                //vm.RName = sysRole.RName;
                //vm.ROrder = sysRole.ROrder;
                //vm.RStatus = sysRole.RStatus;
                //vm.IsEdit = true;
            }
            return vm;
        }
        #endregion

        #region 刪除
        public BaseViewModel.errorMsg DelData(Guid R_UID)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            SysRole sysRole = db.SysRoles.Find(R_UID);
            vm = new AccountViewModel.Modify();

            try
            {
                db.SysRoles.Remove(sysRole);
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

    #region 選單
    /// <summary>
    /// 取得單位選單
    /// </summary>
    /// <returns></returns>
    //public List<AccountViewModel.SearchDropDownItem> UnitDropDownList()
    //{
    //    List<AccountViewModel.SearchDropDownItem> result = new List<AccountViewModel.SearchDropDownItem>();
    //        List<SysUnit> lst_Unit = new List<SysUnit>();
    //        lst_Unit = db.SysUnits.Where(x => x.UnStatus == true).ToList();

    //        if (lst_Unit.Count > 0)
    //        {
    //            foreach (SysUnit code in lst_Unit)
    //            {
    //                result.Add(new AccountViewModel.SearchDropDownItem
    //                {
    //                    Text = code.UnName,
    //                    Value = code.UnId
    //                });
    //            }
    //        }

    //        return result;
    //}
    #endregion
}

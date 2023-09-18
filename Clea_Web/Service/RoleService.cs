using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using X.PagedList;

namespace Clea_Web.Service
{
    //後臺帳號管理
    public class RoleService : BaseService
    {
        private UserRoleViewModel.Modify vm = new UserRoleViewModel.Modify();


        public RoleService(dbContext dbContext)
        {
            db = dbContext;
        }

        #region 查詢
        public IPagedList<UserRoleViewModel.schPageList> schPages (UserRoleViewModel.SchItem data, Int32 page, Int32 pagesize) 
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageLists(data).ToPagedList(page, pagesize);

        }

        public List<UserRoleViewModel.schPageList> GetPageLists(UserRoleViewModel.SchItem data)
        {
            List<UserRoleViewModel.schPageList> result = new List<UserRoleViewModel.schPageList>();


            result = (from r in db.ViewRoles
                          //join user in db.SysUsers on r.Creuser equals user.UName
                      where
                      (
                      //
                      (string.IsNullOrEmpty(data.rId) || r.RId.Contains(data.rId)) &&
                      (string.IsNullOrEmpty(data.rName) || r.RName.Contains(data.rName)) &&
                      (string.IsNullOrEmpty(data.rOrder.ToString()) || r.ROrder == data.rOrder) &&
                      (data.rStatus == null || r.RStatus == data.rStatus)
                      )
                      select new UserRoleViewModel.schPageList
                      {
                          rUid = r.RUid.ToString(),
                          rId = r.RId,
                          rName = r.RName,
                          rOrder = r.ROrder,
                          rStatus = r.RStatus == true ? "是" : "否",
                          //creDate = r.Credate.ToShortDateString(),
                          //creUser = r.Creuser,
                          updDate = r.Upddate == null ? r.Credate.ToShortDateString() : r.Upddate.Value.ToShortDateString(),
                          updUser = string.IsNullOrEmpty(r.Upduser) ? r.Creuser : r.Upduser
                      }).OrderBy(x => x.rOrder).ToList();
            
            return result;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveData(UserRoleViewModel.Modify vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                SysRole? userRole = db.SysRoles.Find(vm.RUId);

                if (userRole is null)
                {
                    userRole = new SysRole();
                }

                userRole.RId = vm.RId;
                userRole.RName = vm.RName;
                userRole.ROrder = Convert.ToByte(vm.ROrder);
                userRole.RStatus = vm.RStatus;

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    userRole.Upduser = Guid.Parse(GetUserID(user));
                    userRole.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    userRole.RUid = Guid.NewGuid();
                    userRole.Creuser = Guid.Parse(GetUserID(user));
                    userRole.Credate = DateTime.Now;
                    db.SysRoles.Add(userRole);
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
        public UserRoleViewModel.Modify GetEditData(Guid R_UID)
        {
            //撈資料
            SysRole sysRole = db.SysRoles.Where(x => x.RUid.Equals(R_UID)).FirstOrDefault();
            vm = new UserRoleViewModel.Modify();
            if (sysRole != null)
            {
                vm.RUId = sysRole.RUid;
                vm.RId = sysRole.RId;
                vm.RName = sysRole.RName;
                vm.ROrder = sysRole.ROrder;
                vm.RStatus = sysRole.RStatus;
                vm.IsEdit = true;
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
            vm = new UserRoleViewModel.Modify();

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
}


using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;

namespace Clea_Web.Service
{
    //後臺帳號管理
    public class RoleService : BaseService
    {
        private UserRoleViewModel.Modify vm = new UserRoleViewModel.Modify();

        //public RoleService()
        //{
        //}

        #region 儲存
        public BaseViewModel.errorMsg SaveData(UserRoleViewModel.Modify vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    SysRole? sysRole =  db.SysRoles.Where(x => x.RId.Equals(vm.RId)).FirstOrDefault();
                    if (sysRole != null)
                    {
                        sysRole.RId = vm.RId;
                        sysRole.RName = vm.RName;
                        sysRole.ROrder = Convert.ToByte(vm.ROrder);
                        sysRole.RStatus = vm.RStatus;
                        sysRole.Upduser =Guid.Parse(GetUserID(user));
                        sysRole.Upddate = DateTime.Now;

                        db.SysRoles.Add(sysRole);
                    }
                    result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    SysRole? sysRole = new SysRole();
                    sysRole.RId = vm.RId;
                    sysRole.RName = vm.RName;
                    sysRole.ROrder = Convert.ToByte(vm.ROrder);
                    sysRole.RStatus = vm.RStatus;
                    sysRole.Creuser = Guid.Parse(GetUserID(user));
                    sysRole.Credate = DateTime.Now;

                    db.SysRoles.Add(sysRole);
                    result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

                }
                else
                {
                    return result;
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
    }
}

using Clea_Web.Models;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.Contracts;
using System.Net.NetworkInformation;
using X.PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public IPagedList<AccountViewModel.schPageList> schPages(AccountViewModel.SchItem data, Int32 page, Int32 pagesize)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageLists(data).ToPagedList(page, pagesize);

        }

        public List<AccountViewModel.schPageList> GetPageLists(AccountViewModel.SchItem data)
        {
            List<AccountViewModel.schPageList> result = new List<AccountViewModel.schPageList>();


            result = (from u in db.SysUsers
                          //join role in db.SysRoles on u.RUid equals role.RUid
                      where
                      (
                      //
                      (string.IsNullOrEmpty(data.uuId) || u.UId.ToString().Contains(data.uuId)) &&
                      (string.IsNullOrEmpty(data.urId.ToString()) || u.RUid == data.urId)

                      )
                      select new AccountViewModel.schPageList
                      {
                          uUId = u.UId.ToString(),
                          rUId = (from role in db.SysRoles where u.RUid.Equals(role.RUid) select role).FirstOrDefault().RName,
                          uAccount = u.UAccount,
                          uPassWord = u.UPassword,
                          uName = u.UName,
                          uEmail = u.UEmail,
                          uPhone = u.UPhone,
                          uStatus = u.UStatus == true ? "是" : "否",
                          updDate = (u.Upddate == null ? u.Credate.ToShortDateString() : u.Upddate.Value.ToShortDateString()),
                          updUser = (from user in db.SysUsers where (u.Upduser == null ? u.Creuser : u.Upduser).Equals(user.UId) select user).FirstOrDefault().UName,
                          Date = u.Upddate == null ? u.Credate : u.Upddate.Value
                      }).OrderByDescending(x => x.Date).ToList();

            return result;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveData(AccountViewModel.Modify vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                SysUser? SysUser = db.SysUsers.Find(vm.UId);

                if (SysUser is null)
                {
                    SysUser = new SysUser();
                }
                
                SysUser.RUid = vm.RUid;
                SysUser.UAccount = vm.UAccount;
                SysUser.UPassword = vm.UPassword;
                SysUser.UName = vm.UName;
                SysUser.UEmail = vm.UEmail;
                SysUser.UPhone = vm.UPhone;
                SysUser.UStatus = vm.UStatus;

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    SysUser.Upduser = Guid.Parse(GetUserID(user));
                    SysUser.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    SysUser.RUid = Guid.NewGuid();
                    SysUser.Creuser = Guid.Parse(GetUserID(user));
                    SysUser.Credate = DateTime.Now;
                    db.SysUsers.Add(SysUser);
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
        public AccountViewModel.Modify GetEditData(Guid U_ID)
        {
            //撈資料
            SysUser sysUser = db.SysUsers.Where(x => x.UId.Equals(U_ID)).FirstOrDefault();
            vm = new AccountViewModel.Modify();
            if (sysUser != null)
            {
                vm.UAccount = sysUser.UAccount;
                vm.UPassword = sysUser.UPassword;
                vm.UName = sysUser.UName;
                vm.UEmail = sysUser.UEmail;
                vm.UPhone = sysUser.UPhone;
                vm.UStatus = sysUser.UStatus;
                vm.RUid = sysUser.RUid;
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

        #endregion

        #region 取得權限下拉選單
        public List<SelectListItem> getSysRoleItem()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
            List<SysRole> lst_sysRole = db.SysRoles.ToList();
            if (lst_sysRole != null && lst_sysRole.Count() > 0)
            {
                foreach (SysRole R in lst_sysRole)
                {
                    result.Add(new SelectListItem() { Text = R.RName, Value = R.RUid.ToString() });
                    //result1.Add(new BaseViewModel.SearchDropDownItem() { Text = L.LName, Value = L.LUid.ToString() });
                }
            }
            return result;
        }
        #endregion
    }


}

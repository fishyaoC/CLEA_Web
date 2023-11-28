using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using NPOI.POIFS.Crypt.Dsig;
using X.PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Clea_Web.Service
{
    //後臺角色權限管理
    public class RoleService : BaseService
    {
        private UserRoleViewModel.Modify vm = new UserRoleViewModel.Modify();


        public RoleService(dbContext dbContext)
        {
            db = dbContext;
        }

        #region 查詢
        public IPagedList<UserRoleViewModel.schPageList> schPages(UserRoleViewModel.SchItem data, Int32 page, Int32 pagesize)
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
                          rBackEnd = r.RBackEnd == true ? "是" : "否",
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
                userRole.RBackEnd = vm.RBackEnd;

                userRole.RStatus = vm.RStatus;

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    userRole.Upduser = Guid.Parse(GetUserID(user));
                    userRole.Upddate = DateTime.Now;
                    //權限儲存
                    SysPower sp = new SysPower();
                    foreach (var item in vm.treeViewList)
                    {
                        sp = db.SysPowers.Where(x => x.RUid == vm.RUId && x.MId == item.MID).FirstOrDefault();
                        sp.CreateData = item.CreateData;
                        sp.SearchData = item.SearchData;
                        sp.ModifyData = item.ModifyData;
                        sp.DeleteData = item.DeleteData;
                        sp.ImportData = item.ImportData;
                        sp.Exportdata = item.Exportdata;
                        sp.Upduser = Guid.Parse(GetUserID(user));
                        sp.Upddate = DateTime.Now;
                    }

                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    userRole.RUid = Guid.NewGuid();
                    userRole.Creuser = Guid.Parse(GetUserID(user));
                    userRole.Credate = DateTime.Now;
                    db.SysRoles.Add(userRole);

                    //權限儲存
                    SysPower sp = new SysPower();
                    foreach (var item in vm.treeViewList)
                    {

                        sp.RUid = vm.RUId;
                        sp.MId = Convert.ToInt32(item.MID);
                        sp.CreateData = item.CreateData;
                        sp.SearchData = item.SearchData;
                        sp.ModifyData = item.ModifyData;
                        sp.DeleteData = item.DeleteData;
                        sp.ImportData = item.ImportData;
                        sp.Exportdata = item.Exportdata;
                        sp.Creuser = Guid.Parse(GetUserID(user));
                        sp.Credate = DateTime.Now;

                        db.SysPowers.Add(sp);

                    }
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
                vm.RBackEnd = sysRole.RBackEnd;
                vm.IsEdit = true;
            }

            vm.treeViewList = new List<UserRoleViewModel.treeView>();

            if (R_UID != null && R_UID.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                //編輯

                var query = (from sp in db.SysPowers
                             join sm in db.SysMenus on sp.MId equals sm.MId
                             where sp.RUid == R_UID && sm.MIsActice == true && sm.MIsShow == true
                             orderby sm.MType, sm.MOrder, sp.MId
                             select new
                             {
                                 sp.Sn,
                                 sp.RUid,
                                 sp.MId,
                                 sm.MName,
                                 sm.MType,
                                 sm.MParentId,
                                 sm.MLevel,
                                 sm.MUrl,
                                 sm.MOrder,
                                 sp.CreateData,
                                 sp.SearchData,
                                 sp.ModifyData,
                                 sp.DeleteData,
                                 sp.ImportData,
                                 sp.Exportdata
                             }).ToList();

                foreach (var item in query)
                {
                    UserRoleViewModel.treeView treeView = new UserRoleViewModel.treeView();
                    treeView.RUid = item.RUid;
                    treeView.MID = item.MId;
                    treeView.MName = item.MName;
                    treeView.MType = item.MType;
                    treeView.MParentID = item.MParentId;
                    treeView.MLevel = item.MLevel;
                    treeView.MUrl = item.MUrl;
                    treeView.MOrder = item.MOrder;
                    treeView.CreateData = item.CreateData;
                    treeView.SearchData = item.SearchData;
                    treeView.ModifyData = item.ModifyData;
                    treeView.DeleteData = item.DeleteData;
                    treeView.ImportData = item.ImportData;
                    treeView.Exportdata = item.Exportdata;

                    vm.treeViewList.Add(treeView);

                }
            }
            else
            {
                //新增
                List<SysMenu> sysMenu = db.SysMenus.Where(x => x.MIsActice == true && x.MIsShow == true).OrderBy(x => x.MType).ThenBy(x => x.MId).ThenBy(x => x.MOrder).ToList();
                foreach (var item in sysMenu)
                {
                    UserRoleViewModel.treeView treeView = new UserRoleViewModel.treeView();
                    treeView.RUid = new Guid();
                    treeView.MID = item.MId;
                    treeView.MName = item.MName;
                    treeView.MType = item.MType;
                    treeView.MParentID = item.MParentId;
                    treeView.MLevel = item.MLevel;
                    treeView.MUrl = item.MUrl;
                    treeView.MOrder = item.MOrder;
                    //treeView.CreateData = item.CreateData;
                    //treeView.SearchData = item.SearchData;
                    //treeView.ModifyData = item.ModifyData;
                    //treeView.DeleteData = item.DeleteData;
                    //treeView.ImportData = item.ImportData;
                    //treeView.Exportdata = item.Exportdata;

                    vm.treeViewList.Add(treeView);

                }

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
            SysPower sysPower = db.SysPowers.Find(R_UID);
            vm = new UserRoleViewModel.Modify();

            try
            {
                db.SysRoles.Remove(sysRole);
                db.SysPowers.Remove(sysPower);

            }
            catch (Exception e)
            {
                result.ErrorMsg = e.Message;
            }
            result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

            return result;
        }

        #endregion

        //#region userPower
        //public UserRoleViewModel.Modify GetUserPower(Guid R_UID)
        //{


        //    UserRoleViewModel.Modify vm = new UserRoleViewModel.Modify();
        //    vm.treeViewList = new List<UserRoleViewModel.treeView>();

        //    if (R_UID != null && R_UID.ToString() != "00000000-0000-0000-0000-000000000000")
        //    {
        //        //編輯

        //        var query = (from sp in db.SysPowers
        //                     join sm in db.SysMenus on sp.MId equals sm.MId
        //                     where sp.RUid == R_UID && sm.MIsActice == true && sm.MIsShow == true
        //                     orderby sm.MType, sp.MId, sm.MOrder
        //                     select new
        //                     {
        //                         sp.Sn,
        //                         sp.RUid,
        //                         sp.MId,
        //                         sm.MName,
        //                         sm.MType,
        //                         sm.MParentId,
        //                         sm.MLevel,
        //                         sm.MUrl,
        //                         sm.MOrder,
        //                         sp.CreateData,
        //                         sp.SearchData,
        //                         sp.ModifyData,
        //                         sp.DeleteData,
        //                         sp.ImportData,
        //                         sp.Exportdata
        //                     }).ToList();

        //        foreach (var item in query)
        //        {
        //            UserRoleViewModel.treeView treeView = new UserRoleViewModel.treeView();
        //            treeView.RUid = item.RUid;
        //            treeView.MID = item.MId;
        //            treeView.MName = item.MName;
        //            treeView.MType = item.MType;
        //            treeView.MParentID = item.MParentId;
        //            treeView.MLevel = item.MLevel;
        //            treeView.MUrl = item.MUrl;
        //            treeView.MOrder = item.MOrder;
        //            treeView.CreateData = item.CreateData;
        //            treeView.SearchData = item.SearchData;
        //            treeView.ModifyData = item.ModifyData;
        //            treeView.DeleteData = item.DeleteData;
        //            treeView.ImportData = item.ImportData;
        //            treeView.Exportdata = item.Exportdata;

        //            vm.treeViewList.Add(treeView);

        //        }
        //    }
        //    else
        //    {
        //        //新增
        //        List<SysMenu> sysMenu = db.SysMenus.Where(x => x.MIsActice == true && x.MIsShow == true).OrderBy(x=> x.MType).ThenBy(x=>x.MId).ThenBy(x => x.MOrder).ToList();
        //        foreach (var item in sysMenu)
        //        {
        //            UserRoleViewModel.treeView treeView = new UserRoleViewModel.treeView();
        //            treeView.RUid = new Guid();
        //            treeView.MID = item.MId;
        //            treeView.MName = item.MName;
        //            treeView.MType = item.MType;
        //            treeView.MParentID = item.MParentId;
        //            treeView.MLevel = item.MLevel;
        //            treeView.MUrl = item.MUrl;
        //            treeView.MOrder = item.MOrder;
        //            //treeView.CreateData = item.CreateData;
        //            //treeView.SearchData = item.SearchData;
        //            //treeView.ModifyData = item.ModifyData;
        //            //treeView.DeleteData = item.DeleteData;
        //            //treeView.ImportData = item.ImportData;
        //            //treeView.Exportdata = item.Exportdata;

        //            vm.treeViewList.Add(treeView);

        //        }

        //    }


        //    return vm;
        //}
        //#endregion
    }
}


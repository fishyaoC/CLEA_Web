using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.Formula.Functions;
using X.PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Clea_Web.Service
{
    //後臺角色權限管理
    public class CodeService : BaseService
    {
        private SysCodeViewModel.Modify vm = new SysCodeViewModel.Modify();


        public CodeService(dbContext dbContext)
        {
            db = dbContext;
        }

        #region 查詢
        public IPagedList<SysCodeViewModel.schPageList> schPages(SysCodeViewModel.SchItem data, Int32 page, Int32 pagesize)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageLists(data).ToPagedList(page, pagesize);

        }

        public List<SysCodeViewModel.schPageList> GetPageLists(SysCodeViewModel.SchItem data)
        {
            List<SysCodeViewModel.schPageList> result = new List<SysCodeViewModel.schPageList>();


            result = (from c in db.SysCodes
                      where
                      (
                      (string.IsNullOrEmpty(data.itemName) || c.CItemName.Contains(data.itemName)) &&
                      (string.IsNullOrEmpty(data.itemCode) || c.CItemCode.Contains(data.itemCode)) &&
                      //(c.IsActive == data.isActive) &&
                      //(c.IsShow == data.isShow) 
                       (c.IsEdit == true)

                      )
                      select new SysCodeViewModel.schPageList
                      {
                          Uid = c.Uid.ToString(),
                          //cParentUid = c.CParentUid.ToString(),
                          cParentCode = c.CItemCode,
                          itemName = c.CItemName,
                          itemOrder = c.CItemOrder,
                          isActive = c.IsActive == true ? "是" : "否",
                          isShow = c.IsShow == true ? "是" : "否",
                          isEdit = c.IsEdit == true ? "是" : "否",
                          //creDate = r.Credate.ToShortDateString(),
                          //creUser = r.Creuser,
                          updDate = c.Upddate == null ? c.Credate.ToShortDateString() : c.Upddate.Value.ToShortDateString(),
                          updUser = (from user in db.SysUsers where (c.Upduser == null ? c.Creuser : c.Upduser).Equals(user.UId) select user).FirstOrDefault().UName,
                      }).OrderBy(x => x.itemOrder).ToList();

            return result;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveData(SysCodeViewModel.Modify vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            //try
            //{
            //    SysCode? sysCode = db.SysCodes.Find(vm.Uid);

            //    if (sysCode is null)
            //    {
            //        sysCode = new SysCode();
            //    }

            //    userRole.RId = vm.RId;
            //    userRole.RName = vm.RName;
            //    userRole.ROrder = Convert.ToByte(vm.ROrder);
            //    //userRole.RBackEnd = vm.RBackEnd;

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
            //        userRole.RBackEnd = vm.IsBack;
            //        userRole.Creuser = Guid.Parse(GetUserID(user));
            //        userRole.Credate = DateTime.Now;
            //        db.SysRoles.Add(userRole);

            //    }

            //    result.CheckMsg = Convert.ToBoolean(db.SaveChanges());
            //}
            //catch (Exception e)
            //{
            //    result.ErrorMsg = e.Message;
            //    //return false;
            //}
            return result;

        }
        #endregion

        #region 新增/編輯
        public SysCodeViewModel.Modify GetEditData(Guid Uid)
        {
            //撈資料
            SysCode? sysCode = db.SysCodes.Where(x => x.Uid.Equals(Uid)).FirstOrDefault();
            vm = new SysCodeViewModel.Modify();
            if (sysCode != null)
            {
                vm.Uid = sysCode.Uid;
                //vm.CParentUid = 
                //vm.CParentCode = 
                vm.CItemCode = sysCode.CItemCode;
                vm.CItemName = sysCode.CItemName;
                //vm.CItemOrder = 
                vm.IsActive = sysCode.IsActive;
                vm.IsShow = sysCode.IsShow;
                vm.IsEdit = true;
                List<SysCode> scList = db.SysCodes.Where(x=>x.CParentUid.Equals(Uid)).OrderBy(x=>x.CItemOrder).ToList();
                vm.modifies = scList;

            }
            else
            {
                //新增
                vm.IsEdit = false;
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
            List<SysPower> sysPower = db.SysPowers.Where(x => x.RUid == R_UID).ToList();
            vm = new SysCodeViewModel.Modify();

            try
            {
                db.SysPowers.RemoveRange(sysPower);
                db.SaveChanges();
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

    }
}


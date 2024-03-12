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
            try
            {
                SysCode? sysCode = db.SysCodes.Find(vm.Uid);

                if (sysCode is null)
                {
                    sysCode = new SysCode();
                }



                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    sysCode.CItemName = vm.CItemName;
                    sysCode.IsActive = vm.IsActive;
                    sysCode.IsShow = vm.IsShow;
                    sysCode.Upduser = Guid.Parse(GetUserID(user));
                    sysCode.Upddate = DateTime.Now;

                    int index = 1;
                    foreach (var item in vm.modifies)
                    {
                        SysCode sc = db.SysCodes.Find(item.Uid);

                        if (sc != null)
                        {
                            //edit
                            //sysCodeChild.Uid = Guid.NewGuid();
                            //sysCodeChild.CParentUid = vm.Uid;
                            //sysCodeChild.CParentCode = vm.CItemCode;
                            sc.CItemOrder = index;
                            //sysCode.CItemCode = index.ToString();
                            sc.CItemName = item.CItemName;
                            //sysCodeChild.IsShow = true;
                            //sysCodeChild.IsActive = true;
                            sc.Upduser = Guid.Parse(GetUserID(user));
                            sc.Upddate = DateTime.Now;
                            //db.SysCodes.Add(sc);
                            db.SaveChanges();
                        }
                        else
                        {
                            //create
                            SysCode sysCodeChild = new SysCode();
                            sysCodeChild.Uid = Guid.NewGuid();
                            sysCodeChild.CParentUid = vm.Uid;
                            sysCodeChild.CParentCode = sysCode.CItemCode;
                            sysCodeChild.CItemOrder = index;
                            sysCodeChild.CItemCode = index.ToString();
                            sysCodeChild.CItemName = item.CItemName;
                            sysCodeChild.IsShow = true;
                            sysCodeChild.IsActive = true;
                            sysCodeChild.Creuser = Guid.Parse(GetUserID(user));
                            sysCodeChild.Credate = DateTime.Now;
                            db.SysCodes.Add(sysCodeChild);
                            db.SaveChanges();


                        }

                        index++;
                    }

                    result.CheckMsg = true;
                    return result;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    sysCode.Uid = Guid.NewGuid();
                    sysCode.CItemName = vm.CItemName;
                    sysCode.CItemCode = vm.CItemCode;
                    sysCode.IsActive = vm.IsActive;
                    sysCode.IsShow = vm.IsShow;
                    sysCode.IsEdit = true;
                    sysCode.Creuser = Guid.Parse(GetUserID(user));
                    sysCode.Credate = DateTime.Now;
                    db.SysCodes.Add(sysCode);

                    //處理子項目
                    int index = 1;
                    List<SysCode> sysCodeList = new List<SysCode>();
                    foreach (var item in vm.modifies)
                    {

                        SysCode sysCodeChild = new SysCode();
                        sysCodeChild.Uid = Guid.NewGuid();
                        sysCodeChild.CParentUid = sysCode.Uid;
                        sysCodeChild.CParentCode = sysCode.CItemCode;
                        sysCodeChild.CItemOrder = index;
                        sysCodeChild.CItemCode = index.ToString();
                        sysCodeChild.CItemName = item.CItemName;
                        sysCodeChild.IsShow = true;
                        sysCodeChild.IsActive = true;
                        sysCodeChild.Creuser = Guid.Parse(GetUserID(user));
                        sysCodeChild.Credate = DateTime.Now;
                        db.SysCodes.Add(sysCodeChild);

                        index++;
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
                List<SysCode> sc = db.SysCodes.Where(x => x.CParentCode.Equals(sysCode.CItemCode)).OrderBy(x => x.CItemOrder).ToList();
                vm.modifies = new List<SysCodeViewModel.ChildList>();
                foreach (var item in sc)
                {
                    SysCodeViewModel.ChildList childList = new SysCodeViewModel.ChildList();
                    childList.Uid = item.Uid;
                    childList.Order = item.CItemOrder;
                    childList.CItemName = item.CItemName;
                    vm.modifies.Add(childList);
                }
                vm.IsEdit = true;
            }
            else
            {
                //新增
                vm.IsEdit = false;
                SysCodeViewModel.ChildList childList = new SysCodeViewModel.ChildList();
                vm.modifies = new List<SysCodeViewModel.ChildList>();
                childList.Order = 1;
                childList.CItemName = "";
                vm.modifies.Add(childList);
            }





            return vm;
        }
        #endregion

        #region 刪除
        public BaseViewModel.errorMsg DelData(Guid UID)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            SysCode sysCode = db.SysCodes.Find(UID);
            List<SysCode> sysCodeList = db.SysCodes.Where(x => x.CParentUid.Equals(UID)).ToList();

            try
            {
                db.SysCodes.Remove(sysCode);
                db.SysCodes.RemoveRange(sysCodeList);
                db.SaveChanges();

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


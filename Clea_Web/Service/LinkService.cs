using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.Formula.Functions;
using X.PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Clea_Web.Service
{
    //相關連結管理
    public class LinkService : BaseService
    {
        private LinkViewModel.Modify vm = new LinkViewModel.Modify();


        public LinkService(dbContext dbContext)
        {
            db = dbContext;
        }

        #region 查詢
        public IPagedList<LinkViewModel.schPageList> schPages(LinkViewModel.SchItem data, Int32 page, Int32 pagesize)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageLists(data).ToPagedList(page, pagesize);

        }

        public List<LinkViewModel.schPageList> GetPageLists(LinkViewModel.SchItem data)
        {
            List<LinkViewModel.schPageList> result = new List<LinkViewModel.schPageList>();

            result = (from pLink in db.PLinks
                      where
                      (
                      (string.IsNullOrEmpty(data.Title) || pLink.LTitle.Contains(data.Title)) &&
                      (string.IsNullOrEmpty(data.Class) || pLink.LClass == data.Class)
                      )
                      select new LinkViewModel.schPageList
                      {
                          Uid = pLink.LinkId.ToString(),
                          Class = (from code in db.SysCodes where code.CParentCode.Equals("P_Link") && pLink.LClass.Equals(code.CItemCode) select code).FirstOrDefault().CItemName,
                          Title = pLink.LTitle,
                          Url = pLink.LUrl,
                          Order = pLink.LOrder,
                          Status = pLink.LStatus == true ? "是" : "否",
                          updDate = pLink.Upddate == null ? pLink.Credate.ToShortDateString() : pLink.Upddate.Value.ToShortDateString(),
                          updUser = (from user in db.SysUsers where (pLink.Upduser == null ? pLink.Creuser : pLink.Upduser).Equals(user.UId) select user).FirstOrDefault().UName,
                      }).OrderBy(x => x.Class).ThenBy(x=>x.Order).ToList();

            return result;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveData(LinkViewModel.Modify vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                PLink? pLink = db.PLinks.Find(vm.Uid);

                if (pLink is null)
                {
                    pLink = new PLink();
                }

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    pLink.LClass = vm.Class;
                    pLink.LTitle = vm.Title;
                    pLink.LOrder = vm.Order;
                    pLink.LUrl = vm.Url;
                    pLink.LStatus = vm.Status;
                    pLink.Upduser = Guid.Parse(GetUserID(user));
                    pLink.Upddate = DateTime.Now;


                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    pLink = new PLink();
                    pLink.LinkId = Guid.NewGuid();
                    pLink.LType = 26;
                    pLink.LClass = vm.Class;
                    pLink.LTitle = vm.Title;
                    pLink.LOrder = vm.Order;
                    pLink.LUrl = vm.Url;
                    pLink.LStatus = vm.Status;
                    pLink.Creuser = Guid.Parse(GetUserID(user));
                    pLink.Credate = DateTime.Now;
                    db.PLinks.Add(pLink);

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
        public LinkViewModel.Modify GetEditData(Guid Uid)
        {
            //撈資料
            PLink? pLink = db.PLinks.Where(x => x.LinkId.Equals(Uid)).FirstOrDefault();
            vm = new LinkViewModel.Modify();
            vm.getTypeItem = getTypeItem();
            if (pLink != null)
            {
                vm.Uid = pLink.LinkId;
                vm.Class = pLink.LClass;
                vm.Title = pLink.LTitle;
                vm.Url = pLink.LUrl;
                vm.Order = pLink.LOrder;
                vm.Status = pLink.LStatus;
                vm.IsEdit = true;
            }
            else
            {
                //新增
                vm.IsEdit = false;
            }
            return vm;
        }
        #endregion

        #region 相關連結分類_選單
        public List<SelectListItem> getTypeItem()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
            List<SysCode> lst_cLectors = db.SysCodes.Where(x => x.CParentCode == "P_Link").ToList();
            if (lst_cLectors != null && lst_cLectors.Count() > 0)
            {
                foreach (SysCode L in lst_cLectors)
                {
                    result.Add(new SelectListItem() { Text = L.CItemName, Value = L.CItemCode.ToString() });
                }
            }
            return result;
        }
        #endregion

        #region 刪除
        public BaseViewModel.errorMsg DelData(Guid Uid)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            PLink pLink = db.PLinks.Find(Uid);
            vm = new LinkViewModel.Modify();

            try
            {
                db.PLinks.Remove(pLink);
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


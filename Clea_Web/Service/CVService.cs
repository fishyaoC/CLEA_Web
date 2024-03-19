using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.Formula.Functions;
using X.PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Clea_Web.Service
{
    //廠商徵才管理
    public class CVService : BaseService
    {
        private CVViewModel.Modify vm = new CVViewModel.Modify();

        public CVService(dbContext dbContext)
        {
            db = dbContext;
        }

        #region 查詢
        public IPagedList<CVViewModel.schPageList> schPages(CVViewModel.SchItem data, Int32 page, Int32 pagesize)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageLists(data).ToPagedList(page, pagesize);

        }

        public List<CVViewModel.schPageList> GetPageLists(CVViewModel.SchItem data)
        {
            List<CVViewModel.schPageList> result = new List<CVViewModel.schPageList>();

            result = (from CV in db.PCompanyCvs
                      //join Member in db.PMembers on CV.CvCompanyName equals Member.Uid
                      where
                      (
                      (string.IsNullOrEmpty(data.Company) || CV.CvCompanyName.Equals(Guid.Parse(data.Company))) &&
                      (string.IsNullOrEmpty(data.isApprove) || CV.IsApprove == data.isApprove)
                      )
                      select new CVViewModel.schPageList
                      {
                          Uid = CV.CvUid.ToString(),
                          ApplyDate = CV.CvExp.ToShortDateString(),
                          Company = (from member in db.PMembers where (CV.CvCompanyName.Equals(member.Uid.ToString())) select member).FirstOrDefault().MName,
                          //Company = CV.CvCompanyName,
                          Title = CV.CvTitle,
                          Approve = (from code in db.SysCodes where code.CParentCode.Equals("CV_Approved") && CV.IsApprove.Equals(code.CItemCode) select code).FirstOrDefault().CItemName,
                          Status = CV.CvStatus == true ? "是" : "否",
                          Memo = string.IsNullOrEmpty(CV.ApproveMemo) ? "無備註" : CV.ApproveMemo,
                          updDate = CV.Upddate == null ? CV.Credate.ToShortDateString() : CV.Upddate.Value.ToShortDateString(),
                          updUser = (from user in db.SysUsers where (CV.Upduser == null ? CV.Creuser : CV.Upduser).Equals(user.UId) select user).FirstOrDefault().UName != null
                          ? (from user in db.SysUsers where (CV.Upduser == null ? CV.Creuser : CV.Upduser).Equals(user.UId) select user).FirstOrDefault().UName
                          : (from member in db.PMembers where (CV.Upduser == null ? CV.Creuser : CV.Upduser).Equals(member.Uid) select member).FirstOrDefault().MName,
                          StartD = CV.Credate,
                      }).OrderByDescending(x => x.StartD).ToList();

            return result;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveData(CVViewModel.Modify vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                PCompanyCv? pCompanyCv = db.PCompanyCvs.Find(vm.Uid);

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    pCompanyCv.CvTitle = vm.Title;
                    pCompanyCv.CvPlace = vm.Place;
                    pCompanyCv.CvNum = vm.Num;
                    pCompanyCv.CvExp = vm.Exp;
                    pCompanyCv.CvPay = vm.Pay;
                    pCompanyCv.CvCompanyName = vm.Company.ToString();
                    pCompanyCv.CvContact = vm.Contact;
                    pCompanyCv.CvPhone = vm.Phone;
                    pCompanyCv.CvAddress = vm.Address;
                    pCompanyCv.CvEmail = vm.EMail;
                    pCompanyCv.CvWay = vm.Way;
                    pCompanyCv.CvContent = vm.Content;
                    pCompanyCv.CvRequire = vm.Require;
                    pCompanyCv.IsApprove = vm.Approve;
                    pCompanyCv.ApproveMemo = vm.Memo;
                    pCompanyCv.Upduser = Guid.Parse(GetUserID(user));
                    pCompanyCv.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    pCompanyCv = new PCompanyCv();
                    pCompanyCv.CvUid = Guid.NewGuid();
                    pCompanyCv.CvTitle = vm.Title;
                    pCompanyCv.CvPlace = vm.Place;
                    pCompanyCv.CvNum = vm.Num;
                    pCompanyCv.CvExp = vm.Exp;
                    pCompanyCv.CvPay = vm.Pay;
                    pCompanyCv.CvCompanyName = vm.Company.ToString();
                    pCompanyCv.CvContact = vm.Contact;
                    pCompanyCv.CvPhone = vm.Phone;
                    pCompanyCv.CvAddress = vm.Address;
                    pCompanyCv.CvEmail = vm.EMail;
                    pCompanyCv.CvWay = vm.Way;
                    pCompanyCv.CvContent = vm.Content;
                    pCompanyCv.CvRequire = vm.Require;
                    pCompanyCv.IsApprove = "1";
                    pCompanyCv.ApproveMemo = vm.Memo;
                    pCompanyCv.Creuser = Guid.Parse(GetUserID(user));
                    pCompanyCv.Credate = DateTime.Now;
                    db.PCompanyCvs.Add(pCompanyCv);
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
        public CVViewModel.Modify GetEditData(Guid Uid)
        {
            //撈資料
            PCompanyCv? pCompanyCv = db.PCompanyCvs.Where(x => x.CvUid.Equals(Uid)).FirstOrDefault();            
            vm = new CVViewModel.Modify();

            if (pCompanyCv != null)
            {
                vm.Uid = pCompanyCv.CvUid;
                vm.Title = pCompanyCv.CvTitle;
                vm.Place = pCompanyCv.CvPlace;
                vm.Num = pCompanyCv.CvNum;
                vm.Exp = pCompanyCv.CvExp;
                vm.Pay = pCompanyCv.CvPay;
                vm.CompanyCName = db.PMembers.Where(x=>x.Uid.Equals(Guid.Parse(pCompanyCv.CvCompanyName))).FirstOrDefault().MName ;
                vm.Company = Guid.Parse(pCompanyCv.CvCompanyName);
                vm.Contact = pCompanyCv.CvContact;
                vm.Phone = pCompanyCv.CvPhone;
                vm.Address = pCompanyCv.CvAddress;
                vm.EMail = pCompanyCv.CvEmail;
                vm.Way = pCompanyCv.CvWay;
                vm.Content = pCompanyCv.CvContent;
                vm.Require = pCompanyCv.CvRequire;
                vm.Approve = pCompanyCv.IsApprove;
                vm.Status = pCompanyCv.CvStatus;
                vm.Memo = pCompanyCv.ApproveMemo;
                vm.IsEdit = true;
                vm.Close = pCompanyCv.CvClose == null ? "無" : (pCompanyCv.CvClose == true ? "上架中" : "下架中");
                vm.CloseTime = pCompanyCv.CvCloseTime.ToString();
            }
            else
            {
                //新增
                vm.IsEdit = false;
                vm.Exp = DateTime.Now;

            }
            return vm;
        }
        #endregion

        #region 刪除
        public BaseViewModel.errorMsg DelData(Guid Uid)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            PCompanyCv CV = db.PCompanyCvs.Find(Uid);
            vm = new CVViewModel.Modify();

            try
            {
                db.PCompanyCvs.Remove(CV);
            }
            catch (Exception e)
            {
                result.ErrorMsg = e.Message;
            }
            result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

            return result;
        }

        #endregion

        #region 核准狀態_選單
        public List<SelectListItem> getApprovedItem()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
            List<SysCode> lst_cLectors = db.SysCodes.Where(x => x.CParentCode == "CV_Approved").ToList();
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

        #region 廠商_選單
        public List<SelectListItem> getCompanyItem()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
            List<PMember> lst_cLectors = db.PMembers.Where(x => x.MType == 2 && x.MStatus == true).ToList();
            if (lst_cLectors != null && lst_cLectors.Count() > 0)
            {
                foreach (PMember L in lst_cLectors)
                {
                    result.Add(new SelectListItem() { Text = L.MName, Value = L.Uid.ToString() });
                }
            }
            return result;
        }
        #endregion

    }
}


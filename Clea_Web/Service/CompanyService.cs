using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using X.PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Clea_Web.Service
{
    //廠商會員管理
    public class CompanyService : BaseService
    {
        private CompanyViewModel.Modify vm = new CompanyViewModel.Modify();

        public CompanyService(dbContext dbContext)
        {
            db = dbContext;
        }

        #region 查詢
        public IPagedList<CompanyViewModel.schPageList> schPages(CompanyViewModel.SchItem data, Int32 page, Int32 pagesize)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageLists(data).ToPagedList(page, pagesize);

        }

        public List<CompanyViewModel.schPageList> GetPageLists(CompanyViewModel.SchItem data)
        {
            List<CompanyViewModel.schPageList> result = new List<CompanyViewModel.schPageList>();

            result = (from pMember in db.PMembers
                          //join Member in db.PMembers on CV.CvCompanyName equals Member.Uid
                      where
                      (
                      (string.IsNullOrEmpty(data.ID) || pMember.MId.Equals(Guid.Parse(data.ID))) &&
                      (string.IsNullOrEmpty(data.Name) || pMember.MName == data.Name) &&
                      (pMember.MType == 2)
                      )
                      select new CompanyViewModel.schPageList
                      {
                          Uid = pMember.Uid.ToString(),
                          Name = pMember.MName,
                          //Level = (from code in db.SysCodes where code.CParentCode.Equals("MemberLevel") && pMember.MLevel.Equals(code.CItemCode) select code).FirstOrDefault().CItemName,
                          ID = pMember.MId,
                          Contact = pMember.MContact,
                          Phone = pMember.MPhone,
                          CellPhone = pMember.MCellPhone,
                          Status = pMember.MStatus == true ? "是" : "否",
                          updDate = pMember.Upddate == null ? pMember.Credate.ToShortDateString() : pMember.Upddate.Value.ToShortDateString(),
                          updUser = (from user in db.SysUsers where (pMember.Upduser == null ? pMember.Creuser : pMember.Upduser).Equals(user.UId) select user).FirstOrDefault().UName,
                          StartD = pMember.Credate,
                      }).OrderByDescending(x => x.StartD).ToList();

            return result;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveData(CompanyViewModel.Modify vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                PMember? pMember = db.PMembers.Find(vm.Uid);

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    if (vm.Password != null)
                    {
                        pMember.MPassword = HashPassword(vm.Password);
                    }
                    pMember.MName = vm.Name;
                    pMember.MId = vm.ID;
                    pMember.MPhone = vm.Phone;
                    pMember.MCellPhone = vm.CellPhone;
                    pMember.MContact = vm.Contact;
                    pMember.MAddress = vm.Address;
                    pMember.MEmail = vm.EMail;
                    pMember.MStatus = vm.Status;
                    pMember.Upduser = Guid.Parse(GetUserID(user));
                    pMember.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    pMember = new PMember();
                    pMember.Uid = Guid.NewGuid();
                    if (vm.Password != null)
                    {
                        pMember.MPassword = HashPassword(vm.Password);
                    }
                    pMember.MName = vm.Name;
                    pMember.MId = vm.ID;
                    pMember.MPhone = vm.Phone;
                    pMember.MCellPhone = vm.CellPhone;
                    pMember.MContact = vm.Contact;
                    pMember.MAddress = vm.Address;
                    pMember.MEmail = vm.EMail;
                    pMember.MStatus = vm.Status;
                    pMember.MType = 2;
                    pMember.Creuser = Guid.Parse(GetUserID(user));
                    pMember.Credate = DateTime.Now;
                    db.PMembers.Add(pMember);
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
        public CompanyViewModel.Modify GetEditData(Guid Uid)
        {
            //撈資料
            PMember? pMember = db.PMembers.Where(x => x.Uid.Equals(Uid)).FirstOrDefault();
            vm = new CompanyViewModel.Modify();

            if (pMember != null)
            {
                vm.Uid = pMember.Uid;
                vm.Name = pMember.MName;
                vm.ID = pMember.MId;
                vm.Phone = pMember.MPhone;
                vm.Contact = pMember.MContact;
                vm.CellPhone = pMember.MCellPhone;
                vm.Address = pMember.MAddress;
                vm.EMail = pMember.MEmail;
                vm.Status = pMember.MStatus;

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

        #region 刪除
        public BaseViewModel.errorMsg DelData(Guid Uid)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();

            //撈資料
            PMember pMember = db.PMembers.Find(Uid);
            vm = new CompanyViewModel.Modify();

            try
            {
                db.PMembers.Remove(pMember);
            }
            catch (Exception e)
            {
                result.ErrorMsg = e.Message;
            }
            result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

            return result;
        }

        #endregion

        #region 統編_選單
        public List<SelectListItem> getIDItem()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
            List<PMember> lst_cLectors = db.PMembers.Where(x => x.MType == 2 && x.MStatus == true).ToList();
            if (lst_cLectors != null && lst_cLectors.Count() > 0)
            {
                foreach (PMember L in lst_cLectors)
                {
                    result.Add(new SelectListItem() { Text = L.MId, Value = L.Uid.ToString() });
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

        #region 密碼加密
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // 將密碼轉為字節數組
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // 計算hash
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                //將hash轉為十六進制字符串
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }

        }
        #endregion

        #region 匯出Excel
        public Byte[] Export_Execl()
        {
            List<PMember> pMember = db.PMembers.Where(x => x.MType == 2).ToList();

            #region ExportExcel
            String[] lst_Header = new string[] { "公司名稱", "統一編號", "聯絡人", "連絡電話(市話)/手機", "聯絡地址", "傳真號碼", "電子信箱", "啟用狀態" };
            using (var exportData = new MemoryStream())
            {
                IWorkbook wb = new XSSFWorkbook();  //创建工作簿
                ISheet sheet = wb.CreateSheet("網路會員資料"); //创建工作表
                XSSFCellStyle TitleStyle = (XSSFCellStyle)wb.CreateCellStyle(); //标题样式
                TitleStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                TitleStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                TitleStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                TitleStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                TitleStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                TitleStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                XSSFFont font = (XSSFFont)wb.CreateFont();
                font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                TitleStyle.SetFont(font);

                // 写入标题
                IRow titleRow = sheet.CreateRow(0);
                for (int i = 0; i < lst_Header.Length; i++)
                {
                    ICell cell = titleRow.CreateCell(i);
                    cell.SetCellValue(lst_Header[i]);
                    cell.CellStyle = TitleStyle;
                    sheet.SetColumnWidth(i, 24 * 512);
                }

                // 将每个会员的数据写入到 Excel 中
                int rowIndex = 1; // 从第二行开始写入数据，第一行已经写入了列名
                foreach (var member in pMember)
                {
                    IRow row = sheet.CreateRow(rowIndex++);

                    // 写入每个字段的数据           
                    //String[] lst_Header = new string[] { "公司名稱", "統一編號", "聯絡人", "連絡電話(市話)/手機", "聯絡地址", "傳真號碼", "電子信箱","啟用狀態" };
                    row.CreateCell(0).SetCellValue(member.MName);
                    row.CreateCell(1).SetCellValue(member.MId);
                    row.CreateCell(2).SetCellValue(member.MContact);
                    row.CreateCell(3).SetCellValue(member.MPhone);
                    row.CreateCell(4).SetCellValue(member.MAddress);
                    row.CreateCell(5).SetCellValue(member.MCellPhone);
                    row.CreateCell(6).SetCellValue(member.MEmail);
                    row.CreateCell(7).SetCellValue(member.MStatus == true ? "是" : "否");
                }

                // 手动设置每列的宽度
                sheet.SetColumnWidth(0, 60 * 256); // 设置第1列的宽度为20个字符的宽度
                sheet.SetColumnWidth(1, 15 * 256);
                sheet.SetColumnWidth(2, 15 * 256);
                sheet.SetColumnWidth(3, 15 * 256);
                sheet.SetColumnWidth(4, 60 * 256);
                sheet.SetColumnWidth(5, 15 * 256);
                sheet.SetColumnWidth(6, 30 * 256);
                sheet.SetColumnWidth(7, 15 * 256);

                wb.Write(exportData, true); // 写入数据到 MemoryStream

                Byte[] result = exportData.ToArray();
                return result;
            }
            #endregion

        }
        #endregion
    }
}


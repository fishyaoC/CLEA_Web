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
    //一般會員管理
    public class MemberService : BaseService
    {
        private MemberViewModel.Modify vm = new MemberViewModel.Modify();

        public MemberService(dbContext dbContext)
        {
            db = dbContext;
        }

        #region 查詢
        public IPagedList<MemberViewModel.schPageList> schPages(MemberViewModel.SchItem data, Int32 page, Int32 pagesize)
        {
            //var result = GetPageLists(data);

            //return result.ToPagedList(page, pagesize);
            return GetPageLists(data).ToPagedList(page, pagesize);

        }

        public List<MemberViewModel.schPageList> GetPageLists(MemberViewModel.SchItem data)
        {
            List<MemberViewModel.schPageList> result = new List<MemberViewModel.schPageList>();

            result = (from pMember in db.PMembers
                          //join Member in db.PMembers on CV.CvCompanyName equals Member.Uid
                      where
                      (
                      (string.IsNullOrEmpty(data.ID) || pMember.MId.Equals(Guid.Parse(data.ID))) &&
                      (string.IsNullOrEmpty(data.Level) || pMember.MLevel == data.Level) &&
                      (pMember.MType == 1)
                      )
                      select new MemberViewModel.schPageList
                      {
                          Uid = pMember.Uid.ToString(),
                          Name = pMember.MName,
                          Level = (from code in db.SysCodes where code.CParentCode.Equals("MemberLevel") && pMember.MLevel.Equals(code.CItemCode) select code).FirstOrDefault().CItemName,
                          Status = pMember.MStatus == true ? "是" : "否",
                          updDate = pMember.Upddate == null ? pMember.Credate.ToShortDateString() : pMember.Upddate.Value.ToShortDateString(),
                          updUser = (from user in db.SysUsers where (pMember.Upduser == null ? pMember.Creuser : pMember.Upduser).Equals(user.UId) select user).FirstOrDefault().UName,
                          StartD = pMember.Credate,
                      }).OrderByDescending(x => x.StartD).ToList();

            return result;
        }
        #endregion

        #region 儲存
        public BaseViewModel.errorMsg SaveData(MemberViewModel.Modify vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                PMember? pMember = db.PMembers.Find(vm.Uid);

                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    pMember.MName = vm.Name;
                    if (CheckROCID(vm.ID) == true)
                    {
                        char secondDigit = vm.ID[1];
                        byte[] byteArray = Encoding.ASCII.GetBytes(new char[] { secondDigit });
                        pMember.MSex = byteArray[0];
                        pMember.MId = vm.ID;
                    }
                    else
                    {
                        result.CheckMsg = false;
                        result.ErrorMsg = "身分證輸入錯誤";
                        return result;
                    }
                    if (vm.Password != null)
                    {
                        pMember.MPassword = HashPassword(vm.Password);
                    }
                    pMember.MBrithday = vm.Brithday;
                    pMember.MGraduatedSchool = vm.School;
                    pMember.MPhone = vm.Phone;
                    pMember.MCellPhone = vm.CellPhone;
                    pMember.MAddress = vm.Address;
                    pMember.MWorkPlace = vm.WorkPlace;
                    pMember.MEmail = vm.EMail;
                    pMember.MLineId = vm.LineID;
                    pMember.MStatus = vm.Status;
                    pMember.MLevel = vm.Level;
                    pMember.Upduser = Guid.Parse(GetUserID(user));
                    pMember.Upddate = DateTime.Now;
                }
                else if (vm != null && vm.IsEdit == false)
                {
                    //新增
                    pMember = new PMember();
                    pMember.Uid = Guid.NewGuid();
                    pMember.MName = vm.Name;
                    if (CheckROCID(vm.ID) == true)
                    {
                        char secondDigit = vm.ID[1];
                        byte[] byteArray = Encoding.ASCII.GetBytes(new char[] { secondDigit });
                        pMember.MSex = byteArray[0];
                        pMember.MId = vm.ID;
                    }
                    else
                    {
                        result.CheckMsg = false;
                        result.ErrorMsg = "身分證輸入錯誤";
                        return result;
                    }
                    // pMember.MPassword = vm.Password;
                    if (vm.Password != null)
                    {
                        pMember.MPassword = HashPassword(vm.Password);
                    }
                    pMember.MBrithday = vm.Brithday;
                    pMember.MGraduatedSchool = vm.School;
                    pMember.MPhone = vm.Phone;
                    pMember.MCellPhone = vm.CellPhone;
                    pMember.MAddress = vm.Address;
                    pMember.MWorkPlace = vm.WorkPlace;
                    pMember.MEmail = vm.EMail;
                    pMember.MLineId = vm.LineID;
                    pMember.MStatus = vm.Status;
                    pMember.MLevel = vm.Level;
                    pMember.MType = 1;
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
        public MemberViewModel.Modify GetEditData(Guid Uid)
        {
            //撈資料
            PMember? pMember = db.PMembers.Where(x => x.Uid.Equals(Uid)).FirstOrDefault();
            vm = new MemberViewModel.Modify();

            if (pMember != null)
            {
                vm.Uid = pMember.Uid;
                vm.Name = pMember.MName;
                vm.ID = pMember.MId;
                //vm.Password = pMember.MPassword;
                vm.Brithday = pMember.MBrithday;
                vm.School = pMember.MGraduatedSchool;
                vm.Phone = pMember.MPhone;
                vm.CellPhone = pMember.MCellPhone;
                vm.Address = pMember.MAddress;
                vm.WorkPlace = pMember.MWorkPlace;
                vm.EMail = pMember.MEmail;
                vm.LineID = pMember.MLineId;
                vm.Status = pMember.MStatus;
                vm.Level = pMember.MLevel;

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
            vm = new MemberViewModel.Modify();

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

        #region 會員等級_選單
        public List<SelectListItem> getLevelItem()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
            List<SysCode> lst_cLectors = db.SysCodes.Where(x => x.CParentCode == "MemberLevel").OrderBy(x => x.CItemOrder).ToList();
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

        #region 會員_選單
        public List<SelectListItem> getMemberItem()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Text = "請選擇", Value = string.Empty });
            List<PMember> lst_cLectors = db.PMembers.Where(x => x.MType == 1).ToList();
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

        #region 身分證驗證
        public bool CheckROCID(string idNo)
        {
            if (idNo == null)
            {
                return false;
            }
            idNo = idNo.ToUpper();
            Regex regex = new Regex(@"^([A-Z])([1-2]\d{8})$");
            System.Text.RegularExpressions.Match match = regex.Match(idNo);
            if (!match.Success)
            {
                return false;
            }

            ///建立字母對應表(A~Z)
            ///A=10 B=11 C=12 D=13 E=14 F=15 G=16 H=17 J=18 K=19 L=20 M=21 N=22
            ///P=23 Q=24 R=25 S=26 T=27 U=28 V=29 X=30 Y=31 W=32  Z=33 I=34 O=35 
            string alphabet = "ABCDEFGHJKLMNPQRSTUVXYWZIO";
            string transferIdNo = $"{(alphabet.IndexOf(match.Groups[1].Value) + 10)}" +
                                  $"{match.Groups[2].Value}";
            int[] idNoArray = transferIdNo.ToCharArray()
                                          .Select(c => Convert.ToInt32(c.ToString()))
                                          .ToArray();
            int sum = idNoArray[0];
            int[] weight = new int[] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 1 };
            for (int i = 0; i < weight.Length; i++)
            {
                sum += weight[i] * idNoArray[i + 1];
            }
            return (sum % 10 == 0);
        }
        #endregion

        #region 匯出Excel
        public Byte[] Export_Execl()
        {
            List<PMember> pMember = db.PMembers.Where(x=>x.MType == 1).ToList();

            #region ExportExcel
            String[] lst_Header = new string[] { "姓名", "身分證字號", "出生年月日", "畢業學校", "連絡電話(市話)", "手機號碼", "戶籍住址", "服務單位", "電子郵件", "LineID", "會員等級", "帳號是否啟用" };
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
                    row.CreateCell(0).SetCellValue(member.MName);
                    row.CreateCell(1).SetCellValue(member.MId);
                    row.CreateCell(2).SetCellValue(member.MBrithday.Value.ToShortDateString());
                    row.CreateCell(3).SetCellValue(member.MGraduatedSchool);
                    row.CreateCell(4).SetCellValue(member.MPhone);
                    row.CreateCell(5).SetCellValue(member.MCellPhone);
                    row.CreateCell(6).SetCellValue(member.MAddress);
                    row.CreateCell(7).SetCellValue(member.MWorkPlace);
                    row.CreateCell(8).SetCellValue(member.MEmail);
                    row.CreateCell(9).SetCellValue(member.MLineId);
                    row.CreateCell(10).SetCellValue((from code in db.SysCodes where code.CParentCode.Equals("MemberLevel") && member.MLevel.Equals(code.CItemCode) select code).FirstOrDefault().CItemName);
                    row.CreateCell(11).SetCellValue(member.MStatus == true ? "是" : "否");
                }

                // 手动设置每列的宽度
                sheet.SetColumnWidth(0, 10 * 256); // 设置第1列的宽度为20个字符的宽度
                sheet.SetColumnWidth(1, 15 * 256); // 设置第2列的宽度为15个字符的宽度
                sheet.SetColumnWidth(2, 15 * 256); // 设置第3列的宽度为15个字符的宽度
                sheet.SetColumnWidth(3, 50 * 256); // 设置第3列的宽度为15个字符的宽度
                sheet.SetColumnWidth(4, 15 * 256); // 设置第3列的宽度为15个字符的宽度
                sheet.SetColumnWidth(5, 15 * 256); // 设置第3列的宽度为15个字符的宽度
                sheet.SetColumnWidth(6, 60 * 256); // 设置第3列的宽度为15个字符的宽度
                sheet.SetColumnWidth(7, 60 * 256); // 设置第3列的宽度为15个字符的宽度
                sheet.SetColumnWidth(8, 35 * 256); // 设置第3列的宽度为15个字符的宽度
                sheet.SetColumnWidth(9, 15 * 256); // 设置第3列的宽度为15个字符的宽度
                sheet.SetColumnWidth(10, 20 * 256); // 设置第3列的宽度为15个字符的宽度
                sheet.SetColumnWidth(11, 15 * 256); // 设置第3列的宽度为15个字符的宽度



                wb.Write(exportData, true); // 写入数据到 MemoryStream

                Byte[] result = exportData.ToArray();
                return result;
            }
            #endregion

        }
        #endregion
    }
}


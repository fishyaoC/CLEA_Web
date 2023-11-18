using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using MathNet.Numerics;
using NPOI.HPSF;
using X.PagedList;

namespace Clea_Web.Service
{
    //後台講師專區-講師進修資料管理
    public class AccountSettingService : BaseService
    {
        private AccountSettingViewModel.Modify vm = new AccountSettingViewModel.Modify();
        private FileService _fileservice;
        private IConfiguration configuration;


        public AccountSettingService(dbContext dbContext, IConfiguration configuration)
        {
            db = dbContext;
            this.configuration = configuration;
        }

        #region Modify
        public AccountSettingViewModel.Modify GetEditData()
        {
            Guid uid = Guid.Parse(GetUserID(user));
            //撈資料
            SysUser su = db.SysUsers.Where(x => x.UId == uid).FirstOrDefault();
            SysFile sf = db.SysFiles.Where(x => x.FMatchKey.ToString() == su.UId.ToString()).FirstOrDefault();

            vm = new AccountSettingViewModel.Modify();
            if (sf != null)
            {
                string fileNameDL = sf.FNameDl + "." + sf.FExt;
                string filePath = Path.Combine(configuration.GetValue<String>("FileRootPath"), sf.FPath, fileNameDL);
                vm.FileID = sf.FileId; //檔案名稱
                vm.FileName = sf.FFullName;
                vm.FilePath = filePath;
                //vm.FExt = sf.FExt;
            }

            if (su != null)
            {
                vm.UId = su.UId;  //帳號Uid
                vm.UName = su.UName; //帳號名稱
                vm.IsEdit = true;
                vm.UptDate = su.Upddate == null ? su.Credate.ToShortDateString() : su.Upddate.Value.ToShortDateString();
            }
            return vm;
        }
        #endregion

        #region Save
        public BaseViewModel.errorMsg SaveData(AccountSettingViewModel.Modify vm)
        {
            BaseViewModel.errorMsg? result = new BaseViewModel.errorMsg();
            try
            {
                SysUser? su = db.SysUsers.Find(vm.UId);



                if (vm != null && vm.IsEdit == true)
                {
                    //編輯
                    //比對新舊密碼

                    //SysUser.UPassword = vm.UPassword; //加密
                    if (vm.OldPW != null && vm.NewPW != null && vm.CheckNewPW != null)
                    {
                        string CheckOldPW = HashPassword(vm.OldPW);

                        if (CheckOldPW == su.UPassword)
                        {
                            if (vm.CheckNewPW == vm.NewPW)
                            {
                                //都符合才可修改密碼
                                su.UPassword = HashPassword(vm.NewPW);
                                su.Upduser = Guid.Parse(GetUserID(user));
                                su.Upddate = DateTime.Now;
                            }
                            else
                            {
                                result.CheckMsg = false;
                                result.ErrorMsg = "新密碼與確認新密碼不同";
                                return result;
                            }
                        }
                        else {
                            result.CheckMsg = false;
                            result.ErrorMsg = "舊密碼不符";
                            return result;
                        }
                    }
                }
                result.CheckMsg = Convert.ToBoolean(db.SaveChanges());

                if (vm.file == null)
                {
                    result.CheckMsg = true;
                }
                else if (vm.file != null)
                {
                    //_fileservice.user = user;
                    result.CheckMsg = _fileservice.UploadSignFile(su.UId, vm.file);
                    if (result.CheckMsg)
                    {
                        //上傳成功
                    }
                    else
                    {
                        result.CheckMsg = false;
                        result.ErrorMsg = "檔案上傳失敗";
                    }
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
    }
}


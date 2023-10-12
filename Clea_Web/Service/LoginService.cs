using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Security.Claims;
using Clea_Web.ViewModels;
using Clea_Web.Models;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Security.Cryptography;

namespace Clea_Web.Service
{
    //後臺登入
    public class LoginService : BaseService
    {

        //public LoginService(dbContext dbContext)
        //{
        //    db = dbContext;
        //}

        private dbContext db = new dbContext();

        public List<Claim> Login(LoginViewModel data)
        {

            //密碼加密
            string passWord = HashPassword(data.loginVM.PassWord);

            List<Claim> result = new List<Claim>();
            LoginViewModel.LoginRoleInfo? info = (from usr in db.SysUsers
                                                  where usr.UStatus && usr.UAccount.Equals(data.loginVM.Account) && usr.UPassword.Equals(passWord)
                                                  join role in db.SysRoles on usr.RUid equals role.RUid
                                                  select new LoginViewModel.LoginRoleInfo
                                                  {
                                                      U_Account = usr.UAccount,
                                                      U_Name = usr.UName,
                                                      R_UID = usr.RUid.ToString(),
                                                      U_ID = usr.UId.ToString(),
                                                      R_BackEnd = role.RBackEnd.ToString()
                                                  }).FirstOrDefault();

            if (info != null)
            {
                result.Add(new Claim(ClaimTypes.NameIdentifier, info.U_Account));
                result.Add(new Claim(ClaimTypes.Name, info.U_Name));
                result.Add(new Claim(ClaimTypes.Role, info.R_UID.ToString()));
                result.Add(new Claim(ClaimTypes.Sid, info.U_ID.ToString()));
                result.Add(new Claim(ClaimTypes.PrimarySid, info.R_BackEnd.ToString()));
            }


            return result;
        }

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

            #endregion
        }
    }
}

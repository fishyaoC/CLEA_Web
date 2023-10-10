using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Security.Claims;
using Clea_Web.ViewModels;
using Clea_Web.Models;
using System.Runtime.Intrinsics.X86;

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
            List<Claim> result = new List<Claim>();
            LoginViewModel.LoginRoleInfo? info = (from usr in db.SysUsers
                                                  where usr.UStatus && usr.UAccount.Equals(data.loginVM.Account) && usr.UPassword.Equals(data.loginVM.PassWord)
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
            //    //轉換加密
            //    //data.loginVM.PassWord = data.loginVM.PassWord;

            //    SysUser? user = db.SysUsers.Where(x => x.UStatus.Equals(true) && x.UAccount.Equals(data.loginVM.Account) && x.UPassword.Equals(data.loginVM.PassWord)).FirstOrDefault();


            //    if (user is null)
            //    {
            //        //null
            //    }
            //    else if (data.loginVM.IsTest && user != null)
            //    {
            //        result.Add(new Claim(ClaimTypes.NameIdentifier, user.UAccount));
            //        result.Add(new Claim(ClaimTypes.Name, user.UName));
            //        result.Add(new Claim(ClaimTypes.Role, "86C6899D-18D5-414C-9BA9-F8F0E28146B9"));
            //        result.Add(new Claim(ClaimTypes.Sid, user.UId.ToString()));
            //    }
            //    else
            //    {
            //        result.Add(new Claim(ClaimTypes.NameIdentifier, user.UAccount));
            //        result.Add(new Claim(ClaimTypes.Name, user.UName));
            //        result.Add(new Claim(ClaimTypes.Role, user.RUid.ToString()));
            //        result.Add(new Claim(ClaimTypes.Sid, user.UId.ToString()));
            //    }


                return result;
        }
    }
}

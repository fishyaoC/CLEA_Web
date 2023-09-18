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

            //轉換加密
            //data.loginVM.PassWord = data.loginVM.PassWord;

            SysUser? user = db.SysUsers.Where(x => x.UStatus.Equals(true) && x.UAccount.Equals(data.loginVM.Account) && x.UPassword.Equals(data.loginVM.PassWord)).FirstOrDefault();


            if (user is null)
            {
                //null
            }
            else if (data.loginVM.IsTest && user != null)
            {
                result.Add(new Claim(ClaimTypes.NameIdentifier, user.UAccount));
                result.Add(new Claim(ClaimTypes.Name, user.UName));
                result.Add(new Claim(ClaimTypes.Role, "86C6899D-18D5-414C-9BA9-F8F0E28146B9"));
                result.Add(new Claim(ClaimTypes.Sid, user.UId.ToString()));
            }
            else
            {
                result.Add(new Claim(ClaimTypes.NameIdentifier, user.UAccount));
                result.Add(new Claim(ClaimTypes.Name, user.UName));
                result.Add(new Claim(ClaimTypes.Role, user.RUid.ToString()));
                result.Add(new Claim(ClaimTypes.Sid, user.UId.ToString()));
            }
            

            return result;
        }
    }
}

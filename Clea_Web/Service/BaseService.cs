using Clea_Web.Models;
using System.Diagnostics.Contracts;
using System.Net;
using System.Security.Claims;
using Clea_Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Clea_Web.Service
{
    public class BaseService
    {
        //public ControllerBase Controller { get; set; }
        public ClaimsPrincipal user { get; set; }
        public BaseService() 
        {
            
        }

        public dbContext db;

        #region 分頁顯示數量
        public static Int32 pagesize = 15;
        #endregion

        #region 連線字串
        public String ConfigData(String path)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var config = builder.Build();

            return config[path];
        }
        #endregion

        #region 取得User帳號U_ID PK
        public Guid GetUserGUID() 
        {
            return Guid.Parse(GetUserID(user));
        }

        public String GetUserID(ClaimsPrincipal userInfo)
        {
            String uid = string.Empty;
            ClaimsIdentity? claimsIdentity = userInfo.Identity as ClaimsIdentity;
            if (claimsIdentity is null)
            {
                uid = string.Empty;
            }
            else
            {
                uid = userInfo.FindFirst(ClaimTypes.Sid).Value;
            }

            return uid;
        }
        #endregion

        #region 取得User帳號U_Account PK
        public String GetUserAct()
        {
            return GetUserAccount(user);
        }
        public String GetUserAccount(ClaimsPrincipal userInfo)
        {
            String account = string.Empty;
            ClaimsIdentity? claimsIdentity = userInfo.Identity as ClaimsIdentity;
            if (claimsIdentity is null)
            {
                account = string.Empty;
            }
            else
            {
                account = userInfo.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            return account;
        }
        #endregion

        #region 取得User RoleUid

        public Guid GetUserRole()
        {
            return Guid.Parse(GetUserRoleUid(user));
        }
        public String GetUserRoleUid(ClaimsPrincipal userInfo)
        {
            String Role = string.Empty;
            Guid R_UID = Guid.Empty;

            ClaimsIdentity? claimsIdentity = userInfo.Identity as ClaimsIdentity;
            if (claimsIdentity is null)
            {
                //uid = string.Empty;
                R_UID = Guid.Empty;
            }
            else
            {
                Role = userInfo.FindFirst(ClaimTypes.Role).Value;
            }
            return Role;
        }

		#endregion

		#region StatusUpdate
        public void StatusUpdate(Int32 table, Guid ID, Int32 Type)
        {
            Boolean result = false;
            if (table == 0)
            {
                EEvaluationSche? eEvaluationSche = db.EEvaluationSches.Find(ID) ?? null;
                eEvaluationSche.Status = Type;
            }
            else
            {
                EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(ID) ?? null;
                eEvaluateDetail.Status = Type;
            }
            result = Convert.ToBoolean(db.SaveChanges());
            //return result;
        }
		#endregion

		#region MailUpdate
		public void MailUpdate(Int32 table, Guid ID, Boolean Type)
		{
			Boolean result = false;
			if (table == 0)
			{
				EEvaluationSche? eEvaluationSche = db.EEvaluationSches.Find(ID) ?? null;
				eEvaluationSche.IsMail = Type;
			}
			else
			{
				EEvaluateDetail? eEvaluateDetail = db.EEvaluateDetails.Find(ID) ?? null;
				eEvaluateDetail.IsMail = Type;
			}
			result = Convert.ToBoolean(db.SaveChanges());
			//return result;
		}
		#endregion
	}
}

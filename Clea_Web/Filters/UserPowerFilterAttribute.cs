using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Clea_Web.Controllers;
using Clea_Web.Models;


namespace Clea_Web.Filters
{
    public class UserPowerFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //base.OnActionExecuting(context);

            var user = context.HttpContext.User;
            if (user.Identities.FirstOrDefault().Claims.Count() > 0)
            {
                GetUserRolePower(context);
            }
            else
            {
                UserRedirctPage(context, "L");
            }


            //context.HttpContext.Session.SetString("TempMsgType", "error");
            //context.HttpContext.Session.SetString("TempMsgTitle", "提醒您");
            //context.HttpContext.Session.SetString("TempMsg", "無本頁面使用權限");
        }

        private void GetUserRolePower(ActionExecutingContext context)
        {
            //Boolean PowerChk = true;


            dbContext db = new dbContext();
            //取得目前controller name
            String _controller = context.RouteData.Values["controller"].ToString();
            //取得目前action name
            String _action = context.RouteData.Values["action"].ToString();
            //組合Url
            //String Url = "/" + _controller + "/Index";
            String _QString = context.HttpContext.Request.QueryString.Value;
            //String Url = "/" + _controller + "/" + _action;

            String Url = "/" + _controller + "/";
            if (_action.Contains("Index"))
            {
                Url += _action;
            }
            else if (_action.Contains("Modify"))
            {
                string NewUrl = _action.Substring(0, _action.Length - 6) + "Index";
                Url += NewUrl;
            }
            else
            {
                Url += "Index";
            }


            //取得USER POWER
            var user = context.HttpContext.User;
            var claims = user.Identities.First().Claims.ToArray();
            Guid R_UID = Guid.Parse(claims[2].Value);
            ViewMenuRolePower? rp = db.ViewMenuRolePowers.Where(x => x.RUid == R_UID && x.MUrl.Equals(Url)).FirstOrDefault();

            //檢查權限
            //
            if (!(rp is null) && rp.SearchData == true) //Index
            {
                Boolean PowerChk = false;
                if (!string.IsNullOrEmpty(_QString) && _QString.Contains("page"))
                {
                    PowerChk = true;
                }
                else
                {
                    if (_action.Contains("Index") && rp.SearchData == true)
                    {
                        PowerChk = true;
                    }
                    else if (_action.Contains("Add") && rp.CreateData == true)
                    //else if (_action.Contains("Modify") && string.IsNullOrEmpty(_QString) && rp.CreateData)                      
                    {
                        PowerChk = true;
                    }
                    else if (_action.Contains("Modify") && rp.ModifyData == true)
                    //else if (_action.Contains("Modify") && !string.IsNullOrEmpty(_QString) && rp.ModifyData)
                    {
                        PowerChk = true;
                    }
                    else if (_action.Contains("Delete") && rp.DeleteData == true)
                    {
                        PowerChk = true;
                    }
                    else if (_action.Contains("Import") && rp.ImportData == true)
                    {
                        PowerChk = true;
                    }
                    else if (_action.Contains("Export") && rp.Exportdata == true)
                    {
                        PowerChk = true;
                    }
                    else
                    {
                        PowerChk = false;
                    }
                }

                if (!PowerChk)
                {
                    UserRedirctPage(context, "F");
                }
            }
            else
            {
                UserRedirctPage(context, "F");
            }



        }

        private void UserRedirctPage(ActionExecutingContext context, String Type)
        {
            String _controller = string.Empty;
            String _action = "Index";

            _controller = Type.Equals("L") ? "Sys_Login" : "B_Home";

            RouteValueDictionary dictionary = new RouteValueDictionary(new { controller = _controller, action = _action });
            if (_controller.Equals("B_Home"))
            {
                context.HttpContext.Session.SetString("TempMsgType", "error");
                context.HttpContext.Session.SetString("TempMsgTitle", "提醒您");
                context.HttpContext.Session.SetString("TempMsg", "無本頁面使用權限");
            }
            else
            {
                context.HttpContext.Session.Clear();
            }

            context.Result = new RedirectToRouteResult(dictionary);
        }
    }
}

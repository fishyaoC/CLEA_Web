using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using X.PagedList;

namespace Clea_Web.Service
{
    //前後台-講師專區公佈欄
    public class LectorBtnService : BaseService
    {
        //private UserLectorBtnViewModel.Modify vm = new UserLectorBtnViewModel.Modify();


        public LectorBtnService(dbContext dbContext)
        {
            db = dbContext;
        }

    }
}


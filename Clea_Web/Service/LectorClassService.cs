using System.Diagnostics.Contracts;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using X.PagedList;

namespace Clea_Web.Service
{
	//後台-評鑑模組 0=課程、1=教材
	public class LectorClassService : BaseService
	{
		public LectorClassService(dbContext dbContext)
		{
			db = dbContext;
		}

		#region Index
		public IPagedList<LectorClassViewModel.ClassMenu> GetClassMenuPageList(Int32 page)
		{
			Guid L_UID = Guid.Parse(GetUserID(user));
			List<LectorClassViewModel.ClassMenu> result = new List<LectorClassViewModel.ClassMenu>();

			result = (from CL in db.ViewAssignViewPs
					  where CL.LUid == L_UID && CL.LevType == 0 && string.IsNullOrEmpty(CL.Remark)
					  select new LectorClassViewModel.ClassMenu()
					  {
						  CL_UID = CL.ClUid,
						  ClassName = string.IsNullOrEmpty(CL.CName) ? string.Empty : CL.CName,
						  SubName = string.IsNullOrEmpty(CL.DName) ? string.Empty : CL.DName,
						  IsUpload = CL.FMatchKey == null ? false : true
					  }).ToList();

			return result.ToPagedList(page, pagesize);
		}
		#endregion

		#region Modify

		#endregion
	}
}


using Clea_Web.Models;
using System.ComponentModel;
using X.PagedList;

namespace Clea_Web.ViewModels
{
    public class LectorClassViewModel : BaseViewModel
    {
		#region Index
		public IPagedList<ClassMenu> classMenuPageList { get; set; }
		public class ClassMenu
		{
			public Guid CL_UID { get; set; }

			public String ClassName { get; set; }

			public String SubName { get; set; }
			public Boolean IsUpload { get; set; }
		}

		#endregion

		#region Modify
		#endregion
	}
}

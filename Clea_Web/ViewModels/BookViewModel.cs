using Clea_Web.Models;
using X.PagedList;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Clea_Web.ViewModels
{
	//教材評鑑
	public class BookViewModel : BaseViewModel
	{

		#region book
		public class SearchModel
		{
			public SchBookItem schBookItem { get; set; }
			public IPagedList<bookInfo> bookListsPageList { get; set; }
		}

		public class SchBookItem
		{
			public String? M_Index { get; set; }
			public String? M_Name { get; set; }
			public String? BP_Name { get; set; }
		}

		public class bookInfo
		{
			public Guid M_ID { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			public String M_Index { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			public String M_Name { get; set; }

			[DisplayName("出版社KEY")]
			[Required(ErrorMessage = "請選擇訓練單位!")]
			public Guid BP_ID { get; set; }

			public List<SelectListItem> PublishItemList { get; set; }
			public List<bdInfo> lst_pubList { get; set; }

			[DisplayName("備查日期")]
			[Required(ErrorMessage = "此欄位為必填!")]
			public String R_Date { get; set; }

			[DisplayName("備查文號")]
			[Required(ErrorMessage = "此欄位為必填!")]
			public String R_Number { get; set; }
		}

		public class bdInfo: PublishInfo
		{
			public Guid MD_ID { get; set; }
			public String R_Number { get; set; }
			public String R_Date { get; set; }

		}
		#endregion

		#region publish
		public class SearchPModel
		{
			public SchBookItem schBookItem { get; set; }
			public IPagedList<PublishInfo> PublishInfoListsPageList { get; set; }
		}
		public class PublishInfo
		{
			public Guid BP_ID { get; set; }

			[Required(ErrorMessage = "此欄位為必填!")]
			public String BP_Name { get; set; }
		}
		#endregion
	}
}

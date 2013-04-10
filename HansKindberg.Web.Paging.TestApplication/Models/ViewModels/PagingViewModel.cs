using System.Collections.Generic;

namespace HansKindberg.Web.Paging.TestApplication.Models.ViewModels
{
	public class PagingViewModel : ViewModel
	{
		#region Properties

		public virtual IPagingResult PagingResult { get; set; }
		public virtual IEnumerable<ListItem> Records { get; set; }

		#endregion
	}
}
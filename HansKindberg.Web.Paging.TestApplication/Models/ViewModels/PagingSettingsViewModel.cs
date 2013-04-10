using System.ComponentModel.DataAnnotations;

namespace HansKindberg.Web.Paging.TestApplication.Models.ViewModels
{
	public class PagingSettingsViewModel : ViewModel
	{
		#region Properties

		[Display(Name = "Maximum number of displayed pages")]
		[Range(1, int.MaxValue)]
		[Required]
		public virtual int MaximumNumberOfDisplayedPages { get; set; }

		[Display(Name = "Number of records")]
		[Range(0, int.MaxValue)]
		[Required]
		public virtual int NumberOfRecords { get; set; }

		[Display(Name = "Page-index querystring-key")]
		[RegularExpression("([a-zA-Z]+)", ErrorMessage = "Enter only alphabetical letters.")]
		public virtual string PageIndexQueryStringKey { get; set; }

		[Display(Name = "Page-size")]
		[Range(1, int.MaxValue)]
		[Required]
		public virtual int PageSize { get; set; }

		[Display(Name = "Enable paging")]
		public virtual bool PagingEnabled { get; set; }

		#endregion
	}
}
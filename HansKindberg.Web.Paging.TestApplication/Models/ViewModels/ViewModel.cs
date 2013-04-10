namespace HansKindberg.Web.Paging.TestApplication.Models.ViewModels
{
	public abstract class ViewModel
	{
		#region Properties

		public virtual string Language { get; set; }
		public virtual string MetaDescription { get; set; }
		public virtual string MetaKeywords { get; set; }
		public virtual string Title { get; set; }

		#endregion
	}
}
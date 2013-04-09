using System.Web.Mvc;
using HansKindberg.Web.Paging.TestApplication.Models.ViewModels;

namespace HansKindberg.Web.Paging.TestApplication.Controllers
{
	public class PagingController : Controller
	{
		#region Methods

		public virtual ActionResult Index()
		{
			PagingViewModel model = new PagingViewModel();

			this.PopulateModel(model);

			return View(model);
		}

		protected internal virtual void PopulateModel(PagingViewModel model)
		{
			base.PopulateModel(model);

			model.Heading = model.Title = "Paging sample";
		}

		#endregion
	}
}
using System.Web.Mvc;
using HansKindberg.Web.Paging.TestApplication.Models.ViewModels;

namespace HansKindberg.Web.Paging.TestApplication.Controllers
{
	public class HomeController : Controller
	{
		#region Methods

		public virtual ActionResult Index()
		{
			HomeViewModel model = new HomeViewModel();

			this.PopulateModel(model);

			return View(model);
		}

		protected internal virtual void PopulateModel(HomeViewModel model)
		{
			base.PopulateModel(model);

			model.Title = "Home";
		}

		#endregion
	}
}
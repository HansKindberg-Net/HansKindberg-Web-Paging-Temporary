using System;
using System.Threading;
using HansKindberg.Web.Paging.TestApplication.Models.ViewModels;

namespace HansKindberg.Web.Paging.TestApplication.Controllers
{
	public class Controller : System.Web.Mvc.Controller
	{
		#region Methods

		protected internal virtual void PopulateModel(ViewModel model)
		{
			if(model == null)
				throw new ArgumentNullException("model");

			model.Language = Thread.CurrentThread.CurrentUICulture.Name;
			model.MetaDescription = string.Empty;
			model.MetaKeywords = string.Empty;
		}

		#endregion
	}
}
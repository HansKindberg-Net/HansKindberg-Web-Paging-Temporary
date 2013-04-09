using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace HansKindberg.Web.Paging.TestApplication.Business.Configuration
{
	public static class RouteConfiguration
	{
		#region Methods

		public static void RegisterRoutes(RouteCollection routes)
		{
			if(routes == null)
				throw new ArgumentNullException("routes");

			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
				);
		}

		#endregion
	}
}
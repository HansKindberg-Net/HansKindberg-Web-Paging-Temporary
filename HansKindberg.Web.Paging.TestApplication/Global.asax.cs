using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using HansKindberg.Web.Paging.TestApplication.Business.Configuration;

namespace HansKindberg.Web.Paging.TestApplication
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801
	public class Global : System.Web.HttpApplication
	{
		#region Methods

		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			WebApiConfiguration.Register(GlobalConfiguration.Configuration);
			FilterConfiguration.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfiguration.RegisterRoutes(RouteTable.Routes);

			//Optimizer.AddStyleBundle(BundleTable.Bundles);
			//Optimizer.AddScriptBundle(BundleTable.Bundles);

			//BundleTable.EnableOptimizations = false;
		}

		#endregion
	}
}
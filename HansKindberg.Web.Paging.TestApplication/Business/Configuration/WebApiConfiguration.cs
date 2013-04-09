using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http;

namespace HansKindberg.Web.Paging.TestApplication.Business.Configuration
{
	[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Api")]
	public static class WebApiConfiguration
	{
		#region Methods

		public static void Register(HttpConfiguration configuration)
		{
			if(configuration == null)
				throw new ArgumentNullException("configuration");

			configuration.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new {id = RouteParameter.Optional}
				);
		}

		#endregion
	}
}
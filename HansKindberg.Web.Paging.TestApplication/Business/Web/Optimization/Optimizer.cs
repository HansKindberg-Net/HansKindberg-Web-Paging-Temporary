using System;
using System.Web.Optimization;

namespace HansKindberg.Web.Paging.TestApplication.Business.Web.Optimization
{
	public static class Optimizer
	{
		#region Methods

		public static void AddScriptBundle(BundleCollection bundles)
		{
			if(bundles == null)
				throw new ArgumentNullException("bundles");

			bundles.Add(new ScriptBundle("~/Scripts").Include(
				"~/Scripts/jquery.js",
				"~/Scripts/custom.js",
				"~/Scripts/jquery.fancybox.js",
				"~/Scripts/jquery.masonry.js",
				"~/Scripts/temporary.masonry.js"));
		}

		public static void AddStyleBundle(BundleCollection bundles)
		{
			if(bundles == null)
				throw new ArgumentNullException("bundles");

			bundles.Add(new StyleBundle("~/Style/Css").Include(
				"~/Style/main.css",
				"~/Style/tablet.css",
				"~/Style/mobile.css",
				"~/Style/jquery.fancybox.css",
				"~/Style/temporary.css"));
		}

		#endregion
	}
}
using HansKindberg.Web.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Web.Tests.Paging
{
	[TestClass]
	public class PageTest
	{
		#region Methods

		[TestMethod]
		public void First_Get_ShouldReturnFalseByDefault()
		{
			Assert.IsFalse(new Page().FirstInGroup);
		}

		[TestMethod]
		public void Index_Get_ShouldReturnZeroByDefault()
		{
			Assert.AreEqual(0, new Page().Index);
		}

		[TestMethod]
		public void Last_Get_ShouldReturnFalseByDefault()
		{
			Assert.IsFalse(new Page().LastInGroup);
		}

		[TestMethod]
		public void Url_Get_ShouldReturnNullByDefault()
		{
			Assert.IsNull(new Page().Url);
		}

		#endregion
	}
}
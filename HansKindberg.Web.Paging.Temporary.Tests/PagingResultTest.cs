using System;
using System.Globalization;
using System.Linq;
using HansKindberg.Web.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Web.Tests.Paging
{
	[TestClass]
	public class PagingResultTest
	{
		#region Methods

		private static int GetRandomNegativeInteger()
		{
			return (DateTime.Now.Millisecond + 1)*-1;
		}

		[TestMethod]
		public void NextPageGroupUrl_Get_ShouldReturnNullByDefault()
		{
			Assert.IsNull(new PagingResult().NextPageGroupUrl);
		}

		[TestMethod]
		public void NextPageUrl_Get_ShouldReturnNullByDefault()
		{
			Assert.IsNull(new PagingResult().NextPageUrl);
		}

		[TestMethod]
		public void Pages_Get_ShouldReturnAnEmptyEnumerableDefault()
		{
			Assert.IsFalse(new PagingResult().Pages.Any());
		}

		[TestMethod]
		public void PreviousPageGroupUrl_Get_ShouldReturnNullByDefault()
		{
			Assert.IsNull(new PagingResult().PreviousPageGroupUrl);
		}

		[TestMethod]
		public void PreviousPageUrl_Get_ShouldReturnNullByDefault()
		{
			Assert.IsNull(new PagingResult().PreviousPageUrl);
		}

		[TestMethod]
		public void TotalNumberOfPages_Get_ShouldReturnZeroByDefault()
		{
			Assert.AreEqual(0, new PagingResult().TotalNumberOfPages);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void TotalNumberOfPages_Set_IfTheValueParameterIsLessThanZero_ShouldThrowAnArgumentException()
		{
			const string parameterName = "value";

			string expectedExceptionMessage = string.Format(CultureInfo.InvariantCulture, "The total number of pages can not be less than zero.{0}Parameter name: {1}", Environment.NewLine, parameterName);

			try
			{
				new PagingResult().TotalNumberOfPages = GetRandomNegativeInteger();
			}
			catch(ArgumentException argumentException)
			{
				if(argumentException.Message == expectedExceptionMessage && argumentException.ParamName == parameterName)
					throw;
			}
		}

		[TestMethod]
		public void TotalNumberOfRecords_Get_ShouldReturnZeroByDefault()
		{
			Assert.AreEqual(0, new PagingResult().TotalNumberOfRecords);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void TotalNumberOfRecords_Set_IfTheValueParameterIsLessThanZero_ShouldThrowAnArgumentException()
		{
			const string parameterName = "value";

			string expectedExceptionMessage = string.Format(CultureInfo.InvariantCulture, "The total number of records can not be less than zero.{0}Parameter name: {1}", Environment.NewLine, parameterName);

			try
			{
				new PagingResult().TotalNumberOfRecords = GetRandomNegativeInteger();
			}
			catch(ArgumentException argumentException)
			{
				if(argumentException.Message == expectedExceptionMessage && argumentException.ParamName == parameterName)
					throw;
			}
		}

		#endregion
	}
}
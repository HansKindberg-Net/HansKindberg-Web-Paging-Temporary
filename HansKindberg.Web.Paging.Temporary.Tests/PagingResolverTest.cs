using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using HansKindberg.Web.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.Web.Tests.Paging
{
	[TestClass]
	public class PagingResolverTest
	{
		#region Fields

		private static CultureInfo _currentCulture;
		private static CultureInfo _currentUiCulture;
		private static readonly Uri _relativeTestUrl = new Uri("/RelativePath", UriKind.Relative);

		#endregion

		#region Methods

		[ClassCleanup]
		public static void ClassCleanup()
		{
			Thread.CurrentThread.CurrentCulture = _currentCulture;
			Thread.CurrentThread.CurrentUICulture = _currentUiCulture;
		}

		[ClassInitialize]
		public static void ClassInitialize(TestContext testContext)
		{
			_currentCulture = Thread.CurrentThread.CurrentCulture;
			_currentUiCulture = Thread.CurrentThread.CurrentUICulture;

			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
		}

		private static int GetRandomNegativeInteger()
		{
			return (DateTime.Now.Millisecond + 1)*-1;
		}

		[TestMethod]
		public void MaximumNumberOfDisplayedPages_Get_ShouldReturnIntegerMaximumValueByDefault()
		{
			Assert.AreEqual(int.MaxValue, new PagingResolver().MaximumNumberOfDisplayedPages);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void MaximumNumberOfDisplayedPages_Set_IfTheValueIsLessThanOne_ShouldThrowAnArgumentException()
		{
			const string parameterName = "value";
			string expectedExceptionMessage = string.Format(CultureInfo.InvariantCulture, "The maximum number of displayed pages must be greater than zero.{0}Parameter name: {1}", Environment.NewLine, parameterName);

			try
			{
				new PagingResolver().MaximumNumberOfDisplayedPages = GetRandomNegativeInteger() + 1;
			}
			catch(ArgumentException argumentException)
			{
				if(argumentException.Message == expectedExceptionMessage && argumentException.ParamName == parameterName)
					throw;
			}
		}

		[TestMethod]
		public void PageSize_Get_ShouldReturnIntegerMaximumValueByDefault()
		{
			Assert.AreEqual(int.MaxValue, new PagingResolver().PageSize);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void PageSize_Set_IfTheValueIsLessThanOne_ShouldThrowAnArgumentException()
		{
			const string parameterName = "value";
			string expectedExceptionMessage = string.Format(CultureInfo.InvariantCulture, "The page-size must be greater than zero.{0}Parameter name: {1}", Environment.NewLine, parameterName);

			try
			{
				new PagingResolver().PageSize = GetRandomNegativeInteger() + 1;
			}
			catch(ArgumentException argumentException)
			{
				if(argumentException.Message == expectedExceptionMessage && argumentException.ParamName == parameterName)
					throw;
			}
		}

		[TestMethod]
		public void PagingEnabled_Get_ShouldReturnTrueByDefault()
		{
			Assert.IsTrue(new PagingResolver().PagingEnabled);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		[SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
		public void Resolve_IfThePageIndexQueryStringKeyParameterIsEmpty_ShouldThrowAnArgumentException()
		{
			List<Func<IPagingResult>> resolveMethods = new List<Func<IPagingResult>>
				{
					() => new PagingResolver().Resolve(string.Empty, 0, new Uri("http://localhost")),
					() => new PagingResolver().Resolve(string.Empty, new ArrayList(), new Uri("http://localhost"))
				};

			int expectedNumberOfExceptions = 0;
			const string parameterName = "pageIndexQueryStringKey";
			string expectedExceptionMessage = string.Format(CultureInfo.InvariantCulture, "The page-index-querystring-key can not be empty.{0}Parameter name: {1}", Environment.NewLine, parameterName);

			foreach(var resolveMethod in resolveMethods)
			{
				try
				{
					resolveMethod.Invoke();
				}
				catch(ArgumentException argumentException)
				{
					if(argumentException.Message == expectedExceptionMessage && argumentException.ParamName == parameterName)
						expectedNumberOfExceptions++;
				}
			}

			if(expectedNumberOfExceptions == 2)
				throw new ArgumentException();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		[SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
		public void Resolve_IfThePageIndexQueryStringKeyParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			List<Func<IPagingResult>> resolveMethods = new List<Func<IPagingResult>>
				{
					() => new PagingResolver().Resolve(null, 0, new Uri("http://localhost")),
					() => new PagingResolver().Resolve(null, new ArrayList(), new Uri("http://localhost"))
				};

			int expectedNumberOfExceptions = 0;

			foreach(var resolveMethod in resolveMethods)
			{
				try
				{
					resolveMethod.Invoke();
				}
				catch(ArgumentNullException argumentNullException)
				{
					if(argumentNullException.ParamName == "pageIndexQueryStringKey")
						expectedNumberOfExceptions++;
				}
			}

			if(expectedNumberOfExceptions == 2)
				throw new ArgumentNullException();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		[SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
		public void Resolve_IfThePageIndexQueryStringKeyParameterValueIsNotTheSameUrlEncoded_ShouldThrowAnArgumentException()
		{
			const string testPageIndexQueryStringKey = "Test?";

			List<Func<IPagingResult>> resolveMethods = new List<Func<IPagingResult>>
				{
					() => new PagingResolver().Resolve(testPageIndexQueryStringKey, 0, new Uri("http://localhost")),
					() => new PagingResolver().Resolve(testPageIndexQueryStringKey, new ArrayList(), new Uri("http://localhost"))
				};

			int expectedNumberOfExceptions = 0;
			const string parameterName = "pageIndexQueryStringKey";
			string expectedExceptionMessage = string.Format(CultureInfo.InvariantCulture, "The value \"{0}\" is not a valid page-index-querystring-key.{1}Parameter name: {2}", testPageIndexQueryStringKey, Environment.NewLine, parameterName);

			foreach(var resolveMethod in resolveMethods)
			{
				try
				{
					resolveMethod.Invoke();
				}
				catch(ArgumentException argumentException)
				{
					if(argumentException.Message == expectedExceptionMessage && argumentException.ParamName == parameterName)
						expectedNumberOfExceptions++;
				}
			}

			if(expectedNumberOfExceptions == 2)
				throw new ArgumentException();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		[SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
		public void Resolve_IfTheTotalNumberOfRecordsParameterIsLessThanZero_ShouldThrowAnArgumentException()
		{
			List<Func<IPagingResult>> resolveMethods = new List<Func<IPagingResult>>
				{
					() => new PagingResolver().Resolve(GetRandomNegativeInteger(), new Uri("http://localhost")),
					() => new PagingResolver().Resolve(0, GetRandomNegativeInteger(), new Uri("http://localhost")),
					() => new PagingResolver().Resolve("Test", GetRandomNegativeInteger(), new Uri("http://localhost")),
				};

			int expectedNumberOfExceptions = 0;
			const string parameterName = "totalNumberOfRecords";
			string expectedExceptionMessage = string.Format(CultureInfo.InvariantCulture, "The total number of records can not be less than zero.{0}Parameter name: {1}", Environment.NewLine, parameterName);

			foreach(var resolveMethod in resolveMethods)
			{
				try
				{
					resolveMethod.Invoke();
				}
				catch(ArgumentException argumentException)
				{
					if(argumentException.Message == expectedExceptionMessage && argumentException.ParamName == parameterName)
						expectedNumberOfExceptions++;
				}
			}

			if(expectedNumberOfExceptions == 3)
				throw new ArgumentException();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		[SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
		public void Resolve_IfTheUrlParameterIsNotAbsolute_ShouldThrowAnArgumentException()
		{
			List<Func<IPagingResult>> resolveMethods = new List<Func<IPagingResult>>
				{
					() => new PagingResolver().Resolve(0, _relativeTestUrl),
					() => new PagingResolver().Resolve(new ArrayList(), _relativeTestUrl),
					() => new PagingResolver().Resolve(0, 0, _relativeTestUrl),
					() => new PagingResolver().Resolve(0, new ArrayList(), _relativeTestUrl),
					() => new PagingResolver().Resolve("Test", 0, _relativeTestUrl),
					() => new PagingResolver().Resolve("Test", new ArrayList(), _relativeTestUrl)
				};

			int expectedNumberOfExceptions = 0;
			const string parameterName = "url";
			string expectedExceptionMessage = string.Format(CultureInfo.InvariantCulture, "The url must be absolute.{0}Parameter name: {1}", Environment.NewLine, parameterName);

			foreach(var resolveMethod in resolveMethods)
			{
				try
				{
					resolveMethod.Invoke();
				}
				catch(ArgumentException argumentException)
				{
					if(argumentException.Message == expectedExceptionMessage && argumentException.ParamName == parameterName)
						expectedNumberOfExceptions++;
				}
			}

			if(expectedNumberOfExceptions == 6)
				throw new ArgumentException();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		[SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
		public void Resolve_IfTheUrlParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			List<Func<IPagingResult>> resolveMethods = new List<Func<IPagingResult>>
				{
					() => new PagingResolver().Resolve(0, null),
					() => new PagingResolver().Resolve(new ArrayList(), null),
					() => new PagingResolver().Resolve(0, 0, null),
					() => new PagingResolver().Resolve(0, new ArrayList(), null),
					() => new PagingResolver().Resolve("Test", 0, null),
					() => new PagingResolver().Resolve("Test", new ArrayList(), null)
				};

			int expectedNumberOfExceptions = 0;

			foreach(var resolveMethod in resolveMethods)
			{
				try
				{
					resolveMethod.Invoke();
				}
				catch(ArgumentNullException argumentNullException)
				{
					if(argumentNullException.ParamName == "url")
						expectedNumberOfExceptions++;
				}
			}

			if(expectedNumberOfExceptions == 6)
				throw new ArgumentNullException();
		}

		#endregion
	}
}
using System;
using System.Collections.Generic;

namespace HansKindberg.Web.Paging
{
	public interface IPagingResult
	{
		#region Properties

		Uri FirstPageUrl { get; }
		Uri LastPageUrl { get; }
		Uri NextPageGroupUrl { get; }
		Uri NextPageUrl { get; }
		int PageIndex { get; }
		IEnumerable<IPage> Pages { get; }
		Uri PreviousPageGroupUrl { get; }
		Uri PreviousPageUrl { get; }
		int TotalNumberOfPages { get; }
		int TotalNumberOfRecords { get; }

		#endregion
	}
}
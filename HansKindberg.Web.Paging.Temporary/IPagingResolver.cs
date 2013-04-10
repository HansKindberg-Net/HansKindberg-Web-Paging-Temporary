using System;
using System.Collections;

namespace HansKindberg.Web.Paging
{
	public interface IPagingResolver
	{
		#region Properties

		int MaximumNumberOfDisplayedPages { get; set; }
		string PageIndexQueryStringKey { get; set; }
		int PageSize { get; set; }
		bool PagingEnabled { get; set; }

		#endregion

		#region Methods

		IPagingResult Resolve(int totalNumberOfRecords, Uri url);
		IPagingResult Resolve(IList records, Uri url);
		IPagingResult Resolve(string pageIndexQueryStringKey, int totalNumberOfRecords, Uri url);
		IPagingResult Resolve(string pageIndexQueryStringKey, IList records, Uri url);
		IPagingResult Resolve(int pageIndex, int totalNumberOfRecords, Uri url);
		IPagingResult Resolve(int pageIndex, IList records, Uri url);

		#endregion
	}
}
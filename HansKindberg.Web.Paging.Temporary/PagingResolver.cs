using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Web;

namespace HansKindberg.Web.Paging
{
	public class PagingResolver : IPagingResolver
	{
		#region Fields

		public const string DefaultPageIndexQueryStringKey = "PageIndex";
		private const int _defaultPageIndex = 0;
		private int _maximumNumberOfDisplayedPages = int.MaxValue;
		private string _pageIndexQueryStringKey;
		private int _pageSize = int.MaxValue;
		private bool _pagingEnabled = true;

		#endregion

		#region Properties

		public virtual int MaximumNumberOfDisplayedPages
		{
			get { return this._maximumNumberOfDisplayedPages; }
			set
			{
				if(value < 1)
					throw new ArgumentException("The maximum number of displayed pages must be greater than zero.", "value");

				this._maximumNumberOfDisplayedPages = value;
			}
		}

		public virtual string PageIndexQueryStringKey
		{
			get { return this._pageIndexQueryStringKey ?? DefaultPageIndexQueryStringKey; }
			set
			{
				if(value != null)
					this.ValidatePageIndexQueryStringKey(value);

				this._pageIndexQueryStringKey = value;
			}
		}

		public virtual int PageSize
		{
			get { return this._pageSize; }
			set
			{
				if(value < 1)
					throw new ArgumentException("The page-size must be greater than zero.", "value");

				this._pageSize = value;
			}
		}

		public virtual bool PagingEnabled
		{
			get { return this._pagingEnabled; }
			set { this._pagingEnabled = value; }
		}

		#endregion

		#region Methods

		protected virtual int Add(int firstNumber, int secondNumber)
		{
			try
			{
				return checked(firstNumber + secondNumber);
			}
			catch(OverflowException)
			{
				return int.MaxValue;
			}
		}

		protected virtual int GetPageIndexFromQueryString(string pageIndexQueryStringKey, bool validatePageIndexQueryStringKey, NameValueCollection queryString)
		{
			if(validatePageIndexQueryStringKey)
				this.ValidatePageIndexQueryStringKey(pageIndexQueryStringKey);

			int pageIndex = _defaultPageIndex;

			if(queryString != null && queryString.Count > 0)
			{
				string pageIndexString = queryString[pageIndexQueryStringKey];

				if(!string.IsNullOrEmpty(pageIndexString))
				{
					if(!int.TryParse(pageIndexString, out pageIndex))
						pageIndex = _defaultPageIndex;
				}
			}

			return pageIndex;
		}

		protected virtual int GetTotalNumberOfPages(int totalNumberOfRecords, bool validateTotalNumberOfRecords)
		{
			if(validateTotalNumberOfRecords)
				this.ValidateTotalNumberOfRecords(totalNumberOfRecords);

			int totalNumberOfPages = 0;

			if(totalNumberOfRecords > 0)
			{
				if(this.PagingEnabled)
				{
					int remainder;

					totalNumberOfPages = Math.DivRem(totalNumberOfRecords, this.PageSize, out remainder);

					if(remainder > 0)
						totalNumberOfPages++;
				}
				else
				{
					totalNumberOfPages = 0;
				}
			}

			return totalNumberOfPages;
		}

		[SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#")]
		[SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "4#")]
		protected virtual int GetValidatedPageIndex(int pageIndex, int totalNumberOfPages, Uri url, out UriBuilder uriBuilder, out NameValueCollection queryString)
		{
			this.ValidatePageIndexAndTotalNumberOfPages(ref pageIndex, totalNumberOfPages, false);

			this.ValidateUrl(url, out uriBuilder, out queryString);

			return pageIndex;
		}

		[SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#")]
		[SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "4#")]
		protected virtual int GetValidatedPageIndex(string pageIndexQueryStringKey, int totalNumberOfPages, Uri url, out UriBuilder uriBuilder, out NameValueCollection queryString)
		{
			this.ValidatePageIndexQueryStringKey(pageIndexQueryStringKey);

			this.ValidateUrl(url, out uriBuilder, out queryString);

			int pageIndex = this.GetPageIndexFromQueryString(pageIndexQueryStringKey, false, queryString);

			this.ValidatePageIndexAndTotalNumberOfPages(ref pageIndex, totalNumberOfPages, true);

			return pageIndex;
		}

		protected virtual int GetValidatedTotalNumberOfRecords(int totalNumberOfRecords)
		{
			this.ValidateTotalNumberOfRecords(totalNumberOfRecords);

			return totalNumberOfRecords;
		}

		protected virtual int Multiply(int firstNumber, int secondNumber)
		{
			try
			{
				return checked(firstNumber*secondNumber);
			}
			catch(OverflowException)
			{
				return int.MaxValue;
			}
		}

		public virtual IPagingResult Resolve(int totalNumberOfRecords, Uri url)
		{
			return this.Resolve(this.PageIndexQueryStringKey, totalNumberOfRecords, url);
		}

		public virtual IPagingResult Resolve(IList records, Uri url)
		{
			return this.Resolve(this.PageIndexQueryStringKey, records, url);
		}

		public virtual IPagingResult Resolve(string pageIndexQueryStringKey, int totalNumberOfRecords, Uri url)
		{
			return this.Resolve(null, pageIndexQueryStringKey, totalNumberOfRecords, null, url);
		}

		public virtual IPagingResult Resolve(string pageIndexQueryStringKey, IList records, Uri url)
		{
			return this.Resolve(null, pageIndexQueryStringKey, null, records, url);
		}

		public virtual IPagingResult Resolve(int pageIndex, int totalNumberOfRecords, Uri url)
		{
			return this.Resolve(pageIndex, null, totalNumberOfRecords, null, url);
		}

		public virtual IPagingResult Resolve(int pageIndex, IList records, Uri url)
		{
			return this.Resolve(pageIndex, null, null, records, url);
		}

		protected virtual IPagingResult Resolve(int? nullablePageIndex, string pageIndexQueryStringKey, int? nullableTotalNumberOfRecords, IList records, Uri url)
		{
			int totalNumberOfRecords = nullableTotalNumberOfRecords.HasValue ? this.GetValidatedTotalNumberOfRecords(nullableTotalNumberOfRecords.Value) : (records != null ? records.Count : 0);

			int totalNumberOfPages = this.GetTotalNumberOfPages(totalNumberOfRecords, false);

			PagingResult pagingResult = new PagingResult {TotalNumberOfPages = totalNumberOfPages, TotalNumberOfRecords = totalNumberOfRecords};

			if(this.PagingEnabled && totalNumberOfPages > 1)
			{
				int pageIndex;
				UriBuilder uriBuilder;
				NameValueCollection queryString;

				if(nullablePageIndex.HasValue)
					pageIndex = this.GetValidatedPageIndex(nullablePageIndex.Value, totalNumberOfPages, url, out uriBuilder, out queryString);
				else
					pageIndex = this.GetValidatedPageIndex(pageIndexQueryStringKey, totalNumberOfPages, url, out uriBuilder, out queryString);

				pagingResult.PageIndex = pageIndex;

				int quotient = pageIndex/this.MaximumNumberOfDisplayedPages;

				IEnumerable<int> indexes = Enumerable.Range(0, totalNumberOfPages).Skip(this.Multiply(quotient, this.MaximumNumberOfDisplayedPages)).Take(this.MaximumNumberOfDisplayedPages).ToArray();
				List<IPage> pages = new List<IPage>();

				for(int i = 0; i < indexes.Count(); i++)
				{
					bool current = indexes.ElementAt(i) == pageIndex;
					bool firstInGroup = i == 0;
					bool firstPage = indexes.ElementAt(i) == 0;
					int index = indexes.ElementAt(i);
					bool lastInGroup = i == indexes.Count() - 1;
					bool lastPage = indexes.ElementAt(i) == totalNumberOfPages - 1;

					queryString.Set(this.PageIndexQueryStringKey, index.ToString(CultureInfo.InvariantCulture));

					uriBuilder.Query = queryString.ToString();

					pages.Add(new Page {Current = current, FirstInGroup = firstInGroup, FirstPage = firstPage, Index = index, LastInGroup = lastInGroup, LastPage = lastPage, Url = uriBuilder.Uri});
				}

				pagingResult.Pages = pages.ToArray();

				queryString.Set(this.PageIndexQueryStringKey, 0.ToString(CultureInfo.InvariantCulture));
				uriBuilder.Query = queryString.ToString();
				pagingResult.FirstPageUrl = uriBuilder.Uri;

				if(pageIndex > 0)
				{
					queryString.Set(this.PageIndexQueryStringKey, (pageIndex - 1).ToString(CultureInfo.InvariantCulture));
					uriBuilder.Query = queryString.ToString();
					pagingResult.PreviousPageUrl = uriBuilder.Uri;

					if(quotient > 0)
					{
						queryString.Set(this.PageIndexQueryStringKey, (this.Multiply(quotient, this.MaximumNumberOfDisplayedPages) - 1).ToString(CultureInfo.InvariantCulture));
						uriBuilder.Query = queryString.ToString();
						pagingResult.PreviousPageGroupUrl = uriBuilder.Uri;
					}
				}

				queryString.Set(this.PageIndexQueryStringKey, (totalNumberOfPages - 1).ToString(CultureInfo.InvariantCulture));
				uriBuilder.Query = queryString.ToString();
				pagingResult.LastPageUrl = uriBuilder.Uri;

				if(pageIndex < totalNumberOfPages - 1)
				{
					queryString.Set(this.PageIndexQueryStringKey, (pageIndex + 1).ToString(CultureInfo.InvariantCulture));
					uriBuilder.Query = queryString.ToString();
					pagingResult.NextPageUrl = uriBuilder.Uri;

					int firstIndexInNextPageGroup = this.Add(this.Multiply(quotient, this.MaximumNumberOfDisplayedPages), this.MaximumNumberOfDisplayedPages);

					if(firstIndexInNextPageGroup < totalNumberOfPages)
					{
						queryString.Set(this.PageIndexQueryStringKey, firstIndexInNextPageGroup.ToString(CultureInfo.InvariantCulture));
						uriBuilder.Query = queryString.ToString();
						pagingResult.NextPageGroupUrl = uriBuilder.Uri;
					}
				}

				if(records != null && pages.Any())
				{
					int firstPageIndex = this.Multiply(pageIndex, this.PageSize);
					int lastPageIndex = this.Add(firstPageIndex, this.PageSize) - 1;

					for(int i = records.Count - 1; i >= 0; i--)
					{
						if(i < firstPageIndex || i > lastPageIndex)
							records.RemoveAt(i);
					}
				}
			}

			return pagingResult;
		}

		[SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
		protected virtual void ValidatePageIndex(ref int pageIndex, bool resolve)
		{
			if(pageIndex >= 0)
				return;

			if(!resolve)
				throw new ArgumentException("The page-index must be greater than or equal to zero.", "pageIndex");

			pageIndex = 0;
		}

		[SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
		protected virtual void ValidatePageIndexAndTotalNumberOfPages(ref int pageIndex, int totalNumberOfPages, bool resolve)
		{
			this.ValidatePageIndex(ref pageIndex, resolve);

			if(totalNumberOfPages < 0)
				throw new ArgumentException("The total number of pages can not be less than zero.", "totalNumberOfPages");

			if(pageIndex == 0)
				return;

			if(pageIndex < totalNumberOfPages)
				return;

			if(!resolve)
				throw new ArgumentException("The page-index must be less than the total number of pages.", "pageIndex");

			pageIndex = totalNumberOfPages - 1;
		}

		protected virtual void ValidatePageIndexQueryStringKey(string pageIndexQueryStringKey)
		{
			if(pageIndexQueryStringKey == null)
				throw new ArgumentNullException("pageIndexQueryStringKey");

			if(pageIndexQueryStringKey.Length == 0)
				throw new ArgumentException("The page-index-querystring-key can not be empty.", "pageIndexQueryStringKey");

			if(pageIndexQueryStringKey != HttpUtility.UrlEncode(pageIndexQueryStringKey))
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The value \"{0}\" is not a valid page-index-querystring-key.", pageIndexQueryStringKey), "pageIndexQueryStringKey");
		}

		protected virtual void ValidateTotalNumberOfRecords(int totalNumberOfRecords)
		{
			if(totalNumberOfRecords < 0)
				throw new ArgumentException("The total number of records can not be less than zero.", "totalNumberOfRecords");
		}

		[SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
		[SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#")]
		protected virtual void ValidateUrl(Uri url, out UriBuilder uriBuilder, out NameValueCollection queryString)
		{
			if(url == null)
				throw new ArgumentNullException("url");

			if(!url.IsAbsoluteUri)
				throw new ArgumentException("The url must be absolute.", "url");

			uriBuilder = new UriBuilder(url);

			queryString = HttpUtility.ParseQueryString(uriBuilder.Query);
		}

		#endregion
	}
}
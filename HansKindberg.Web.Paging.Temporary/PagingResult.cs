using System;
using System.Collections.Generic;

namespace HansKindberg.Web.Paging
{
	public class PagingResult : IPagingResult
	{
		#region Fields

		private IEnumerable<IPage> _pages;
		private int _totalNumberOfPages;
		private int _totalNumberOfRecords;

		#endregion

		#region Properties

		public virtual Uri FirstPageUrl { get; set; }
		public virtual Uri LastPageUrl { get; set; }
		public virtual Uri NextPageGroupUrl { get; set; }
		public virtual Uri NextPageUrl { get; set; }
		public virtual int PageIndex { get; set; }

		public virtual IEnumerable<IPage> Pages
		{
			get { return this._pages ?? (this._pages = new IPage[0]); }
			set { this._pages = value; }
		}

		public virtual Uri PreviousPageGroupUrl { get; set; }
		public virtual Uri PreviousPageUrl { get; set; }

		public virtual int TotalNumberOfPages
		{
			get { return this._totalNumberOfPages; }
			set
			{
				if(value < 0)
					throw new ArgumentException("The total number of pages can not be less than zero.", "value");

				this._totalNumberOfPages = value;
			}
		}

		public virtual int TotalNumberOfRecords
		{
			get { return this._totalNumberOfRecords; }
			set
			{
				if(value < 0)
					throw new ArgumentException("The total number of records can not be less than zero.", "value");

				this._totalNumberOfRecords = value;
			}
		}

		#endregion
	}
}
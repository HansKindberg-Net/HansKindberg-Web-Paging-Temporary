using System;

namespace HansKindberg.Web.Paging
{
	public class Page : IPage
	{
		#region Properties

		public virtual bool Current { get; set; }
		public virtual bool FirstInGroup { get; set; }
		public virtual bool FirstPage { get; set; }
		public virtual int Index { get; set; }
		public virtual bool LastInGroup { get; set; }
		public virtual bool LastPage { get; set; }
		public virtual Uri Url { get; set; }

		#endregion
	}
}
using System;

namespace HansKindberg.Web.Paging
{
	public interface IPage
	{
		#region Properties

		bool Current { get; }
		bool FirstInGroup { get; }
		bool FirstPage { get; }
		int Index { get; }
		bool LastInGroup { get; }
		bool LastPage { get; }
		Uri Url { get; }

		#endregion
	}
}
using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity
{
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Ensures we are working with a list of T
		/// </summary>
		internal static IList<T> EnsureIsList<T>(this IEnumerable<T> items)
		{
			return items == null
				? new List<T>()
				: (items as IList<T> ?? items.ToList());
		}
	}
}
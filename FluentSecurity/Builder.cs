using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FluentSecurity
{
	///<summary>
	/// Builds collections of T
	///</summary>
	public abstract class Builder<T> : ICollection<T> where T : class
	{
		protected readonly ICollection<T> _itemValues = new Collection<T>();

		public IEnumerator<T> GetEnumerator()
		{
			return _itemValues.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		void ICollection<T>.Add(T itemValue)
		{
			_itemValues.Add(itemValue);
		}

		void ICollection<T>.Clear()
		{
			_itemValues.Clear();
		}

		bool ICollection<T>.Contains(T itemValue)
		{
			return _itemValues.Contains(itemValue);
		}

		void ICollection<T>.CopyTo(T[] array, int arrayIndex)
		{
			_itemValues.CopyTo(array, arrayIndex);
		}

		bool ICollection<T>.Remove(T itemValue)
		{
			return _itemValues.Remove(itemValue);
		}

		int ICollection<T>.Count
		{
			get { return _itemValues.Count; }
		}

		bool ICollection<T>.IsReadOnly
		{
			get { return false; }
		}
	}
}
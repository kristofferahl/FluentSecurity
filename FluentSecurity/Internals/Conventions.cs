using System;
using System.Collections;
using System.Collections.Generic;

namespace FluentSecurity.Internals
{
	public class Conventions : IEnumerable<IConvention>
	{
		private readonly List<IConvention> _conventions = new List<IConvention>();

		internal Conventions() {}

		internal Conventions(List<IConvention> conventions)
		{
			_conventions = conventions;
		}

		public void Add(IConvention convention)
		{
			_conventions.Add(convention);
		}

		public void Remove(IConvention convention)
		{
			_conventions.Remove(convention);
		}

		public void RemoveAll(Predicate<IConvention> filter)
		{
			_conventions.RemoveAll(filter);
		}

		public void Insert(int index, IConvention convention)
		{
			_conventions.Insert(index, convention);
		}

		public IEnumerator<IConvention> GetEnumerator()
		{
			return _conventions.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
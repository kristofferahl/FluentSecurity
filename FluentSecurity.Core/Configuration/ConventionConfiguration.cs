using System;
using System.Collections.Generic;
using FluentSecurity.Core;

namespace FluentSecurity.Configuration
{
	public class ConventionConfiguration
	{
		private readonly List<IConvention> _conventions = new List<IConvention>();

		public ConventionConfiguration(List<IConvention> conventions)
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
	}
}
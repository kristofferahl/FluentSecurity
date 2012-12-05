using System;
using System.Collections.Generic;
using System.Reflection;
using FluentSecurity.Scanning.TypeScanners;

namespace FluentSecurity.Scanning
{
	public class ScannerContext
	{
		private readonly List<Assembly> _assemblies = new List<Assembly>();
		private readonly List<ITypeScanner> _typeScanners = new List<ITypeScanner>();
		private readonly IList<Func<Type, bool>> _filters = new List<Func<Type, bool>>();

		public IEnumerable<Assembly> AssembliesToScan
		{
			get { return _assemblies; }
		}

		public IEnumerable<ITypeScanner> TypeScanners
		{
			get { return _typeScanners; }
		}

		public IEnumerable<Func<Type, bool>> Filters
		{
			get { return _filters; }
		}

		internal void AddAssembly(Assembly assembly)
		{
			if (assembly == null) throw new ArgumentNullException("assembly");
			if (!_assemblies.Contains(assembly))
				_assemblies.Add(assembly);
		}

		internal void AddTypeScanner(ITypeScanner typeScanner)
		{
			if (typeScanner == null) throw new ArgumentNullException("typeScanner");
			if (!_typeScanners.Contains(typeScanner))
				_typeScanners.Add(typeScanner);
		}

		internal void AddFilter(Func<Type, bool> filter)
		{
			if (filter == null) throw new ArgumentNullException("filter");
			_filters.Add(filter);
		}
	}
}
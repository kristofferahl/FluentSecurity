using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentSecurity.Scanning.TypeScanners
{
	public interface ITypeScanner
	{
		IEnumerable<Type> Scan(IEnumerable<Assembly> assemblies);
	}
}
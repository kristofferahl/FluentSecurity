using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentSecurity.Scanning
{
	public interface ITypeScanner
	{
		IEnumerable<Type> Scan(IEnumerable<Assembly> assemblies);
	}
}
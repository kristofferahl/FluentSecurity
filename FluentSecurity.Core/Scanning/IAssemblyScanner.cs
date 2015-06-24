using System;
using System.Collections.Generic;

namespace FluentSecurity.Scanning
{
	public interface IAssemblyScanner : IAssemblyScannerConfiguration
	{
		IEnumerable<Type> Scan();
	}
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace FluentSecurity.Scanning
{
	internal class AssemblyScanner
	{
		private readonly List<Assembly> _assemblies = new List<Assembly>();
		private readonly List<ITypeScanner> _scanners = new List<ITypeScanner>();

		public void AddTheCallingAssembly()
		{
			var callingAssembly = FindCallingAssembly();
			if (callingAssembly != null) _assemblies.Add(callingAssembly);
		}

		private static Assembly FindCallingAssembly()
		{
			var thisAssembly = Assembly.GetExecutingAssembly();
			Assembly callingAssembly = null;

			var trace = new StackTrace(false);
			for (var i = 0; i < trace.FrameCount; i++)
			{
				var frame = trace.GetFrame(i);
				var assembly = frame.GetMethod().DeclaringType.Assembly;
				if (assembly != thisAssembly)
				{
					callingAssembly = assembly;
					break;
				}
			}
			return callingAssembly;
		}

		public void With(ITypeScanner typeScanner)
		{
			_scanners.Add(typeScanner);
		}

		public void With<TTypeScanner>() where TTypeScanner : ITypeScanner, new()
		{
			With(new TTypeScanner());
		}

		public IEnumerable<Type> Scan()
		{
			var results = new List<Type>();
			_scanners.Each(scanner => scanner.Scan(_assemblies).Each(results.Add));
			return results;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace FluentSecurity.Scanning
{
	internal class ControllerTypeScanner : ITypeScanner
	{
		public IEnumerable<Type> Scan(IEnumerable<Assembly> assemblies)
		{
			var results = new List<Type>();
			foreach (var assembly in assemblies)
			{
				var controllerTypes = assembly.GetExportedTypes().Where(type => typeof(IController).IsAssignableFrom(type));
				results.AddRange(controllerTypes);
			}
			return results;
		}
	}
}
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
			return Scan<IController>(assemblies);
		}

		public IEnumerable<Type> Scan<TController>(IEnumerable<Assembly> assemblies) 
			where TController : IController
		{
			var results = new List<Type>();
			foreach (var assembly in assemblies)
			{
				var controllerTypes = assembly.GetExportedTypes().Where(type => typeof(TController).IsAssignableFrom(type));
				results.AddRange(controllerTypes);
			}
			return results;
		}
	}
}
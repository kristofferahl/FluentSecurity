using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace FluentSecurity.Scanning
{
	internal interface ITypeScanner
	{
		IEnumerable<Type> Scan(IEnumerable<Assembly> assemblies);
		IEnumerable<Type> Scan<TController>(IEnumerable<Assembly> assemblies) where TController : IController;
	}
}
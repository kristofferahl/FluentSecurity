using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace FluentSecurity.Scanning
{
	internal class ControllerTypeScanner : ITypeScanner
	{
		public Type ControllerType { get; private set; }
		
		public ControllerTypeScanner() : this(typeof(IController)) {}

		public ControllerTypeScanner(Type controllerType)
		{
			if (controllerType == null) throw new ArgumentNullException("controllerType");
			
			ControllerType = controllerType;
		}

		public IEnumerable<Type> Scan(IEnumerable<Assembly> assemblies)
		{
			var results = new List<Type>();
			foreach (var assembly in assemblies)
			{
				var controllerTypes = ControllerType.IsGenericType
					? assembly.GetExportedTypes().Where(IsAssignableFromGenericType).ToList()
					: assembly.GetExportedTypes().Where(type => ControllerType.IsAssignableFrom(type)).ToList();

				var filteredControllerTypes = controllerTypes.Where(type => !type.IsAbstract);
				results.AddRange(filteredControllerTypes);
			}
			return results;
		}

		private bool IsAssignableFromGenericType(Type instanceType)
		{
			if (!instanceType.IsGenericType && instanceType.BaseType == null)
				return false;

			var baseType = instanceType.BaseType;
			if (baseType == null)
				return false;

			if (!baseType.IsGenericType || baseType.GetGenericTypeDefinition() != ControllerType.GetGenericTypeDefinition())
				return false;

			var instanceTs = baseType.GetGenericArguments();
			var genericTs = ControllerType.GetGenericArguments();

			if (instanceTs.Length != genericTs.Length)
				return false;

			for (var i = 0; i < genericTs.Length; ++i)
			{
				var instanceT = instanceTs.ElementAt(i);
				var genericT = genericTs.ElementAt(i);
				if (!genericT.IsAssignableFrom(instanceT))
					return false;
			}

			return true;
		}
	}
}
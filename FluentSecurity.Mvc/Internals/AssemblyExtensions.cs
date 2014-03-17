using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentSecurity.Internals
{
	internal static class AssemblyExtensions
	{
		internal static Func<Assembly, IEnumerable<Type>> GetTypesProvider;

		static AssemblyExtensions()
		{
			Reset();
		}

		internal static void Reset()
		{
			GetTypesProvider = assembly => assembly.GetTypes();
		}

		internal static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
		{
			if (assembly == null) throw new ArgumentNullException("assembly");
			try
			{
				return GetTypesProvider.Invoke(assembly).Where(type => type.IsVisible);
			}
			catch (ReflectionTypeLoadException e)
			{
				return e.Types.Where(type => type != null);
			}
		}

		internal static IEnumerable<Type> GetLoadableExportedTypes(this Assembly assembly)
		{
			if (assembly == null) throw new ArgumentNullException("assembly");
			try
			{
				return GetTypesProvider.Invoke(assembly).Where(type => type.IsVisible);
			}
			catch (ReflectionTypeLoadException e)
			{
				return e.Types.Where(type => type != null && type.IsVisible);
			}
		} 
	}
}
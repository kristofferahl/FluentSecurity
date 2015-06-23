using System;
using System.Collections.Generic;
using System.Reflection;
using FluentSecurity.Scanning.TypeScanners;

namespace FluentSecurity.Scanning
{
	public interface IAssemblyScanner
	{
		void TheCallingAssembly();
		void Assembly(Assembly assembly);
		void Assemblies(IEnumerable<Assembly> assemblies);
		void AssembliesFromApplicationBaseDirectory();
		void AssembliesFromApplicationBaseDirectory(Predicate<Assembly> assemblyFilter);
		void AssembliesFromPath(string path, Predicate<Assembly> assemblyFilter);
		void IncludeAssembly(Func<string, bool> filePredicate);
		void ExcludeAssembly(Func<string, bool> filePredicate);
		void With(ITypeScanner typeScanner);
		void With<TTypeScanner>() where TTypeScanner : ITypeScanner, new();
		void IncludeNamespaceContainingType<T>();
		void ExcludeNamespaceContainingType<T>();
		IEnumerable<Type> Scan();
	}
}
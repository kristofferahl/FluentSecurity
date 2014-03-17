using System;
using System.IO;
using System.Linq;
using FluentSecurity.Scanning;
using NUnit.Framework;

namespace FluentSecurity.Specification.Scanning
{
	[TestFixture]
	[Category("AssemblyScannerSpec")]
	public class When_creating_an_assembly_scanner
	{
		[Test]
		public void Should_have_scanner_context()
		{
			// Act
			var scanner = new AssemblyScanner();

			// Assert
			Assert.That(scanner.Context, Is.Not.Null);
			Assert.That(scanner.Context, Is.TypeOf<ScannerContext>());
		}
	}

	[TestFixture]
	[Category("AssemblyScannerSpec")]
	public class When_adding_assemblies_from_base_directory_to_assembly_scanner
	{
		[Test]
		public void Should_scan_assemblies_from_application_base_directory()
		{
			// Arrange
			var scanner = new AssemblyScanner();
			var extensionsToScan = new[] { ".exe", ".dll" };
			var filesInBaseDirectory = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory);
			var expectedAssembliesCount = filesInBaseDirectory.Count(file =>
			{
				var extension = Path.GetExtension(file);
				return extensionsToScan.Contains(extension);
			});

			// Act
			scanner.AssembliesFromApplicationBaseDirectory();

			// Assert
			Assert.That(scanner.Context.AssembliesToScan.Count(), Is.EqualTo(expectedAssembliesCount));
		}

		[Test]
		public void Should_scan_assemblies_from_application_base_directory_matching_predicate()
		{
			// Arrange
			var scanner = new AssemblyScanner();
			const int expectedAssembliesCount = 5;

			// Act
			scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("FluentSecurity."));

			// Assert
			Assert.That(scanner.Context.AssembliesToScan.Count(), Is.EqualTo(expectedAssembliesCount));
		}

		[Test]
		public void Should_scan_assemblies_from_application_base_directory_exluding_assemblies_matching_predicate()
		{
			// Arrange
			var scanner = new AssemblyScanner();
			const int expectedAssembliesCount = 3;

			// Act
			scanner.ExcludeAssembly(assembly => Path.GetFileNameWithoutExtension(assembly).Contains(".Specification"));
			scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("FluentSecurity."));

			// Assert
			Assert.That(scanner.Context.AssembliesToScan.Count(), Is.EqualTo(expectedAssembliesCount));
			Assert.That(scanner.Context.AssembliesToScan.ElementAt(0).GetName().Name, Is.EqualTo("FluentSecurity.Core"));
			Assert.That(scanner.Context.AssembliesToScan.ElementAt(1).GetName().Name, Is.EqualTo("FluentSecurity.Mvc"));
			Assert.That(scanner.Context.AssembliesToScan.ElementAt(2).GetName().Name, Is.EqualTo("FluentSecurity.TestHelper"));
		}

		[Test]
		public void Should_scan_assemblies_from_application_base_directory_including_assemblies_matching_predicate()
		{
			// Arrange
			var scanner = new AssemblyScanner();
			const int expectedAssembliesCount = 2;

			// Act
			scanner.IncludeAssembly(assembly => Path.GetFileNameWithoutExtension(assembly).StartsWith("FluentSecurity.TestHelper"));
			scanner.AssembliesFromApplicationBaseDirectory();

			// Assert
			Assert.That(scanner.Context.AssembliesToScan.Count(), Is.EqualTo(expectedAssembliesCount));
			Assert.That(scanner.Context.AssembliesToScan.First().GetName().Name, Is.EqualTo("FluentSecurity.TestHelper"));
			Assert.That(scanner.Context.AssembliesToScan.Last().GetName().Name, Is.EqualTo("FluentSecurity.TestHelper.Specification"));
		}

		[Test]
		public void Should_scan_assemblies_from_application_base_directory_for_assemblies_matching_include_and_exclude_predicates()
		{
			// Arrange
			var scanner = new AssemblyScanner();
			const int expectedAssembliesCount = 3;

			// Act
			scanner.IncludeAssembly(assembly => Path.GetFileNameWithoutExtension(assembly).StartsWith("FluentSecurity."));
			scanner.ExcludeAssembly(assembly => Path.GetFileNameWithoutExtension(assembly).EndsWith(".Specification"));
			scanner.AssembliesFromApplicationBaseDirectory();

			// Assert
			Assert.That(scanner.Context.AssembliesToScan.Count(), Is.EqualTo(expectedAssembliesCount));
			Assert.That(scanner.Context.AssembliesToScan.ElementAt(0).GetName().Name, Is.EqualTo("FluentSecurity.Core"));
			Assert.That(scanner.Context.AssembliesToScan.ElementAt(1).GetName().Name, Is.EqualTo("FluentSecurity.Mvc"));
			Assert.That(scanner.Context.AssembliesToScan.ElementAt(2).GetName().Name, Is.EqualTo("FluentSecurity.TestHelper"));
		}
	}
}
using System;
using System.IO;
using System.Linq;
using FluentSecurity.Scanning;
using NUnit.Framework;

namespace FluentSecurity.Specification.Scanning
{
	[TestFixture]
	[Category("AssemblyScannerSpec")]
	public class When_adding_assembly_to_assembly_scanner
	{
		[Test]
		public void Should_throw_when_null()
		{
			// Arrange
			var scanner = new AssemblyScanner();

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => scanner.Assembly(null));
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
			Assert.That(scanner.AssembliesToScan.Count(), Is.EqualTo(expectedAssembliesCount));
		}

		[Test]
		public void Should_scan_assemblies_from_application_base_directory_matching_predicate()
		{
			// Arrange
			var scanner = new AssemblyScanner();
			const int expectedAssembliesCount = 3;

			// Act
			scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("FluentSecurity."));

			// Assert
			Assert.That(scanner.AssembliesToScan.Count(), Is.EqualTo(expectedAssembliesCount));
		}
	}
}
using System;
using System.IO;
using System.Linq;
using FluentSecurity.Configuration;
using FluentSecurity.Scanning;
using NUnit.Framework;

namespace FluentSecurity.Specification.Scanning
{
	[TestFixture]
	[Category("ProfileScannerSpec")]
	public class When_scanning_for_profiles
	{
		[Test]
		public void Should_scan_assemblies_from_application_base_directory()
		{
			// Arrange
			var scanner = new ProfileScanner();
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
			var scanner = new ProfileScanner();
			const int expectedAssembliesCount = 3;

			// Act
			scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("FluentSecurity."));

			// Assert
			Assert.That(scanner.AssembliesToScan.Count(), Is.EqualTo(expectedAssembliesCount));
		}

		[Test]
		public void Should_locate_profiles_in_assemblies_to_scan()
		{
			// Arrange
			var scanner = new ProfileScanner();
			scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("FluentSecurity."));

			// Act
			scanner.LookForProfiles();

			// Assert
			var profiles = scanner.Scan();
			Assert.That(profiles.Count(), Is.EqualTo(2));
			Assert.That(profiles.First(), Is.EqualTo(typeof(TestProfile1)));
			Assert.That(profiles.Last(), Is.EqualTo(typeof(TestProfile2)));
		}

		public class TestProfile1 : SecurityProfile {}
		public class TestProfile2 : SecurityProfile {}
	}
}
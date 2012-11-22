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
		public void Should_locate_profiles_in_assemblies_to_scan()
		{
			// Arrange
			var scanner = new ProfileScanner();
			scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("FluentSecurity."));
			scanner.IncludeNamespaceContainingType<When_scanning_for_profiles>();

			// Act
			scanner.LookForProfiles();

			// Assert
			var profiles = scanner.Scan();
			Assert.That(profiles.Count(), Is.EqualTo(2));
			Assert.That(profiles.First(), Is.EqualTo(typeof(TestProfile1)));
			Assert.That(profiles.Last(), Is.EqualTo(typeof(TestProfile2)));
		}

		public class TestProfile1 : SecurityProfile
		{
			public override void Configure() {}
		}

		public class TestProfile2 : SecurityProfile
		{
			public override void Configure() {}
		}
	}
}
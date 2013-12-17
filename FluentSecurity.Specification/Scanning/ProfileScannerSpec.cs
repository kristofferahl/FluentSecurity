using System.Linq;
using FluentSecurity.Configuration;
using FluentSecurity.Scanning;
using FluentSecurity.Specification.Scanning.Level1;
using FluentSecurity.Specification.Scanning.Level1.Level2;
using NUnit.Framework;

namespace FluentSecurity.Specification.Scanning
{
	[TestFixture]
	[Category("ProfileScannerSpec")]
	public class When_scanning_for_profiles
	{
		[Test]
		public void Should_locate_all_profiles()
		{
			// Arrange
			var scanner = new ProfileScanner();
			scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("FluentSecurity."));

			// Act
			scanner.LookForProfiles();

			// Assert
			var profiles = scanner.Scan();
			Assert.That(profiles.Count(), Is.EqualTo(3));
		}

		[Test]
		public void Should_locate_profiles_in_namespace()
		{
			// Arrange
			var scanner = new ProfileScanner();
			scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("FluentSecurity."));
			scanner.IncludeNamespaceContainingType<Level1Marker>();

			// Act
			scanner.LookForProfiles();

			// Assert
			var profiles = scanner.Scan();
			Assert.That(profiles.Count(), Is.EqualTo(2));
			Assert.That(profiles.First(), Is.EqualTo(typeof(TestProfile1)));
			Assert.That(profiles.Last(), Is.EqualTo(typeof(TestProfile2)));
		}

		[Test]
		public void Should_not_locate_profiles_in_namespace()
		{
			// Arrange
			var scanner = new ProfileScanner();
			scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("FluentSecurity."));
			scanner.IncludeNamespaceContainingType<Level1Marker>();
			scanner.ExcludeNamespaceContainingType<Level2Marker>();

			// Act
			scanner.LookForProfiles();

			// Assert
			var profiles = scanner.Scan();
			Assert.That(profiles.Count(), Is.EqualTo(1));
			Assert.That(profiles.First(), Is.EqualTo(typeof(TestProfile1)));
		}
	}

	namespace Level1
	{
		public class Level1Marker {}

		public class TestProfile1 : SecurityProfile
		{
			public override void Configure()
			{
				Scan(scan =>
				{
					scan.Assembly(GetType().Assembly);
					scan.LookForProfiles();
				});
			}
		}

		namespace Level2
		{
			public class Level2Marker {}

			public class TestProfile2 : SecurityProfile
			{
				public override void Configure()
				{
					Scan(scan =>
					{
						scan.Assembly(GetType().Assembly);
						scan.LookForProfiles();
					});
				}
			}
		}
	}
}
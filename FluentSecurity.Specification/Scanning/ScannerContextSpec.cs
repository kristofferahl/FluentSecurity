using System;
using System.Linq;
using FluentSecurity.Configuration;
using FluentSecurity.Scanning;
using FluentSecurity.Scanning.TypeScanners;
using NUnit.Framework;

namespace FluentSecurity.Specification.Scanning
{
	[TestFixture]
	[Category("ScannerContextSpec")]
	public class When_creating_a_new_scanner_context
	{
		[Test]
		public void Should_have_no_assemblies_to_scan()
		{
			// Act
			var context = new ScannerContext();

			// Assert
			Assert.That(context.AssembliesToScan.Count(), Is.EqualTo(0));
		}

		[Test]
		public void Should_have_no_type_scanners()
		{
			// Act
			var context = new ScannerContext();

			// Assert
			Assert.That(context.TypeScanners.Count(), Is.EqualTo(0));
		}

		[Test]
		public void Should_have_no_filters()
		{
			// Act
			var context = new ScannerContext();

			// Assert
			Assert.That(context.MatchOneTypeFilters.Count(), Is.EqualTo(0));
			Assert.That(context.MatchAllTypeFilters.Count(), Is.EqualTo(0));
			Assert.That(context.MatchOneFileFilters.Count(), Is.EqualTo(0));
			Assert.That(context.MatchAllFileFilters.Count(), Is.EqualTo(0));
		}
	}

	[TestFixture]
	[Category("ScannerContextSpec")]
	public class When_adding_assemblies_to_scanner_context
	{
		[Test]
		public void Should_throw_when_null()
		{
			// Arrange
			var context = new ScannerContext();

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => context.AddAssembly(null));
		}

		[Test]
		public void Should_only_add_unique_assemblies()
		{
			// Arrange
			var context = new ScannerContext();
			var assembly = GetType().Assembly;

			// Act
			context.AddAssembly(assembly);
			context.AddAssembly(assembly);

			// Assert
			Assert.That(context.AssembliesToScan.Single(), Is.EqualTo(assembly));
		}
	}

	[TestFixture]
	[Category("ScannerContextSpec")]
	public class When_adding_type_scanner_to_scanner_context
	{
		[Test]
		public void Should_throw_when_null()
		{
			// Arrange
			var context = new ScannerContext();

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => context.AddTypeScanner(null));
		}

		[Test]
		public void Should_only_add_unique_type_scanners()
		{
			// Arrange
			var context = new ScannerContext();
			var typeScanner = new ProfileTypeScanner<SecurityProfile>();

			// Act
			context.AddTypeScanner(typeScanner);
			context.AddTypeScanner(typeScanner);

			// Assert
			Assert.That(context.TypeScanners.Single(), Is.EqualTo(typeScanner));
		}
	}

	[TestFixture]
	[Category("ScannerContextSpec")]
	public class When_adding_typefilters_to_scanner_context
	{
		[Test]
		public void Should_throw_when_null()
		{
			// Arrange
			var context = new ScannerContext();

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => context.AddMatchOneTypeFilter(null));
			Assert.Throws<ArgumentNullException>(() => context.AddMatchAllTypeFilter(null));
		}

		[Test]
		public void Should_add_match_one_filter()
		{
			// Arrange
			var context = new ScannerContext();
			Func<Type, bool> filter = t => true;

			// Act
			context.AddMatchOneTypeFilter(filter);

			// Assert
			Assert.That(context.MatchOneTypeFilters.Single(), Is.EqualTo(filter));
			Assert.That(context.MatchAllTypeFilters.Any(), Is.False);
		}

		[Test]
		public void Should_add_match_all_filter()
		{
			// Arrange
			var context = new ScannerContext();
			Func<Type, bool> filter = t => true;

			// Act
			context.AddMatchAllTypeFilter(filter);

			// Assert
			Assert.That(context.MatchAllTypeFilters.Single(), Is.EqualTo(filter));
			Assert.That(context.MatchOneTypeFilters.Any(), Is.False);
		}
	}

	[TestFixture]
	[Category("ScannerContextSpec")]
	public class When_adding_filefilters_to_scanner_context
	{
		[Test]
		public void Should_throw_when_null()
		{
			// Arrange
			var context = new ScannerContext();

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => context.AddMatchOneFileFilter(null));
			Assert.Throws<ArgumentNullException>(() => context.AddMatchAllFileFilter(null));
		}

		[Test]
		public void Should_add_match_one_filter()
		{
			// Arrange
			var context = new ScannerContext();
			Func<string, bool> filter = t => true;

			// Act
			context.AddMatchOneFileFilter(filter);

			// Assert
			Assert.That(context.MatchOneFileFilters.Single(), Is.EqualTo(filter));
			Assert.That(context.MatchAllFileFilters.Any(), Is.False);
		}

		[Test]
		public void Should_add_match_all_filter()
		{
			// Arrange
			var context = new ScannerContext();
			Func<string, bool> filter = t => true;

			// Act
			context.AddMatchAllFileFilter(filter);

			// Assert
			Assert.That(context.MatchAllFileFilters.Single(), Is.EqualTo(filter));
			Assert.That(context.MatchOneFileFilters.Any(), Is.False);
		}
	}
}
using System;
using System.Linq;
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
			Assert.That(context.Filters.Count(), Is.EqualTo(0));
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
			var typeScanner = new ProfileTypeScanner();

			// Act
			context.AddTypeScanner(typeScanner);
			context.AddTypeScanner(typeScanner);

			// Assert
			Assert.That(context.TypeScanners.Single(), Is.EqualTo(typeScanner));
		}
	}

	[TestFixture]
	[Category("ScannerContextSpec")]
	public class When_adding_filters_to_scanner_context
	{
		[Test]
		public void Should_throw_when_null()
		{
			// Arrange
			var context = new ScannerContext();

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => context.AddFilter(null));
		}

		[Test]
		public void Should_add_filter()
		{
			// Arrange
			var context = new ScannerContext();
			Func<Type, bool> filter = t => true;

			// Act
			context.AddFilter(filter);

			// Assert
			Assert.That(context.Filters.Single(), Is.EqualTo(filter));
		}
	}
}
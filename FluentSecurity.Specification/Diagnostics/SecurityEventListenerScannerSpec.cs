using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentSecurity.Diagnostics;
using NUnit.Framework;

namespace FluentSecurity.Specification.Diagnostics
{
	[TestFixture]
	[Category("SecurityEventListenerScannerSpec")]
	public class When_scanning_for_security_event_listeners
	{
		[Test]
		public void Should_find_0_event_listeners()
		{
			// Arrange
			var scanner = new SecurityEventListenerScanner();
			var assemblies = new List<Assembly> { scanner.GetType().Assembly };

			// Act
			var types = scanner.Scan(assemblies);

			// Assert
			Assert.That(types.Count(), Is.EqualTo(0));
		}

		[Test]
		public void Should_find_1_event_listener()
		{
			// Arrange
			var scanner = new SecurityEventListenerScanner();
			var assemblies = new List<Assembly> { GetType().Assembly };

			// Act
			var types = scanner.Scan(assemblies);

			// Assert
			Assert.That(types.Count(), Is.EqualTo(1));
		}

		[Test]
		public void Should_find_2_event_listeners()
		{
			// Arrange
			var scanner = new SecurityEventListenerScanner();
			var assemblies = new List<Assembly>
			{
				GetType().Assembly,
				GetType().Assembly
			};

			// Act
			var types = scanner.Scan(assemblies);

			// Assert
			Assert.That(types.Count(), Is.EqualTo(2));
		}
	}
}
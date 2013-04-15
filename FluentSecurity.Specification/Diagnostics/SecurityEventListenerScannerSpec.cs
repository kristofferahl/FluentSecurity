using System;
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
		public void Should_handle_type_load_exception_and_find_1_event_listener()
		{
			// Arrange
			var log = new List<string>();
			SecurityDoctor.Register(e => log.Add(e.Message));
			var throwException = true;
			const string expectedExceptionMessage = "Could not load type X.";
			var scanner = new SecurityEventListenerScanner(assembly =>
			{
				if (throwException)
				{
					throwException = false;
					throw new TypeLoadException(expectedExceptionMessage);
				}
				return assembly.GetExportedTypes();
			});
			var assemblies = new List<Assembly>
			{
				GetType().Assembly,
				GetType().Assembly
			};

			// Act
			var types = scanner.Scan(assemblies);

			// Assert
			Assert.That(types.Count(), Is.EqualTo(1));
			Assert.That(log.Single(), Is.EqualTo(expectedExceptionMessage));
		}
	}
}
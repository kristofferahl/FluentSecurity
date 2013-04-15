using System;
using System.Linq;
using FluentSecurity.Diagnostics;
using FluentSecurity.Diagnostics.Events;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification.Diagnostics
{
	[TestFixture]
	[Category("SecurityDoctorSpecs")]
	public class When_SecurityDoctor_is_created
	{
		[SetUp]
		public void SetUp()
		{
			SecurityDoctor.Reset();
		}

		[Test]
		public void Should_not_have_event_listeners_registered()
		{
			Assert.That(SecurityDoctor.Listeners, Is.Null);
		}

		[Test]
		public void Should_ignore_TypeLoadExceptions()
		{
			Assert.That(SecurityDoctor.IgnoreTypeLoadExceptions, Is.True);
		}

		[Test]
		public void Should_register_event_listener()
		{
			// Arrange
			Action<ISecurityEvent> eventListener = e  => {};
			
			// Act
			SecurityDoctor.Register(eventListener);

			// Assert
			Assert.That(SecurityDoctor.Listeners.Cast<AnonymousSecurityEventListener>().Single().EventListener, Is.EqualTo(eventListener));
		}

		[Test]
		public void Should_register_multiple_event_listener()
		{
			// Arrange
			SecurityDoctor.Reset();
			Action<ISecurityEvent> eventListener1 = e => {};
			Action<ISecurityEvent> eventListener2 = e => {};

			// Act
			SecurityDoctor.Register(eventListener1);
			SecurityDoctor.Register(eventListener2);

			// Assert
			Assert.That(SecurityDoctor.Listeners.Count(), Is.EqualTo(2));
			Assert.That(SecurityDoctor.Listeners.Cast<AnonymousSecurityEventListener>().First().EventListener, Is.EqualTo(eventListener1));
			Assert.That(SecurityDoctor.Listeners.Cast<AnonymousSecurityEventListener>().Last().EventListener, Is.EqualTo(eventListener2));
		}
	}

	[TestFixture]
	[Category("SecurityDoctorSpecs")]
	public class When_event_listeners_are_registered
	{
		[Test]
		public void Should_reset_event_listeners()
		{
			// Arrange
			SecurityDoctor.Register(e => {});
			SecurityDoctor.Register(e => {});
			SecurityDoctor.Register(e => {});

			// Act
			SecurityDoctor.Reset();

			// Assert
			Assert.That(SecurityDoctor.Listeners, Is.Null);
		}
	}

	[TestFixture]
	[Category("SecurityDoctorSpecs")]
	public class When_scanning_for_event_listeners
	{
		[Test]
		public void Should_find_a_single_event_listener()
		{
			// Arrange
			SecurityDoctor.Reset();

			// Act
			SecurityDoctor.ScanForEventListeners();

			// Assert
			Assert.That(SecurityDoctor.Listeners.Single(), Is.TypeOf<TestSecurityEventListener>());
		}
	}
}
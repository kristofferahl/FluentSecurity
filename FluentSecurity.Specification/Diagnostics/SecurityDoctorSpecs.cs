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
	public class When_creating_a_security_doctor
	{
		private SecurityDoctor _doctor;

		[SetUp]
		public void SetUp()
		{
			// Act
			_doctor = new SecurityDoctor();
		}

		[Test]
		public void Should_have_no_event_listeners()
		{
			// Assert
			Assert.That(_doctor.Listeners, Is.Null);
		}

		[Test]
		public void Should_have_ScanForEventListenersOnConfigure_set_to_true()
		{
			// Assert
			Assert.That(_doctor.ScanForEventListenersOnConfigure, Is.True);
		}

		[Test]
		public void Should_have_ScannedForEventListeners_set_to_false()
		{
			// Assert
			Assert.That(_doctor.ScannedForEventListeners, Is.False);
		}
	}

	[TestFixture]
	[Category("SecurityDoctorSpecs")]
	public class When_registering_event_listeners
	{
		[Test]
		public void Should_register_anonymous_event_listener()
		{
			// Arrange
			var doctor = new SecurityDoctor();
			Action<ISecurityEvent> eventListener = e => { };

			// Act
			doctor.RegisterListener(eventListener);

			// Assert
			Assert.That(doctor.Listeners.Cast<AnonymousSecurityEventListener>().Single().EventListener, Is.EqualTo(eventListener));
		}

		[Test]
		public void Should_register_multiple_anonymous_event_listener()
		{
			// Arrange
			var doctor = new SecurityDoctor();
			Action<ISecurityEvent> eventListener1 = e => { };
			Action<ISecurityEvent> eventListener2 = e => { };

			// Act
			doctor.RegisterListener(eventListener1);
			doctor.RegisterListener(eventListener2);

			// Assert
			Assert.That(doctor.Listeners.Count(), Is.EqualTo(2));
			Assert.That(doctor.Listeners.Cast<AnonymousSecurityEventListener>().First().EventListener, Is.EqualTo(eventListener1));
			Assert.That(doctor.Listeners.Cast<AnonymousSecurityEventListener>().Last().EventListener, Is.EqualTo(eventListener2));
		}

		[Test]
		public void Should_register_event_listener()
		{
			// Arrange
			var doctor = new SecurityDoctor();
			ISecurityEventListener eventListener = new TestSecurityEventListener();

			// Act
			doctor.RegisterListener(eventListener);

			// Assert
			Assert.That(doctor.Listeners.Single(), Is.EqualTo(eventListener));
		}

		[Test]
		public void Should_register_multiple_event_listener()
		{
			// Arrange
			var doctor = new SecurityDoctor();
			ISecurityEventListener eventListener1 = new TestSecurityEventListener();
			ISecurityEventListener eventListener2 = new AnonymousSecurityEventListener(e => { });

			// Act
			doctor.RegisterListener(eventListener1);
			doctor.RegisterListener(eventListener2);

			// Assert
			Assert.That(doctor.Listeners.Count(), Is.EqualTo(2));
			Assert.That(doctor.Listeners.First(), Is.EqualTo(eventListener1));
			Assert.That(doctor.Listeners.Last(), Is.EqualTo(eventListener2));
		}

		[Test]
		public void Should_register_anonymous_event_listener_using_the_static_method()
		{
			// Arrange
			SecurityDoctor.Reset();
			Action<ISecurityEvent> eventListener = e => { };

			// Act
			SecurityDoctor.Register(eventListener);

			// Assert
			Assert.That(SecurityDoctor.Current.Listeners.Cast<AnonymousSecurityEventListener>().Single().EventListener, Is.EqualTo(eventListener));
		}

		[Test]
		public void Should_register_event_listener_using_the_static_method()
		{
			// Arrange
			SecurityDoctor.Reset();
			ISecurityEventListener eventListener = new TestSecurityEventListener();

			// Act
			SecurityDoctor.Register(eventListener);

			// Assert
			Assert.That(SecurityDoctor.Current.Listeners.Single(), Is.EqualTo(eventListener));
		}
	}

	[TestFixture]
	[Category("SecurityDoctorSpecs")]
	public class When_resetting_security_doctor
	{
		[Test]
		public void Should_create_a_new_instance_of_SecurityDoctor()
		{
			// Arrange
			var securityDoctor = SecurityDoctor.Current;

			SecurityDoctor.Reset();

			// Assert
			Assert.That(SecurityDoctor.Current, Is.Not.EqualTo(securityDoctor));
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
			var doctor = new SecurityDoctor();

			// Act
			doctor.ScanForEventListeners();

			// Assert
			Assert.That(doctor.Listeners.Single(), Is.TypeOf<TestSecurityEventListener>());
		}
	}

	[TestFixture]
	[Category("SecurityDoctorSpecs")]
	public class When_setting_eventlistener_scanner_setup
	{
		[Test]
		public void Should_scan_using_the_specified_scanner()
		{
			// Arrange
			var doctor = new SecurityDoctor();

			var calledScannerSetup = false;
			doctor.EventListenerScannerSetup = scanner =>
			{
				calledScannerSetup = true;
			};

			// Act
			doctor.ScanForEventListeners();

			// Assert
			Assert.That(calledScannerSetup, Is.True);
		}
	}
}
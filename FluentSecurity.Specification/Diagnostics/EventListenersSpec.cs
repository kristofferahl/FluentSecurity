using System;
using FluentSecurity.Diagnostics;
using FluentSecurity.Diagnostics.Events;
using NUnit.Framework;

namespace FluentSecurity.Specification.Diagnostics
{
	[TestFixture]
	[Category("EventListenersSpec")]
	public class When_EventListeners_is_created
	{
		[Test]
		public void Should_not_have_event_listener_registered()
		{
			Assert.That(EventListeners.Current, Is.Null);
		}

		[Test]
		public void Should_register_runtime_event_listener()
		{
			// Arrange
			Action<ISecurityEvent> eventListener = e  => {};
			
			// Act
			EventListeners.Register(eventListener);

			// Assert
			Assert.That(EventListeners.Current, Is.EqualTo(eventListener));
		}
	}

	[TestFixture]
	[Category("EventListenersSpec")]
	public class When_event_listeners_are_registered
	{
		[Test]
		public void Should_reset_event_listeners()
		{
			// Arrange
			EventListeners.Register(e => {});

			// Act
			EventListeners.Reset();

			// Assert
			Assert.That(EventListeners.Current, Is.Null);
		}
	}
}
using System;
using System.Linq;
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
		public void Should_not_have_event_listeners_registered()
		{
			Assert.That(EventListeners.Listeners, Is.Null);
		}

		[Test]
		public void Should_register_event_listener()
		{
			// Arrange
			Action<ISecurityEvent> eventListener = e  => {};
			
			// Act
			EventListeners.Register(eventListener);

			// Assert
			Assert.That(EventListeners.Listeners.Single(), Is.EqualTo(eventListener));
		}

		[Test]
		public void Should_register_multiple_event_listener()
		{
			// Arrange
			EventListeners.Reset();
			Action<ISecurityEvent> eventListener1 = e => {};
			Action<ISecurityEvent> eventListener2 = e => {};

			// Act
			EventListeners.Register(eventListener1);
			EventListeners.Register(eventListener2);

			// Assert
			Assert.That(EventListeners.Listeners.Count(), Is.EqualTo(2));
			Assert.That(EventListeners.Listeners.First(), Is.EqualTo(eventListener1));
			Assert.That(EventListeners.Listeners.Last(), Is.EqualTo(eventListener2));
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
			EventListeners.Register(e => {});
			EventListeners.Register(e => {});

			// Act
			EventListeners.Reset();

			// Assert
			Assert.That(EventListeners.Listeners, Is.Null);
		}
	}
}
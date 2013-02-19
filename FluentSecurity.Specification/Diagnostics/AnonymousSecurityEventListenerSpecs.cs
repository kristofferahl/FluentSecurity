using System;
using FluentSecurity.Diagnostics;
using FluentSecurity.Diagnostics.Events;
using NUnit.Framework;

namespace FluentSecurity.Specification.Diagnostics
{
	[TestFixture]
	[Category("AnonymousSecurityEventListenerSpecs")]
	public class When_creating_an_anonymous_security_event_listener
	{
		[Test]
		public void Should_have_eventlistener()
		{
			// Arrange
			Action<ISecurityEvent> eventListener = @event => {};

			// Act
			var listener = new AnonymousSecurityEventListener(eventListener);

			// Assert
			Assert.That(listener.EventListener, Is.EqualTo(eventListener));
		}

		[Test]
		public void Should_throw_when_event_listener_is_null()
		{
			// Arrange
			Action<ISecurityEvent> eventListener = null;

			// Act & assert
			Assert.Throws<ArgumentNullException>(() => new AnonymousSecurityEventListener(eventListener));
		}
	}

	[TestFixture]
	[Category("AnonymousSecurityEventListenerSpecs")]
	public class When_handling_an_event_with__anonymous_security_event_listener
	{
		[Test]
		public void Should_not_throw_if_event_listener_throws()
		{
			// Arrange
			var @event = new ConfigurationEvent(Guid.NewGuid(), "Message");
			Action<ISecurityEvent> eventListener = e =>
			{
				throw new Exception("Exception message");
			};
			var listener = new AnonymousSecurityEventListener(eventListener);

			// Act & assert
			Assert.DoesNotThrow(() => listener.Handle(@event));
		}
	}
}
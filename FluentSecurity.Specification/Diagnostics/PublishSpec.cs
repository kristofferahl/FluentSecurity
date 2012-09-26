using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Diagnostics;
using FluentSecurity.Diagnostics.Events;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification.Diagnostics
{
	[TestFixture]
	[Category("PublishSpec")]
	public class When_publishing_event
	{
		[Test]
		public void Should_produce_runtime_event_when_event_listener_is_registered()
		{
			// Arrange
			const string expectedMessage = "Message";

			var events = new List<ISecurityEvent>();
			EventListeners.RuntimeEventListener = events.Add;
			var context = TestDataFactory.CreateSecurityContext(false);

			// Act
			Publish.RuntimeEvent(() => expectedMessage, context);

			// Assert
			var @event = events.Single();
			Assert.That(@event.Id, Is.EqualTo(context.Id));
			Assert.That(@event.Message, Is.EqualTo(expectedMessage));
		}

		[Test]
		public void Should_produce_runtime_policy_event_when_event_listener_is_registered()
		{
			// Arrange
			const string expectedMessage = "Message";

			var events = new List<ISecurityEvent>();
			EventListeners.RuntimePolicyEventListener = events.Add;
			var context = TestDataFactory.CreateSecurityContext(false);

			// Act
			Publish.RuntimePolicyEvent(() => expectedMessage, context);

			// Assert
			var @event = events.Single();
			Assert.That(@event.Id, Is.EqualTo(context.Id));
			Assert.That(@event.Message, Is.EqualTo(expectedMessage));
		}

		[Test]
		public void Should_produce_configuration_event_when_event_listener_is_registered()
		{
			// Arrange
			const string expectedMessage = "Message";

			var events = new List<ISecurityEvent>();
			EventListeners.ConfigurationEventListener = events.Add;
			var context = TestDataFactory.CreateSecurityContext(false);

			// Act
			Publish.ConfigurationEvent(() => expectedMessage, context);

			// Assert
			var @event = events.Single();
			Assert.That(@event.Id, Is.EqualTo(context.Id));
			Assert.That(@event.Message, Is.EqualTo(expectedMessage));
		}
	}
}
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
			EventListeners.Current = events.Add;
			var context = TestDataFactory.CreateSecurityContext(false);

			// Act
			Publish.RuntimeEvent(() => expectedMessage, context);

			// Assert
			var @event = events.Single();
			Assert.That(@event.CorrelationId, Is.EqualTo(context.Id));
			Assert.That(@event.Message, Is.EqualTo(expectedMessage));
		}

		[Test]
		public void Should_produce_runtime_event_with_timing_when_event_listener_is_registered()
		{
			// Arrange
			const int expectedMilliseconds = 13;
			var expectedResult = new {};
			const string expectedMessage = "Message";

			var events = new List<ISecurityEvent>();
			EventListeners.Current = events.Add;
			var context = TestDataFactory.CreateSecurityContext(false);

			// Act
			var result = Publish.RuntimeEvent(() =>
			{
				Thread.Sleep(expectedMilliseconds + 5);
				return expectedResult;
			}, r => expectedMessage, context);

			// Assert
			Assert.That(result, Is.EqualTo(expectedResult));

			var @event = events.Single();
			Assert.That(@event.CorrelationId, Is.EqualTo(context.Id));
			Assert.That(@event.Message, Is.EqualTo(expectedMessage));
			Assert.That(@event.CompletedInMilliseconds, Is.GreaterThanOrEqualTo(expectedMilliseconds));
		}

		[Test]
		public void Should_produce_runtime_policy_event_when_event_listener_is_registered()
		{
			// Arrange
			const string expectedMessage = "Message";

			var events = new List<ISecurityEvent>();
			EventListeners.Register(events.Add);
			var context = TestDataFactory.CreateSecurityContext(false);

			// Act
			Publish.RuntimePolicyEvent(() => expectedMessage, context);

			// Assert
			var @event = events.Single();
			Assert.That(@event.CorrelationId, Is.EqualTo(context.Id));
			Assert.That(@event.Message, Is.EqualTo(expectedMessage));
		}

		[Test]
		public void Should_produce_runtime_policy_event_with_timing_when_event_listener_is_registered()
		{
			// Arrange
			const int expectedMilliseconds = 9;
			var expectedResult = new { };
			const string expectedMessage = "Message";

			var events = new List<ISecurityEvent>();
			EventListeners.Register(events.Add);
			var context = TestDataFactory.CreateSecurityContext(false);

			// Act
			var result = Publish.RuntimePolicyEvent(() =>
			{
				Thread.Sleep(expectedMilliseconds + 5);
				return expectedResult;
			}, r => expectedMessage, context);

			// Assert
			Assert.That(result, Is.EqualTo(expectedResult));

			var @event = events.Single();
			Assert.That(@event.CorrelationId, Is.EqualTo(context.Id));
			Assert.That(@event.Message, Is.EqualTo(expectedMessage));
			Assert.That(@event.CompletedInMilliseconds, Is.GreaterThanOrEqualTo(expectedMilliseconds));
		}

		[Test]
		public void Should_produce_configuration_event_when_event_listener_is_registered()
		{
			// Arrange
			const string expectedMessage = "Message";

			var events = new List<ISecurityEvent>();
			EventListeners.Register(events.Add);
			var context = TestDataFactory.CreateSecurityContext(false);

			// Act
			Publish.ConfigurationEvent(() => expectedMessage, context);

			// Assert
			var @event = events.Single();
			Assert.That(@event.CorrelationId, Is.EqualTo(context.Id));
			Assert.That(@event.Message, Is.EqualTo(expectedMessage));
		}
	}
}
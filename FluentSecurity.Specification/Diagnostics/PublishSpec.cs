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
			SecurityDoctor.Register(events.Add);
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
			SecurityDoctor.Register(events.Add);
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
			SecurityDoctor.Register(events.Add);
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
			SecurityDoctor.Register(events.Add);
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
			SecurityDoctor.Register(events.Add);

			// Act
			Publish.ConfigurationEvent(() => expectedMessage);

			// Assert
			var @event = events.Single();
			Assert.That(@event.CorrelationId, Is.EqualTo(SecurityConfigurator.CorrelationId));
			Assert.That(@event.Message, Is.EqualTo(expectedMessage));
		}

		[Test]
		public void Should_produce_configuration_event_with_timing_when_event_listener_is_registered()
		{
			// Arrange
			const int expectedMilliseconds = 9;
			var expectedResult = new { };
			const string expectedMessage = "Message";

			var events = new List<ISecurityEvent>();
			SecurityDoctor.Register(events.Add);

			// Act
			var result = Publish.ConfigurationEvent(() =>
			{
				Thread.Sleep(expectedMilliseconds + 5);
				return expectedResult;
			}, r => expectedMessage);

			// Assert
			Assert.That(result, Is.EqualTo(expectedResult));

			var @event = events.Single();
			Assert.That(@event.CorrelationId, Is.EqualTo(SecurityConfigurator.CorrelationId));
			Assert.That(@event.Message, Is.EqualTo(expectedMessage));
			Assert.That(@event.CompletedInMilliseconds, Is.GreaterThanOrEqualTo(expectedMilliseconds));
		}

		[Test]
		public void Should_return_result_when_no_event_listener_is_registered()
		{
			// Arrange
			SecurityDoctor.Reset();
			var context = TestDataFactory.CreateSecurityContext(false);
			
			var expectedResult1 = new {};
			var expectedResult2 = new {};
			var expectedResult3 = new {};
			

			// Act
			var result1 = Publish.ConfigurationEvent(() => expectedResult1, r => null);
			var result2 = Publish.RuntimeEvent(() => expectedResult2, r => null, context);
			var result3 = Publish.RuntimePolicyEvent(() => expectedResult3, r => null, context);

			// Assert
			Assert.That(result1, Is.EqualTo(expectedResult1));
			Assert.That(result2, Is.EqualTo(expectedResult2));
			Assert.That(result3, Is.EqualTo(expectedResult3));
		}
	}
}
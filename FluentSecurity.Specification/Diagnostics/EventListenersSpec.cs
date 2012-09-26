using System;
using FluentSecurity.Diagnostics;
using FluentSecurity.Diagnostics.Events;
using NUnit.Framework;

namespace FluentSecurity.Specification.Diagnostics
{
	[TestFixture]
	[Category("EventListenersSpec")]
	public class When_EventListeners_container_is_created
	{
		[Test]
		public void Should_not_have_event_listeners_registered()
		{
			Assert.That(EventListeners.RuntimeEventListener, Is.Null);
			Assert.That(EventListeners.RuntimePolicyEventListener, Is.Null);
			Assert.That(EventListeners.ConfigurationEventListener, Is.Null);
		}

		[Test]
		public void Should_register_runtime_event_listener()
		{
			// Arrange
			Action<SecurityRuntimeEvent> runtimeEventListener = e  => {};
			Action<SecurityRuntimePolicyEvent> runtimePolicyEventListener = e  => {};
			Action<SecurityConfigurationEvent> configurationEventListener = e  => {};
			
			// Act
			EventListeners.RuntimeEventListener = runtimeEventListener;
			EventListeners.RuntimePolicyEventListener = runtimePolicyEventListener;
			EventListeners.ConfigurationEventListener = configurationEventListener;

			// Assert
			Assert.That(EventListeners.RuntimeEventListener, Is.EqualTo(runtimeEventListener));
			Assert.That(EventListeners.RuntimePolicyEventListener, Is.EqualTo(runtimePolicyEventListener));
			Assert.That(EventListeners.ConfigurationEventListener, Is.EqualTo(configurationEventListener));
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
			EventListeners.RuntimeEventListener = e => {};
			EventListeners.RuntimePolicyEventListener = e => { };
			EventListeners.ConfigurationEventListener = e => { };

			// Act
			EventListeners.Reset();

			// Assert
			Assert.That(EventListeners.RuntimeEventListener, Is.Null);
			Assert.That(EventListeners.RuntimePolicyEventListener, Is.Null);
			Assert.That(EventListeners.ConfigurationEventListener, Is.Null);
		}
	}
}
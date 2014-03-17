using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Routing;
using FluentSecurity.Configuration;
using FluentSecurity.Policy;
using FluentSecurity.Policy.Contexts;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy
{
	[TestFixture]
	[Category("SecurityPolicyBaseSpec")]
	public class When_enforcing_polciy_using_SecurityPolicyBase
	{
		[Test]
		public void Should_create_context_with_empty_contructor()
		{
			// Arrange
			FakeIoC.GetAllInstancesProvider = () => new List<ISecurityContext>();
			SecurityConfigurator.Configure<MvcConfiguration>(configuration => configuration.ResolveServicesUsing(FakeIoC.GetAllInstances));
			var policy = new Policy<ContextWithEmptyConstructor>();
			var context = new MockSecurityContext();

			// Act
			policy.Enforce(context);

			// Assert
			Assert.That(policy.WasCalledWithCustomContext, Is.True);
		}

		[Test]
		public void Should_create_context_with_security_context_as_the_only_contructor_argument()
		{
			// Arrange
			FakeIoC.GetAllInstancesProvider = () => new List<ISecurityContext>();
			SecurityConfigurator.Configure<MvcConfiguration>(configuration => configuration.ResolveServicesUsing(FakeIoC.GetAllInstances));
			var policy = new Policy<ContextWithContextConstructor>();
			var context = new MockSecurityContext();

			// Act
			policy.Enforce(context);

			// Assert
			Assert.That(policy.WasCalledWithCustomContext, Is.True);
		}

		[Test]
		public void Should_get_context_from_external_service_locator()
		{
			// Arrange
			FakeIoC.GetAllInstancesProvider = () => new List<ISecurityContext>
			{
				new ContextFromContainer()
			};
			SecurityConfigurator.Configure<MvcConfiguration>(configuration => configuration.ResolveServicesUsing(FakeIoC.GetAllInstances));

			var policy = new Policy<ContextFromContainer>();
			var context = new MockSecurityContext();

			// Act
			policy.Enforce(context);

			// Assert
			Assert.That(policy.WasCalledWithCustomContext, Is.True);
		}

		[Test]
		public void Should_create_MvcSecurityContext_with_security_context_as_the_only_constructor_argument()
		{
			// Arrange
			SecurityConfigurator.Configure<MvcConfiguration>(configuration => {});

			var policy = new Policy<MvcSecurityContext>();
			var expectedRouteValues = new RouteValueDictionary();
			var context = new MockSecurityContext(routeValues: expectedRouteValues);

			// Act
			policy.Enforce(context);

			// Assert
			Assert.That(policy.WasCalledWithCustomContext, Is.True);
			Assert.That(policy.CustomContext.RouteValues, Is.EqualTo(expectedRouteValues));
		}

		[Test]
		public void Should_throw_when_context_can_not_be_created_or_resolved()
		{
			// Arrange
			SecurityConfigurator.Configure<MvcConfiguration>(configuration => {});

			var policy = new Policy<ContextWithConstructorArgs>();
			var context = new MockSecurityContext();

			// Act & assert
			var exception = Assert.Throws<ArgumentException>(() => policy.Enforce(context));
			Assert.That(exception.Message, Is.EqualTo("The generic argument ContextWithConstructorArgs could not be created or resolved from the container."));
		}

		public class Policy<TSecurityContext> : SecurityPolicyBase<TSecurityContext> where TSecurityContext : class, ISecurityContext
		{
			public bool WasCalledWithCustomContext { get; private set; }
			public TSecurityContext CustomContext { get; private set; }
			
			public override PolicyResult Enforce(TSecurityContext securityContext)
			{
				if (securityContext != null)
					WasCalledWithCustomContext = true;

				CustomContext = securityContext;

				return PolicyResult.CreateSuccessResult(this);
			}
		}

		public class ContextFromContainer : BaseContext {}

		public class ContextWithConstructorArgs : BaseContext
		{
			public ContextWithConstructorArgs(string arg1, string arg2, string arg3) {}
		}

		public class ContextWithContextConstructor : BaseContext
		{
			public ContextWithContextConstructor(ISecurityContext innerContext)
			{
				if (innerContext == null) throw new ArgumentNullException("innerContext");
			}
		}

		public class ContextWithEmptyConstructor : BaseContext {}

		public abstract class BaseContext : ISecurityContext
		{
			public Guid Id { get; set; }

			public dynamic Data { get; set; }

			public bool CurrentUserIsAuthenticated()
			{
				return true;
			}

			public IEnumerable<object> CurrentUserRoles()
			{
				return null;
			}

			public ISecurityRuntime Runtime { get; set; }
		}
	}
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using FluentSecurity.Configuration;
using FluentSecurity.Diagnostics;
using FluentSecurity.Diagnostics.Events;
using FluentSecurity.Internals;
using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("SecurityHandlerSpec")]
	public class When_handling_security
	{
		private MockSecurityContext _context;

		[SetUp]
		public void SetUp()
		{
			SecurityConfigurator.Reset();
			FakeIoC.Reset();
			_context = new MockSecurityContext();
		}

		[Test]
		public void Should_throw_ArgumentException_when_controllername_is_null_or_empty()
		{
			// Arrange
			var securityHandler = new SecurityHandler();

			// Assert
			Assert.Throws<ArgumentException>(() => securityHandler.HandleSecurityFor(null, "A", _context));
			Assert.Throws<ArgumentException>(() => securityHandler.HandleSecurityFor("", "A", _context));
		}

		[Test]
		public void Should_throw_ArgumentException_when_actionname_is_null_or_empty()
		{
			// Arrange
			var securityHandler = new SecurityHandler();

			// Assert
			Assert.Throws<ArgumentException>(() => securityHandler.HandleSecurityFor("A", null, _context));
			Assert.Throws<ArgumentException>(() => securityHandler.HandleSecurityFor("A", "", _context));
		}

		[Test]
		public void Should_throw_ArgumentNulllException_when_security_context_is_null()
		{
			// Arrange
			var securityHandler = new SecurityHandler();
			const ISecurityContext securityContext = null;

			// Assert
			Assert.Throws<ArgumentNullException>(() => securityHandler.HandleSecurityFor("A", "A", securityContext));
		}

		[Test]
		public void Should_not_throw_when_when_controllername_is_Blog_and_actionname_is_Index()
		{
			// Arrange
			SecurityConfigurator.Configure<MvcConfiguration>(policy =>
			{
				policy.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsTrue);
				policy.For<BlogController>(x => x.Index()).DenyAnonymousAccess();
			});

			var securityHandler = new SecurityHandler();

			// Assert
			Assert.DoesNotThrow(() => securityHandler.HandleSecurityFor(NameHelper.Controller<BlogController>(), "Index", SecurityContext.Current));
		}

		[Test]
		public void Should_resolve_policy_violation_handler_for_exception_from_container()
		{
			// Arrange
			var controllerName = NameHelper.Controller<BlogController>();
			const string actionName = "Index";

			var events = new List<ISecurityEvent>();
			SecurityDoctor.Register(events.Add);
			var expectedActionResult = new ViewResult { ViewName = "SomeViewName" };
			var violationHandler = new DenyAnonymousAccessPolicyViolationHandler(expectedActionResult);
			FakeIoC.GetAllInstancesProvider = () => new List<IPolicyViolationHandler>
			{
			    violationHandler
			};

			SecurityConfigurator.Configure<MvcConfiguration>(policy =>
			{
				policy.ResolveServicesUsing(FakeIoC.GetAllInstances);
				policy.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);
				policy.For<BlogController>(x => x.Index()).DenyAnonymousAccess();
			});

			var securityHandler = new SecurityHandler();

			// Act
			var result = securityHandler.HandleSecurityFor(controllerName, actionName, SecurityContext.Current);

			// Assert
			Assert.That(result, Is.EqualTo(expectedActionResult));
			Assert.That(events.Any(e => e.Message == "Handling security for {0} action {1}.".FormatWith(controllerName, actionName)));
			Assert.That(events.Any(e => e.Message == "Finding policy violation handler using convention {0}.".FormatWith(typeof(FindByPolicyNameConvention))));
			Assert.That(events.Any(e => e.Message == "Found policy violation handler {0}.".FormatWith(violationHandler.GetType().FullName)));
			Assert.That(events.Any(e => e.Message == "Handling violation with {0}.".FormatWith(violationHandler.GetType().FullName)));
			Assert.That(events.Any(e => e.Message == "Done enforcing policies. Violation occured!"));
		}
	}

	[TestFixture]
	[Category("SecurityHandlerSpec")]
	public class When_handling_security_for_a_controlleraction_with_DenyAnonymousAccess
	{
		[SetUp]
		public void SetUp()
		{
			SecurityConfigurator.Reset();
		}

		[Test]
		public void Should_not_throw_exception_when_the_user_is_authenticated()
		{
			// Arrange
			var controllerName = NameHelper.Controller<BlogController>();
			const string actionName = "Index";

			var events = new List<ISecurityEvent>();
			SecurityDoctor.Register(events.Add);
			SecurityConfigurator.Configure<MvcConfiguration>(policy =>
			{
				policy.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsTrue);
				policy.For<BlogController>(x => x.Index()).DenyAnonymousAccess();
			});

			var securityHandler = new SecurityHandler();

			// Act & Assert
			Assert.DoesNotThrow(() => securityHandler.HandleSecurityFor(controllerName, actionName, SecurityContext.Current));
			Assert.That(events.Any(e => e.Message == "Handling security for {0} action {1}.".FormatWith(controllerName, actionName)));
			Assert.That(events.Any(e => e.Message == "Done enforcing policies. Success!"));
		}

		[Test]
		public void Should_throw_when_the_user_is_anonymous()
		{
			// Arrange
			SecurityConfigurator.Configure<MvcConfiguration>(policy =>
			{
				policy.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);
				policy.For<BlogController>(x => x.Index()).DenyAnonymousAccess();
			});

			var securityHandler = new SecurityHandler();

			// Act
			var exception = Assert.Throws<PolicyViolationException>(() => securityHandler.HandleSecurityFor(NameHelper.Controller<BlogController>(), "Index", SecurityContext.Current));
			
			// Assert
			Assert.That(exception.PolicyType, Is.EqualTo(typeof(DenyAnonymousAccessPolicy)));
			Assert.That(exception.Message, Is.StringContaining("Anonymous access denied"));
		}
	}

	[TestFixture]
	[Category("SecurityHandlerSpec")]
	public class When_handling_security_for_a_controlleraction_with_RequireAnyRole_Owner
	{
		[SetUp]
		public void SetUp()
		{
			SecurityConfigurator.Reset();
		}

		[Test]
		public void Should_not_throw_exception_when_the_user_is_authenticated_with_role_Owner()
		{
			// Arrange
			SecurityConfigurator.Configure<MvcConfiguration>(policy =>
			{
				policy.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsTrue);
				policy.GetRolesFrom(StaticHelper.GetRolesIncludingOwner);
				policy.For<BlogController>(x => x.DeletePost(0)).RequireAnyRole(UserRole.Owner);
			});

			var securityHandler = new SecurityHandler();

			// Act & Assert
			Assert.DoesNotThrow(() => securityHandler.HandleSecurityFor(NameHelper.Controller<BlogController>(), "DeletePost", SecurityContext.Current));
		}

		[Test]
		public void Should_throw_when_the_user_is_anonymous()
		{
			// Arrange
			SecurityConfigurator.Configure<MvcConfiguration>(policy =>
			{
				policy.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);
				policy.GetRolesFrom(StaticHelper.GetRolesExcludingOwner);
				policy.For<BlogController>(x => x.DeletePost(0)).RequireAnyRole(UserRole.Owner);
			});

			var securityHandler = new SecurityHandler();

			// Act
			var exception = Assert.Throws<PolicyViolationException>(() => securityHandler.HandleSecurityFor(NameHelper.Controller<BlogController>(), "DeletePost", SecurityContext.Current));

			// Assert
			Assert.That(exception.PolicyType, Is.EqualTo(typeof(RequireAnyRolePolicy)));
			Assert.That(exception.Message, Is.StringContaining("Anonymous access denied"));
		}

		[Test]
		public void Should_throw_when_the_user_does_not_have_the_role_Owner()
		{
			// Arrange
			SecurityConfigurator.Configure<MvcConfiguration>(policy =>
			{
				policy.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsTrue);
				policy.GetRolesFrom(StaticHelper.GetRolesExcludingOwner);
				policy.For<BlogController>(x => x.DeletePost(0)).RequireAnyRole(UserRole.Owner);
			});

			var securityHandler = new SecurityHandler();

			// Act
			var exception = Assert.Throws<PolicyViolationException>(() => securityHandler.HandleSecurityFor(NameHelper.Controller<BlogController>(), "DeletePost", SecurityContext.Current));

			// Assert
			Assert.That(exception.PolicyType, Is.EqualTo(typeof(RequireAnyRolePolicy)));
			Assert.That(exception.Message, Is.StringContaining("Access requires one of the following roles: Owner."));
		}
	}

	[TestFixture]
	[Category("SecurityHandlerSpec")]
	public class When_handling_security_for_a_controller_and_action_that_has_no_container
	{
		[SetUp]
		public void SetUp()
		{
			SecurityConfigurator.Reset();
		}

		[Test]
		public void Should_throw_ConfigurationErrorsException_when_IgnoreMissingConfigurations_is_false()
		{
			SecurityConfigurator.Configure<MvcConfiguration>(policy =>
			{
				policy.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsTrue);
				policy.For<BlogController>(x => x.Index()).DenyAnonymousAccess();
			});

			var securityHandler = new SecurityHandler();

			// Act & Assert
			Assert.Throws<ConfigurationErrorsException>(() => securityHandler.HandleSecurityFor("NonConfiguredController", "Action", SecurityContext.Current));
		}

		[Test]
		public void Should_not_throw_ConfigurationErrorsException_when_IgnoreMissingConfigurations_is_true()
		{
			// Arrange
			var events = new List<ISecurityEvent>();
			SecurityDoctor.Register(events.Add);
			SecurityConfigurator.Configure<MvcConfiguration>(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsTrue);
				configuration.Advanced.IgnoreMissingConfiguration();
				configuration.For<BlogController>(x => x.Index()).DenyAnonymousAccess();
			});

			var securityHandler = new SecurityHandler();

			// Act & Assert
			Assert.DoesNotThrow(() => securityHandler.HandleSecurityFor("NonConfiguredController", "Action", SecurityContext.Current));
			Assert.That(events.Any(e => e.Message == "Ignoring missing configuration."));
		}
	}
}

using FluentSecurity.Policy;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("PolicyViolationExceptionSpec")]
	public class When_creating_a_PolicyViolationException
	{
		[Test]
		public void Should_have_PolicyType_set_to_DenyAnonymousAccessPolicy()
		{
			// Act
			var exception = new PolicyViolationException<DenyAnonymousAccessPolicy>("Anonymous access denied");

			// Assert
			Assert.That(exception.PolicyType, Is.EqualTo(typeof(DenyAnonymousAccessPolicy)));
			Assert.That(exception.Message, Is.EqualTo("Anonymous access denied"));
		}

		[Test]
		public void Should_have_PolicyType_set_to_DenyAuthenticatedAccessPolicy()
		{
			// Act
			var exception = new PolicyViolationException<DenyAuthenticatedAccessPolicy>("Authenticated access denied");

			// Assert
			Assert.That(exception.PolicyType, Is.EqualTo(typeof(DenyAuthenticatedAccessPolicy)));
			Assert.That(exception.Message, Is.EqualTo("Authenticated access denied"));
		}

		[Test]
		public void Should_have_PolicyType_set_to_RequireRolePolicy()
		{
			// Act
			var exception = new PolicyViolationException<RequireRolePolicy>("Access denied");

			// Assert
			Assert.That(exception.PolicyType, Is.EqualTo(typeof(RequireRolePolicy)));
			Assert.That(exception.Message, Is.EqualTo("Access denied"));
		}
	}
}
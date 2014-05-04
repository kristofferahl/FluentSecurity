using FluentSecurity.Policy;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("SecurityPolicyExtensionsSpec")]
	public class When_getting_the_policy_type_of_an_ISecurityPolicy
	{
		[Test]
		public void Should_retun_the_type_of_normal_policies()
		{
			// Arrange
			ISecurityPolicy policy = new IgnorePolicy();

			// Act & assert
			Assert.That(policy.GetPolicyType(), Is.EqualTo(typeof(IgnorePolicy)));
		}

		[Test]
		public void Should_retun_the_type_of_lazy_policies()
		{
			// Arrange
			ISecurityPolicy policy = new LazySecurityPolicy<IgnorePolicy>();

			// Act & assert
			Assert.That(policy.GetPolicyType(), Is.EqualTo(typeof(IgnorePolicy)));
		}
	}
}
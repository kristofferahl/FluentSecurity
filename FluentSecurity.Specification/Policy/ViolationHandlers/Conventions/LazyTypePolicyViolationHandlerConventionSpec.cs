using System;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy.ViolationHandlers.Conventions
{
	[TestFixture]
	[Category("LazyTypePolicyViolationHandlerConventionSpec")]
	public class When_creating_a_LazyTypePolicyViolationHandlerConvention
	{
		[Test]
		public void Should_throw_when_predicate_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new DerrivedLazyTypePolicyViolationHandlerConvention());
		}

		public class DerrivedLazyTypePolicyViolationHandlerConvention : LazyTypePolicyViolationHandlerConvention<DefaultPolicyViolationHandler>
		{
			public DerrivedLazyTypePolicyViolationHandlerConvention() : base(null) {}
		}
	}
}
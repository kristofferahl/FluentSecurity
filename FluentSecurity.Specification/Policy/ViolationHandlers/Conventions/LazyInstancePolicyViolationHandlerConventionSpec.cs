using System;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy.ViolationHandlers.Conventions
{
	[TestFixture]
	[Category("LazyInstancePolicyViolationHandlerConventionSpec")]
	public class When_creating_a_LazyInstancePolicyViolationHandlerConvention
	{
		[Test]
		public void Should_throw_when_predicate_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new DerrivedLazyInstancePolicyViolationHandlerConvention());
		}

		public class DerrivedLazyInstancePolicyViolationHandlerConvention : LazyInstancePolicyViolationHandlerConvention<DefaultPolicyViolationHandler>
		{
			public DerrivedLazyInstancePolicyViolationHandlerConvention() : base(() => new DefaultPolicyViolationHandler(), null) {}
		}
	}
}
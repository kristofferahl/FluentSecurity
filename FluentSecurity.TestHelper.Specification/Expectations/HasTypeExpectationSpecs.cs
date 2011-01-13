using FluentSecurity.TestHelper.Expectations;
using FluentSecurity.TestHelper.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.TestHelper.Specification.Expectations
{
	[TestFixture]
	[Category("HasTypeExpectationSpecs")]
	public class When_creating_a_HasTypeExpectation
	{
		[Test]
		public void Should_have_type()
		{
			var expectation = new HasTypeExpectation<DenyInternetExplorerPolicy>();
			Assert.That(expectation.Type, Is.EqualTo(typeof(DenyInternetExplorerPolicy)));
		}
	}
}
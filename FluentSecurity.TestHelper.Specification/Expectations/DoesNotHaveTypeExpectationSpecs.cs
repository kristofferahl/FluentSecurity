using FluentSecurity.TestHelper.Expectations;
using FluentSecurity.TestHelper.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.TestHelper.Specification.Expectations
{
	[TestFixture]
	[Category("DoesNotHaveTypeExpectationSpecs")]
	public class When_creating_a_DoesNotHaveTypeExpectation
	{
		[Test]
		public void Should_have_type()
		{
			var expectation = new DoesNotHaveTypeExpectation<DenyInternetExplorerPolicy>();
			Assert.That(expectation.Type, Is.EqualTo(typeof(DenyInternetExplorerPolicy)));
		}
	}
}
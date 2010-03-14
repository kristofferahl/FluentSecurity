using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("ExtensionsSpecs")]
	public class When_getting_the_container
	{
		private ICollection<IPolicyContainer> _containers;

		[SetUp]
		public void SetUp()
		{
			// Arrange
			_containers = new Collection<IPolicyContainer>
			{
				TestDataFactory.CreateValidPolicyContainer("Controller", "ActionThatDoesExist")
			};
		}

		[Test]
		public void Should_return_a_container_for_Controller_ActionThatDoesExist()
		{
			// Act
			var policyContainer = _containers.GetContainerFor("Controller", "ActionThatDoesExist");

			// Assert
			Assert.That(policyContainer, Is.Not.Null);
		}

		[Test]
		public void Should_return_null_for_Controller_ActionThatDoesNotExists()
		{
			// Act
			var policyContainer = _containers.GetContainerFor("Controller", "ActionThatDoesNotExists");

			// Assert
			Assert.That(policyContainer, Is.Null);
		}
		
		[Test]
		public void Should_return_a_container_for_controller_ActionThatDoesExist()
		{
			// Act
			var policyContainer = _containers.GetContainerFor("controller", "ActionThatDoesExist");

			// Assert
			Assert.That(policyContainer, Is.Not.Null);
		}
		
		[Test]
		public void Should_return_a_container_for_Controller_actionthatdoesexist()
		{
			// Act
			var policyContainer = _containers.GetContainerFor("Controller", "actionthatdoesexist");

			// Assert
			Assert.That(policyContainer, Is.Not.Null);
		}
		
		[Test]
		public void Should_return_a_container_for_controller_actionthatdoesexist()
		{
			// Act
			var policyContainer = _containers.GetContainerFor("controller", "actionthatdoesexist");

			// Assert
			Assert.That(policyContainer, Is.Not.Null);
		}
	}
}
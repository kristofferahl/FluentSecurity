using System;
using System.Collections.Generic;
using FluentSecurity.ServiceLocation;
using NUnit.Framework;

namespace FluentSecurity.Specification.ServiceLocation
{
	[TestFixture]
	[Category("ExternalServiceLocatorSpec")]
	public class When_creating_an_external_servicelocator
	{
		[Test]
		public void Should_throw_when_servicessource_is_null()
		{
			Func<Type, object> validSingleSource = t => null;
			Assert.Throws<ArgumentNullException>(() => new ExternalServiceLocator(null, validSingleSource));
		}

		[Test]
		public void Should_not_throw_when_servicessource_is_null()
		{
			var expectedInstance = new MyClass1();
			var expectedInstances = new List<object> { new MyClass1(), new MyClass1() };
			
			Func<Type, object> validSingleSource = t => expectedInstance;
			Func<Type, IEnumerable<object>> validServicesSource = t => expectedInstances;
			
			var serviceLocator = new ExternalServiceLocator(validServicesSource, validSingleSource);
			
			Assert.That(serviceLocator.Resolve(typeof(MyClass1)), Is.EqualTo(expectedInstance));
			Assert.That(serviceLocator.ResolveAll(typeof(MyClass1)), Is.EqualTo(expectedInstances));
		}

		private class MyClass1 {}
	}
}
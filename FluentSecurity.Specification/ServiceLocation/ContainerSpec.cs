using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.ServiceLocation;
using NUnit.Framework;

namespace FluentSecurity.Specification.ServiceLocation
{
	[TestFixture]
	[Category("ContainerSpec")]
	public class When_using_the_container
	{
		[Test]
		public void Should_resolve_transient_object()
		{
			// Arrange
			IContainer container = new Container(new MvcLifecycleResolver());
			container.Register<ITypeToResolve>(ctx => new ConcreteType());

			// Act
			var instance1 = container.Resolve<ITypeToResolve>();
			var instance2 = container.Resolve<ITypeToResolve>();

			// Assert
			Assert.That(instance1, Is.TypeOf(typeof(ConcreteType)));
			Assert.That(instance2, Is.TypeOf(typeof(ConcreteType)));
			Assert.AreNotEqual(instance1, instance2);
		}

		[Test]
		public void Should_resolve_singleton_object()
		{
			// Arrange
			IContainer container = new Container(new MvcLifecycleResolver());
			container.Register<ITypeToResolve>(ctx => new ConcreteType(), Lifecycle.Singleton);

			// Act
			var instance1 = container.Resolve<ITypeToResolve>();
			var instance2 = container.Resolve<ITypeToResolve>();

			// Assert
			Assert.AreEqual(instance1, instance2);
		}

		[Test]
		public void Should_resolve_transient_object_with_context()
		{
			// Arrange
			IContainer container = new Container(new MvcLifecycleResolver());
			container.Register<ITypeToResolve>(ctx => new ConcreteType());
			container.Register<ITypeToResolveWithConstructorParams>(ctx => new ConcreteTypeWithConstructorParams(ctx.Resolve<ITypeToResolve>()));

			// Act
			var instance1 = container.Resolve<ITypeToResolveWithConstructorParams>();
			var instance2 = container.Resolve<ITypeToResolveWithConstructorParams>();

			// Assert
			Assert.That(instance1, Is.TypeOf(typeof(ConcreteTypeWithConstructorParams)));
			Assert.That(instance1.InternalInstance, Is.TypeOf(typeof(ConcreteType)));
			Assert.That(instance2, Is.TypeOf(typeof(ConcreteTypeWithConstructorParams)));
			Assert.That(instance2.InternalInstance, Is.TypeOf(typeof(ConcreteType)));
			Assert.AreNotEqual(instance1, instance2);
			Assert.AreNotEqual(instance1.InternalInstance, instance2.InternalInstance);
		}

		[Test]
		public void Should_resolve_singleton_object_with_context()
		{
			// Arrange
			IContainer container = new Container(new MvcLifecycleResolver());
			container.Register<ITypeToResolve>(ctx => new ConcreteType());
			container.Register<ITypeToResolveWithConstructorParams>(ctx => new ConcreteTypeWithConstructorParams(ctx.Resolve<ITypeToResolve>()), Lifecycle.Singleton);

			// Act
			var instance1 = container.Resolve<ITypeToResolveWithConstructorParams>();
			var instance2 = container.Resolve<ITypeToResolveWithConstructorParams>();

			// Assert
			Assert.AreEqual(instance1, instance2);
			Assert.AreEqual(instance1.InternalInstance, instance2.InternalInstance);
		}

		[Test]
		public void Should_resolve_singleton_and_transient_object()
		{
			// Arrange
			IContainer container = new Container(new MvcLifecycleResolver());
			container.Register<ITypeToResolve>(ctx => new ConcreteType(), Lifecycle.Singleton);
			container.Register<ITypeToResolveWithConstructorParams>(ctx => new ConcreteTypeWithConstructorParams(ctx.Resolve<ITypeToResolve>()));

			// Act
			var instance1 = container.Resolve<ITypeToResolveWithConstructorParams>();
			var instance2 = container.Resolve<ITypeToResolveWithConstructorParams>();

			// Assert
			Assert.AreNotEqual(instance1, instance2);
			Assert.AreEqual(instance1.InternalInstance, instance2.InternalInstance);
		}

		[Test]
		public void Should_resolve_all_instances()
		{
			// Arrange
			IContainer container = new Container(new MvcLifecycleResolver());

			// Act & assert
			var instancesBefore = container.ResolveAll<ITypeToResolve>();
			Assert.That(instancesBefore.Count(), Is.EqualTo(0));
			
			container.Register<ITypeToResolve>(ctx => new ConcreteType());
			container.Register<ITypeToResolve>(ctx => new ConcreteType2());
			container.Register<ITypeToResolve>(ctx => new ConcreteType3());

			var instancesAfter = container.ResolveAll<ITypeToResolve>();
			Assert.That(instancesAfter.Count(), Is.EqualTo(3));
		}

		[Test]
		public void Should_resolve_the_first_registered_instance_as_the_default_instance()
		{
			// Arrange
			IContainer container = new Container(new MvcLifecycleResolver());

			// Act & assert
			container.Register<ITypeToResolve>(ctx => new ConcreteType3());
			container.Register<ITypeToResolve>(ctx => new ConcreteType2());
			container.Register<ITypeToResolve>(ctx => new ConcreteType());

			var instance = container.Resolve<ITypeToResolve>();
			Assert.That(instance, Is.TypeOf(typeof(ConcreteType3)));
		}

		[Test]
		public void Should_throw_when_resolving_type_that_is_not_registered()
		{
			// Arrange
			IContainer container = new Container(new MvcLifecycleResolver());

			// Act & assert
			Assert.Throws<TypeNotRegisteredException>(() => container.Resolve(typeof (ConcreteType)));
		}

		[Test]
		public void Should_return_empty_list_when_resolving_type_that_is_not_registered()
		{
			// Arrange
			IContainer container = new Container(new MvcLifecycleResolver());

			// Act
			var instances = container.ResolveAll(typeof(ConcreteType));

			// Assert
			Assert.That(instances, Is.Not.Null);
			Assert.That(instances, Is.InstanceOf(typeof(IEnumerable<object>)));
		}

		[Test]
		public void Should_resolve_single_instance_from_primary_source()
		{
			// Arrange
			var expectedInstance = new ConcreteType();
			IContainer container = new Container(new MvcLifecycleResolver());
			container.SetPrimarySource(ctx => new FakeContainerSource(expectedInstance));

			// Act
			var instance = container.Resolve(typeof(ConcreteType));

			// Assert
			Assert.That(instance, Is.EqualTo(expectedInstance));
		}

		[Test]
		public void Should_resolve_instances_from_primary_source()
		{
			// Arrange
			var expectedInstances = new List<object> { new ConcreteType3(), new ConcreteType3() };
			IContainer container = new Container(new MvcLifecycleResolver());
			container.SetPrimarySource(ctx => new FakeContainerSource(expectedInstances));

			// Act
			var instance = container.ResolveAll(typeof(ConcreteType3));

			// Assert
			Assert.That(instance, Is.EqualTo(expectedInstances));
		}

		public interface ITypeToResolve {}

		public class ConcreteType : ITypeToResolve {}

		public class ConcreteType2 : ITypeToResolve {}

		public class ConcreteType3 : ITypeToResolve {}

		public interface ITypeToResolveWithConstructorParams
		{
			ITypeToResolve InternalInstance { get; }
		}

		public class ConcreteTypeWithConstructorParams : ITypeToResolveWithConstructorParams
		{
			public ITypeToResolve InternalInstance { get; private set; }

			public ConcreteTypeWithConstructorParams(ITypeToResolve typeToResolve)
			{
				InternalInstance = typeToResolve;
			}
		}


		public class FakeContainerSource : IContainerSource
		{
			private readonly object _instance;
			private readonly IEnumerable<object> _instances;

			public FakeContainerSource(object instance)
			{
				_instance = instance;
			}

			public FakeContainerSource(IEnumerable<object> instances)
			{
				_instances = instances;
			}

			public object Resolve(Type typeToResolve)
			{
				return _instance;
			}

			public IEnumerable<object> ResolveAll(Type typeToResolve)
			{
				return _instances;
			}
		}
	}
}
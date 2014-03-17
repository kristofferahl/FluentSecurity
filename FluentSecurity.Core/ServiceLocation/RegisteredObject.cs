using System;

namespace FluentSecurity.ServiceLocation
{
	internal class RegisteredObject
	{
		public RegisteredObject(Type typeToResolve, Func<IContainer, object> instanceExpression, Lifecycle lifecycle)
		{
			TypeToResolve = typeToResolve;
			InstanceExpression = instanceExpression;
			InstanceKey = Guid.NewGuid();
			Lifecycle = lifecycle;
		}

		public Type TypeToResolve { get; private set; }
		public Guid InstanceKey { get; private set; }
		public Func<IContainer, object> InstanceExpression { get; private set; }
		public Lifecycle Lifecycle { get; private set; }

		public object CreateInstance(IContainer container)
		{
			return InstanceExpression.Invoke(container);
		}
	}
}
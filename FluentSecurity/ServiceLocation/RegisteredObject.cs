using System;

namespace FluentSecurity.ServiceLocation
{
    public class RegisteredObject
    {
		public RegisteredObject(Type typeToResolve, Func<IContainer, object> instanceExpression, LifeCycle lifeCycle)
		{
			TypeToResolve = typeToResolve;
			InstanceExpression = instanceExpression;
			LifeCycle = lifeCycle;
		}

        public Type TypeToResolve { get; private set; }
		public Func<IContainer, object> InstanceExpression { get; private set; }
        public object Instance { get; private set; }
    	public LifeCycle LifeCycle { get; private set; }

    	public void CreateInstance(IContainer container)
    	{
    		Instance = InstanceExpression(container);
    	}
    }
}
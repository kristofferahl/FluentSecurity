using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity.ServiceLocation
{
	// NOTE: Based on the work of Tim Ross
	// http://timross.wordpress.com/2010/01/21/creating-a-simple-ioc-container/
	public class Container : IContainer
	{
		private IContainerSource _primarySource;
		private readonly IList<RegisteredObject> _registeredObjects = new List<RegisteredObject>();

		public void Register<TTypeToResolve>(Func<IContainer, object> instanceExpression, Lifecycle lifecycle)
		{
			var registeredObject = new RegisteredObject(typeof(TTypeToResolve), instanceExpression, lifecycle);
			_registeredObjects.Add(registeredObject);
		}

		public TTypeToResolve Resolve<TTypeToResolve>()
		{
			return (TTypeToResolve)ResolveObject(typeof(TTypeToResolve));
		}

		public object Resolve(Type typeToResolve)
		{
			return ResolveObject(typeToResolve);
		}

		public IEnumerable<TTypeToResolve> ResolveAll<TTypeToResolve>()
		{
			return ResolveObjects(typeof(TTypeToResolve)).Cast<TTypeToResolve>();
		}

		public IEnumerable<object> ResolveAll(Type typeToResolve)
		{
			return ResolveObjects(typeToResolve);
		}

		public void SetPrimarySource(Func<IContainer, IContainerSource> source)
		{
			_primarySource = source.Invoke(this);
		}

		private IEnumerable<object> ResolveObjects(Type typeToResolve)
		{
			if (_primarySource != null)
			{
				var objects = _primarySource.ResolveAll(typeToResolve);
				if (objects.Any())
				{
					foreach (var registeredObject in objects)
						yield return registeredObject;

					yield break;
				}
			}
			
			var registeredObjects = _registeredObjects.Where(o => o.TypeToResolve == typeToResolve);
			foreach (var registeredObject in registeredObjects)
				yield return GetInstance(registeredObject);
		}

		private object ResolveObject(Type typeToResolve)
		{
			if (_primarySource != null)
			{
				var resolvedObject = _primarySource.Resolve(typeToResolve);
				if (resolvedObject != null) return resolvedObject;
			}
			
			var registeredObject = _registeredObjects.FirstOrDefault(o => o.TypeToResolve == typeToResolve);
			if (registeredObject == null)
				throw new TypeNotRegisteredException(String.Format("The type {0} has not been registered", typeToResolve.Name));
			
			return GetInstance(registeredObject);
		}

		private object GetInstance(RegisteredObject registeredObject)
		{
			var lifecycleCache = registeredObject.Lifecycle.Get().FindCache();
			var instance = lifecycleCache.Get(registeredObject.InstanceKey);
			
			if (instance == null)
			{
				instance = registeredObject.CreateInstance(this);
				lifecycleCache.Set(registeredObject.InstanceKey, instance);
			}

			return instance;
		}
	}
}
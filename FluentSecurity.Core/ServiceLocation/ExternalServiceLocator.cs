using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity.ServiceLocation
{
	public class ExternalServiceLocator : ISecurityServiceLocator
	{
		private readonly Func<Type, IEnumerable<object>> _servicesSource;
		private readonly Func<Type, object> _singleServiceSource;

		public ExternalServiceLocator(Func<Type, IEnumerable<object>> servicesSource, Func<Type, object> singleServiceSource = null)
		{
			if (servicesSource == null) throw new ArgumentNullException("servicesSource");			
			_servicesSource = servicesSource;
			_singleServiceSource = singleServiceSource;
		}

		public object Resolve(Type typeToResolve)
		{
			if (_singleServiceSource != null)
				return _singleServiceSource.Invoke(typeToResolve);

			return _servicesSource.Invoke(typeToResolve).FirstOrDefault();
		}

		public IEnumerable<object> ResolveAll(Type typeToResolve)
		{
			return _servicesSource.Invoke(typeToResolve);
		}
	}
}
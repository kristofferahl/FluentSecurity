using System;

namespace FluentSecurity.ServiceLocation
{
	public class TypeNotRegisteredException : Exception
	{
		public TypeNotRegisteredException(string message) : base(message) {}
	}
}
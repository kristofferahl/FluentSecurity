using System;

namespace FluentSecurity.Core.Policy.ViolationHandlers
{
	public class PolicyViolationHandlerConversionException : Exception
	{
		public PolicyViolationHandlerConversionException(Type instanceType, Type expectedType) : base(String.Format("The violation handler {0} does not implement the interface {1}!", instanceType.FullName, expectedType.FullName)) {}
	}
}
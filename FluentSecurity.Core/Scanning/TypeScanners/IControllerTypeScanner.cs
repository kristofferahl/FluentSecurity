using System;

namespace FluentSecurity.Scanning.TypeScanners
{
	public interface IControllerTypeScanner : ITypeScanner
	{
		void SetControllerType(Type controllerType);
	}
}
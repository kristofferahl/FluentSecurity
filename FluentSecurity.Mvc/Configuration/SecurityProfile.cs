using FluentSecurity.Scanning;
using FluentSecurity.Scanning.TypeScanners;

namespace FluentSecurity.Configuration
{
	public abstract class SecurityProfile : ConfigurationExpression, ISecurityProfile
	{
		void ISecurityProfile.Initialize(ISecurityRuntime runtime)
		{
			Initialize((SecurityRuntime) runtime);
		}

		public abstract void Configure();

		public override IPolicyContainerConfiguration ForAllControllers()
		{
			var assemblyScanner = Runtime.Container.Resolve<IAssemblyScanner>();
			var controllerTypeScanner = Runtime.Container.Resolve<IControllerTypeScanner>();

			assemblyScanner.Assembly(GetType().Assembly);
			assemblyScanner.With(controllerTypeScanner);
			var controllerTypes = assemblyScanner.Scan();

			return CreateConventionPolicyContainerFor(controllerTypes);
		}
	}
}
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
			var assemblyScanner = new AssemblyScanner();
			assemblyScanner.Assembly(GetType().Assembly);
			assemblyScanner.With<MvcControllerTypeScanner>();
			var controllerTypes = assemblyScanner.Scan();

			return CreateConventionPolicyContainerFor(controllerTypes);
		}
	}
}
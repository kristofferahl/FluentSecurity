using FluentSecurity.Configuration;
using FluentSecurity.Scanning;
using FluentSecurity.WebApi.Scanning.TypeScanners;

namespace FluentSecurity.WebApi.Configuration
{
	public abstract class WebApiSecurityProfile : WebApiConfigurationExpression, ISecurityProfile
	{
		void ISecurityProfile.Initialize(ISecurityRuntime runtime)
		{
			Initialize((WebApiSecurityRuntime) runtime);
		}

		public abstract void Configure();

		public override IPolicyContainerConfiguration ForAllControllers()
		{
			var assemblyScanner = Runtime.Container.Resolve<IAssemblyScanner>();
			assemblyScanner.Assembly(GetType().Assembly);
			assemblyScanner.With<WebApiControllerTypeScanner>();
			var controllerTypes = assemblyScanner.Scan();

			return CreateConventionPolicyContainerFor(controllerTypes);
		}
	}
}
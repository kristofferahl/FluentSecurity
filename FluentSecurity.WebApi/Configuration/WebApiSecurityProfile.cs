using FluentSecurity.Configuration;
using FluentSecurity.WebApi.Scanning;
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
			var assemblyScanner = new WebApiAssemblyScanner();
			assemblyScanner.Assembly(GetType().Assembly);
			assemblyScanner.With<WebApiControllerTypeScanner>();
			var controllerTypes = assemblyScanner.Scan();

			return CreateConventionPolicyContainerFor(controllerTypes);
		}
	}
}
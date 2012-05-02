using System.Web.Mvc;
using FluentSecurity.Policy;

namespace FluentSecurity.Specification.Helpers
{
	internal static class NameHelper
	{
		public static string Controller<TController>() where TController : IController
		{
			return typeof (TController).FullName;
		}

		public static string Policy<TPolicy>() where TPolicy : ISecurityPolicy
		{
			return typeof(TPolicy).FullName;
		}
	}
}
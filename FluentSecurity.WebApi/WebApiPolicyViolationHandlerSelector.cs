using System;
using System.Collections.Generic;
using FluentSecurity.Diagnostics;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.WebApi.Policy.ViolationHandlers;

namespace FluentSecurity.WebApi
{
	public class WebApiPolicyViolationHandlerSelector : IPolicyViolationHandlerSelector<object>
	{
		private readonly IEnumerable<IPolicyViolationHandlerConvention> _conventions;

		public WebApiPolicyViolationHandlerSelector(IEnumerable<IPolicyViolationHandlerConvention> conventions)
		{
			if (conventions == null) throw new ArgumentNullException("conventions");
			_conventions = conventions;
		}

		public ISecurityPolicyViolationHandler<object> FindHandlerFor(PolicyViolationException exception)
		{
			IWebApiPolicyViolationHandler matchingHandler = null;
			foreach (var violationHandlerConvention in _conventions)
			{
				var convention = violationHandlerConvention;

				Publish.RuntimeEvent(() =>
				{
					var conventionName = convention.ToString();
					return "Finding policy violation handler using convention {0}.".FormatWith(conventionName);
				}, exception.SecurityContext);

				matchingHandler = (IWebApiPolicyViolationHandler) convention.GetHandlerFor(exception);

				if (matchingHandler != null)
				{
					Publish.RuntimeEvent(() => "Found policy violation handler {0}.".FormatWith(matchingHandler.GetType().FullName), exception.SecurityContext);
					break;
				}
			}
			return matchingHandler;
		}
	}
}
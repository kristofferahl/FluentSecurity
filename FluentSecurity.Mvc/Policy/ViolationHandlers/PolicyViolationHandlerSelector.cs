using System;
using System.Collections.Generic;
using FluentSecurity.Diagnostics;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity.Policy.ViolationHandlers
{
	public class PolicyViolationHandlerSelector : IPolicyViolationHandlerSelector
	{
		private readonly IEnumerable<IPolicyViolationHandlerConvention> _conventions;

		public PolicyViolationHandlerSelector(IEnumerable<IPolicyViolationHandlerConvention> conventions)
		{
			if (conventions == null) throw new ArgumentNullException("conventions");
			_conventions = conventions;
		}

		public IPolicyViolationHandler FindHandlerFor(PolicyViolationException exception)
		{
			IPolicyViolationHandler matchingHandler = null;
			foreach (var violationHandlerConvention in _conventions)
			{
				var convention = violationHandlerConvention;
				
				Publish.RuntimeEvent(() =>
				{
					var conventionName = convention.ToString();
					return "Finding policy violation handler using convention {0}.".FormatWith(conventionName);
				}, exception.SecurityContext);

				matchingHandler = convention.GetHandlerFor(exception);

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
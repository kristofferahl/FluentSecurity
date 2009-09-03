using System.Linq;
using System.Text;

namespace FluentSecurity
{
	public class DefaultWhatDoIHaveBuilder : IWhatDoIHaveBuilder
	{
		public string WhatDoIHave()
		{
			var builder = new StringBuilder();

			builder.AppendFormat("Ignore missing configuration: {0}", Configuration.IgnoreMissingConfiguration);

			builder.AppendLine().AppendLine().AppendLine("------------------------------------------------------------------------------------");
			
			foreach (var policyContainer in Configuration.GetPolicyContainers().OrderBy(x => x.ControllerName).OrderBy(x => x.ActionName))
			{
				builder.AppendFormat(
					"{0} > {1}{2}",
					policyContainer.ControllerName + "Controller",
					policyContainer.ActionName,
					policyContainer.GetPolicies().ToText()
					);
				builder.AppendLine();
			}

			builder.Append("------------------------------------------------------------------------------------");

			return builder.ToString();
		}
	}
}
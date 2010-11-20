using System.Linq;
using System.Text;

namespace FluentSecurity
{
	public class DefaultWhatDoIHaveBuilder : IWhatDoIHaveBuilder
	{
		public string WhatDoIHave(ConfigurationExpression configurationExpression)
		{
			var builder = new StringBuilder();

			builder.AppendFormat("Ignore missing configuration: {0}", configurationExpression.ShouldIgnoreMissingConfiguration);

			builder.AppendLine().AppendLine().AppendLine("------------------------------------------------------------------------------------");

			foreach (var policyContainer in configurationExpression.OrderBy(x => x.ActionName).OrderBy(x => x.ControllerName))
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
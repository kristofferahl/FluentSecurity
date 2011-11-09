using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Specification.Helpers;

namespace FluentSecurity.Specification
{
	public abstract class AssemblyScannerNamespaceSpecification
	{
		protected static void Because(Action<ConfigurationExpression> configurationExpression)
		{
			// Arrange
			PolicyContainers = Enumerable.Empty<IPolicyContainer>();
			var expression = TestDataFactory.CreateValidConfigurationExpression();
			configurationExpression(expression);
			PolicyContainers = expression.ToList();
		}

		protected static IEnumerable<IPolicyContainer> PolicyContainers { get; private set; }
	}
}
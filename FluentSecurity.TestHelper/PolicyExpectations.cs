using System;
using System.Collections.Generic;

namespace FluentSecurity.TestHelper
{
	public class PolicyExpectations
	{
		private readonly IList<KeyValuePair<string, string>> _expectationsFor;

		public PolicyExpectations(IEnumerable<IPolicyContainer> policyContainers)
		{
			if (policyContainers == null) throw new ArgumentNullException("policyContainers");

			PolicyContainers = policyContainers;
			_expectationsFor = new List<KeyValuePair<string, string>>();
		}

		public IEnumerable<KeyValuePair<string, string>> ExpectationsFor { get { return _expectationsFor; } }
		public IEnumerable<IPolicyContainer> PolicyContainers { get; private set; }

		public PolicyExpectations For(string controllerName, string actionName)
		{
			if (string.IsNullOrEmpty(controllerName)) throw new ArgumentException("Controller name must not be null or empty", "controllerName");
			if (string.IsNullOrEmpty(actionName)) throw new ArgumentException("Action name must not be null or empty", "actionName");

			_expectationsFor.Add(new KeyValuePair<string, string>(controllerName, actionName));
			return this;
		}
	}
}
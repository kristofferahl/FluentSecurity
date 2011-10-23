using System;

namespace FluentSecurity.Policy
{
	public class DelegatePolicy : ISecurityPolicy
	{
		public string Name { get; private set; }
		public Func<DelegateSecurityContext, PolicyResult> PolicyDelegate { get; private set; }

		public DelegatePolicy(string uniqueName, Func<DelegateSecurityContext, PolicyResult> policyDelegate)
		{
			if (String.IsNullOrWhiteSpace(uniqueName))
				throw new ArgumentException("uniqueName");

			if (policyDelegate == null)
				throw new ArgumentNullException("policyDelegate");

			Name = uniqueName;
			PolicyDelegate = policyDelegate;
		}

		public PolicyResult Enforce(ISecurityContext context)
		{
			var wrappedContext = new DelegateSecurityContext(this, context);
			return PolicyDelegate.Invoke(wrappedContext);
		}

		public class DelegateSecurityContext : SecurityContextWrapper
		{
			public DelegateSecurityContext(ISecurityPolicy policy, ISecurityContext securityContext) : base(securityContext)
			{
				Policy = policy;
			}

			public ISecurityPolicy Policy { get; private set; }
		}
	}
}
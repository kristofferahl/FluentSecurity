using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FluentSecurity
{
	public static class Configuration
	{
		private static IWhatDoIHaveBuilder WhatDoIHaveBuilder;
		private static readonly IList<IPolicyContainer> PolicyContainers;
		private static bool IsConfigured;

		static Configuration()
		{
			WhatDoIHaveBuilder = new DefaultWhatDoIHaveBuilder();
			PolicyContainers = new List<IPolicyContainer>();
		}

		public static void Configure(Action<PolicyBuilder> securityPolicyBuilder)
		{
			Reset();

			var builder = new PolicyBuilder();
			securityPolicyBuilder(builder);

			foreach (var securityPolicyContainer in builder)
			{
				PolicyContainers.Add(securityPolicyContainer);
			}

			IgnoreMissingConfiguration = builder.ShouldIgnoreMissingConfiguration;
			IsConfigured = true;
		}

		public static void Reset()
		{
			PolicyContainers.Clear();
			IsConfigured = false;
		}

		public static IEnumerable<IPolicyContainer> GetPolicyContainers()
		{
			if (IsConfigured == false)
				throw new InvalidOperationException("You must configure security before calling GetPolicyContainers");

			return new ReadOnlyCollection<IPolicyContainer>(PolicyContainers);
		}

		public static bool IgnoreMissingConfiguration { get; private set; }

		public static string WhatDoIHave()
		{
			return WhatDoIHaveBuilder.WhatDoIHave();
		}
	}
}
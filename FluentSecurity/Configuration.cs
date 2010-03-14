using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FluentSecurity
{
	public static class Configuration
	{
		private static readonly IWhatDoIHaveBuilder WhatDoIHaveBuilder;
		private static readonly IList<IPolicyContainer> PolicyContainers;
		private static bool _isConfigured;

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
			_isConfigured = true;
		}

		public static void Reset()
		{
			PolicyContainers.Clear();
			_isConfigured = false;
		}

		public static IEnumerable<IPolicyContainer> GetPolicyContainers()
		{
			if (_isConfigured == false)
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
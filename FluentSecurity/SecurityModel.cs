using System;
using System.Collections.Generic;

namespace FluentSecurity
{
	public class SecurityModel
	{
		internal IList<Type> Profiles { get; private set; }
		internal IList<IPolicyContainer> PolicyContainers { get; private set; } 

		public SecurityModel()
		{
			Profiles = new List<Type>();
			PolicyContainers = new List<IPolicyContainer>();
		}
	}
}
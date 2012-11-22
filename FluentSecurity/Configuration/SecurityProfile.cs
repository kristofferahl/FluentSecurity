namespace FluentSecurity.Configuration
{
	public abstract class SecurityProfile : ConfigurationExpression
	{
		internal void Apply(SecurityModel model, IPolicyAppender policyAppender)
		{
			Initialize(model, policyAppender);
			Configure();
			model.Profiles.Add(GetType());
		}

		public abstract void Configure();
	}
}
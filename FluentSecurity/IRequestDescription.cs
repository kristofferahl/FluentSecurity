namespace FluentSecurity
{
	public interface IRequestDescription
	{
		string AreaName { get; }
		string ControllerName { get; }
		string ActionName { get; }
	}
}
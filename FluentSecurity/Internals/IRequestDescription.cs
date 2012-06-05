namespace FluentSecurity.Internals
{
	public interface IRequestDescription
	{
		string AreaName { get; }
		string ControllerName { get; }
		string ActionName { get; }
	}
}